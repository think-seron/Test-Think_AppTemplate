using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using IO.Swagger.Model;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
	public partial class OAuthFaceBookPage : ContentPage
	{

		const string fb_Oauth_URL = "https://m.facebook.com/dialog/oauth";
		const string fb_AppId = "@@@Facebook_AppId";
		const string fb_AppSecret = "@@@Facebook_AppSecret";
		static string fb_Redirect_URL = APIManager.BaseDomain() + "facebook";
		SNS_LoginStates loginState = SNS_LoginStates.Error;
		public OAuthFaceBookPage()
		{
			InitializeComponent();
			NavigationPage.SetBackButtonTitle(this, "");
			loginState = SnsAccountSelectViewModel.SnsLoginState;

			this.OAuthView.Source = string.Format(@"https://m.facebook.com/dialog/oauth?client_id={0}&redirect_uri={1}&response_type=token",
			fb_AppId,
			WebUtility.UrlEncode(@fb_Redirect_URL));


			this.OAuthView.Navigating += async (sender, e) =>
			{
				System.Diagnostics.Debug.WriteLine("navigation url  :" + e.Url);
				if (e.Url.StartsWith(@fb_Redirect_URL))
				{



					System.Diagnostics.Debug.WriteLine("success");
					DependencyService.Get<IToast>().Show("認証しました。");
					e.Cancel = true;

					if (App.ProcessManager.CanInvoke())
					{
						App.customNavigationPage.IsRunning = true;


						var uri = new Uri(e.Url);
						if (!string.IsNullOrEmpty(uri.Fragment) && uri.Fragment.StartsWith("#access_token"))
						{

							var token = uri.Fragment.Split('&').First().Split('=').LastOrDefault();

							var client = new HttpClient();

							var result = await client.GetAsync(string.Format("https://graph.facebook.com/v3.0/me?fields=id%2Cname&access_token={0}", token));

							if (result == null)
							{
								await DisplayAlert("通信エラー", "ユーザーデータを取得できませんでした。通信環境の良い場所で改めてお試しください。", "戻る");
								await App.customNavigationPage.PopAsync();

								App.customNavigationPage.IsRunning = false;
								App.ProcessManager.OnComplete();
								return;
							}
							App.ProcessManager.OnComplete();
							var json = await result.Content.ReadAsStringAsync();

							System.Diagnostics.Debug.WriteLine("json :" + json);

							try
							{
								var profile = JsonManager.Deserialize<FaceBookPersonalData>(json);

								System.Diagnostics.Debug.WriteLine(profile);

								var uData = new UserData()
								{
									name = profile.name,
									gender = profile.gender,
									fb_id = profile.id
								};


								switch (loginState)
								{
									case SNS_LoginStates.Registoration:
										await App.customNavigationPage.PushAsync(new AccountRegistration(1, uData));

										break;
									case SNS_LoginStates.TakeOver:

										var dic = new Dictionary<string, string>()
										{
											{"facebookId", uData.fb_id},
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
											await DisplayAlert("エラー", "データ取得に失敗しました。改めてお試しください。", "戻る");
											await App.customNavigationPage.PopAsync();
											App.customNavigationPage.IsRunning = false;
											return;
										}

										ret = await StaticMethod.DeviceIDReg(resUserData.Data.DeviceId, resUserData.Data.TransferId);
										if (ret != "OK")
										{
											await DisplayAlert("エラー", "データ取得に失敗しました。改めてお試しください。", "戻る");
											await App.customNavigationPage.PopAsync();
											App.customNavigationPage.IsRunning = false;
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
									default:
										await DisplayAlert("エラー", "データ取得に失敗しました。改めてお試しください。", "戻る");
										await App.customNavigationPage.PopAsync();
										break;
								}

							}
							catch (Exception ex)
							{
								App.ProcessManager.OnComplete();
								App.customNavigationPage.IsRunning = false;

								var profile = JsonConvert.DeserializeObject(json);

								System.Diagnostics.Debug.WriteLine("fb login ex :" + ex);

								await DisplayAlert("通信エラー", "ユーザーデータを取得できませんでした。通信環境の良い場所で改めてお試しください。", "戻る");
								await App.customNavigationPage.PopAsync();
							}
						}
						else
						{
							await DisplayAlert("エラー", "認証に失敗しました", "OK");
						}

						App.customNavigationPage.IsRunning = false;
						App.ProcessManager.OnComplete();
					}
				}
			};
		}

		async Task ManageUser(UserData uData)
		{

		}
	}
	public class UserData
	{

		public string name { get; set; }

		public string fb_id { get; set; }
		public string line_id { get; set; }
		public string google_id { get; set; }
		public string apple_id { get; set; }

		public string email { get; set; }

		public string gender { get; set; }
		public string tel { get; set; }
	}


	public class FaceBookPersonalData
	{
		public string name { get; set; }
		public string id { get; set; }
		public string gender { get; set; }
	}

}
