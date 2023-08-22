using System;
using Think_App.iOS;
using Think_App;
using Google.SignIn;
using Foundation;
using UIKit;
using System.Collections.Generic;
using System.Linq;
using IO.Swagger.Model;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform;

[assembly: Dependency(typeof(GoogleSignInService_iOS))]

namespace Think_App.iOS
{
    public class GoogleSignInService_iOS : IGoogleSignInService
    {
        SNS_LoginStates loginState = SNS_LoginStates.Error;
        public void SignIn()
        {
            try
            {
                Google.SignIn.SignIn.SharedInstance.PresentingViewController = GetVisibleViewController();
                var googleServiceDictionary = NSDictionary.FromFile("GoogleService-Info.plist");
                Google.SignIn.SignIn.SharedInstance.ClientID = googleServiceDictionary["CLIENT_ID"].ToString();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(" ex  :" + ex);
            }

            Google.SignIn.SignIn.SharedInstance.SignedIn += async (sender, e) =>
            {
                System.Diagnostics.Debug.WriteLine("SnsAccountSelectViewModel.SnsLoginState  1:" + SnsAccountSelectViewModel.SnsLoginState);

                if (e.Error != null)
                {
                    System.Diagnostics.Debug.Write("sign in error code :" + e.Error.Code + " Error Domain " + e.Error.Domain);
                    if (e.Error.UserInfo != null)
                    {
                        System.Diagnostics.Debug.WriteLine(" e error userinfo  ");
                        foreach (var n in e.Error.UserInfo)
                        {
                            System.Diagnostics.Debug.WriteLine(" key  " + n.Key);
                            System.Diagnostics.Debug.WriteLine(" value  " + n.Value);
                        }
                    }
                }
                // ここにログイン後の処理を記述します。
                if (e.User != null && e.Error == null)
                {

                    //ここにログイン成功時の処理を記述してください
                    System.Diagnostics.Debug.WriteLine("Signed in user: {0}", e.User.Profile.Name);
                    foreach (var n in e.User.GrantedScopes)
                    {
                        System.Diagnostics.Debug.WriteLine(" scope   " + n);
                    }
                    System.Diagnostics.Debug.WriteLine("SnsAccountSelectViewModel.SnsLoginState  2:" + SnsAccountSelectViewModel.SnsLoginState);

                    try
                    {
                        System.Diagnostics.Debug.WriteLine(" " + e.User.UserID);
                        System.Diagnostics.Debug.WriteLine(" " + e.User.HostedDomain);
                        System.Diagnostics.Debug.WriteLine("Email :" + e.User.Profile.Email);
                        System.Diagnostics.Debug.WriteLine(" " + e.User.Profile.FamilyName);
                        System.Diagnostics.Debug.WriteLine(" Name :" + e.User.Profile.Name);
                        System.Diagnostics.Debug.WriteLine(" " + e.User.Profile.GivenName);
                        System.Diagnostics.Debug.WriteLine(" " + e.User.Profile.HasImage);

                        System.Diagnostics.Debug.WriteLine(" UserID : " + e.User.UserID);
                        var toast = new ToastView();


                        toast.Show(UIApplication.SharedApplication.KeyWindow.RootViewController.View, "認証しました。");

                        var udata = new UserData()
                        {
                            name = e.User.Profile.Name,

                            email = e.User.Profile.Email,

                            google_id = e.User.UserID
                        };

                        System.Diagnostics.Debug.WriteLine("SnsAccountSelectViewModel.SnsLoginState  3:" + SnsAccountSelectViewModel.SnsLoginState);
                        loginState = SnsAccountSelectViewModel.SnsLoginState;
                        System.Diagnostics.Debug.WriteLine("  loginState  :" + loginState);
                        switch (loginState)
                        {
                            case SNS_LoginStates.Registoration:
                                Device.BeginInvokeOnMainThread(async () =>
                                        {
                                            System.Diagnostics.Debug.Write("  get udata from google" + udata.email);
                                            await App.customNavigationPage.PushAsync(new AccountRegistration(1, udata));
                                        });
                                SignOut();
                                Disconnect();
                                break;
                            case SNS_LoginStates.TakeOver:

                                var dic = new Dictionary<string, string>()
                                {
                                    {"googleId", udata.google_id},
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
                                // TODO Xamarin.Forms.Device.RuntimePlatform is no longer supported. Use Microsoft.Maui.Devices.DeviceInfo.Platform instead. For more details see https://learn.microsoft.com/en-us/dotnet/maui/migration/forms-projects#device-changes
                                if (Device.RuntimePlatform == Device.Android)
                                    DeviceTokenManager.PostAndRegistDeviceToken(DeviceTokenInfo.Instance.DeviceToken);
                                apiName = "home";
                                var resHomeJson = await APIManager.GET(apiName);

                                var param = JsonManager.Deserialize<ResponseHome>(resHomeJson);

                                System.Diagnostics.Debug.WriteLine("Home:" + resHomeJson);
                                PushNotificationService_iOS.InitializeNotification();
                                Device.BeginInvokeOnMainThread(async () =>
                                    {
                                        await App.customNavigationPage.PushAsync(new Home(param), false);
                                    });
                                SignOut();
                                Disconnect();
                                break;
                            default:
                                await App.customNavigationPage.Navigation.NavigationStack.Last().DisplayAlert("エラー", "データ取得に失敗しました。改めてお試しください。", "戻る");
                                await App.customNavigationPage.PopAsync();
                                break;
                        }


                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("  success ex  : " + ex);
                    }
                }
            };

            Google.SignIn.SignIn.SharedInstance.Disconnected += (sender, e) =>
            {
                //ここに切断時の処理を記述してください
                System.Diagnostics.Debug.WriteLine("Disconnected user");
                if (e.Error != null)
                {
                    System.Diagnostics.Debug.Write("sign in error code :" + e.Error.Code + " Error Domain " + e.Error.Domain);
                    if (e.Error.UserInfo != null)
                    {
                        System.Diagnostics.Debug.WriteLine(" e error userinfo  ");
                        foreach (var n in e.Error.UserInfo)
                        {
                            System.Diagnostics.Debug.WriteLine(" key  " + n.Key);
                            System.Diagnostics.Debug.WriteLine(" value  " + n.Value);
                        }
                    }
                }
            };

            //自動サイレントログイン
            //Google.SignIn.SignIn.SharedInstance.SignInUserSilently();
            //手動ログイン
            try
            {
                Google.SignIn.SignIn.SharedInstance.SignInUser();
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("ex :" + ex);
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await App.Current.MainPage.DisplayAlert("Googleとの連携が正常にできませんでした。", null, "OK");
                });
            }
        }

        public void SignOut()
        {
            System.Diagnostics.Debug.WriteLine("sign out ");
            Google.SignIn.SignIn.SharedInstance.SignOutUser();
        }

        public void Disconnect()
        {
            System.Diagnostics.Debug.WriteLine("disconnect ");
            Google.SignIn.SignIn.SharedInstance.DisconnectUser();
        }
        /// <summary>
        /// Google Sign In 4.4.0→5.0.1.1に変更時にdelegateクラスを作成からviewControllerの設定に変更された
        /// </summary>
        /// <returns></returns>
        UIViewController GetVisibleViewController()
        {
            var rootController = UIApplication.SharedApplication.KeyWindow.RootViewController;

            if (rootController.PresentedViewController == null)
                return rootController;

            if (rootController.PresentedViewController is UINavigationController)
            {
                return ((UINavigationController)rootController.PresentedViewController).VisibleViewController;
            }

            if (rootController.PresentedViewController is UITabBarController)
            {
                return ((UITabBarController)rootController.PresentedViewController).SelectedViewController;
            }

            return rootController.PresentedViewController;
        }

    }
}