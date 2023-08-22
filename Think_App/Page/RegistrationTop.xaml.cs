using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using IO.Swagger.Model;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
namespace Think_App
{
    public partial class RegistrationTop : ContentPage
    {
        public RegistrationTop()
        {
            InitializeComponent();

            NavigationPage.SetBackButtonTitle(this, "");

            resgistorationText.PropertyChanged += (sender, e) =>
            {
                System.Diagnostics.Debug.WriteLine("resgistorationText PropertyName  :" + e.PropertyName);
            };

            resgistorationTextUnderBtn.PropertyChanged += (sender, e) =>
            {
                System.Diagnostics.Debug.WriteLine("resgistorationTextUnderBtn PropertyName  :" + e.PropertyName);
            };
            App.ProcessManager.OnComplete();
        }

        async void OnLabelClicked(object sender, EventArgs e)
        {
            if (App.ProcessManager.CanInvoke())
            {
                // POST処理
                string action = "account_guest_regist";
                var parameters = new Dictionary<string, string> { { "deviceCode", Config.Instance.deviceCode } };
                var apiRet = await APIManager.Post(action, parameters);
                if (apiRet != null)
                {
                    var responseJson = JsonConvert.DeserializeObject<ResponseAccount>(apiRet);
                    if (responseJson != null)
                    {
                        if (responseJson != null)
                        {
                            var ret = await StaticMethod.DeviceIDReg(responseJson.Data.DeviceId, responseJson.Data.TransferId);
                            if (ret == "OK")
                            {
                                await MovePage();
                            }
                        }
                        else
                        {
                            await this.DisplayAlert("エラー", "通信に失敗しました。再度やり直してください。", "OK");
                        }
                    }
                    else
                    {
                        await this.DisplayAlert("エラー", "通信に失敗しました。再度やり直してください。", "OK");
                    }
                }
                App.ProcessManager.OnComplete();
            }
        }

        //登録店舗が１店舗だった場合はhome画面へ遷移させる。
        async Task MovePage()
        {
            var salonListJson = await APIManager.Post("salon_list", new Dictionary<string, string>());
            if (salonListJson == null)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    DependencyService.Get<IToast>().Show("通信エラー");
                });
            }
            else
            {
                var responseSalonRegionList = JsonConvert.DeserializeObject<ResponseSalonList>(salonListJson);
                var salonListCount = responseSalonRegionList.Data.List.Count;
                System.Diagnostics.Debug.WriteLine("List Count :" + salonListCount);

                //1店舗じゃない場合は選択させる
                if (salonListCount != 1)
                {
                    await App.customNavigationPage.PushAsync(new FavoriteStoreReg());
                }
                //１店舗ならばホームに登録させて遷移
                else
                {
                    string action = "salon_home_regist";
                    var parameters = new Dictionary<string, string> {
                            { "deviceId", Config.Instance.Data.deviceId },
                        { "salonId", responseSalonRegionList.Data.List[0].SalonId.ToString() }
                        };
                    var homeRegistJson = await APIManager.Post(action, parameters);
                    if (!string.IsNullOrEmpty(homeRegistJson))
                    {
                        var json = await APIManager.GET("home");
                        var param = JsonManager.Deserialize<ResponseHome>(json);
                        DeviceTokenManager.PostAndRegistDeviceToken(DeviceTokenInfo.Instance.DeviceToken);
                        await App.customNavigationPage.PushAsync(new Home(param));
                    }
                    else
                    {
                        await this.DisplayAlert("エラー", "登録に失敗しました。再度登録してください。", "OK");
                        return;
                    }

                }

            }
        }


        int version;

        const string userId = "&u=", baseAppURL = "yoyakuapp.sipss.jp/app/?b=";
        async void QRCodeScanClicked(object sender, EventArgs s)
        {


            bool res = int.TryParse(Config.Instance.Data.nativeVersion.Substring(0, 1), out version);
            if (!res)
                return;
            // TODO Xamarin.Forms.Device.RuntimePlatform is no longer supported. Use Microsoft.Maui.Devices.DeviceInfo.Platform instead. For more details see https://learn.microsoft.com/en-us/dotnet/maui/migration/forms-projects#device-changes
            if (Device.RuntimePlatform == Device.Android && version >= 6)
            {
                DependencyService.Get<IScanerPermissionService>().Call();
            }
            else
            {
                var customOverlay = new ScanView();
                //var scanPage = new ZXingScannerPage(null, customOverlay);
                var scanPage = new QRcodeScan(null, customOverlay);

                // スキャナページを表示
                await Navigation.PushAsync(scanPage);
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
                            int num = ret.IndexOf(userId);
                            //http://think.entap.xyz/introduction/page.html?appid=jp.comcomcompany.ThinkApp&uid=%20JL2Hhm7g
                            if (ret.IndexOf(baseAppURL) > 0 && num > 0)
                            {
                                string uid = ret.Substring(num + userId.Length);
                                await DisplayAlert("読み込み完了", null, "OK");

                                var dic = new Dictionary<string, string>() { { "uid", uid } };
                                try
                                {
                                    var resJson = await APIManager.GET("qrcode_data", dic);
                                    var response = JsonManager.Deserialize<ResponseQRcodeData>(resJson);
                                    if (response != null && response.Data != null)
                                    {
                                        await Navigation.PushAsync(new QRcodeLogin(response));
                                    }
                                    else
                                    {
                                        await DisplayAlert("データを取得できませんでした。", response.Message, "OK");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    System.Diagnostics.Debug.WriteLine("ex :" + ex);
                                }
                            }
                            else
                            {
                                await DisplayAlert("読み込み完了", "無効なバーコードです", "OK");
                                await App.customNavigationPage.PopAsync();
                            }
                            // xamarinのバグ？ Navigation.RemovePage原因で落ちてしまう
                            //Navigation.RemovePage(scanPage); // スキャンするページには戻らないようNavigationのstackから消しておく
                            App.ProcessManager.OnComplete();
                        }
                    });
                };
            }



        }
    }
}
