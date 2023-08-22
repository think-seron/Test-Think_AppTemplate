using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
	public partial class SavedDataGalleryPage : ContentPage
	{
		List<string> _filepathList;
		List<GalleryViewCellModel> Models;

		public event EventHandler<ImageSource> ImageSelected = delegate { };

		double _tileSize = -1;
		double _scale;

		// 今表示している最初のタイル
		int _startIndex = 0;
		// 今表示している最後のタイル
		int _endIndex = 0;

		bool IsRunning;
		public bool IsSetupCompleted { private get; set; }
		private Purpose PagePurpose { get; set; }

		public enum Purpose
		{
			HairSimulation,
			SelectImage
		}

		public SavedDataGalleryPage(Purpose purpose = Purpose.HairSimulation)
		{
			InitializeComponent();

			// このページに戻るときにタイトルを表示しない。
			NavigationPage.SetBackButtonTitle(this, "");

			PagePurpose = purpose;

			var service = DependencyService.Get<IScreenService>();
			_scale = service.GetScreenScale();
			var screenWidth = service.GetScreenWidth();

			// タイルサイズの設定。
			_tileSize = (screenWidth - this.GalleryView.InnerPadding.Left - this.GalleryView.InnerPadding.Right - this.GalleryView.ColumnSpacing * (this.GalleryView.MaxColumns - 1)) / this.GalleryView.MaxColumns;
			this.GalleryView.TileHeight = _tileSize;

			Models = new List<GalleryViewCellModel>();

			this.GalleryView.Scrolled += GalleryView_Scrolled;
			this.GalleryView.Command = new Command(OnThumbnailTapped);
		}

		async void GalleryView_Scrolled(object sender, ScrolledEventArgs e)
		{
			if (IsRunning)
			{
				return;
			}
			IsRunning = true;

			Debug.WriteLine("ScrollY :{0}", e.ScrollY);

			var viewingStartIndex = CalculateViewingStartIndex(e.ScrollY);
			var viewingEndIndex = CalculateViewingEndIndex(e.ScrollY);

			Debug.WriteLine("ViewingStartIndex :{0}", viewingStartIndex);
			Debug.WriteLine("ViewingEndIndex :{0}", viewingEndIndex);

			if (_endIndex < viewingEndIndex)
			{
				// タイルが足りないので追加する。
				await this.GalleryView.LoadTiles(_endIndex + 1, viewingEndIndex - _endIndex);
				_endIndex = viewingEndIndex;
			}
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			if (!IsSetupCompleted)
			{
				Models.Clear();
				Setup();
			}
		}

		void Setup()
		{
			Task.Run(async () =>
			{
				// ファイルパスリストの取得。
				_filepathList = await StorageManager.UserFilePathsReadAsync(ConstantManager.FolderName_SavedImage);
				// リストを降順ソート。(これで最新の日付が最初になる)
				_filepathList.Reverse();
				// ファイルパスリストから、Modelsを作成する
				if (_filepathList != null && _filepathList.Count > 0)
				{
					var suffixWithExtension = ConstantManager.Suffix_Image + ".png";
					var suffixWithExtensionLength = suffixWithExtension.Length;
					foreach (var path in _filepathList)
					{
						var filename = Path.GetFileName(path);
						var str = filename.Substring(filename.Length - suffixWithExtensionLength);

						if (str == suffixWithExtension)
						{
							Debug.WriteLine("追加:{0}", path);
							Models.Add(new GalleryViewCellModel()
							{
								GVCThumbnailImgSource = new FileImageSource() { File = path },
								GVCThumbnailImgBGColor = Colors.Transparent,
								GVCThumbnailImgDSWidth = _tileSize * _scale,
								GVCThumbnailImgDSHeight = _tileSize * _scale
							});
						}
					}
				}

				this.GalleryView.ItemsSource = Models;

				Device.BeginInvokeOnMainThread(async () =>
				{
					this.GalleryView.Setup();

					// 初期表示に必要なタイルの個数を計算
					var height = this.GalleryView.Height - this.GalleryView.InnerPadding.Top;
					var count = CalculatePuttableTilesCount(height);

					// 一応全タイルを消す
					this.GalleryView.RemoveAllTiles();
					// 初期データをロード
					Debug.WriteLine("タイルをロード:{0}", count);
					await this.GalleryView.LoadTiles(0, count);
					_startIndex = 0;
					_endIndex = count - 1;
					IsSetupCompleted = true;
				});
			});
		}

		int CalculatePuttableTilesCount(double height)
		{
			var rowCount = (int)Math.Ceiling(height / (_tileSize + this.GalleryView.RowSpacing));
			return rowCount * this.GalleryView.MaxColumns;
		}

		int CalculateViewingStartIndex(double scrollY)
		{
			var rowIndex = (int)Math.Floor((scrollY - this.GalleryView.InnerPadding.Top) / (_tileSize + this.GalleryView.RowSpacing));
			if (rowIndex < 0) { rowIndex = 0; }
			var index = rowIndex * this.GalleryView.MaxColumns;
			return index.Clamp(0, this.GalleryView.ItemsSource.Count);
		}

		int CalculateViewingEndIndex(double scrollY)
		{
			var rowIndex = (int)Math.Floor((scrollY + this.GalleryView.Height - this.GalleryView.InnerPadding.Top) / (_tileSize + this.GalleryView.RowSpacing));
			if (rowIndex < 0) { rowIndex = 0; }
			var index = (rowIndex + 1) * this.GalleryView.MaxColumns - 1;
			return index.Clamp(0, this.GalleryView.ItemsSource.Count);
		}

		async void OnThumbnailTapped(object obj)
		{
			if (IsRunning)
			{
				return;
			}
			IsRunning = true;

			if (PagePurpose == Purpose.HairSimulation)
			{
				// ヘアシミュのとき
				var model = obj as GalleryViewCellModel;
				if (model != null && model.GVCThumbnailImgSource is FileImageSource)
				{
					var imgFilePath = ((FileImageSource)model.GVCThumbnailImgSource).File;
					var imageFilename = Path.GetFileName(imgFilePath);
					// imageFilenameから基本ファイル名を読み出す
					var length = (ConstantManager.Suffix_Image + ".png").Length;
					var basisFilename = imageFilename.Substring(0, imageFilename.Length - length);
					// 1)画像の編集情報
					var infoFilename = basisFilename + ConstantManager.Suffix_EditInfo + ".txt";
					// 2)髪型の画像
					var hairFilename = basisFilename + ConstantManager.Suffix_Hair + ".png";
					// 3)本人の画像
					var faceFilename = basisFilename + ConstantManager.Suffix_Face + ".png";

					// それぞれファイルの存在をチェックし、すべてある場合のみ、ページ遷移する。
					var saveData = await StorageManager.UserDataReadAsync<EditHairSaveData>(ConstantManager.FolderName_EditData, infoFilename);
					var hairExist = await StorageManager.UserDataCheckExistAsync(ConstantManager.FolderName_EditData, hairFilename);
					var faceExist = await StorageManager.UserDataCheckExistAsync(ConstantManager.FolderName_EditData, faceFilename);
					if (saveData != null &&
					   hairExist == StorageManager.ExistStatus.Exists &&
					   faceExist == StorageManager.ExistStatus.Exists)
					{
						await Navigation.PushAsync(new SavadDataSelectedPage(model.GVCThumbnailImgSource, infoFilename, hairFilename, faceFilename, imageFilename, saveData));
					}
				}
			}
			else if (PagePurpose == Purpose.SelectImage)
			{
				// 画像選択のとき
				var model = obj as GalleryViewCellModel;
				var imageSource = model.GVCThumbnailImgSource;

				// モーダルページ作成
				var modalView = new ModalView();
				modalView.modalViewViewModel.ImageSource = imageSource;
				modalView.modalViewViewModel.ImageAspect = Aspect.AspectFill;
				modalView.modalViewViewModel.ImageWidth = 270;
				modalView.modalViewViewModel.ImageHeight = 270;
				modalView.modalViewViewModel.ModalLabelTxt = "この写真でよろしいですか？";
				modalView.modalViewViewModel.YesButtonTxt = "はい";
				modalView.modalViewViewModel.NoButtonTxt = "いいえ";
				// TODO Xamarin.Forms.Device.RuntimePlatform is no longer supported. Use Microsoft.Maui.Devices.DeviceInfo.Platform instead. For more details see https://learn.microsoft.com/en-us/dotnet/maui/migration/forms-projects#device-changes
				double posY = (Device.RuntimePlatform == Device.Android) ? 0.2 : 0.31;
				modalView.modalViewViewModel.ImageRect = new Rect(0.5, posY, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize);
				modalView.modalViewViewModel.NomalModalLabelRect = new Rect(0.5, 0.71, 1, AbsoluteLayout.AutoSize);
				modalView.modalViewViewModel.SelectBtnLayoutBounds = new Rect(0.9, 0.77, 1, AbsoluteLayout.AutoSize);


				modalView.yesButton.Clicked += async (sender, e) =>
				{
					// 選択したイメージを送る。
					if (ImageSelected != null)
					{
						ImageSelected(this, imageSource);
					}

					// モーダルを閉じる
					await DialogManager.Instance.HideView();

					// ページを閉じる
					await this.Navigation.PopAsync(false);
				};


				modalView.noButton.Clicked += async (sender, e) =>
				{
                    // モーダルを閉じる
                    await DialogManager.Instance.HideView();
                };

                // モーダルを表示
                await DialogManager.Instance.ShowDialogView(modalView);
            }

			IsRunning = false;
		}
	}
}
