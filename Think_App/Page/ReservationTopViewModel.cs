using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using IO.Swagger.Model;
namespace Think_App
{
    public class ReservationTopViewModel : ViewModelBase
    {
        public ReservationTopViewModel(ResponseHome responseHomeData, Guid pageId)
        {
            PageId = pageId;
            ResponseHomeDatas = responseHomeData;

            //単独店舗の場合は他の店舗から予約するのボタンを表示しない
            if (responseHomeData.Data.SalonCount > 1)
            {
                ReservationOtherSalon = true;
            }
            else
            {
                ReservationOtherSalon = false;
            }

            ReservationList = new ObservableCollection<ReservationItems>();
            InitializeList();

            SetFavoriteStaffId();

            StartReservation();

        }

        void SetListEmpty()
        {
            ReservationList = null;

            ListHeight = 0;

            PageBGColor = ColorList.colorWhite;

            BackgroundImageVisible = true;

            BGImageSource = "ReservationTopBgImg.png";
        }

        void SetListNotEmpty(int listCount)
        {
            PageBGColor = ColorList.colorBackground;

            ListHeight = listCount * 84.0 + 5;
            BackgroundImageVisible = false;
            BGImageSource = null;
        }

        void SetListHeight()
        {
            Device.BeginInvokeOnMainThread(() =>
                            {
                                PageBGColor = ColorList.colorBackground;

                                ListHeight = ReservationList.Count * 84.0 + 5;

                                BackgroundImageVisible = false;
                            });
        }

        //TopPageを開いたときはリスト初期化
        public void InitializeList()
        {
            Task.Run(async () =>
                        {
                            try
                            {
                                var dataJson = await APIManager.GET("reservation_list");
                                var data = JsonManager.Deserialize<ResponseReservationList>(dataJson);
                                if (data.Status == 0 && data.Data != null)
                                {
                                    if (data.Data.List != null && data.Data.List.Count != 0)
                                    {

                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            foreach (var n in data.Data.List)
                                            {
                                                if (ReservationList != null)
                                                {
                                                    ReservationList.Add(new ReservationItems(n));
                                                }
                                            }

                                            SetListNotEmpty(data.Data.List.Count);
                                        });
                                    }
                                    else
                                    {
                                        Device.BeginInvokeOnMainThread(() => SetListEmpty());

                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine("ex  :" + ex);
                            }
                        });

        }

        //Detailなどからリスト項目の削除など、アップデートがあった場合
        public async Task UpdateList()
        {
            await Task.Run(async () =>
                        {
                            try
                            {
                                var dataJson = await APIManager.GET("reservation_list");
                                var data = JsonManager.Deserialize<ResponseReservationList>(dataJson);
                                if (data.Status == 0 && data.Data != null)
                                {
                                    if (data.Data.List != null && data.Data.List.Count != 0)
                                    {
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            for (int i = 0; i < data.Data.List.Count; i++)
                                            {
                                                if (ReservationList[i] != null)
                                                {
                                                    ReservationList[i].UpdateReservationItem(data.Data.List[i]);
                                                }
                                                else
                                                {
                                                    ReservationList.Add(new ReservationItems(data.Data.List[i]));
                                                }
                                            }
                                            if (ReservationList.Count > data.Data.List.Count)
                                            {
                                                ReservationList.RemoveAt((ReservationList.Count - 1));
                                            }

                                            SetListNotEmpty(data.Data.List.Count);
                            });
                                    }
                                    else
                                    {
                                        //複数店舗の場合はトップページを表示する
                                        if (ResponseHomeDatas.Data.SalonCount > 1)
                                        {
                                            SetListEmpty();
                                        }
                                        //単独エンポの場合
                                        else
                                        {
                                            Device.BeginInvokeOnMainThread(async () =>
                                            {
                                                await App.customNavigationPage.Navigation.PopAsync();
                                            });
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine("ex  :" + ex);
                            }
                        });
        }

        async void SetFavoriteStaffId()
        {
            if (ResponseHomeDatas.Data.HomeSalonInfo.StaffList != null || ResponseHomeDatas.Data.HomeSalonInfo.StaffList.Count != 0)
            {
                var list = ResponseHomeDatas.Data.HomeSalonInfo.StaffList.Where((arg) => arg.IsFavorite == true);
                if (list == null || list.Count() == 0 || list.First().StaffId == null)
                {
                    System.Diagnostics.Debug.WriteLine("SetFavoriteStaffId   == 0 ");
                    FavoriteStaffId = 0;
                }
                else
                {
                    FavoriteStaffId = (int)(list.First().StaffId);

                    System.Diagnostics.Debug.WriteLine("SetFavoriteStaffId   ==  :" + FavoriteStaffId);
                }
            }
        }

        //予約を始めるボタンのコマンドを設定
        void StartReservation()
        {
            SelectOtherStoreCommand = new Command(async () =>
            {
                if (App.ProcessManager.CanInvoke())
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
                        var response = JsonManager.Deserialize<ResponseSalonRegionList>(json);

                        if (response.Data != null)
                        {
                            await App.customNavigationPage.PushAsync(new StoreAreaSelect(3, response));
                        }
                        else
                        {
                            await App.customNavigationPage.PushAsync(new StoreSelect(4, null));
                        }
                    }
                    App.ProcessManager.OnComplete();
                }
            });

