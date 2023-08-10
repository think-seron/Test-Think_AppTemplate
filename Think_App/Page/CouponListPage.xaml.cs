using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Newtonsoft.Json;
using IO.Swagger.Model;

namespace Think_App
{
    public partial class CouponListPage : ContentPage
    {
        CouponListPageViewModel couponListPageViewModel;
        List<ListViewCouponViewModel> itemList;
        public string salonName;
        public int? salonId;
        bool soloSalon;
        public CouponListPage(string _salonName, int? _salonId = null, bool _soloSalon = false)
        {
            InitializeComponent();

            App.customNavigationPage.IsRunning = true;
            salonName = _salonName;
            salonId = _salonId;
            soloSalon = _soloSalon;
            NavigationPage.SetBackButtonTitle(this, "");


            GetData();

            this.ListView.ItemSelected += async (sender, e) =>
            {
                if (App.ProcessManager.CanInvoke())
                {
                    await Navigation.PushAsync(new CouponInfoPage(((ListViewCouponViewModel)e.SelectedItem)));
                    this.ListView.SelectedItem = null;
                    App.ProcessManager.OnComplete();
                }
            };
            if (!soloSalon)
            {
                ToolbarItems.Add(new ToolbarItem("ChangeStore", "Icon_Home.png", async () =>
                {
                    if (!App.ProcessManager.CanInvoke())
                        return;
                    var page = new StoreSelect(3, null, this);
                    if (Device.RuntimePlatform == Device.iOS)
                        App.customNavigationPage.IsBadgeVisble = false;

                    await App.customNavigationPage.PushAsync(page);

                    App.ProcessManager.OnComplete();
                }));
                this.Appearing += (sender, e) =>
                {
                    SetIcon();
                };
            }

        }

        async void SetIcon()
        {
            try
            {
                var jsonNotRead = await APIManager.GET("check_badge");
                var responseNotRead = JsonManager.Deserialize<ResponseCheckBatch>(jsonNotRead);
                if (responseNotRead != null)
                {

                    if ((bool)(responseNotRead.Data.CouponNotification) && this.ToolbarItems != null)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            System.Diagnostics.Debug.WriteLine("init  badge true");
                            if (Device.RuntimePlatform == Device.Android)
                            {
                                couponListPageViewModel.ToolbarIcon = "Icon_HomeAndBadge.png";
                            }
                            else
                            {
                                App.customNavigationPage.IsBadgeVisble = true;
                            }
                        });

                    }
                    else
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            System.Diagnostics.Debug.WriteLine("init  badge false");
                            if (Device.RuntimePlatform == Device.Android)
                            {
                                couponListPageViewModel.ToolbarIcon = "Icon_Home.png";
                            }
                            else
                            {
                                App.customNavigationPage.IsBadgeVisble = false;
                            }
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("ex :" + ex);
            }
        }

        public void GetData()
        {
            itemList = new List<ListViewCouponViewModel>();
            Task.Run(async () =>
            {
                couponListPageViewModel = new CouponListPageViewModel(soloSalon);
                couponListPageViewModel.SalonName = salonName;

                string json;
                Dictionary<string, string> parameters;
                if (salonId == null)
                {
                    parameters = new Dictionary<string, string> { };
                }
                else
                {
                    parameters = new Dictionary<string, string> { { "salonId", salonId.ToString() } };
                }
                json = await APIManager.GET("coupon_list", parameters);
                if (json == null)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        this.BindingContext = couponListPageViewModel;
                        DependencyService.Get<IToast>().Show("通信エラー");
                    });
                }
                else
                {
                    var response = JsonConvert.DeserializeObject<ResponseCouponList>(json);
                    var regionList = response.Data.List;
                    if (regionList != null)
                    {
                        foreach (var val in regionList)
                        {
                            itemList.Add(new ListViewCouponViewModel()
                            {
                                ID = val.CouponId,
                                //CouponImageSouce = val.ThumbnailImage.Path,
                                CouponImageSouce = DependencyService.Get<IImageService>().ResizeNetImage(val.ThumbnailImage.Path),
                                CouponTitle = val.Title,
                                ShopName = val.SalonName,
                                OperationContent = val.Treatment,
                                DiscountContent = val.Discount,
                                TermsOfUse = val.Condition,
                                SpatialCondition = val.Timing,
                                Description = val.Description,
                                IsAssociated = (bool)val.IsAssociated,
                                ShopID = salonId
                            });
                        }
                    }

                    int rectHeight = 223 * itemList.Count;
                    int heightAgust = 111;
                    if (Device.RuntimePlatform == Device.Android)
                    {
                        heightAgust = heightAgust - 9;
                    }

                    if ((ScaleManager.ScreenHeight - heightAgust) < rectHeight)
                    {
                        couponListPageViewModel.ListViewRect = new Rectangle(0, 46, 1, (ScaleManager.ScreenHeight - heightAgust));
                    }
                    else
                    {
                        couponListPageViewModel.ListViewRect = new Rectangle(0, 46, 1, rectHeight);
                    }

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        this.ListView.ItemsSource = itemList;
                        this.BindingContext = couponListPageViewModel;
                        App.customNavigationPage.IsRunning = false;
                    });
                }

                ////単独店舗の場合は以下の処理は不要
                //if (soloSalon)
                //    return;

                //if (Device.RuntimePlatform == Device.iOS)
                //{
                //    Device.BeginInvokeOnMainThread(() =>
                //    {
                //        couponListPageViewModel.ToolbarIcon = "Icon_Home.png";
                //    });
                //}
                //couponListPageViewModel.ToolbarItemsClick = new Command(async () =>
                //{
                //    if (!App.ProcessManager.CanInvoke())
                //        return;
                //    var page = new StoreSelect(3, null, this);
                //    if (Device.RuntimePlatform == Device.iOS)
                //        App.customNavigationPage.IsBadgeVisble = false;

                //    await App.customNavigationPage.PushAsync(page);

                //    App.ProcessManager.OnComplete();
                //});
            });
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            if (Device.RuntimePlatform == Device.iOS && App.customNavigationPage.IsBadgeVisble == true)
                App.customNavigationPage.IsBadgeVisble = false;
        }
    }
}
