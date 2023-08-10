using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Util;
using FFImageLoading.Forms.Platform;
using Plugin.Permissions;
using Android.Gms.Common;
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Auth.Api;
using Android.Gms.Common.Apis;
using Xamarin.Forms;
using System.Linq;
using IO.Swagger.Model;
using System.Collections.Generic;
using Firebase.Iid;
using Firebase.Messaging;
using Android.Support.V4.App;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Platform.Android;
using Plugin.FacebookClient;

namespace Think_App.Droid
{
    // 下記のThemeのところでsplashの画像のあるstyleを指定しておき
    // OnCreate内のbase.SetThemeでsplashの画像のないstyleに指定し直すことで
    // アプリ内でページ遷移した際にsplash画像がちらつくのを防止
    [Activity(Label = "@@@appName", Icon = "@drawable/icon", Theme = "@style/MyTheme.Splash", MainLauncher = true, Exported = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    //[Activity(Icon = "@drawable/icon", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    [IntentFilter(
          actions: new[] { Intent.ActionView }
        , Categories = new[]
        { Intent.CategoryDefault, Intent.CategoryBrowsable, Intent.CategoryAppBrowser }
            , DataHost = "example.jp"
            , DataScheme = "@@@schemeName"//アプリによってスキーム名変更
     )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity,
        GoogleApiClient.IConnectionCallbacks, GoogleApiClient.IOnConnectionFailedListener, ActivityCompat.IOnRequestPermissionsResultCallback
    {
        public static Activity activity;
        public const int RC_SIGN_IN = 9001;
        public static Context context;
        public static AndroidX.AppCompat.Widget.Toolbar ToolBar { get; private set; }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.SetTheme(Resource.Style.MyTheme);

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, savedInstanceState);
            Plugin.InputKit.Platforms.Droid.Config.Init(this, savedInstanceState);
            FirebaseAnalyticsService.CreateInstance(this);
            FacebookClientManager.Initialize(this);

            activity = this;
            context = this;
            //NControls.Init();
            // FFImageLoading初期化
            CachedImageRenderer.Init(enableFastRenderer: true);
            // ステータスバーサイズ取得
            int statusBarHeight = 0;
            int totalHeight = 0;
            int contentHeight = 0;
            int resourceId = Resources.GetIdentifier("status_bar_height", "dimen", "android");
            if (resourceId > 0)
            {
                statusBarHeight = Resources.GetDimensionPixelSize(resourceId);

                totalHeight = Resources.DisplayMetrics.HeightPixels;
                contentHeight = totalHeight - statusBarHeight;
            }
            var styledAttributes = this.Theme.ObtainStyledAttributes(
            new int[] { Android.Resource.Attribute.ActionBarSize });
            var actionbar = (int)styledAttributes.GetDimension(0, 0);
            styledAttributes.Recycle();
            //}
            // 画面サイズ取得
            Android.Graphics.Point size = new Android.Graphics.Point();
            WindowManager.DefaultDisplay.GetSize(size);
            DisplayMetrics metrics = new DisplayMetrics();
            WindowManager.DefaultDisplay.GetMetrics(metrics);
            double width = (double)size.X / (double)metrics.Density;
            double height = (double)size.Y / (double)metrics.Density;
            if (contentHeight >= 0)
            {
                height = (double)contentHeight / (double)metrics.Density;
            }
            double toolbarH = (double)actionbar / (double)metrics.Density;
            //プッシュ通知取得時にここにデータが
            if (this.Intent.Data != null)
            {
                System.Diagnostics.Debug.WriteLine("this.Intent.Data  :" + this.Intent.Data);
                GASCall.Track_App_Page("Android_プッシュ通知_");
            }
            Task.Run(() =>
            {
                try
                {
#if DEBUG
                    //var instanceid = FirebaseInstanceId.Instance;
                    //var new_token = instanceid.GetToken(MainActivity.context.GetString(Resource.String.gcm_defaultSenderId), Firebase.Messaging.FirebaseMessaging.InstanceIdScope);
                    //Android.Util.Log.Debug("TAG", "{0} {1}", instanceid.Token, new_token);
                    //System.Diagnostics.Debug.WriteLine("new_token" + new_token);
                    //FirebaseMessaging.Instance.SubscribeToTopic("all");
#else
                    FirebaseMessaging.Instance.SubscribeToTopic("all");
#endif
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("ex :" + ex);
                }
            });
            Task.Run(async () =>
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
                            //DeviceTokenInfo.Instance.DeviceToken = FirebaseInstanceId.Instance.Token;
                            //DeviceTokenManager.PostAndRegistDeviceToken(DeviceTokenInfo.Instance.DeviceToken);
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("ex :" + ex);
                    }
                }
            });


            LoadApplication(new App(width, height, (double)metrics.Density));
            try
            {
#if DEBUG
                ////Google SignInテスト用
                //iGoogleTestService_Droid.GoogleApiClient = new GoogleApiClient.Builder(this)
                //															.AddConnectionCallbacks(this)
                //															.AddOnConnectionFailedListener(this)
                //															.AddApi(PlusClass.API)
                //															.AddScope(PlusClass.ScopePlusLogin)
                //															.Build();
                ///Google SignInリリースと同じもの
                GoogleSignInOptions gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn)
                .RequestProfile()
                                                             .RequestEmail()
                .Build();

                GoogleSignInService_Droid.GoogleApiClient = new GoogleApiClient.Builder(this)
                    .AddConnectionCallbacks(this)
                                                        .AddOnConnectionFailedListener(this)
                                                        .AddApi(Auth.GOOGLE_SIGN_IN_API, gso)
                                                        .Build();
