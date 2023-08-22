using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using IO.Swagger.Model;
using System.Threading.Tasks;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
    public partial class CouponInfoPage : ContentPage
    {
        CouponInfoPageViewModel couponInfoPageViewModel;
        public CouponInfoPage(ListViewCouponViewModel couponInfo)
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "");

            couponInfoPageViewModel = new CouponInfoPageViewModel();

            // ------------------------------------------------------
            // 選択したクーポンの情報を取得し表示
            couponInfoPageViewModel.ImageSouce = couponInfo.CouponImageSouce;
            couponInfoPageViewModel.Type = couponInfo.Type;
            couponInfoPageViewModel.CouponTitle = couponInfo.CouponTitle;
            couponInfoPageViewModel.ShopName = couponInfo.ShopName;
            couponInfoPageViewModel.OperationContent = couponInfo.OperationContent;
            couponInfoPageViewModel.DiscountContent = couponInfo.DiscountContent;
            couponInfoPageViewModel.TermsOfUse = couponInfo.TermsOfUse;
            couponInfoPageViewModel.SpatialCondition = couponInfo.SpatialCondition;
            couponInfoPageViewModel.Description = couponInfo.Description;
            // ------------------------------------------------------
            couponInfoPageViewModel.CouponTitleFontSize = ScaleManager.SizeSet(14.0);
            couponInfoPageViewModel.ShopNameFontSize = ScaleManager.SizeSet(14.0);
            couponInfoPageViewModel.OperationContentFontSize = ScaleManager.SizeSet(12.0);
            couponInfoPageViewModel.DiscountContentFontSize = ScaleManager.SizeSet(12.0);
            couponInfoPageViewModel.TermsOfUseFontSize = ScaleManager.SizeSet(12.0);
            couponInfoPageViewModel.SpatialConditionFontSize = ScaleManager.SizeSet(12.0);
            couponInfoPageViewModel.DescriptionFontSize = ScaleManager.SizeSet(14.0);
            //couponInfoPageViewModel.GridRect = new Rectangle(1, 0, 1, (ScaleManager.ScreenHeight / 2));
            //couponInfoPageViewModel.ButtonRect = new Rectangle(1, 0, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize);

            couponInfoPageViewModel.ImageHeght = ScaleManager.SizeSet(135.0);
            couponInfoPageViewModel.ImageWidth = ScaleManager.SizeSet(135.0);

            couponInfoPageViewModel.ButtonWidth = ScaleManager.SizeSet(177.0);
            couponInfoPageViewModel.ButtonHeight = ScaleManager.SizeSet(36.0);
            //couponInfoPageViewModel.ButtonRect = new Rectangle(1, 1, 177 * ScaleManager.Scale, 36 * ScaleManager.Scale);

            couponInfoPageViewModel.BtnIsVisebld = couponInfo.IsAssociated;

            this.BindingContext = couponInfoPageViewModel;


            this.ReserveBtn.Clicked += async (sender, e) =>
            {
                var modalView = new ModalView();
                modalView.modalViewViewModel.ModalLabelTxt = "スタッフを指名して予約しますか？";
                modalView.modalViewViewModel.NomalModalLabelRect = new Rect(0.5, 0.4, 1, AbsoluteLayout.AutoSize);
                modalView.modalViewViewModel.SelectBtnLayoutBounds = new Rect(0.9, 0.6, 1, AbsoluteLayout.AutoSize);
                modalView.modalViewViewModel.YesButtonTxt = "指名する";
                modalView.modalViewViewModel.NoButtonTxt = "指名しない";


                //var currentApp = Xamarin.Forms.Application.Current;
                modalView.yesButton.Clicked += async (yesSender, yesE) =>
                {
                    if (App.ProcessManager.CanInvoke())
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            App.customNavigationPage.IsRunning = true;
                        });
                        // ------------------------------------------------------
                        // 予約ページ(スタッフ指名)への遷移
                        //await Navigation.PushAsync(new AccountRegistration());
                        await DialogManager.Instance.HideView();
                        //if (currentApp != Xamarin.Forms.Application.Current)
                        //{
                        //	throw new InvalidOperationException("Application.Current changed");
                        //}

                        var response = await GetStaffList(couponInfo);
                        if (response == null)
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                DependencyService.Get<IToast>().Show("通信エラー");
                                App.customNavigationPage.IsRunning = false;
                            });
                            App.ProcessManager.OnComplete();
                            return;
                        }
                        else if (response.Status == 0 && response.Data == null)
                        {

                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                await App.Current.MainPage.DisplayAlert("エラー", "該当店舗にスタッフが登録されていません。", "OK");
                                App.customNavigationPage.IsRunning = false;
                            });
                            App.ProcessManager.OnComplete();
                            return;
                        }
                        var ReservationContent = new ReservationContentInfo()
                        {
                            StoreName = couponInfo.ShopName,
                            SalonId = couponInfo.ShopID,
                            CouponId = couponInfo.ID,
                            MenuId = 0,
                            CouponContent = couponInfo.CouponTitle,
                            MenuName = couponInfo.OperationContent
                        };
                        //await App.customNavigationPage.PushAsync(new ReservationSelectStaff(response, ReservationContent,2));

                        await App.customNavigationPage.PushAsync(new ReservationSelectStaff(response, ReservationContent, 2));

                        App.customNavigationPage.IsRunning = false;
                        App.ProcessManager.OnComplete();
                        // ------------------------------------------------------
                    }
                };
                modalView.noButton.Clicked += async (noSender, noE) =>
                {
                    if (App.ProcessManager.CanInvoke())
                    {
                        // ------------------------------------------------------
                        // 予約ページ(スタッフなし)への遷移
                        await DialogManager.Instance.HideView();
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            App.customNavigationPage.IsRunning = true;
                        });

                        var awaiter = GetStaffList(couponInfo).GetAwaiter();
                        awaiter.OnCompleted(async () =>
                        {
                            var response = awaiter.GetResult();
                            if (response == null || response.Data == null)
                            {
                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    DependencyService.Get<IToast>().Show("通信エラー");
                                    App.customNavigationPage.IsRunning = false;
                                });
                                App.ProcessManager.OnComplete();
                                return;
                            }

                            foreach (var val in response.Data.List)
                            {
                                if (val.Name == "指名なし")
                                {
                                    var ReservationContent = new ReservationContentInfo()
                                    {
                                        StoreName = couponInfo.ShopName,
                                        SalonId = couponInfo.ShopID,
                                        CouponId = couponInfo.ID,
                                        StaffId = val.StaffId,
                                        StaffName = val.Name,
                                        MenuId = 0,
                                        CouponContent = couponInfo.CouponTitle,
                                        MenuName = couponInfo.OperationContent
                                    };

                                    string apiName = "reservation_schedule";
                                    var param = new Dictionary<string, string>(){
                                    { "deviceId", Config.Instance.Data.deviceId },
                                    {"salonId", ReservationContent.SalonId.ToString()},
                                    {"staffId", ReservationContent.StaffId.ToString()},
                                    {"date", DependencyService.Get<IDateTimeService>().GetNow().ToString("d",new System.Globalization.CultureInfo("ja-JP"))},
                                    {"couponId", ReservationContent.CouponId.ToString()}
                                };

                                    var res = await APIManager.GET(apiName, param);
                                    var scheduleContent = JsonManager.Deserialize<ResponseReservationSchedule>(res);

                                    await App.customNavigationPage.Navigation.PushAsync(new ReservationSchedule(scheduleContent, ReservationContent));
                                    Device.BeginInvokeOnMainThread(() =>
                                    {
                                        App.customNavigationPage.IsRunning = false;
                                    });
                                    break;
                                }
                            }
                            App.ProcessManager.OnComplete();
                        });
                        // ------------------------------------------------------
                    }
                };

                await DialogManager.Instance.ShowDialogView(modalView);
            };
        }


        async Task<ResponseStaffList> GetStaffList(ListViewCouponViewModel couponInfo)
        {
            var parameters = new Dictionary<string, string> { { "couponId", couponInfo.ID.ToString() } };
            var json = await APIManager.GET("treatment_possible_staff", parameters);
            if (json == null)
            {
                return null;
            }
            var response = JsonConvert.DeserializeObject<ResponseStaffList>(json);
            if (response == null)
            {
                return null;
            }
            return response;
        }


    }
}
