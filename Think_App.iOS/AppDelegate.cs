using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using FFImageLoading.Forms.Platform;
using Google.SignIn;
using UserNotifications;
using Firebase.CloudMessaging;
using System.Threading.Tasks;
using IO.Swagger.Model;
using Plugin.FacebookClient;

namespace Think_App.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate, IUNUserNotificationCenterDelegate, IMessagingDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            //NControls.Init();
            // FFImageLoading初期化
            CachedImageRenderer.Init();

            // QRコードスキャンに必要
            global::ZXing.Net.Mobile.Forms.iOS.Platform.Init();

            FacebookClientManager.Initialize(app, options);

            //SetNavigationBarStyle();
            // 画面サイズ取得
            double layoutW = UIScreen.MainScreen.Bounds.Width;
            double layoutH = UIScreen.MainScreen.Bounds.Height;

            LoadApplication(new App(layoutW, layoutH));
            SetUpNotification();
            return base.FinishedLaunching(app, options);
        }
        //void SetNavigationBarStyle()
        //{
        //    UINavigationBar.Appearance.BarTintColor = UIColor.FromRGB((nfloat)ColorList.colorMain.R, (nfloat)ColorList.colorMain.G, (nfloat)ColorList.colorMain.B);

        //    UINavigationBar.Appearance.SetTitleTextAttributes(new UITextAttributes()
        //    {
        //        TextColor = UIColor.FromRGB((nfloat)ColorList.colorNavibarTextColor.R, (nfloat)ColorList.colorNavibarTextColor.G, (nfloat)ColorList.colorNavibarTextColor.B),
        //    });
        //    UINavigationBar.Appearance.TintColor = UIColor.FromRGB((nfloat)ColorList.colorWhite.R, (nfloat)ColorList.colorWhite.G, (nfloat)ColorList.colorWhite.B);
        //}

        public override void OnActivated(UIApplication uiApplication)
        {
            base.OnActivated(uiApplication);
            FacebookClientManager.OnActivated();
        }

        string scheme = "thinkapp://";
        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            var openUrlOptions = new UIApplicationOpenUrlOptions(options);

            if (url.ToString().Contains(scheme))
            {
                System.Diagnostics.Debug.WriteLine("url  :" + url);

                var paramHost = url.Host;

                System.Diagnostics.Debug.WriteLine("paramHost  :" + paramHost);

                NavigateFromQRCode(paramHost);


            }
            var googleSignInHandled = SignIn.SharedInstance.HandleUrl(url, openUrlOptions.SourceApplication, openUrlOptions.Annotation);
            if (googleSignInHandled)
                return googleSignInHandled;

            return FacebookClientManager.OpenUrl(app, url, options);
        }


        // For iOS 8 and older
        public override bool OpenUrl(UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
        {
            if (url.ToString().Contains(scheme))
            {
                System.Diagnostics.Debug.WriteLine("url  :" + url);

                var paramHost = url.Host;

                System.Diagnostics.Debug.WriteLine("paramHost  :" + paramHost);
                NavigateFromQRCode(paramHost);
            }
            var googleSignInHandled = SignIn.SharedInstance.HandleUrl(url, sourceApplication, annotation);
            if (googleSignInHandled)
                return googleSignInHandled;

            return FacebookClientManager.OpenUrl(application, url, sourceApplication, annotation);
        }

        async void NavigateFromQRCode(string data)
        {
            //app未登録
            if (string.IsNullOrEmpty(Config.Instance.Data.deviceId))
            {
                System.Diagnostics.Debug.WriteLine("notRegisted");

                var dic = new Dictionary<string, string>() { { "uid", data } };

                try
                {
                    var resJson = await APIManager.GET("qrcode_data", dic);

                    var response = JsonManager.Deserialize<ResponseQRcodeData>(resJson);

                    await App.customNavigationPage.PushAsync(new QRcodeLogin(response));

                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("ex :" + ex);
                    await App.customNavigationPage.Navigation.NavigationStack.Last().DisplayAlert("読み込みエラー", "データを読み込むことができませんでした。", "OK");
                }

            }
        }

        public async void SetUpNotification()
        {
            Firebase.Core.App.Configure();
            System.Diagnostics.Debug.WriteLine("Messaging.SharedInstance.FcmToken :" + Messaging.SharedInstance.FcmToken);
            DeviceTokenManager.PostAndRegistDeviceToken(Messaging.SharedInstance.FcmToken);

            Messaging.SharedInstance.AutoInitEnabled = true;
            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                // For iOS 10 display notification (sent via APNS)
                UNUserNotificationCenter.Current.Delegate = this;

                var authOptions = UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound;
                UNUserNotificationCenter.Current.RequestAuthorization(authOptions, (granted, error) =>
                {
                    Console.WriteLine(granted);
                });
            }
            else
            {
                // iOS 9 or before
                var allNotificationTypes = UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound;
                var settings = UIUserNotificationSettings.GetSettingsForTypes(allNotificationTypes, null);
                UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
            }

            UIApplication.SharedApplication.RegisterForRemoteNotifications();

            Messaging.SharedInstance.Delegate = this;


            await Task.Run(async () =>
             {
                 if (!string.IsNullOrEmpty(Config.Instance.Data.deviceId))
                 {
                     try
                     {
                         var res = await APIManager.GET("deviceToken_check");

                         var param = JsonManager.Deserialize<ResponseDeviceTokenCheck>(res);

                         System.Diagnostics.Debug.WriteLine("res" + res);

                         System.Diagnostics.Debug.WriteLine("DeviceTokenInfo.Instance.DeviceToken  :" + DeviceTokenInfo.Instance.DeviceToken);

                         if (param.Data.IsInvalid == 1 || DeviceTokenInfo.Instance.IsRegistServer == false)
                         {
                             DeviceTokenManager.PostAndRegistDeviceToken(DeviceTokenInfo.Instance.DeviceToken);
                         }
                     }
                     catch (Exception ex)
                     {
                         System.Diagnostics.Debug.WriteLine("ex :" + ex);
                     }

                 }
             });
        }

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            System.Diagnostics.Debug.WriteLine(" apns token :" + deviceToken);
            Messaging.SharedInstance.ApnsToken = deviceToken;
        }

        //Tokenが登録された場合
        [Export("messaging:didReceiveRegistrationToken:")]
        public void DidReceiveRegistrationToken(Messaging messaging, string fcmToken)
        {
            Console.WriteLine($"Firebase registration token: {fcmToken}");
            //todo:トークンの保存or登録処理
            Task.Run(() => DeviceTokenManager.PostAndRegistDeviceToken(fcmToken));
        }

        //Tokenがリフレッシュされた場合
        public void DidRefreshRegistrationToken(Messaging messaging, string fcmToken)
        {
            Console.WriteLine($"Firebase refresh token: {fcmToken}");
            //todo:トークンの保存or登録処理
            Task.Run(() => DeviceTokenManager.PostAndRegistDeviceToken(fcmToken));
        }
        // iOS 9 <=, fire when recieve notification foreground
        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            try
            {
                Messaging.SharedInstance.AppDidReceiveMessage(userInfo);
                SetNotifiData(userInfo);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("ex :" + ex);
            }
        }

        public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
        {
            base.ReceivedRemoteNotification(application, userInfo);

            SetNotifiData(userInfo);

        }
        void SetNotifiData(NSDictionary userInfo, bool IsInit = false)
        {
            System.Diagnostics.Debug.WriteLine(userInfo);
            try
            {
                foreach (var n in userInfo)
                {
                    System.Diagnostics.Debug.WriteLine("Key  :" + n.Key.ToString());
                    System.Diagnostics.Debug.WriteLine("Value  :" + n.Value.ToString());

                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("ex  :" + ex);
            }
        }

        [Export("userNotificationCenter:willPresentNotification:withCompletionHandler:")]
        public void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            var title = notification.Request.Content.Title;
            var body = notification.Request.Content.Body;

            Messaging.SharedInstance.AppDidReceiveMessage(notification.Request.Content.UserInfo);
        }

        #region obsolute code
        //public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        //{
        //            var DeviceToken = deviceToken.Description;
        //            if (!string.IsNullOrWhiteSpace(DeviceToken))
        //            {
        //                DeviceToken = DeviceToken.Trim('<').Trim('>');
        //            }
        //            //dependencyではなくappdelegate内にこの処理をおかないと、プッシュ通知が届かない。
        //#if DEBUG
        //            Firebase.InstanceID.InstanceId.SharedInstance. SetApnsToken(deviceToken, Firebase.InstanceID.ApnsTokenType.Sandbox);
        //#else
        //			Firebase.InstanceID.InstanceId.SharedInstance.SetApnsToken(deviceToken, Firebase.InstanceID.ApnsTokenType.Prod);
        //#endif
        //            Firebase.InstanceID.InstanceId.Notifications.ObserveTokenRefresh((sender, e) =>
        //            {
        //                var newToken = Firebase.InstanceID.InstanceId.SharedInstance.Token;
        //                // if you want to send notification per user, use this token
        //                //fcmToken
        //                System.Diagnostics.Debug.WriteLine("FCM TOKEN :" + newToken);
        //                DeviceTokenManager.PostAndRegistDeviceToken(newToken);
        //                connectFCM();
        //            });
        //}
        //public static void connectFCM()
        //{
        //    Messaging.SharedInstance.Connect((error) =>
        //    {
        //        if (error == null)
        //        {
        //            Messaging.SharedInstance.Subscribe("/topics/all");
        //        }
        //        else
        //        {
        //            System.Diagnostics.Debug.WriteLine("  error    occured :  " + error);
        //        }
        //        System.Diagnostics.Debug.WriteLine(error != null ? "error occured" : "connect success");
        //    });
        //}
        #endregion
    }
}