#else
                GoogleSignInOptions gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn)
                .RequestProfile()
                                                             .RequestEmail()
                .Build();

                GoogleSignInService_Droid.GoogleApiClient = new GoogleApiClient.Builder(this)
                                        .AddConnectionCallbacks(this)
                                        .AddOnConnectionFailedListener(this)
                                        .AddApi(Auth.GOOGLE_SIGN_IN_API, gso)
                                        .Build();
#endif

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message + System.Environment.NewLine + ex.StackTrace);
            }

            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                // Webviewで入力箇所がキーボードに隠れないようにするのに必要な処理
                try
                {
                    Window.DecorView.SystemUiVisibility = 0;
                    var statusBarHeightInfo = typeof(Xamarin.Forms.Platform.Android.FormsAppCompatActivity).GetField("_statusBarHeight", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                    statusBarHeightInfo.SetValue(this, 0);
                    Window.ClearFlags(WindowManagerFlags.TranslucentStatus);
                    Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
                    var statusBarColor = Xamarin.Forms.Color.FromHex("#2196F3").ToAndroid();
                    Window.SetStatusBarColor(statusBarColor);
                }
                catch (Exception ex)
                {
                }
            }
            // Webviewで入力箇所がキーボードに隠れないようにするのに必要な処理

            App.Current.On<Xamarin.Forms.PlatformConfiguration.Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);
            SetChannel();
        }
        public static bool BackButtonDisable;

        private void SetChannel()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O) return;

            var notificationManager = Android.App.NotificationManager.FromContext(this);

            NotificationChannel channel = new NotificationChannel(
              // 一意のチャンネルID
              // ここはどこかで定数にしておくのが良さそう
              Think_App.Droid.FireBaseMassagingService.ChannelId,
                 // 設定に表示されるチャンネル名
                 // ここは実際にはリソースを指定するのが良さそう
                 "店舗からのお知らせ",
                // チャンネルの重要度
                // 重要度によって表示箇所が異なる
                NotificationImportance.Default
            );
            // 通知時にライトを有効にする
            channel.EnableLights(true);
            // 通知時のライトの色
            // ロック画面での表示レベル
            channel.LockscreenVisibility = NotificationVisibility.Public;
            // チャンネルの登録
            notificationManager.CreateNotificationChannel(channel);
        }

        public override void OnTrimMemory(TrimMemory level)
        {
            //FFImageLoading.ImageService.Instance.InvalidateMemoryCache();
            GC.Collect();
            base.OnTrimMemory(level);
        }

        public override Android.Content.Res.Resources Resources
        {
            get
            {
                var config = new Android.Content.Res.Configuration();
                config.SetToDefaults();
                return CreateConfigurationContext(config)?.Resources;
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            if (App.customNavigationPage.CurrentPage is RegistrationTop)
            {
                foreach (var m in permissions)
                {
                    if ((grantResults[0] == Permission.Granted) && (m == "android.permission.CAMERA"))
                    {
                        var customOverlay = new ScanView();
                        //var scanPage = new ZXingScannerPage(null, customOverlay);
                        var scanPage = new QRcodeScan(null, customOverlay);

                        // スキャナページを表示
                        App.customNavigationPage.PushAsync(scanPage);
                        App.ProcessManager.OnComplete();
                        // データが取れると発火
                        scanPage.OnScanResult += (result) =>
                        {
                            System.Diagnostics.Debug.WriteLine("OnScanResult result   " + result);

                            // スキャン停止
                            scanPage.IsScanning = false;
                            scanPage.IsAnalyzing = false;
                            scanPage.IsEnabled = false;
                            // PopAsyncで元のページに戻り、結果をダイアログで表示
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                if (App.ProcessManager.CanInvoke())
                                {
                                    string ret = result.Text;
                                    int num = ret.IndexOf("&uid=");
                                    //http://think.entap.works/introduction/page.html?appid=jp.comcomcompany.ThinkApp&uid=%20JL2Hhm7g
                                    //think.entap.works/introduction/
                                    //yoyaku.sipss.jp/app
                                    if (ret.IndexOf("yoyaku.sipss.jp/app") > 0 && num > 0)
                                    {
                                        string uid = ret.Substring(num + 5);
                                        await App.customNavigationPage.CurrentPage.DisplayAlert("読み込み完了", null, "OK");

                                        var dic = new Dictionary<string, string>() { { "uid", uid } };
                                        try
                                        {
                                            var resJson = await APIManager.GET("qrcode_data", dic);
                                            var response = JsonManager.Deserialize<ResponseQRcodeData>(resJson);
                                            if (response != null && response.Data != null)
                                            {
                                                await App.customNavigationPage.PushAsync(new QRcodeLogin(response));
                                            }
                                            else
                                            {
                                                await App.customNavigationPage.CurrentPage.DisplayAlert("データを取得できませんでした。", response.Message, "OK");
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            System.Diagnostics.Debug.WriteLine("ex :" + ex);
                                        }
                                    }
                                    else
                                    {
                                        await App.customNavigationPage.CurrentPage.DisplayAlert("読み込み完了", "無効なバーコードです", "OK");
                                        await App.customNavigationPage.PopAsync();
                                    }
                                    // xamarinのバグ？ Navigation.RemovePage原因で落ちてしまう
                                    //Navigation.RemovePage(scanPage); // スキャンするページには戻らないようNavigationのstackから消しておく
                                    App.ProcessManager.OnComplete();
                                }
                            });
                            //};
                        };
                    }


                }

            }
            else if (App.customNavigationPage.CurrentPage is SelectPhotoSourcePage)
            {
                foreach (var m in permissions)
                {
                    if ((grantResults[0] == Permission.Granted) && (m == "android.permission.CAMERA"))
                    {
                        App.customNavigationPage.PushAsync(new TakePhotoPage());
                    }
                }

            }
            global::ZXing.Net.Mobile.Android.PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            if (ToolBar == null)
            {
                ToolBar = FindViewById<AndroidX.AppCompat.Widget.Toolbar>(Resource.Id.toolbar);
                SetSupportActionBar(ToolBar);
                if (SupportActionBar != null)
                {
                    SupportActionBar.SetDisplayShowHomeEnabled(true);
                }
            }
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            // これがバックボタンのIdです。
            if (item.ItemId == 16908332)
            {
                try
                {
                    var count = Xamarin.Forms.Application.Current.MainPage.Navigation.NavigationStack.Count;
                    var page = Xamarin.Forms.Application.Current.MainPage.Navigation.NavigationStack[count - 1] as BackCustomizeContentPage;

                    if (page?.CustomBackButtonAction != null)
                    {
                        page?.CustomBackButtonAction.Invoke();
                        return false;
                    }
                    else
                    {
                        return base.OnOptionsItemSelected(item);
                    }
                }
                catch
                {
                    return base.OnOptionsItemSelected(item);
                }
            }
            return base.OnOptionsItemSelected(item);
        }

        public override void OnBackPressed()
        {
            //base.OnBackPressed();
            if (BackButtonDisable) return;

            try
            {
                var count = Xamarin.Forms.Application.Current.MainPage.Navigation.NavigationStack.Count;
                var thisPage = Xamarin.Forms.Application.Current.MainPage.Navigation.NavigationStack[count - 1];

                var page = thisPage as BackCustomizeContentPage;

                if (page?.CustomBackButtonAction != null)
                {
                    page?.CustomBackButtonAction.Invoke();
                    return;
                }
                else
                {
                    base.OnBackPressed();
                    return;
                }
            }
            catch
            {
                base.OnBackPressed();
                return;
            }
        }
        protected override void OnActivityResult(int requestCode, Result resultCode, global::Android.Content.Intent data)
        {
            // Ask the open service connection's billing handler to process this request
            System.Diagnostics.Debug.WriteLine("OnActivityResult");
            try
            {
                base.OnActivityResult(requestCode, resultCode, data);
                System.Diagnostics.Debug.WriteLine(" request code  :" + requestCode);
                if (requestCode == RC_SIGN_IN)
                {
                    if (resultCode == Result.Ok)
                    {
                        System.Diagnostics.Debug.WriteLine(" result code == rc sign in");
                        //iGoogleTestService_Droid.GoogleApiClient.Connect();
                        GoogleSignInResult result = Auth.GoogleSignInApi.GetSignInResultFromIntent(data);
                        handleSignInResult(result);
                    }
                }
                FacebookClientManager.OnActivityResult(requestCode, resultCode, data);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("OnActivityResult ex" + ex.Message + System.Environment.NewLine + ex.StackTrace);
            }
        }

        SNS_LoginStates loginState = SNS_LoginStates.Error;
        private void handleSignInResult(GoogleSignInResult result)
        {
            System.Diagnostics.Debug.WriteLine("handleSignInResult start  :");
            if (result != null &&
                result.IsSuccess)
            {

                Task.Run(async () =>
                {

                    //メールアドレス・トークンを取得する
                    GoogleSignInAccount account = result.SignInAccount;
                    var email = account.Email;
                    var name = account.GivenName;

                    var udata = new UserData()
                    {
                        name = account.DisplayName,

                        email = account.Email,

                        google_id = account.Id
                    };

                    var toast = new Toast();
                    Device.BeginInvokeOnMainThread(() =>
                                                   toast.Show("認証しました。"));

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

                            apiName = "home";
                            var resHomeJson = await APIManager.GET(apiName);

                            var param = JsonManager.Deserialize<ResponseHome>(resHomeJson);

                            System.Diagnostics.Debug.WriteLine("Home:" + resHomeJson);
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                await App.customNavigationPage.PushAsync(new Home(param), false);
                                PushNotificationService_Droid.InitializeNotification();
                            });
                            break;
                        default:
                            await App.customNavigationPage.Navigation.NavigationStack.Last().DisplayAlert("エラー", "データ取得に失敗しました。改めてお試しください。", "戻る");
                            await App.customNavigationPage.PopAsync();
                            break;
                    }


                }).ConfigureAwait(false);
            }

            else
            {
                if (!result.IsSuccess)
                {
                    //OnConnectionFailed(result);
                }
            }
        }
        public void OnConnectionFailed(ConnectionResult result)
        {
            System.Diagnostics.Debug.WriteLine("OnConnectionFailed");
            //接続失敗時の処理が必要であれば記述します。
            if (!result.HasResolution)
            {
                // show the localized error dialog.
                //GoogleApiAvailability.Instance.GetErrorDialog(this, result.ErrorCode, 0).Show();
                return;
            }

            //アカウントログインが必要な場合
            try
            {
                result.StartResolutionForResult(this, RC_SIGN_IN);
            }
            catch (IntentSender.SendIntentException e)
            {
                System.Diagnostics.Debug.WriteLine("OnConnectionFailed ex" + e.Message + System.Environment.NewLine + e.StackTrace);
            }
        }

        public void OnConnected(Bundle connectionHint)
        {
            //接続成功時の処理が必要であれば記述します。
            System.Diagnostics.Debug.WriteLine("OnConnected");
        }

        public void OnConnectionSuspended(int cause)
        {
            System.Diagnostics.Debug.WriteLine("OnConnectionSuspended");
            //接続中断時の処理が必要であれば記述します。
            //iGoogleTestService_Droid.GoogleApiClient

        }

        string scheme = "thinkapp://";
        string host = "example.jp";
        protected override void OnResume()
        {
            base.OnResume();

            var intent = this.Intent;

            if (Intent.ActionView.Equals(intent.Action))
            {
                var uri = intent.Data;
                System.Diagnostics.Debug.WriteLine("uri :" + uri);

                if (uri != null)
                {
                    if (uri.ToString().Contains(scheme + host))
                    {
                        var param = scheme + host + "/";
                        var data = uri.ToString().Substring(param.Length);
                        System.Diagnostics.Debug.WriteLine("data :" + data);
                        NavigateFromQRcode(data);
                    }

                }
            }
        }

        async void NavigateFromQRcode(string data)
        {
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
    }
}
