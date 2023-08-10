using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using Xamarin.Forms;
using Newtonsoft.Json.Linq;
using IO.Swagger.Model;

namespace Think_App
{
	public class SnsAccountSelectViewModel : ViewModelBase
	{


		const string fb_Oauth_URL = "https://m.facebook.com/dialog/oauth";
		const string fb_AppId = "123537551608738";
		const string fb_AppSecret = "1151e7a1369b618ee4c19b91d84ce0e9";
		const string fb_Redirect_URL = "https://www.facebook.com/connect/login_success.html";

		//loginstate
		//error 0, 登録時 1, 引き継ぎ 2
		public static SNS_LoginStates SnsLoginState = SNS_LoginStates.Error;



		public SnsAccountSelectViewModel(int transitionSource)
		{

			switch (transitionSource)
			{
				case 1:
					SnsLoginState = SNS_LoginStates.Registoration;
					break;
				case 2:
					SnsLoginState = SNS_LoginStates.TakeOver;
					break;
				default:
					SnsLoginState = SNS_LoginStates.Error;
					break;
			}
			ButtonWidth = 224 * ScaleManager.Scale;
			ButtonHeight = 50 * ScaleManager.Scale;
			ButtonFontSize = 18 * ScaleManager.Scale;
			ScreenSizeScale = ScaleManager.Scale;
			CustomNavibarBC = new CustomNavigationBarViewModel(null, CustomNavigationBarViewModel.LeftBtnKinds.BackBtn, CustomNavigationBarViewModel.RightBtnKinds.None, null);
			CustomNavibarBC.LeftBtnClicked = new Command(() => { ScreenTransition(); });

			facebookCommand = new Command(async () =>
		   {
			   if (App.ProcessManager.CanInvoke())
			   {
				   App.customNavigationPage.IsRunning = true;
				   await FacebookLoginManager.LoginAsync();
				   App.customNavigationPage.IsRunning = false;
				   App.ProcessManager.OnComplete();
			   }
		   });

			GoogleCommand = new Command(() =>
		   {
			   if (App.ProcessManager.CanInvoke())
			   {
				   App.customNavigationPage.IsRunning = true;
				   DependencyService.Get<IGoogleSignInService>().SignIn();
				   App.customNavigationPage.IsRunning = false;
				   App.ProcessManager.OnComplete();
			   }
		   });


			LineCommand = new Command(async () =>
		{
			if (App.ProcessManager.CanInvoke())
			{
				App.customNavigationPage.IsRunning = true;
				await App.customNavigationPage.PushAsync(new OAuthLinePage());
				App.customNavigationPage.IsRunning = false;
				App.ProcessManager.OnComplete();
			}
		});

			AppleCommand = new Command(async () =>
			{
				if (App.ProcessManager.CanInvoke())
				{
					App.customNavigationPage.IsRunning = true;
					if (IsAppleSignInAvailable)
					{
						var account = await _appleSignInService.SignInAsync();
						if (account != null)
						{
							switch (SnsLoginState)
							{
								case SNS_LoginStates.Registoration:
							        var userData = new UserData()
							        {
								        name = account.Name,
								        email = account.Email,
								        apple_id = account.UserId
							        };
							        await App.customNavigationPage.PushAsync(new AccountRegistration(1, userData));
									break;
								case SNS_LoginStates.TakeOver:

									var dic = new Dictionary<string, string>()
								    {
									    {"appleId",account.UserId},
								    };
									string apiName = "snsId_login";
									var resJson = await APIManager.Post(apiName, dic);
									if (resJson == null)
										break;
									var resUserData = JsonManager.Deserialize<ResponseLogin>(resJson);
									string ret = await StaticMethod.AccountReg(
															resUserData.Data.Name,
															resUserData.Data.Phonetic,
															resUserData.Data.Tel,
															resUserData.Data.Mail,
															(int)resUserData.Data.Sex
														);
									if (ret != "OK")
									{
										await App.customNavigationPage.Navigation.NavigationStack.Last().DisplayAlert("エラー", "データ取得に失敗しました。改めてお試しください。", "戻る");
										await App.customNavigationPage.PopAsync();
										App.customNavigationPage.IsRunning = false;
										App.ProcessManager.OnComplete();
										return;
									}

									ret = await StaticMethod.DeviceIDReg(resUserData.Data.DeviceId, resUserData.Data.TransferId);
									if (ret != "OK")
									{
										await App.customNavigationPage.Navigation.NavigationStack.Last().DisplayAlert("エラー", "データ取得に失敗しました。改めてお試しください。", "戻る");
										await App.customNavigationPage.PopAsync();
										App.customNavigationPage.IsRunning = false;
										App.ProcessManager.OnComplete();
										return;
									}
									DeviceTokenManager.PostAndRegistDeviceToken(DeviceTokenInfo.Instance.DeviceToken);
									apiName = "home";
									var resHomeJson = await APIManager.GET(apiName);

									var param = JsonManager.Deserialize<ResponseHome>(resHomeJson);

									System.Diagnostics.Debug.WriteLine("Home:" + resHomeJson);

									Device.BeginInvokeOnMainThread(async () =>
									{
										await App.customNavigationPage.PushAsync(new Home(param), false);
									});

									break;
						    };
						}
					}
					App.customNavigationPage.IsRunning = false;
					App.ProcessManager.OnComplete();
				}
			});
		}

		public double ScreenSizeScale { get; set; }
		public FormattedString CustomFormattedText { get; set; }

		CustomNavigationBarViewModel _CustomNavibarBC;
		public CustomNavigationBarViewModel CustomNavibarBC
		{
			get { return _CustomNavibarBC; }
			set
			{
				if (_CustomNavibarBC != value)
				{
					_CustomNavibarBC = value;
					OnPropertyChanged("CustomNavibarBC");
				}
			}
		}

		public Command facebookCommand { get; set; }
		public Command GoogleCommand { get; set; }
		public Command LineCommand { get; set; }
        public Command AppleCommand { get; set; }

		IAppleSignInService _appleSignInService = DependencyService.Get<IAppleSignInService>();
        public bool IsAppleSignInAvailable => Device.RuntimePlatform == Device.iOS && _appleSignInService.IsAvailable;

        public Thickness TextMargin => new Thickness(0, 0, 0, 13 * ScreenSizeScale);
		public Thickness SNSButtonMargin => new Thickness(0, 14 * ScreenSizeScale, 0, 0);
		public Thickness BottomMargin => new Thickness(0, 0, 0, 51 * ScreenSizeScale);

		public double ButtonWidth { get; set; }
		public double ButtonHeight { get; set; }
		public double ButtonFontSize { get; set; }
	}

	public enum SNS_LoginStates
	{
		Error,
		Registoration,
		TakeOver
	}
}
