using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IO.Swagger.Model;
using Plugin.FacebookClient;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
    public class FacebookLoginManager
    {
        public static async Task LoginAsync()
        {
            var userData = await CrossFacebookClient.Current.RequestUserDataAsync(new string[] { "id", "name" }, new string[] { "public_profile" });
			if (userData.Status == FacebookActionStatus.Canceled)
				return;

			if (userData.Status != FacebookActionStatus.Completed)
            {
                await App.Current.MainPage.DisplayAlert("通信エラー", "ユーザーデータを取得できませんでした。通信環境の良い場所で改めてお試しください。", "戻る");
				return;
            }
            var data = GetUserData(userData.Data);

            await HandleLoginStatesAsync(data);
        }

        static UserData GetUserData(string userData)
        {
            var profile = JsonManager.Deserialize<FaceBookPersonalData>(userData);
            return new UserData()
            {
                name = profile.name,
                gender = profile.gender,
                fb_id = profile.id
            };
        }

        static async Task HandleLoginStatesAsync(UserData uData)
        {
            var loginState = SnsAccountSelectViewModel.SnsLoginState;
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
						await App.Current.MainPage.DisplayAlert("エラー", "データ取得に失敗しました。改めてお試しください。", "戻る");
						App.customNavigationPage.IsRunning = false;
						return;
					}

					ret = await StaticMethod.DeviceIDReg(resUserData.Data.DeviceId, resUserData.Data.TransferId);
					if (ret != "OK")
					{
						await App.Current.MainPage.DisplayAlert("エラー", "データ取得に失敗しました。改めてお試しください。", "戻る");
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
					await App.Current.MainPage.DisplayAlert("エラー", "データ取得に失敗しました。改めてお試しください。", "戻る");
					break;
			}
        }
    }
}
