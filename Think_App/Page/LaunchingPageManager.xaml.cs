using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using IO.Swagger.Model;
using Xamarin.Forms;
using Newtonsoft.Json;

namespace Think_App
{
    public partial class LaunchingPageManager : ContentPage
    {
        public LaunchingPageManager()
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "");
            PageManage();

            // ナビゲーションバーの戻るボタン連打でこのページに戻るのを防ぐ
            //this.Disappearing += (sender, e) =>
            //{
            //	Navigation.RemovePage(this);
            //};
        }
        string json = null;
        ResponseHome param = null;
        //コンストラクタに直接書いた場合画面が表示されなかった。
        async Task PageManage()
        {

            if (string.IsNullOrEmpty(Config.Instance.Data.deviceId))
            {
                System.Diagnostics.Debug.WriteLine("AppStart:");
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await App.customNavigationPage.PushAsync(new AppStart(), false);
                });
            }
            else
            {
                while (string.IsNullOrEmpty(json))
                {
                    json = await APIManager.GET("home");
                    param = JsonManager.Deserialize<ResponseHome>(json);
                    System.Diagnostics.Debug.WriteLine("Home:" + json);

                    if (param.Status == APIManager.APIStatus.TakenOver ||
                        param.Status == APIManager.APIStatus.HomeDeleted)
                        break;

                    if (string.IsNullOrEmpty(json) || param == null)
                    {
                        await DisplayAlert("通信エラー", "通信環境の良い場所で、時間をおいて改めてログインを行ってください", "OK");
                    }
                }

                if (param?.Status == APIManager.APIStatus.TakenOver ||
                    param.Status == APIManager.APIStatus.HomeDeleted)
                    return;
                try
                {
                    if (param?.Data?.HomeSalonInfo == null ||
                       param.Data.HomeSalonInfo.SalonId == null)
                    {
                        await MovePage();
                    }
                    else
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await App.customNavigationPage.PushAsync(new Home(param), false);
                        });
                }
                catch
                {
                }
            }
        }
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
    }
}