using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Think_App
{
	public partial class TakePhotoPage : BackCustomizeContentPage
	{
		bool IsRunning;
		bool IsInitSelectHairViewCompleted;

		string _url;
		double _scale;
		double _shiftX;
		double _shiftY;

		double _imageW;
		double _imageH;

		Rectangle _hairImgRect;

		public TakePhotoPage()
		{
			InitializeComponent();

			// このページに戻るときにタイトルを表示しない。
			NavigationPage.SetBackButtonTitle(this, "");

			if (EnableBackButtonOverride)
			{
				this.CustomBackButtonAction = async () =>
				{
					if (string.IsNullOrEmpty(_url))
					{
						// 通常時はページをポップする。
						Debug.WriteLine("ページを閉じます。");
						await Navigation.PopAsync();
					}
					else
					{
						// 髪型が選択されているときは、髪型の選択を外すのみ。
						Debug.WriteLine("髪型の選択を外します。");
						_url = null;
						this.HairImg.Source = null;
						this.HairImg.IsVisible = false;
						// 選択ビューの選択も外す。
						this.SelectHairView.UpdateHairStyleSelection(null);
					}
				};
			}

			this.CameraView.PhotoTaken += CameraView_PhotoTaken;
			this.CameraView.FaceDetected += CameraView_FaceDetected;
			this.CameraView.FaceDetectionFailed += CameraView_FaceDetectionFailed;

			this.CameraController.TakePhotoButtonClicked += CameraController_TakePhotoButtonClicked;
			this.CameraController.SwitchCameraButtonClicked += CameraController_SwitchCameraButtonClicked;

			this.SelectHairBtn.Clicked += SelectHairBtn_Clicked;

			this.CloseSelectHairViewTap.Tapped += CloseSelectHairViewTap_Tapped;

			this.SelectHairView.HairStyleSelected += SelectHairView_HairStyleSelected;

			this.SelectHairView.InitViewCompleted += async (sender, e) =>
			{
				IsInitSelectHairViewCompleted = true;
				// ビューの構築前のカメラのプレビュー開始はここ。
				// そうしないとビューの構築に時間がかかってしまう。
				this.CameraView.IsPreviewing = true;
				Debug.WriteLine("カメラのプレビューを開始しました。");
				// ビューの構築が終了したら、CameraControllerを表示させる。
				this.CameraController.IsVisible = true;
				// 初期から髪の毛を選べるようにする。
				await SwitchSelectHairViewShowing(false);
			};

			// デバッグ用
			//this.FaceTestBox.IsVisible = true;
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			if (IsInitSelectHairViewCompleted)
			{
				this.CameraView.IsPreviewing = true;
				Debug.WriteLine("カメラのプレビューを開始しました。");
				this.CameraController.IsVisible = true;
			}
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();

			this.CameraView.IsPreviewing = false;
			Debug.WriteLine("カメラのプレビューを終了しました。");
		}

		void CameraController_TakePhotoButtonClicked(object sender, EventArgs e)
		{
			this.CameraView.TakePhoto();
		}

		void CameraController_SwitchCameraButtonClicked(object sender, EventArgs e)
		{
			var camera = this.CameraView.Camera;
			if (camera == CameraOptions.Front)
			{
				this.CameraView.Camera = CameraOptions.Rear;
			}
			else if (camera == CameraOptions.Rear)
			{
				this.CameraView.Camera = CameraOptions.Front;
			}
		}

		async void CameraView_PhotoTaken(object sender, PhotoTakenEventArgs e)
		{
			if (e.ImageSource == null || this.HairImg.Source == null)
			{
				return;
			}
			Debug.WriteLine("撮影成功！");
			this.CameraView.IsPreviewing = false;

			// この時点での写真を保存して次回使えるようにしておきます。
			await ImageManager.SaveImageToLocalStorageAsync(e.ImageSource, ConstantManager.FolderName_Main, ConstantManager.FileName_LastImage, true);

			var page = new EditHairPage(_hairImgRect, e.ImageSource, this.HairImg.Source);

			// ページ遷移。
			await Navigation.PushAsync(page);
		}

		void CameraView_FaceDetected(object sender, FaceDetectedEventArgs e)
		{
			Debug.WriteLine("顔認証 Left:{0} Top:{1} Width:{2} Height:{3}", e.FaceRange.Left, e.FaceRange.Top, e.FaceRange.Width, e.FaceRange.Height);

			//AbsoluteLayout.SetLayoutBounds(this.FaceTestBox, new Rectangle(e.FaceRange.Left, e.FaceRange.Top, e.FaceRange.Width, e.FaceRange.Height));

			if (this.HairImg.Source != null && this.HairImg.IsVisible == false)
			{
				// ヘアスタイルイメージにソースが設定されているのに表示が切れている場合は表示
				this.HairImg.IsVisible = true;
			}

			if (this.HairImg.IsVisible)
			{
				// ヘアスタイルイメージが表示状態であれば、位置とサイズ調整

				// サイズ(横基準)
				var imageW = e.FaceRange.Width * _scale;
				var imageH = (_imageW > 0) ? imageW * _imageH / _imageW : imageW;
				// Left位置
				var shiftLeft = (imageW - e.FaceRange.Width) / 2;
				var left = e.FaceRange.Left - shiftLeft + imageW * _shiftX;
				// Top位置
				var shiftTop = (imageH - e.FaceRange.Height) / 2;
				var top = e.FaceRange.Top - shiftTop + imageH * _shiftY;

				// サイズ適用
				_hairImgRect = new Rectangle(left, top, imageW, imageH);
				AbsoluteLayout.SetLayoutBounds(this.HairImg, _hairImgRect);
			}
		}

		void CameraView_FaceDetectionFailed(object sender, EventArgs e)
		{
			Debug.WriteLine("顔認証できていません。");

			if (this.HairImg.IsVisible)
			{
				// ヘアスタイルイメージが表示状態であれば、表示を非表示にする。
				this.HairImg.IsVisible = false;
			}
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

				// CameraController有効化
				this.CameraController.IsEnabled = true;

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

				// CameraController無効化
				this.CameraController.IsEnabled = false;

				// BoxForClosingSelectHairView表示
				this.BoxForClosingSelectHairView.IsVisible = true;
			}
		}

		void SelectHairView_HairStyleSelected(object sender, HairStyleSelectedEventArgs e)
		{
			// ローカルに保存
			_url = e.Url;
			_scale = e.Scale;
			_shiftX = e.ShiftX;
			_shiftY = e.SHiftY;

			// デバッグ表示
			Debug.WriteLine("Url:{0}", _url);
			Debug.WriteLine("Scale:{0}", _scale);
			Debug.WriteLine("ShiftX:{0}", _shiftX);
			Debug.WriteLine("ShiftY:{0}", _shiftY);

			// ヘアスタイルイメージ更新
			var awaiter = ImageManager.LoadImageFromLocalStorageByUrlAsync(_url, false).GetAwaiter();
			awaiter.OnCompleted(async () =>
			{
				var source = awaiter.GetResult();
				if (source == null)
				{
					// ローカルのイメージソースがないので、サーバーから取得する。
					source = ImageSource.FromUri(new Uri(_url));
				}

				if (source != null)
				{
					// イメージのサイズ取得
					var service = DependencyService.Get<IImageService>();
					var size = await service.GetImageSizeAsync(source);

					_imageW = size.Width;
					_imageH = size.Height;

					// イメージソース設定
					this.HairImg.Source = source;
				}
			});
		}
	}
}
