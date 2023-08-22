using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Plugin.Media;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
	public partial class SelectPhotoSourcePage : ContentPage
	{
		const double _infoLblBaseFontSize = 20;

		SelectPhotoSourcePageModel Model { get; set; }
		bool NeedsRestartFadeImages { get; set; }

		public SelectPhotoSourcePage()
		{
			InitializeComponent();

			// このページに戻るときにタイトルを表示しない。
			NavigationPage.SetBackButtonTitle(this, "");

			Model = new SelectPhotoSourcePageModel()
			{
				ScreenSizeScale = ScaleManager.Scale,
				TakePhotoBtnCommand = new Command(TakePhotoBtn_Clicked),
				SelectFromGalleryBtnCommand = new Command(SelectFromGalleryBtn_Clicked),
				UseLastPhotoBtnCommand = new Command(UseLastPhotoBtn_Clicked),
				TakePhotoBtnEnable = DependencyService.Get<ICameraService>().IsCameraAvailable(),
				UseLastPhotoBtnEnable = false,
				InfoLblFontSize = _infoLblBaseFontSize * ScaleManager.Scale,
				BGFadeImageViewInfoList = GetFadeInfoList()
			};

			this.BindingContext = Model;

			Task.Run(async () =>
			{
				var result = await StorageManager.UserDataCheckExistAsync(ConstantManager.FolderName_Main, ConstantManager.FileName_LastImage);
				System.Diagnostics.Debug.WriteLine("前回の写真がある？: " + (result == StorageManager.ExistStatus.Exists));
				Device.BeginInvokeOnMainThread(async () =>
				{
					// 「前回の写真を使う」ボタン有効・無効処理
					Model.UseLastPhotoBtnEnable = (result == StorageManager.ExistStatus.Exists);
					// フェードスタート処理
					await this.BGFadeImageView.StartFadeImagesAsync();
				});
			});
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();

			if (NeedsRestartFadeImages)
			{
				await this.BGFadeImageView.StartFadeImagesAsync();
			}
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();

			this.BGFadeImageView.FinishFadeImages();
			NeedsRestartFadeImages = true;
		}

		List<FadeImageView.FadeInfo> GetFadeInfoList()
		{
			var list = new List<FadeImageView.FadeInfo>();
			list.Add(new FadeImageView.FadeInfo("bg_hairsimulation_01.png"));
			list.Add(new FadeImageView.FadeInfo("bg_hairsimulation_02.png"));
			list.Add(new FadeImageView.FadeInfo("bg_hairsimulation_03.png"));
			return list;
		}
		int resInt;

		async void TakePhotoBtn_Clicked()
		{
			if (!App.ProcessManager.CanInvoke())
			{
				return;
			}

			bool res = int.TryParse(Config.Instance.Data.nativeVersion.Substring(0, 1), out resInt);
			if (!res)
				return;
			// TODO Xamarin.Forms.Device.RuntimePlatform is no longer supported. Use Microsoft.Maui.Devices.DeviceInfo.Platform instead. For more details see https://learn.microsoft.com/en-us/dotnet/maui/migration/forms-projects#device-changes
			if (Device.RuntimePlatform == Device.Android && resInt >= 6)
			{
				DependencyService.Get<IScanerPermissionService>().Call();
			}
			else
			{
				await Navigation.PushAsync(new TakePhotoPage());
			}
			App.ProcessManager.OnComplete();
		}

		async void SelectFromGalleryBtn_Clicked()
		{
			if (!App.ProcessManager.CanInvoke())
			{
				return;
			}

			if (!CrossMedia.Current.IsPickPhotoSupported)
			{
				System.Diagnostics.Debug.WriteLine("写真のピッカーに対応していません！");
				await DisplayAlert("写真へのアクセスができません", "設定画面から写真へのアクセスを有効にしてください。", "閉じる");
				App.ProcessManager.OnComplete();
				return;
			}

			var file = await CrossMedia.Current.PickPhotoAsync();

			if (file == null)
			{
				App.ProcessManager.OnComplete();
				return;
			}

			System.Diagnostics.Debug.WriteLine("File Path:{0}", file.Path);

			var service = DependencyService.Get<IImageService>();
			var imageSource = service.GetOrientationAdjustedImageSource(file.Path);
			var size = await service.GetImageSizeAsync(imageSource);
			// ページ遷移
			await Navigation.PushAsync(new ImagePage(imageSource, size.Width, size.Height, true));
			App.ProcessManager.OnComplete();
		}

		async void UseLastPhotoBtn_Clicked()
		{
			if (!App.ProcessManager.CanInvoke())
			{
				return;
			}

			// 前回撮影した写真を取得。
			var imageSource = await ImageManager.LoadImageFromLocalStorageAsync(ConstantManager.FolderName_Main, ConstantManager.FileName_LastImage);
			if (imageSource == null)
			{
				App.ProcessManager.OnComplete();
				return;
			}

			var service = DependencyService.Get<IImageService>();
			var size = await service.GetImageSizeAsync(imageSource);
			// ページ遷移
			await Navigation.PushAsync(new ImagePage(imageSource, size.Width, size.Height, false));
			App.ProcessManager.OnComplete();
		}
	}
}
