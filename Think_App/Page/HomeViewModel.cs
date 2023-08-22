using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using IO.Swagger.Model;
using Newtonsoft.Json;
using System.Linq;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
namespace Think_App
{
    public class HomeViewModel : ViewModelBase
    {
        bool bilogAvailable = false;
        public HomeViewModel(ResponseHome response)
        {
            SliderHeight = ScaleManager.ScreenWidth * 0.5625;

            HomeResponseData = response;
            bilogAvailable = HomeResponseData.Data.HomeSalonInfo.BilogAvailable == null
                                      ? false
                                      : (bool)HomeResponseData.Data.HomeSalonInfo.BilogAvailable;
            SetHomeSelectCommand(response);
            if (1.0 > ScaleManager.Scale)
            {
                ScreenSizeScale = ScaleManager.Scale;
            }
            else
            {
                ScreenSizeScale = 1.0;
            }

            TitleImageSource = "AppHeaderLogo.png";

            ToolBarIcon = "Icon_DottedLine";
            ConfigVisible = false;
            ConfigListBC = new ConfigListVIewModel(HomeResponseData);
            ToolbarCommand = new Command(() =>
            {
                // TODO Xamarin.Forms.Device.RuntimePlatform is no longer supported. Use Microsoft.Maui.Devices.DeviceInfo.Platform instead. For more details see https://learn.microsoft.com/en-us/dotnet/maui/migration/forms-projects#device-changes
                if (Device.RuntimePlatform == Device.Android)
                {
                    ConfigVisible = !ConfigVisible;
                    System.Diagnostics.Debug.WriteLine("android");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("ios");
                    SetActionSheet();
                }
            });

            //var listdata = HomeData.GetSampleData();
            //var listdata = new ObservableCollection<HomeButtonViewViewModel>();
            //foreach (var n in HomeResponseData.Data.MenuList)
            //{
            //  listdata.Add(new HomeButtonViewViewModel(n));
            //}


            ItemsSource = new ObservableCollection<HomeButtonViewViewModel>()
            {
                HomeButton1_1_BC ,HomeButton1_3_BC,HomeButton1_5_BC,HomeButton1_7_BC,
                HomeButton3_1_BC,HomeButton3_3_BC,HomeButton3_5_BC,HomeButton3_7_BC
            };

            for (int i = 0; i < 8; i++)
            {
                if (i < HomeResponseData.Data.MenuList.Count)
                {
                    ItemsSource[i].SetView(HomeResponseData.Data.MenuList[i], SetCommand((int)(HomeResponseData.Data.MenuList[i].MenuId)));

                }
                else
                {
                    ItemsSource[i].
                                  //SetView(new InlineResponse2002DataMenuList(0, null, null, null));
                                  SetView(new InlineResponse2002DataMenuList()
                                  {
                                      MenuId = 0,
                                      IconImage = null,
                                      Name = null,
                                      Notification = null
                                  });
                }
            }
            SetReservationStyle();
            SetHomeShopInfo();
            SetCarouselImages();
        }

        const string accountSetting = "アカウント設定";
        const string takeOverCode = "引き継ぎコード";
        const string privacy = "プライバシーポリシー";
        const string terms = "利用規約";
        const string beautyLogSetting = "公開設定";
        const string notificationSetting = "プッシュ通知設定";
        const string deleteAccount = "退会";
        const string license = "ライセンス";

        const string cansel = "キャンセル";





