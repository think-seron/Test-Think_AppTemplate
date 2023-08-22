using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IO.Swagger.Model;
using Newtonsoft.Json;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
    public partial class StoreSelect : ContentPage
    {
        StoreSelectViewModel storeSelectViewModel;

        // lastPageFlgについて
        // 1: 新規登録ページからの遷移
        // 2: お知らせページからの遷移
        // 3: クーポンページからの遷移
        // 4: 予約からの遷移
        public StoreSelect(int lastPageFlg, int? regionId, Microsoft.Maui.Controls.Page lastPage = null)
        {
            InitializeComponent();
            App.customNavigationPage.IsRunning = true;

            NavigationPage.SetBackButtonTitle(this, "");

            storeSelectViewModel = new StoreSelectViewModel(lastPageFlg);
            if (lastPageFlg == 1)
            {
                storeSelectViewModel.TopLabelTxt = "店舗選択";
                storeSelectViewModel.TopRightLabelTxt = "※  後で変更できます";
                this.Title = "店舗選択";
            }
            else if (lastPageFlg == 4)
            {
                storeSelectViewModel.TopLabelTxt = "店舗選択";
                storeSelectViewModel.TopRightLabelTxt = null;
                this.Title = "予約";
            }
            else
            {
                if (lastPageFlg == 2)
                {
                    this.Title = "お知らせ";
                }
                else if (lastPageFlg == 3)
                {
                    this.Title = "クーポン";
                }
                storeSelectViewModel.TopLabelTxt = "店舗の切り替え";
            }


            List<ListViewLabelViewModel> itemList = new List<ListViewLabelViewModel>();

            // ---------------------------------------------------------------------
            // 該当地域のサロンの一覧を取得してitemListにadd
            Task.Run(async () =>
            {
                string json = null;
                if (regionId == null)
                {
                    var parameters = new Dictionary<string, string> { };
                    json = await APIManager.GET("salon_list", parameters);
                }
                else
                {
                    var parameters = new Dictionary<string, string> { { "regionId", regionId.ToString() } };
                    json = await APIManager.GET("salon_list", parameters);
                }
                if (json == null)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        DependencyService.Get<IToast>().Show("通信エラー");
                    });
                }
                else
                {
                    var responseSalonRegionList = JsonConvert.DeserializeObject<ResponseSalonList>(json);
                    var salonList = responseSalonRegionList.Data.List;
                    foreach (var val in salonList)
                    {
                        //itemList.Add(new ListViewLabelViewModel(val));
                        itemList.Add(new ListViewLabelViewModel()
                        {
                            LabelText = val.Name,
                            salonId = val.SalonId
                        });
                    }
                }

                int rectHeight = 48 * itemList.Count;

                int heightAgust = 111;
                // TODO Xamarin.Forms.Device.RuntimePlatform is no longer supported. Use Microsoft.Maui.Devices.DeviceInfo.Platform instead. For more details see https://learn.microsoft.com/en-us/dotnet/maui/migration/forms-projects#device-changes
                if (Device.RuntimePlatform == Device.Android)
                {
                    heightAgust = heightAgust - 9;
                }
                if ((ScaleManager.ScreenHeight - heightAgust) < rectHeight)
                {
                    storeSelectViewModel.ListViewRect = new Rect(0, 46, 1, (ScaleManager.ScreenHeight - heightAgust));
                }
                else
                {
                    storeSelectViewModel.ListViewRect = new Rect(0, 46, 1, rectHeight);
                }

                Device.BeginInvokeOnMainThread(() =>
                {
                    this.ListView.ItemsSource = itemList;
                    this.BindingContext = storeSelectViewModel;
                    App.customNavigationPage.IsRunning = false;
                });
            });
            // ---------------------------------------------------------------------

            this.ListView.ItemSelected += async (sender, e) =>
            {
                System.Diagnostics.Debug.WriteLine(" item selected ");
                if (App.ProcessManager.CanInvoke())
                {
                    System.Diagnostics.Debug.WriteLine(" item selected CanInvoke");
                    // 新規登録の場合お気に入り店舗を選択する処理記入予定
                    // 登録完了後ホーム画面へ遷移？
                    App.customNavigationPage.IsRunning = true;

                    if (lastPageFlg == 1)
                    {
                        string action = "salon_home_regist";
                        var parameters = new Dictionary<string, string> {
                            { "deviceId", Config.Instance.Data.deviceId },
                            { "salonId", ((ListViewLabelViewModel)e.SelectedItem).salonId.ToString() }
                        };
                        var apiRet = await APIManager.Post(action, parameters);
                        if (apiRet != null)
                        {
                            var responseJson = JsonConvert.DeserializeObject<ResponseBase>(apiRet);
                            if (responseJson != null)
                            {

                                //DependencyService.Get<IToast>().Show("お気に入り店舗に登録されました");
                                var json = await APIManager.GET("home");

                                var param = JsonManager.Deserialize<ResponseHome>(json);
                                DeviceTokenManager.PostAndRegistDeviceToken(DeviceTokenInfo.Instance.DeviceToken);
                                await App.customNavigationPage.PushAsync(new Home(param));
                            }
                            else
                            {
                                await this.DisplayAlert("エラー", "登録に失敗しました。再度登録してください。", "OK");
                            }
                        }
                        App.customNavigationPage.IsRunning = false;
                    }
                    else if (lastPageFlg == 2)
                    {
                        ((NoticeList)lastPage).salonName = ((ListViewLabelViewModel)e.SelectedItem).LabelText;
                        ((NoticeList)lastPage).salonId = ((ListViewLabelViewModel)e.SelectedItem).salonId;
                        ((NoticeList)lastPage).GetData();
                        await App.customNavigationPage.PopAsync();
                    }
                    else if (lastPageFlg == 3)
                    {
                        //// 戻るボタンで一つ前のNoticeListに戻らないように削除
                        //var delPage = Navigation.NavigationStack.ToList()[Navigation.NavigationStack.Count - 2];
                        //Navigation.RemovePage(delPage);
                        //// クーポンページからの遷移　クーポン一覧を表示
                        //await App.customNavigationPage.PushAsync(new CouponListPage(((ListViewLabelViewModel)e.SelectedItem).LabelText, ((ListViewLabelViewModel)e.SelectedItem).salonId));
                        //Navigation.RemovePage(this); // 戻るボタンでここに戻らないように削除
                        ((CouponListPage)lastPage).salonName = ((ListViewLabelViewModel)e.SelectedItem).LabelText;
                        ((CouponListPage)lastPage).salonId = ((ListViewLabelViewModel)e.SelectedItem).salonId;
                        ((CouponListPage)lastPage).GetData();
                        await App.customNavigationPage.PopAsync();
                    }
                    else if (lastPageFlg == 4)
                    {
                        try
                        {
                            System.Diagnostics.Debug.WriteLine("スタッフリストページ");

                            string action = "reservation_stafflist";
                            var parameters = new Dictionary<string, string> {
                            { "deviceId", Config.Instance.Data.deviceId },
                            { "salonId", ((ListViewLabelViewModel)e.SelectedItem).salonId.ToString() }
                        };

                            System.Diagnostics.Debug.WriteLine(" item selected reservation_stafflist Post");
                            var apiRet = await APIManager.Post(action, parameters);
                            if (apiRet != null)
                            {
                                var response = JsonConvert.DeserializeObject<ResponseStaffList>(apiRet);
                                if (response.Status == 0 && response.Data == null)
                                {
                                    System.Diagnostics.Debug.WriteLine(" item selected reservation_stafflist Post response.Status == 0 && response.Data == null");
                                    Device.BeginInvokeOnMainThread(async () =>
                                    {
                                        await App.Current.MainPage.DisplayAlert("エラー", "該当店舗にスタッフが登録されていません。", "OK");
                                    });
                                }
                                else if (response != null)
                                {
                                    System.Diagnostics.Debug.WriteLine(" item selected reservation_stafflist Post response != null" + apiRet);
                                    //DependencyService.Get<IToast>().Show("お気に入り店舗に登録されました");
                                    //var json = await APIManager.GET("home");

                                    //var param = JsonManager.Deserialize<ResponseHome>(json);

                                    //System.Diagnostics.Debug.WriteLine("Home:" + json);
                                    //await Navigation.PushAsync(new Home(param));

                                    var reservationContent = new ReservationContentInfo()
                                    {
                                        SalonId = ((ListViewLabelViewModel)e.SelectedItem).salonId,
                                        StoreName = ((ListViewLabelViewModel)e.SelectedItem).LabelText
                                    };

                                    //if (Device.RuntimePlatform == Device.iOS)
                                    //{
                                    await App.customNavigationPage.PushAsync(new ReservationSelectStaff(response, reservationContent, 1));
                                    //}
                                    //else
                                    //{
                                    //	await App.customNavigationPage.PushAsync(new ReservationSelectStaff_Droid(response, reservationContent, 1));
                                    //}
                                }
                                else
                                {
                                    System.Diagnostics.Debug.WriteLine(" item selected reservation_stafflist Post else");
                                    //await this.DisplayAlert("エラー", "登録に失敗しました。再度登録してください。", "OK");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine("   ex  :" + ex);
                            await this.DisplayAlert("エラー", "読み込みに失敗しました。通信環境の良い場所で再度選択してください。", "OK");
                        }
                        System.Diagnostics.Debug.WriteLine(" item selected App.customNavigationPage.IsRunning = false");
                        App.customNavigationPage.IsRunning = false;
                    }

                    this.ListView.SelectedItem = null;
                    App.ProcessManager.OnComplete();
                }
                this.ListView.SelectedItem = null;
            };
        }
    }
}