            SelectHomeCommand = new Command(async () =>
            {
                if (App.ProcessManager.CanInvoke())
                {

                    var content = new ReservationContentInfo()
                    {
                        SalonId = ResponseHomeDatas.Data.HomeSalonInfo.SalonId,
                        StoreName = ResponseHomeDatas.Data.HomeSalonInfo.Name,
                    };
                    System.Diagnostics.Debug.WriteLine("favorite staff" + FavoriteStaffId);
                    //お気に入りスタッフが登録されていない場合
                    if (FavoriteStaffId == 0)
                    {
                        System.Diagnostics.Debug.WriteLine("staff id == 0");
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

                                    //	await App.customNavigationPage.PushAsync(new ReservationSelectStaff_Droid(response, content, 1));
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

                        App.customNavigationPage.IsRunning = false;
                    }
                    //お気に入りスタッフが登録されている場合
                    else
                    {
                        content.StaffName = ResponseHomeDatas.Data.HomeSalonInfo.FavoriteStaffName;
                        content.StaffId = ResponseHomeDatas.Data.HomeSalonInfo.FavoriteStaffId;

                        System.Diagnostics.Debug.WriteLine("staff id  :" + FavoriteStaffId);

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
                    App.ProcessManager.OnComplete();
                }
            });
        }

        //予約の詳細ページに遷移@
        async void SelectReservation(ReservationItems item)
        {
            if (App.ProcessManager.CanInvoke())
                try
                {
                    string action = "reservation__detail";
                    var parameters = new Dictionary<string, string> {
                            { "deviceId", Config.Instance.Data.deviceId },
                    { "sipssStoreId", item.SipssStoreId},
                    {"sipssCompanyId", item.SipssCompanyId},
                    {"reservationId",item.ResevertionID}
                        };
                    var apiRet = await APIManager.Post(action, parameters);
                    if (apiRet != null)
                    {
                        var response = JsonManager.Deserialize<IO.Swagger.Model.ReservationDetail>(apiRet);

                        var page = new ReservationDetail(response, PageId);
                        await App.customNavigationPage.PushAsync(page);


                    }
                }
                catch (Exception ex)
                {

                    System.Diagnostics.Debug.WriteLine(" ex  :" + ex);
                }
            App.ProcessManager.OnComplete();
        }
        public Guid PageId { get; set; }


        private Command _SelectHomeCommand;
        public Command SelectHomeCommand
        {
            get { return _SelectHomeCommand; }
            set
            {
                if (_SelectHomeCommand != value)
                {
                    _SelectHomeCommand = value;
                    OnPropertyChanged("SelectHomeCommand");
                }
            }
        }

        //他の店舗で予約をはじめるボタンのビジブル
        public bool ReservationOtherSalon { get; set; }


        public int FavoriteStaffId { get; set; }

