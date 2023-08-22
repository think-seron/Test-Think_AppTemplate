using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
	public partial class TakePhotoMessagePage : ContentPage
	{
		public event EventHandler<ImageSource> TakenPhotoCompleted = delegate { };

		public TakePhotoMessagePage()
		{
			InitializeComponent();
			NavigationPage.SetBackButtonTitle(this, "");
			this.CameraView.PhotoTaken += CameraView_PhotoTaken;

			this.CameraController.TakePhotoButtonClicked += CameraController_TakePhotoButtonClicked;
			this.CameraController.SwitchCameraButtonClicked += CameraController_SwitchCameraButtonClicked;
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			// カメラプレビュースタート
			this.CameraView.IsPreviewing = true;
			Debug.WriteLine("カメラのプレビューをスタートしました。");
			this.CameraController.IsVisible = true;
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();

			// カメラプレビューエンド
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
			if (!App.ProcessManager.CanInvoke())
			{
				return;
			}

			if (e.ImageSource == null)
			{
				App.ProcessManager.OnComplete();
				return;
			}
			Debug.WriteLine("撮影成功！");

			// 一旦プレビュー切ります。
			this.CameraView.IsPreviewing = false;
			Debug.WriteLine("カメラのプレビューを中止しました。");

			// モーダルページ作成
			var modalView = new ModalView();
			modalView.modalViewViewModel.ImageSource = e.ImageSource;
			modalView.modalViewViewModel.ImageAspect = Aspect.AspectFill;
			modalView.modalViewViewModel.ImageWidth = 270;
			modalView.modalViewViewModel.ImageHeight = 270;
			modalView.modalViewViewModel.ModalLabelTxt = "この写真をメッセージで送りますか？";
			modalView.modalViewViewModel.YesButtonTxt = "送る";
			modalView.modalViewViewModel.NoButtonTxt = "再撮影";
			// TODO Xamarin.Forms.Device.RuntimePlatform is no longer supported. Use Microsoft.Maui.Devices.DeviceInfo.Platform instead. For more details see https://learn.microsoft.com/en-us/dotnet/maui/migration/forms-projects#device-changes
			double posY = (Device.RuntimePlatform == Device.Android) ? 0.2 : 0.31;
			modalView.modalViewViewModel.ImageRect = new Rect(0.5, posY, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize);
			modalView.modalViewViewModel.NomalModalLabelRect = new Rect(0.5, 0.71, 1, AbsoluteLayout.AutoSize);
			modalView.modalViewViewModel.SelectBtnLayoutBounds = new Rect(0.9, 0.77, 1, AbsoluteLayout.AutoSize);

			modalView.yesButton.Clicked += async (__, _) =>
			{
				// モーダルを閉じる
				await DialogManager.Instance.HideView();
				App.ProcessManager.OnComplete();

				if (TakenPhotoCompleted != null)
				{
					// 写真撮影情報をイベントとして通知する。
					TakenPhotoCompleted(this, e.ImageSource);
				}
			};

			modalView.noButton.Clicked += async (__, _) =>
			{
                // モーダルを閉じる
                await DialogManager.Instance.HideView();
                App.ProcessManager.OnComplete();

				// プレビュー再開
				this.CameraView.IsPreviewing = true;
				Debug.WriteLine("カメラのプレビューを再開しました。");
			};

            // モーダルを表示
            await DialogManager.Instance.ShowDialogView(modalView);
        }
	}
}