        async void SetActionSheet()
        {
            var str = bilogAvailable ? new string[] { accountSetting, takeOverCode, privacy, terms, license, beautyLogSetting, notificationSetting, deleteAccount, }
                : new string[] { accountSetting, takeOverCode, privacy, terms, license, notificationSetting, deleteAccount, };
            var result = await App.customNavigationPage.CurrentPage.DisplayActionSheet(null, cansel, null, str);

            //ここでConfigListViewModelと同様の処理を行う。

            //DependencyService.Get<IToast>().Show(ItemTitle);
            switch (result)
            {
                case accountSetting:
                    await App.customNavigationPage.PushAsync(new AccountRegistration(2));
                    break;
                case takeOverCode:
                    await App.customNavigationPage.PushAsync(new TransferIdPage());
                    break;
                case privacy:
                    await App.customNavigationPage.PushAsync(new WebViewPage(WebViewPage.webViewType.PrivacyPolicy));
                    break;
                case terms:
                    await App.customNavigationPage.PushAsync(new WebViewPage(WebViewPage.webViewType.TermsOfService));
                    break;
                case beautyLogSetting:
                    App.customNavigationPage.IsRunning = true;
                    var json = await APIManager.GET("setting_publish");
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        if (json == null)
                        {
                            DependencyService.Get<IToast>().Show("通信エラー");
                            App.customNavigationPage.IsRunning = false;
                        }
                        else
                        {
                            await App.customNavigationPage.PushAsync(new OpeningSetting(json));
                        }
                    });
                    break;
                case notificationSetting:
                    App.customNavigationPage.IsRunning = true;
                    var setting_json = await APIManager.GET("setting_notification");
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        if (setting_json == null)
                        {
                            DependencyService.Get<IToast>().Show("通信エラー");
                            App.customNavigationPage.IsRunning = false;
                        }
                        else
                        {
                            await App.customNavigationPage.PushAsync(new PushNotificationSetting(HomeResponseData, setting_json));
                        }
                    });
                    break;
                case deleteAccount:
                    var vm = new WithdrawalPageViewModel();
                    await App.customNavigationPage.PushAsync(new WithdrawalPage { BindingContext = vm });
                    break;
                case license:
                    await App.customNavigationPage.PushAsync(new WebViewPage(WebViewPage.webViewType.License));
                    break;
                default:
                    break;
            }
        }
        //double gridHeight, gridWidth;


        //予約についてここで設定
        void SetReservationStyle()
        {
            //var data = HomeData.GetResevationDataSample();
            var data = HomeResponseData.Data.ReservationList;
            //var dateString = data.FirstOrDefault().DateStr;
            //var reservationDate = DateManager.ConvertMessageDateStringToDateTime(dateString);
            if (HomeResponseData.Data.ReservationList != null && HomeResponseData.Data.ReservationList.Count != 0)
            {
                ReservationCondition = "予約中";

                //*********************//
                //ここでshippsに登録してある予約の日付を取得。
                //現状はもらったデータをそのまま表示する状態。
                //*********************//
                try
                {
                    ReservationDate = data.FirstOrDefault().DateStr + "〜";
                    //reservationDate.ToString("d")+ reservationDate.ToString("ddd")+reservationDate.ToString("t")+ "〜";
                    ReservedShopName = data.FirstOrDefault().SalonName;
                }
                catch
                {

                }
                if ((data.Count - 1) > 0)
                {
                    ReservationCount = "他" + (data.Count - 1) + "件";
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("  予約　他　0 件");
                    ReservationCount = null;
                }
            }
            else
            {
                ReservationCondition = "予約無し";
                ReservationDate = "現在予約はございません。";
                ReservedShopName = null;
                ReservationCount = null;
            }


            ReservationTap = ReservationCommand();

            MainTextColor = ColorList.colorFont;

            ReservationFontSizes = 11 * ReturnFontScale();
            CountFontSizes = ReservationFontSizes;
        }


        public void UpdateHomeSalon(ResponseHome response)
        {
            System.Diagnostics.Debug.WriteLine("  update home salon   from store regist  :" + response.Data.ToJson());
            HomeResponseData = response;
            bilogAvailable = HomeResponseData.Data.HomeSalonInfo.BilogAvailable == null
                                      ? false
                                      : (bool)HomeResponseData.Data.HomeSalonInfo.BilogAvailable;
            SetHomeSelectCommand(response);

            TitleText =
                "ホーム";

            ToolBarIcon =
            "Icon_DottedLine";
            ConfigVisible = false;

            for (int i = 0; i < 8; i++)
            {
                if (i < HomeResponseData.Data.MenuList.Count)
                {
                    ItemsSource[i].SetView(HomeResponseData.Data.MenuList[i], SetCommand((int)(HomeResponseData.Data.MenuList[i].MenuId)));

                }
                else
                {
                    ItemsSource[i].
                                  SetView(new InlineResponse2002DataMenuList()
                                  {
                                      MenuId = 0,
                                      IconImage = null,
                                      Name = null,
                                      Notification = null
                                  });
                }
            }
            SetReservationStyle();
            SetHomeShopInfo();
            SetCarouselImages();
            System.Diagnostics.Debug.WriteLine("  home  update   ");
        }





        void SetHomeSelectCommand(ResponseHome res)
        {
            HomeSelectCommand = new Command(async () =>
            {
                //単独店舗だった場合はホーム店舗を切り替えさせない
                if (res.Data.SalonCount <= 1)
                    return;

                var jsonSalonFavo = await APIManager.GET("salon_favoritelist");
                var responseSalonFavo = JsonConvert.DeserializeObject<ResponseFavoriteSalonList>(jsonSalonFavo);
                await App.customNavigationPage.PushAsync(new HomeSelect(responseSalonFavo, (int)res.Data.HomeSalonInfo.SalonId));
            });
        }


        //カルーセルのイメージをセットする
        void SetCarouselImages()
        {
            CarouselItem = new ObservableCollection<CarouselItems>();
            if (HomeResponseData == null
                || HomeResponseData.Data == null
                || HomeResponseData.Data.SlideList == null
                || !(HomeResponseData.Data.SlideList.Any())) return;
            foreach (var n in HomeResponseData.Data.SlideList)
            {
                CarouselItem.Add(new CarouselItems()
                {
                    CarouselImage = n.Path,
                    Comment = n.Message,
                    MainTextColor = MainTextColor,
                });
            }
        }


        //ホーム店舗の情報をセットする
        void SetHomeShopInfo()
        {
            //HomeThumbNail = "Icon_Hair.png";
            //HomeShopName = "Matsuoka 銀座店";
            HomeShopNameFontSizes = 18.0
                                      * ReturnFontScale();

            StylistNameVisible = true;

            StylistNameFontSizes = 11
                * ReturnFontScale();

            if (HomeResponseData.Data.HomeSalonInfo.Points != null)
            {
                PointsVisible = true;
                foreach (var n in HomeResponseData.Data.HomeSalonInfo.Points)
                {
                    //２つ目以降ポイントがあった場合にカンマを追加
                    if (!string.IsNullOrEmpty(HomeShopPoints))
                        HomeShopPoints = HomeShopPoints + ",";

                    //0ポイントだったら表示しない
                    if (n.Value != 0)
                        HomeShopPoints = n.Name + ":" + n.Value + n.Unit;
                }
            }
        }

        double ReturnFontScale()
        {
            if (ScaleManager.Scale < 1.0)
            {
                return ScaleManager.Scale * 0.8;
            }
            else
            {
                return 1.0;
            }
        }

        public AvailableImageStatus GetAvailableImageStatus()
        {
            if (HomeResponseData == null
               || HomeResponseData.Data == null
               || HomeResponseData.Data.HomeSalonInfo == null
               || HomeResponseData.Data.MenuList == null
                || !HomeResponseData.Data.MenuList.Any((x) => x.MenuId == HairSimulation))
                return AvailableImageStatus.Neutral;
            return AvailableImageStatus.AddHairSimulation;
        }

        private ResponseHome _HomeResponseData;
        public ResponseHome HomeResponseData
        {
            get { return _HomeResponseData; }
            set
            {
                if (_HomeResponseData != value)
                {
                    _HomeResponseData = value;

                    TitleText = _HomeResponseData.Data.HomeSalonInfo.Name;

                    HomeShopName = _HomeResponseData.Data.HomeSalonInfo.Name;

                    if (_HomeResponseData.Data.HomeSalonInfo.FavoriteStaffImage == null || string.IsNullOrEmpty(_HomeResponseData.Data.HomeSalonInfo.FavoriteStaffImage.Path))
                    {
                        try
                        {
                            HomeThumbNail = _HomeResponseData.Data.HomeSalonInfo.ThumbnailImage.Path;
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine("ex" + ex);
                        }
                    }
                    else
                    {
                        HomeThumbNail = _HomeResponseData.Data.HomeSalonInfo.FavoriteStaffImage.Path;
                    }
                    StylistName = _HomeResponseData.Data.HomeSalonInfo.FavoriteStaffName;
                    HomeShopTelNumber = _HomeResponseData.Data.HomeSalonInfo.Tel;


                    OnPropertyChanged("HomeShopTelNumber");
                    OnPropertyChanged("StylistName");
                    OnPropertyChanged("HomeThumbNail");
                    OnPropertyChanged("HomeShopName");
                    OnPropertyChanged("HomeShopName");
                    OnPropertyChanged("TitleText");

                    OnPropertyChanged("HomeData");
                }
            }
        }
        private string _ReservationCondition;
        public string ReservationCondition
        {
            get { return _ReservationCondition; }
            set
            {
                if (_ReservationCondition != value)
                {
                    _ReservationCondition = value;
                    OnPropertyChanged("ReservationCondition");
                }
            }
        }
        private ObservableCollection<CarouselItems> _CarouselItem;
        public ObservableCollection<CarouselItems> CarouselItem
        {
            get { return _CarouselItem; }
            set
            {
                if (_CarouselItem != value)
                {
                    _CarouselItem = value;
                    Device.BeginInvokeOnMainThread(() => OnPropertyChanged("CarouselItem"));
                }
            }
        }

        public class CarouselItems : ViewModelBase
        {
            private string _CarouselImage;
            public string CarouselImage
            {
                get { return _CarouselImage; }
                set
                {
                    if (_CarouselImage != value)
                    {
                        _CarouselImage = value;
                        OnPropertyChanged("CarouselImage");
                    }
                }
            }

            private string _Comment;
            public string Comment
            {
                get => _Comment;
                set
                {
                    if (_Comment != value)
                    {
                        _Comment = value;

                        CommentMaskVisible = string.IsNullOrEmpty(_Comment)
                                                   ? false : true;
                        OnPropertyChanged(nameof(Comment));
                    }
                }
            }

            private Color _MainTextColor;
            public Color MainTextColor
            {
                get => _MainTextColor;
                set
                {
                    if (_MainTextColor != value)
                    {
                        _MainTextColor = value;
                        OnPropertyChanged(nameof(MainTextColor));
                    }
                }
            }

            private bool _CommentMaskVisible;
            public bool CommentMaskVisible
            {
                get => _CommentMaskVisible;
                set
                {
                    if (_CommentMaskVisible != value)
                    {
                        _CommentMaskVisible = value;
                        OnPropertyChanged(nameof(CommentMaskVisible));
                    }
                }
            }
        }

        private Command _HomeSelectCommand;
        public Command HomeSelectCommand
        {
            get { return _HomeSelectCommand; }
            set
            {
                if (_HomeSelectCommand != value)
                {
                    _HomeSelectCommand = value;
                    OnPropertyChanged("HomeSelectCommand");
                }
            }
        }

        private Command _ReservationTap;
        public Command ReservationTap
        {
            get { return _ReservationTap; }
            set
            {
                if (_ReservationTap != value)
                {
                    _ReservationTap = value;
                    OnPropertyChanged("ReservationTap");
                }
            }
        }

        private string _ReservationDate;
        public string ReservationDate
        {
            get { return _ReservationDate; }
            set
            {
                if (_ReservationDate != value)
                {
                    _ReservationDate = value;
                    OnPropertyChanged("ReservationDate");
                }
            }
        }
        private string _ReservedShopName;
        public string ReservedShopName
        {
            get { return _ReservedShopName; }
            set
            {
                if (_ReservedShopName != value)
                {
                    _ReservedShopName = value;
                    OnPropertyChanged("ReservedShopName");
                }
            }
        }

        private string _ReservationCount;
        public string ReservationCount
        {
            get { return _ReservationCount; }
            set
            {
                if (_ReservationCount != value)
                {
                    _ReservationCount = value;
                    System.Diagnostics.Debug.WriteLine("ReservationCount   :" + ReservationCount);
                    OnPropertyChanged("ReservationCount");
                }
            }
        }

        private double _SliderHeight;
        public double SliderHeight
        {
            get => _SliderHeight;
            set
            {
                if (_SliderHeight != value)
                {
                    _SliderHeight = value;
                    OnPropertyChanged(nameof(SliderHeight));
                }
            }
        }

        public double ReservationFontSizes { get; set; }
        public double CountFontSizes { get; set; }
        public Color MainTextColor { get; set; }



        public string TitleText { get; set; }
        public ImageSource ToolBarIcon { get; set; }
        public string ToolbarText { get; set; }

        private Command _ToolbarCommand;
        public Command ToolbarCommand
        {
            get { return _ToolbarCommand; }
            set
            {
                if (_ToolbarCommand != value)
                {
                    _ToolbarCommand = value;
                    OnPropertyChanged("ToolbarCommand");
                }
            }
        }

        private ImageSource _TitleImageSource;
        public ImageSource TitleImageSource
        {
            get
            {
                return _TitleImageSource;
            }
            set
            {
                if (_TitleImageSource != value)
                {

                    _TitleImageSource = value;
                    System.Diagnostics.Debug.WriteLine("googleBtn.png set");
                    OnPropertyChanged("TitleImageSource");
                }
            }
        }


        private ImageSource _HomeThumbNail;
        public ImageSource HomeThumbNail
        {
            get { return _HomeThumbNail; }
            set
            {
                if (_HomeThumbNail != value)
                {
                    _HomeThumbNail = value;
                    OnPropertyChanged("HomeThumbNail");
                }
            }
        }
        private string _HomeShopName;
        public string HomeShopName
        {
            get { return _HomeShopName; }
            set
            {
                if (_HomeShopName != value)
                {
                    _HomeShopName = value;
                    OnPropertyChanged("HomeShopName");
                }
            }
        }

        public double HomeShopNameFontSizes { get; set; }

        private double _ThumnNailSize;
        public double ThumnNailSize
        {
            get { return _ThumnNailSize; }
            set
            {
                if (_ThumnNailSize != value)
                {
                    _ThumnNailSize = value;
                    OnPropertyChanged("ThumnNailSize");
                }
            }
        }

        private string _StylistName;
        public string StylistName
        {
            get { return _StylistName; }
            set
            {
                if (_StylistName != value)
                {
                    _StylistName = value;
                    OnPropertyChanged("StylistName");
                }
            }
        }
        private bool _StylistNameVisible;
        public bool StylistNameVisible
        {
            get { return _StylistNameVisible; }
            set
            {
                if (_StylistNameVisible != value)
                {
                    _StylistNameVisible = value;
                    OnPropertyChanged("StylistNameVisible");
                }
            }
        }
        public double StylistNameFontSizes { get; set; }


        private string _HomeShopPoints;
        public string HomeShopPoints
        {
            get { return _HomeShopPoints; }
            set
            {
                if (_HomeShopPoints != value)
                {
                    _HomeShopPoints = value;
                    OnPropertyChanged("HomeShopPoints");
                }
            }
        }

        private bool _PointsVisible;
        public bool PointsVisible
        {
            get { return _PointsVisible; }
            set
            {
                if (_PointsVisible != value)
                {
                    _PointsVisible = value;
                    OnPropertyChanged("PointsVisible");
                }
            }
        }


        private string _HomeShopTelNumber;
        public string HomeShopTelNumber
        {
            get { return _HomeShopTelNumber; }
            set
            {
                if (_HomeShopTelNumber != value)
                {
                    _HomeShopTelNumber = value;
                    OnPropertyChanged("HomeShopTelNumber");
                }
            }
        }




        private double _ScreenSizeScale;
        public double ScreenSizeScale
        {
            get { return _ScreenSizeScale; }
            set
            {
                if (_ScreenSizeScale != value)
                {
                    _ScreenSizeScale = value;
                    OnPropertyChanged("ScreenSizeScale");
                }
            }
        }

        private bool _ConfigVisible;
        public bool ConfigVisible
        {
            get { return _ConfigVisible; }
            set
            {
                if (_ConfigVisible != value)
                {
                    _ConfigVisible = value;
                    OnPropertyChanged("ConfigVisible");
                }
            }
        }

        private ConfigListVIewModel _ConfigListBC;
        public ConfigListVIewModel ConfigListBC
        {
            get => _ConfigListBC;
            set
            {
                if (_ConfigListBC != value)
                {
                    _ConfigListBC = value;
                    OnPropertyChanged(nameof(ConfigListBC));
                }
            }
        }

        private ObservableCollection<HomeButtonViewViewModel> _ItemsSource;
        public ObservableCollection<HomeButtonViewViewModel> ItemsSource
        {
            get
            {
                if (_ItemsSource == null)
                    _ItemsSource = new ObservableCollection<HomeButtonViewViewModel>();
                return _ItemsSource;
            }
            set
            {
                if (_ItemsSource == null)
                    _ItemsSource = new ObservableCollection<HomeButtonViewViewModel>();
                if (_ItemsSource != value)
                {
                    _ItemsSource = value;
                    OnPropertyChanged("ItemsSource");
                }
            }
        }


        private HomeButtonViewViewModel _HomeButton1_1_BC;
        public HomeButtonViewViewModel HomeButton1_1_BC
        {
            get
            {
                if (_HomeButton1_1_BC == null)
                    _HomeButton1_1_BC = new HomeButtonViewViewModel();
                return _HomeButton1_1_BC;
            }
            set
            {
                if (_HomeButton1_1_BC == null)
                    _HomeButton1_1_BC = new HomeButtonViewViewModel();

                if (_HomeButton1_1_BC != value)
                {
                    _HomeButton1_1_BC = value;
                    OnPropertyChanged("HomeButton1_1_BC");
                }
            }
        }
        private HomeButtonViewViewModel _HomeButton1_3_BC;
        public HomeButtonViewViewModel HomeButton1_3_BC
        {
            get
            {
                if (_HomeButton1_3_BC == null)
                    _HomeButton1_3_BC = new HomeButtonViewViewModel();
                return _HomeButton1_3_BC;
            }
            set
            {
                if (_HomeButton1_3_BC == null)
                    _HomeButton1_3_BC = new HomeButtonViewViewModel();
                if (_HomeButton1_3_BC != value)
                {
                    _HomeButton1_3_BC = value;
                    OnPropertyChanged("HomeButton1_3_BC");
                }
            }
        }
        private HomeButtonViewViewModel _HomeButton1_5_BC;
        public HomeButtonViewViewModel HomeButton1_5_BC
        {

            get
            {
                if (_HomeButton1_5_BC == null)
                    _HomeButton1_5_BC = new HomeButtonViewViewModel();
                return _HomeButton1_5_BC;
            }
            set
            {
                if (_HomeButton1_5_BC == null)
                    _HomeButton1_5_BC = new HomeButtonViewViewModel();
                if (_HomeButton1_5_BC != value)
                {
                    _HomeButton1_5_BC = value;
                    OnPropertyChanged("HomeButton1_5_BC");
                }
            }
        }
        private HomeButtonViewViewModel _HomeButton1_7_BC;
        public HomeButtonViewViewModel HomeButton1_7_BC
        {
            get
            {
                if (_HomeButton1_7_BC == null)
                    _HomeButton1_7_BC = new HomeButtonViewViewModel();
                return _HomeButton1_7_BC;
            }
            set
            {
                if (_HomeButton1_7_BC == null)
                    _HomeButton1_7_BC = new HomeButtonViewViewModel();
                if (_HomeButton1_7_BC != value)
                {
                    _HomeButton1_7_BC = value;
                    OnPropertyChanged("HomeButton1_7_BC");
                }
            }
        }
        private HomeButtonViewViewModel _HomeButton3_1_BC;
        public HomeButtonViewViewModel HomeButton3_1_BC
        {
            get
            {
                if (_HomeButton3_1_BC == null)
                    _HomeButton3_1_BC = new HomeButtonViewViewModel();
                return _HomeButton3_1_BC;
            }
            set
            {
                if (_HomeButton3_1_BC == null)
                    _HomeButton3_1_BC = new HomeButtonViewViewModel();
                if (_HomeButton3_1_BC != value)
                {
                    _HomeButton3_1_BC = value;
                    OnPropertyChanged("HomeButton3_1_BC");
                }
            }
        }
        private HomeButtonViewViewModel _HomeButton3_3_BC;
        public HomeButtonViewViewModel HomeButton3_3_BC
        {
            get
            {
                if (_HomeButton3_3_BC == null)
                    _HomeButton3_3_BC = new HomeButtonViewViewModel();
                return _HomeButton3_3_BC;
            }
            set
            {
                if (_HomeButton3_3_BC == null)
                    _HomeButton3_3_BC = new HomeButtonViewViewModel();
                if (_HomeButton3_3_BC != value)
                {
                    _HomeButton3_3_BC = value;
                    OnPropertyChanged("HomeButton3_3_BC");
                }
            }
        }
        private HomeButtonViewViewModel _HomeButton3_5_BC;
        public HomeButtonViewViewModel HomeButton3_5_BC
        {
            get
            {
                if (_HomeButton3_5_BC == null)
                    _HomeButton3_5_BC = new HomeButtonViewViewModel();
                return _HomeButton3_5_BC;
            }
            set
            {
                if (_HomeButton3_5_BC == null)
                    _HomeButton3_5_BC = new HomeButtonViewViewModel();
                if (_HomeButton3_5_BC != value)
                {
                    _HomeButton3_5_BC = value;
                    OnPropertyChanged("HomeButton3_5_BC");
                }
            }
        }
        private HomeButtonViewViewModel _HomeButton3_7_BC;
        public HomeButtonViewViewModel HomeButton3_7_BC
        {
            get
            {
                if (_HomeButton3_7_BC == null)
                    _HomeButton3_7_BC = new HomeButtonViewViewModel();
                return _HomeButton3_7_BC;
            }
            set
            {
                if (_HomeButton3_7_BC == null)
                    _HomeButton3_7_BC = new HomeButtonViewViewModel();
                if (_HomeButton3_7_BC != value)
                {
                    _HomeButton3_7_BC = value;
                    OnPropertyChanged("HomeButton3_7_BC");
                }
            }

        }

        void UpdateHomeVM()
        {
            Task.Run(async () =>
            {
                //homeのバッジ更新処理
                var jsonResponse = await APIManager.GET("home");
                var responseHome = JsonManager.Deserialize<ResponseHome>(jsonResponse);
                UpdateHomeSalon(responseHome);
            });
        }
        Command ReservationCommand()
        {
            return new Command(async () =>
            {
                if (!App.ProcessManager.CanInvoke())
                    return;
                // TODO Xamarin.Forms.Device.RuntimePlatform is no longer supported. Use Microsoft.Maui.Devices.DeviceInfo.Platform instead. For more details see https://learn.microsoft.com/en-us/dotnet/maui/migration/forms-projects#device-changes
                if (Device.RuntimePlatform == Device.Android)
                {
                    GASCall.Track_App_Page("Android_予約");
                }
                else
                {
                    GASCall.Track_App_Page("iOS_予約");
                }

                if (HomeResponseData.Data.SalonCount > 1 || HomeResponseData.Data.ReservationList.Count() > 0)
                {
                    await App.customNavigationPage.PushAsync(new ReservationTop(HomeResponseData));
                }
                else
                {
                    await SetHomeReservation();
                }
                UpdateHomeVM();
                App.ProcessManager.OnComplete();
            });
        }

        //ホームメニューのボタンを押された場合の処理
        public const int Reservation = 1, News = 2, Coupon = 3, Message = 4, StoreList = 5, History = 6, HairSimulation = 7, Bolg = 8;

        public Command SetCommand(int menuid)
        {
            bool soloSalon = HomeResponseData.Data.SalonCount > 1 ? false : true;
            switch (menuid)
            {
                case Reservation:
                    return ReservationCommand();
                    break;
                case News:
                    var NewsCommand = new Command(async () =>
                    {
                        if (!App.ProcessManager.CanInvoke())
                            return;
                        // TODO Xamarin.Forms.Device.RuntimePlatform is no longer supported. Use Microsoft.Maui.Devices.DeviceInfo.Platform instead. For more details see https://learn.microsoft.com/en-us/dotnet/maui/migration/forms-projects#device-changes
                        if (Device.RuntimePlatform == Device.Android)
                        {
                            GASCall.Track_App_Page("Android_お知らせ");
                        }
                        else
                        {
                            GASCall.Track_App_Page("iOS_お知らせ");
                        }


                        await App.customNavigationPage.PushAsync(new NoticeList(HomeResponseData.Data.HomeSalonInfo.Name, HomeResponseData.Data.HomeSalonInfo.SalonId, soloSalon));
                        UpdateHomeVM();
                        App.ProcessManager.OnComplete();
                    });
                    return NewsCommand;

                case Coupon:
                    var CouponCommand = new Command(async () =>
                    {
                        if (!App.ProcessManager.CanInvoke())
                            return;
                        // TODO Xamarin.Forms.Device.RuntimePlatform is no longer supported. Use Microsoft.Maui.Devices.DeviceInfo.Platform instead. For more details see https://learn.microsoft.com/en-us/dotnet/maui/migration/forms-projects#device-changes
                        if (Device.RuntimePlatform == Device.Android)
                        {
                            GASCall.Track_App_Page("Android_クーポン");
                        }
                        else
                        {
                            GASCall.Track_App_Page("iOS_クーポン");
                        }
                        //responseからホームサロン名は取得して変更するので現在はサンプル対応
                        await App.customNavigationPage.PushAsync(new CouponListPage(HomeResponseData.Data.HomeSalonInfo.Name, HomeResponseData.Data.HomeSalonInfo.SalonId, soloSalon));
                        UpdateHomeVM();
                        App.ProcessManager.OnComplete();
                    });
                    return CouponCommand;
                case Message:
                    var MessageCommand = new Command(async () =>
                    {
                        if (!App.ProcessManager.CanInvoke())
                            return;
                        // TODO Xamarin.Forms.Device.RuntimePlatform is no longer supported. Use Microsoft.Maui.Devices.DeviceInfo.Platform instead. For more details see https://learn.microsoft.com/en-us/dotnet/maui/migration/forms-projects#device-changes
                        if (Device.RuntimePlatform == Device.Android)
                        {
                            GASCall.Track_App_Page("Android_メッセージ");
                        }
                        else
                        {
                            GASCall.Track_App_Page("iOS_メッセージ");
                        }
                        await App.customNavigationPage.PushAsync(new MessageMainPage(HomeResponseData.Data.HomeSalonInfo.SalonId, HomeResponseData.Data.HomeSalonInfo.Name, null, soloSalon, GetAvailableImageStatus()));

                        UpdateHomeVM();
                        App.ProcessManager.OnComplete();
                    });
                    return MessageCommand;
                case StoreList:
                    var StoreListCommand = new Command(async () =>
                    {
                        if (!App.ProcessManager.CanInvoke())
                            return;
                        // TODO Xamarin.Forms.Device.RuntimePlatform is no longer supported. Use Microsoft.Maui.Devices.DeviceInfo.Platform instead. For more details see https://learn.microsoft.com/en-us/dotnet/maui/migration/forms-projects#device-changes
                        if (Device.RuntimePlatform == Device.Android)
                        {
                            GASCall.Track_App_Page("Android_店舗一覧");
                        }
                        else
                        {
                            GASCall.Track_App_Page("iOS_店舗一覧");
                        }

                        //複数店舗の場合
                        if (HomeResponseData.Data.SalonCount > 1)
                        {
                            var parameters = new Dictionary<string, string> { };
                            var json = await APIManager.GET("salon_regionlist", parameters);
                            if (json == null)
                            {
                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    DependencyService.Get<IToast>().Show("通信エラー");
                                });
                            }
                            else
                            {
                                var response = JsonConvert.DeserializeObject<ResponseSalonRegionList>(json);
                                if (response.Data != null)
                                {
                                    await App.customNavigationPage.PushAsync(new StoreAreaSelect(2, response));
                                }
                                else
                                {
                                    await App.customNavigationPage.PushAsync(new StoreListPage(null));
                                }
                            }
                        }
                        //単独店舗の場合
                        else
                        {
                            await App.customNavigationPage.PushAsync(new StoreInformationPage(HomeResponseData.Data.HomeSalonInfo.SalonId, soloSalon));
                        }

                        App.ProcessManager.OnComplete();
                    });
                    return StoreListCommand;
                case History:
                    var HistoryCommand = new Command(async () =>
                    {
                        if (!App.ProcessManager.CanInvoke())
                            return;
                        // TODO Xamarin.Forms.Device.RuntimePlatform is no longer supported. Use Microsoft.Maui.Devices.DeviceInfo.Platform instead. For more details see https://learn.microsoft.com/en-us/dotnet/maui/migration/forms-projects#device-changes
                        if (Device.RuntimePlatform == Device.Android)
                        {
                            GASCall.Track_App_Page("Android_履歴");
                        }
                        else
                        {
                            GASCall.Track_App_Page("iOS_履歴");
                        }
                        await App.customNavigationPage.PushAsync(new HistoryTop(bilogAvailable));
                        App.ProcessManager.OnComplete();
                    });
                    return HistoryCommand;
                case HairSimulation:
                    var HairSimulationCommand = new Command(async () =>
                    {
                        if (!App.ProcessManager.CanInvoke())
                            return;
                        // TODO Xamarin.Forms.Device.RuntimePlatform is no longer supported. Use Microsoft.Maui.Devices.DeviceInfo.Platform instead. For more details see https://learn.microsoft.com/en-us/dotnet/maui/migration/forms-projects#device-changes
                        if (Device.RuntimePlatform == Device.Android)
                        {
                            GASCall.Track_App_Page("Android_ヘアシミュ");
                        }
                        else
                        {
                            GASCall.Track_App_Page("iOS_ヘアシミュ");
                        }
                        await App.customNavigationPage.PushAsync(new HairSimulationStartPage());
                        App.ProcessManager.OnComplete();
                    });
                    return HairSimulationCommand;
                case Bolg:
                    var BolgCommand = new Command(async () =>
                    {
                        if (!App.ProcessManager.CanInvoke())
                            return;
                        // TODO Xamarin.Forms.Device.RuntimePlatform is no longer supported. Use Microsoft.Maui.Devices.DeviceInfo.Platform instead. For more details see https://learn.microsoft.com/en-us/dotnet/maui/migration/forms-projects#device-changes
                        if (Device.RuntimePlatform == Device.Android)
                        {
                            GASCall.Track_App_Page("Android_ブログ");
                        }
                        else
                        {
                            GASCall.Track_App_Page("iOS_ブログ");
                        }

                        try
                        {
                            var dic = new Dictionary<string, string> {
                                {"storeId",HomeResponseData.Data.HomeSalonInfo.SalonId.ToString()}
                            };

                            var json = await APIManager.GET("blog_url_get", dic);
                            var response = JsonManager.Deserialize<ResponseBlogUrlGet>(json);
                            if (response == null || string.IsNullOrEmpty(response.Data.BlogUrl))
                            {
                                await App.customNavigationPage.CurrentPage.DisplayAlert("通信エラー", "ブログページのURLを取得できませんでした。", "OK");
                            }
                            else
                            {

                                DependencyService.Get<IWebBrowserService>().Open(new Uri(response.Data.BlogUrl));
                            }

                        }
                        catch
                        {
                            await App.customNavigationPage.CurrentPage.DisplayAlert("エラー", "ブログページのURLが有効ではりません。", "OK");
                        }
                        App.ProcessManager.OnComplete();
                    });
                    return BolgCommand;
                default:
                    App.ProcessManager.OnComplete();
                    return null;

            }
            return null;
        }


        async Task SetHomeReservation()
        {
            var data = HomeResponseData.Data.HomeSalonInfo;

            var content = new ReservationContentInfo()
            {
                SalonId = data.SalonId,
                StoreName = data.Name,
            };

            //お気に入りスタッフがいた場合
            if (data.FavoriteStaffId != null && data.FavoriteStaffId != 0)
            {
                content.StaffId = data.FavoriteStaffId;
                content.StaffName = data.FavoriteStaffName;

                try
                {
                    string action = "reservation_menulist";
                    var parameters = new Dictionary<string, string> {
                            { "deviceId", Config.Instance.Data.deviceId },
                            { "staffId", content.StaffId.ToString() },
                            {"salonId", content.SalonId.ToString()}
                        };
                    var apiRet = await APIManager.Post(action, parameters);
                    if (apiRet != null)
                    {
                        var response = JsonManager.Deserialize<ResponseReservationMenu>(apiRet);
                        if (response != null)
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                await App.customNavigationPage.PushAsync(new ReservationMenuList(response, content));
                            });
                        }
                        else
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                await App.customNavigationPage.Navigation.NavigationStack.Last().DisplayAlert("エラー", "読み込みに失敗しました。通信環境の良い場所で再度選択してください。", "OK");
                            });
                        }
                    }

                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("ex :" + ex);
                }
            }
            else
            {
                try
                {
                    System.Diagnostics.Debug.WriteLine("スタッフリストページ");

                    string action = "reservation_stafflist";
                    var parameters = new Dictionary<string, string> {
                            { "deviceId", Config.Instance.Data.deviceId },
                            { "salonId", content.SalonId.ToString() }
                        };
                    var apiRet = await APIManager.Post(action, parameters);
                    if (apiRet != null)
                    {
                        var response = JsonManager.Deserialize<ResponseStaffList>(apiRet);

                        if (response.Status == 0 && response.Data == null)
                        {
                            await App.Current.MainPage.DisplayAlert("エラー", "該当店舗にスタッフが登録されていません。", "OK");
                        }
                        else if (response != null)
                        {

                            //if (Device.RuntimePlatform == Device.iOS)
                            //{
                            await App.customNavigationPage.PushAsync(new ReservationSelectStaff(response, content, 1));
                            //}
                            //else
                            //{

                            //  await App.customNavigationPage.PushAsync(new ReservationSelectStaff_Droid(response, content, 1));
                            //}
                        }
                        else
                        {
                            //DependencyService.Get<IToast>().Show("読み込みに失敗しました。通信環境の良い場所で再度選択してください。");
                            await App.Current.MainPage.DisplayAlert("エラー", "読み込みに失敗しました。", "OK");
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("   ex  :" + ex);
                    //DependencyService.Get<IToast>().Show("読み込みに失敗しました。通信環境の良い場所で再度選択してください。");
                    await App.Current.MainPage.DisplayAlert("エラー", "スタッフ情報の読み込みに失敗しました。通信環境の良い場所で再度選択してください。", "OK");
                }

            }
        }
    }
}
