using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IO.Swagger.Model;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
	public partial class QRcodeLogin : ContentPage
	{
		QRcodeLoginViewModel qrcodeLoginViewModel;

		int errorCount = 0;
		// 実際にスキャンしているのはRegistrationTopのQRCodeScanClicked内
		public QRcodeLogin(ResponseQRcodeData data)
		{
			InitializeComponent();
			NavigationPage.SetBackButtonTitle(this, "");

			qrcodeLoginViewModel = new QRcodeLoginViewModel();

			this.LoginBtn.Clicked += async (sender, e) =>
			{
				if (App.ProcessManager.CanInvoke())
				{
					System.Diagnostics.Debug.WriteLine("EntryText:" + qrcodeLoginViewModel.CustomEntryTel.EntryText);
					System.Diagnostics.Debug.WriteLine("loginTel:" + data.Data.Tel);
					// 電話番号で別のシステムから登録情報取得してくる
					// RegistrationTopで取得した情報と入力された電話番号を使いログイン

					if (!string.Equals(data.Data.Tel, qrcodeLoginViewModel.CustomEntryTel.EntryText))
					{
						errorCount++;
						// 登録情報が不一致の場合(3回連続不一致)はモーダル出して新規登録画面へ
						if (errorCount <= 2)
						{
							await DisplayAlert("エラー", "電話番号が間違っている可能性があります" + Environment.NewLine + "もう一度入力しなおしてください", "確認");
						}
						else
						{
							//await DisplayAlert("エラー", "QRコードで認証ができませんでした。" + Environment.NewLine + "新規で登録を行なってください。", "確認");
							await ShowModalPage();
							//await App.customNavigationPage.PushAsync(new AccountRegistration(1));

							//qrcodeLoginViewModel.BindModalView.ModalViewLayoutBounds = new Rectangle(0, 0, 1, 1);
							//qrcodeLoginViewModel.BindModalView.ModalBgLayoutBounds = new Rectangle(0, 0, 1, 1);
							//qrcodeLoginViewModel.BindModalView.OKBtnLayoutBounds = new Rectangle(0.5, 0.58, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize);
							//qrcodeLoginViewModel.BindModalView.NomalModalLabelRect = new Rectangle(0.5, 0.38, 1, AbsoluteLayout.AutoSize);
							//this.BindingContext = qrcodeLoginViewModel;
							//((ModalView)this.ModalView).okButton.Clicked += (okBtnSender, okBtnE) =>
							//{
							//	if (App.ProcessManager.CanInvoke())
							//	{
							//		Navigation.PushAsync(new AccountRegistration());
							//		App.ProcessManager.OnComplete();
							//	}
							//};
						}
					}
					else
					{
						DependencyService.Get<IToast>().Show("認証しました");
						await Navigation.PushAsync(new AccountRegistration(1, null, null, data));
						// 登録情報が一致
						// 名前等の情報取得
						//Task.Run(async () =>
						//{
						//	await StaticMethod.AccountReg();
						//	await Navigation.PushAsync(new AccountRegistration());
						//});

					}
					App.ProcessManager.OnComplete();
				}
			};

			this.BindingContext = qrcodeLoginViewModel;
		}


		async Task ShowModalPage()
		{
			var modalView = new ModalView();

			modalView.modalViewViewModel.ModalLabelTxt = "登録情報と異なるため" + Environment.NewLine + "新規登録をしてください";
			modalView.modalViewViewModel.NomalModalLabelRect = new Rect(0.5, 0.4, 1, AbsoluteLayout.AutoSize);
			modalView.modalViewViewModel.OKBtnLayoutBounds = new Rect(0.5, 0.6, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize);

			var currentApp = Microsoft.Maui.Controls.Application.Current;

			modalView.okButton.Clicked += async (sender, e) =>
			{
				if (App.ProcessManager.CanInvoke())
				{
					await App.customNavigationPage.PopAsync();
					await App.customNavigationPage.PopAsync();
					await App.customNavigationPage.PushAsync(new AccountRegistration(1));
					await DialogManager.Instance.HideView();
					if (currentApp != Microsoft.Maui.Controls.Application.Current)
					{
						throw new InvalidOperationException("Application.Current changed");
					}
					App.ProcessManager.OnComplete();
				}
			};

			await DialogManager.Instance.ShowDialogView(modalView);
		}
	}
}
