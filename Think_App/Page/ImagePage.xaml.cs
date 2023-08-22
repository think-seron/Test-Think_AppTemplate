using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
	public partial class ImagePage : BackCustomizeContentPage
	{
		Rect? _faceRect;
		Rect _hairImgRect;

		bool IsRunning;
		bool IsInitialized;
		bool _isGallery;

		ImageSource _source;

		public ImagePage(ImageSource source, double imageWidth, double imageHeight, bool isGallery)
		{
			InitializeComponent();
			// このページに戻るときにタイトルを表示しない。
			NavigationPage.SetBackButtonTitle(this, "");

			if (EnableBackButtonOverride)
			{
				this.CustomBackButtonAction = async () =>
				{
					if (!this.HairImg.IsVisible)
					{
						// 通常時はページをポップする。
						Debug.WriteLine("ページを閉じます。");
						await Navigation.PopAsync();
					}
					else
					{
						// 髪型が選択されているときは、髪型の選択を外すのみ。
						Debug.WriteLine("髪型の選択を外します。");
						this.HairImg.Source = null;
						this.HairImg.IsVisible = false;
						// 選択ビューの選択も外す。
						this.SelectHairView.UpdateHairStyleSelection(null);
					}
				};
			}

			var decideItem = new ToolbarItem()
			{
				Text = "決定",
				Command = new Command(OnDecide)
			};
			this.ToolbarItems.Add(decideItem);

			_isGallery = isGallery;
			if (isGallery)
			{
				// PhotoImgを非表示に
				this.PhotoImg.IsVisible = false;
				// GalleyImgにソースを設定
				this.GalleryImg.Source = source;
			}
			else
			{
				// GalleyImgを非表示に
				this.GalleryImg.IsVisible = false;
				// PhotoImgにソースを設定
				this.PhotoImg.Source = source;
			}

			this.ImageControlContainer.ImageWidth = imageWidth;
			this.ImageControlContainer.ImageHeight = imageHeight;

			this.SelectHairBtn.Clicked += SelectHairBtn_Clicked;
			this.CloseSelectHairViewTap.Tapped += CloseSelectHairViewTap_Tapped;
			this.SelectHairView.HairStyleSelected += SelectHairView_HairStyleSelected;

			var nl = Environment.NewLine;
			this.FaceDetectionAttentionLbl.Text = "顔が正常に検出されませんでした。" + nl + "決定後、編集画面で調整ができます。";

			// デバッグ用
			//this.FaceTestBox.IsVisible = true;
		}

		async void OnDecide(object obj)
		{
			var source = _isGallery ? this.GalleryImg.Source : this.PhotoImg.Source;
			if (source is StreamImageSource)
			{
				// StreamImageSourceはクローンしておかないと変なデータになることがある。
				var service = DependencyService.Get<IImageService>();
				_source = await service.CloneImageSourceAsync(source);
			}
			else
			{
				_source = source;
			}

			if (this.HairImg.Source == null)
			{
				Debug.WriteLine("髪型が選ばれていません！");
				// アラートを表示。
				await DisplayAlert("", "髪型を選択してください", "OK");
				return;
			}

			// 画像にかけるScale/Translationの値を算出。
			var scale = this.ImageControlContainer.Content.Scale;
			var translationX = this.ImageControlContainer.Content.TranslationX;
			var translationY = this.ImageControlContainer.Content.TranslationY;

			var imageWidth = this.ImageControlContainer.ImageWidth;
			var imageHeight = this.ImageControlContainer.ImageHeight;

			// 髪の毛画像の表示範囲を算出する。
			var viewImageRange = this.ImageControlContainer.ImageRange;
			var hairImgRect = CalcHairImageViewRect(_hairImgRect, imageWidth, imageHeight, this.ImageControlContainer.Content.Scale, viewImageRange);
			var page = new EditHairPage(hairImgRect, _source, this.HairImg.Source, _isGallery, scale, translationX, translationY, viewImageRange);

			// ページ遷移。
			await Navigation.PushAsync(page);
		}

		Rect CalcHairImageViewRect(Rect orgHairImageRect, double imageWidth, double imageHeight, double scale, Rect viewImageRange)
		{
			var rect = Rect.Zero;

			double viewWidth, viewHeight;
			if (_isGallery)
			{
				viewWidth = this.GalleryImg.Width;
				viewHeight = this.GalleryImg.Height;
			}
			else
			{
				viewWidth = this.PhotoImg.Width;
				viewHeight = this.PhotoImg.Height;
			}

			// 初期の正しい画像表示エリアを求める。
			var scaleW = viewWidth / imageWidth;
			var scaleH = viewHeight / imageHeight;
			var s = Math.Min(scaleW, scaleH);
			var realViewWidth = imageWidth * s;
			var realViewHeight = imageHeight * s;
			var shiftX = (viewWidth - realViewWidth) / 2.0;
			var shiftY = (viewHeight - realViewHeight) / 2.0;

			// orgHairImageRectの左上の座標を、画像座標上で求める。
			var ix = orgHairImageRect.X / realViewWidth * imageWidth;
			var iy = orgHairImageRect.Y / realViewHeight * imageHeight;

			// 現在のviewImageRangeにより、x,y位置を画像座標上で求める。
			var x = ix - viewImageRange.X;
			var y = iy - viewImageRange.Y;

			// 画像座標からview座標に変換。
			var vx = x / imageWidth * realViewWidth * scale - shiftX * scale;
			var vy = y / imageHeight * realViewHeight * scale - shiftY * scale;

			// 幅・高さはscale倍。
			var w = orgHairImageRect.Width * scale;
			var h = orgHairImageRect.Height * scale;

			// 全て代入。
			rect.X = vx;
			rect.Y = vy;
			rect.Width = w;
			rect.Height = h;

			Debug.WriteLine("座標変換 X:{0} Y:{1} W:{2} H:{3}", rect.X, rect.Y, rect.Width, rect.Height);

			return rect;
		}

		async void SelectHairBtn_Clicked(object sender, EventArgs e)
		{
			if (IsRunning)
			{
				return;
			}
			IsRunning = true;

			await SwitchSelectHairViewShowing();

			IsRunning = false;
		}

		async void CloseSelectHairViewTap_Tapped(object sender, EventArgs e)
		{
			if (!this.SelectHairView.IsVisible || IsRunning)
			{
				return;
			}
			IsRunning = true;

			await SwitchSelectHairViewShowing();

			IsRunning = false;
		}

		async Task SwitchSelectHairViewShowing(bool animated = true)
		{
			// SelectHairViewの高さ
			var height = this.SelectHairView.Height;
			if (this.SelectHairView.IsVisible)
			{
				// SelectHairView非表示
				if (animated)
				{
					await this.SelectHairView.TranslateTo(0, height, 300);
				}
				this.SelectHairView.IsVisible = false;

				// SelectHairBtn表示
				this.SelectHairBtn.IsVisible = true;

				// BoxForClosingSelectHairView非表示
				this.BoxForClosingSelectHairView.IsVisible = false;
			}
			else
			{
				// SelectHairBtn非表示
				this.SelectHairBtn.IsVisible = false;

				// SelectHairView表示
				if (animated)
				{
					await this.SelectHairView.TranslateTo(0, height, 0);
				}
				else
				{
					await this.SelectHairView.TranslateTo(0, 0, 0);
				}
				this.SelectHairView.IsVisible = true;
				if (animated)
				{
					await this.SelectHairView.TranslateTo(0, 0, 300);
				}

				// BoxForClosingSelectHairView表示
				this.BoxForClosingSelectHairView.IsVisible = true;
			}
		}

		void SelectHairView_HairStyleSelected(object sender, HairStyleSelectedEventArgs e)
		{
			var url = e.Url;
			var scale = e.Scale;
			var shiftX = e.ShiftX;
			var shiftY = e.SHiftY;

			// デバッグ表示
			Debug.WriteLine("Url:{0}", url);
			Debug.WriteLine("Scale:{0}", scale);
			Debug.WriteLine("ShiftX:{0}", shiftX);
			Debug.WriteLine("ShiftY:{0}", shiftY);

			// ヘアスタイルイメージ更新
			var awaiter = ImageManager.LoadImageFromLocalStorageByUrlAsync(url, false).GetAwaiter();
			awaiter.OnCompleted(async () =>
			{
				var source = awaiter.GetResult();
				if (source == null)
				{
					// ローカルのイメージソースがないので、サーバーから取得する。
					source = ImageSource.FromUri(new Uri(url));
				}

				if (source != null)
				{
					// イメージのサイズ取得
					var service = DependencyService.Get<IImageService>();
					var size = await service.GetImageSizeAsync(source);

					var imageW = size.Width;
					var imageH = size.Height;

					// イメージソース設定
					this.HairImg.Source = source;

					var faceRange = Rect.Zero;
					if (_faceRect != null)
					{
						// 顔認識がされている場合。
						faceRange = _faceRect.Value;
					}

					// サイズ(横基準)
					var scaledImageW = faceRange.Width * scale;
					var scaledImageH = (imageW > 0) ? scaledImageW * imageH / imageW : scaledImageW;
					// Left位置
					var shiftLeft = (scaledImageW - faceRange.Width) / 2;
					var left = faceRange.Left - shiftLeft + scaledImageW * shiftX;
					// Top位置
					var shiftTop = (scaledImageH - faceRange.Height) / 2;
					var top = faceRange.Top - shiftTop + scaledImageH * shiftY;

					if (this.HairImg.Source != null && !this.HairImg.IsVisible)
					{
						// 髪型画像ソースが設定されているのに非表示の場合は表示にする。
						this.HairImg.IsVisible = true;
					}
					// サイズ適用
					_hairImgRect = new Rect(left, top, scaledImageW, scaledImageH);
					AbsoluteLayout.SetLayoutBounds(this.HairImg, _hairImgRect);
				}
			});
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();

			if (IsInitialized)
			{
				return;
			}

			// 顔認識を行う。
			var service = DependencyService.Get<IImageService>();
			var source = _isGallery ? this.GalleryImg.Source : this.PhotoImg.Source;
			var width = _isGallery ? this.GalleryImg.Width : this.PhotoImg.Width;
			var height = _isGallery ? this.GalleryImg.Height : this.PhotoImg.Height;
			var aspect = _isGallery ? this.GalleryImg.Aspect : this.PhotoImg.Aspect;
			var rect = await service.GetFaceRangeFromImageSource(source, width, height, aspect);
			if (rect == null)
			{
				Debug.WriteLine("顔認識できませんでした。");
				// 仮の顔認識を行う。
				double w = 180;
				double h = 224;
				double x = (width - w) / 2.0;
				double y = 102;
				_faceRect = new Rect(x, y, w, h);
				this.FaceDetectionAttention.IsVisible = true;
			}
			else
			{
				Debug.WriteLine("顔認識に成功！");
				_faceRect = rect.Value;
				this.FaceDetectionAttention.IsVisible = false;
			}
			// 顔認識が確定したら、SelectHairBtnを表示する。
			this.SelectHairBtn.IsVisible = true;
			this.ImageControlContainer.IsControllable = true;

			IsInitialized = true;
		}
	}
}