        private ResponseHome _ResponseHomeDatas;
        public ResponseHome ResponseHomeDatas
        {
            get { return _ResponseHomeDatas; }
            set
            {
                if (_ResponseHomeDatas != value)
                {
                    _ResponseHomeDatas = value;

                }
            }
        }



        private Command _SelectOtherStoreCommand;
        public Command SelectOtherStoreCommand
        {
            get { return _SelectOtherStoreCommand; }
            set
            {
                if (_SelectOtherStoreCommand != value)
                {
                    _SelectOtherStoreCommand = value;
                    OnPropertyChanged("SelectOtherStoreCommand");
                }
            }
        }

        private Color _PageBGColor;
        public Color PageBGColor
        {
            get { return _PageBGColor; }
            set
            {
                if (_PageBGColor != value)
                {
                    _PageBGColor = value;
                    OnPropertyChanged("PageBGColor");
                }
            }
        }

        private ObservableCollection<ReservationItems> _ReservationList;
        public ObservableCollection<ReservationItems> ReservationList
        {
            get
            {
                if (_ReservationList == null)
                {
                    _ReservationList = new ObservableCollection<ReservationItems>();
                }
                return _ReservationList;
            }

            set
            {
                if (_ReservationList == null)
                {
                    _ReservationList = new ObservableCollection<ReservationItems>();
                }
                if (_ReservationList != value)
                {
                    _ReservationList = value;
                    OnPropertyChanged("ReservationList");
                }
            }
        }


        private bool _TextVisible;
        public bool TextVisible
        {
            get { return _TextVisible; }
            set
            {
                if (_TextVisible != value)
                {
                    _TextVisible = value;
                    OnPropertyChanged("TextVisible");
                }
            }
        }


        private bool _BackgroundImageVisible;
        public bool BackgroundImageVisible
        {
            get { return _BackgroundImageVisible; }
            set
            {

                _BackgroundImageVisible = value;
                TextVisible = !value;
                OnPropertyChanged("TextVisible");

                OnPropertyChanged("BackgroundImageVisible");

            }
        }
        private ReservationItems _SelectedReservation;
        public ReservationItems SelectedReservation
        {
            get { return _SelectedReservation; }
            set
            {
                if (_SelectedReservation != value)
                {
                    _SelectedReservation = value;
                    SelectReservation(value);
                    _SelectedReservation = null;
                    OnPropertyChanged("SelectedReservation");
                }
            }
        }

        private double _ListHeight;
        public double ListHeight
        {
            get { return _ListHeight; }
            set
            {
                if (_ListHeight != value)
                {
                    _ListHeight = value;
                    OnPropertyChanged("ListHeight");
                }
            }
        }




        private ImageSource _BGImageSource;
        public ImageSource BGImageSource
        {
            get { return _BGImageSource; }
            set
            {
                if (_BGImageSource != value)
                {
                    _BGImageSource = value;
                    OnPropertyChanged("BGImageSource");
                }
            }
        }

        public class ReservationItems : ViewModelBase
        {

            public ReservationItems(string sippsStoreId, string sippsCompanyId, string id, string date, string shop)
            {
                SipssStoreId = sippsStoreId;

                SipssCompanyId = sippsCompanyId;

                ResevertionID = id;

                ReservationDate = date + "〜";

                ReservationShopName = shop;
            }


            public void UpdateReservationItem(InlineResponse2002DataReservationList data)
            {
                SipssStoreId = data.SipssStoreId;

                SipssCompanyId = data.SipssCompanyId;

                ResevertionID = data.ReservationId;

                ReservationDate = data.DateStr + "〜";

                ReservationShopName = data.SalonName;
            }


            public ReservationItems(InlineResponse2002DataReservationList data) : this(data.SipssStoreId, data.SipssCompanyId, data.ReservationId, data.DateStr, data.SalonName)
            {

            }

            public string SipssStoreId { get; set; }
            public string SipssCompanyId { get; set; }


            public string ResevertionID { get; set; }

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
            private string _ReservationShopName;
            public string ReservationShopName

            {
                get { return _ReservationShopName; }
                set
                {
                    if (_ReservationShopName != value)
                    {
                        _ReservationShopName = value;
                        OnPropertyChanged("ReservationShopName");
                    }
                }
            }


        }
    }
}