using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using IO.Swagger.Model;
using Newtonsoft.Json;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
    public class ReservationSelectStaffViewModel : ViewModelBase
    {
        public ReservationSelectStaffViewModel(ResponseStaffList stafflist, ReservationContentInfo content, int ReservationType)
        {

            responseStaffList = stafflist;
            if (responseStaffList.Data.List == null || responseStaffList.Data.List.Count == 0)
            {
                return;
            }
            foreach (var n in responseStaffList.Data.List)
            {
                //var item = new StaffListItem(n, content);
                //item.SetStaffSelectCommand(SetDetail(n, content));
                StaffList.Add(new StaffListItem(n, content, ReservationType));

            }

            foreach (var n in StaffList)
            {
                System.Diagnostics.Debug.WriteLine("" + n.StaffData.Name);

                System.Diagnostics.Debug.WriteLine("" + n.StaffData.StaffId);
            }

            StaffListHeight = StaffList.Count * StaffList[0].StaffListItemHeight;
            //StaffListRowHeight = StaffList[0].StaffListItemHeight;

            System.Diagnostics.Debug.WriteLine("StaffListHeight :" + StaffListHeight);
        }
        public const string shimeinasi = "指名なし";
        private double _StaffListRowHeight;
        public double StaffListRowHeight
        {
            get { return _StaffListRowHeight; }
            set
            {
                if (_StaffListRowHeight != value)
                {
                    _StaffListRowHeight = value;
                    OnPropertyChanged("StaffListRowHeight");
                }
            }
        }


        public ResponseStaffList responseStaffList { get; set; }


        private double _StaffListHeight;
        public double StaffListHeight
        {
            get { return _StaffListHeight; }
            set
            {
                if (_StaffListHeight != value)
                {
                    _StaffListHeight = value;
                    OnPropertyChanged("StaffListHeight");
                }
            }
        }

        private ObservableCollection<StaffListItem> _StaffList;
        public ObservableCollection<StaffListItem> StaffList
        {
            get
            {
                if (_StaffList == null)
                {
                    _StaffList = new ObservableCollection<StaffListItem>();
                }
                return _StaffList;
            }
            set
            {
                if (_StaffList == null)
                {
                    _StaffList = new ObservableCollection<StaffListItem>();
                }

                if (_StaffList != value)
                {
                    _StaffList = value;
                    OnPropertyChanged("StaffList");
                }
            }
        }


        public class StaffListItem : ViewModelBase
        {

            public StaffListItem(InlineResponse2009DataList data, ReservationContentInfo content, int ReservationType)
            {
                StaffData = data;
                ReservationContent = content;

                if (ScaleManager.Scale > 1.0)
                {
                    StaffListItemHeight = 223;
                    ThumbNailSize = 135.0;
                    WhiteBtnWidth = 97;
                    WhiteBtnHight = 28;
                }
                else
                {
                    ThumbNailSize = 135.0 * ScaleManager.Scale;
                    StaffListItemHeight = 223 * ScaleManager.Scale;
                    WhiteBtnWidth = 97 * ScaleManager.Scale;
                    WhiteBtnHight = 28 * ScaleManager.Scale;
                }

                StaffDetailCommand = new Command(async () =>
                {
                    if (App.ProcessManager.CanInvoke())
                    {
                        App.customNavigationPage.IsRunning = true;
                        await StaffDetail();
                        App.customNavigationPage.IsRunning = false;
                        App.ProcessManager.OnComplete();

                    }
                });

                SelectedStaffCommand = new Command(async () =>
                {
                    System.Diagnostics.Debug.WriteLine("staff tap:");
                    if (App.ProcessManager.CanInvoke())
                    {
                        App.customNavigationPage.IsRunning = true;
                        //default
                        switch (ReservationType)
                        {
                            case 1:
                                await ReservationCouponMenuPage();
                                break;
                            case 2:
                                await ReservationSchedulePage();
                                break;
                            case 3:

                                System.Diagnostics.Debug.WriteLine("reservationRegist");
                                await ReservationRegistPage();
                                break;
                            default:
                                break;
                        }
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            App.ProcessManager.OnComplete();
                            App.customNavigationPage.IsRunning = false;
                        });
                    }
                });
                System.Diagnostics.Debug.WriteLine("StaffCareerVisible" + StaffCareerVisible);
            }

            public ReservationContentInfo ReservationContent { get; set; }

            private InlineResponse2009DataList _StaffData;
            public InlineResponse2009DataList StaffData
            {
                get { return _StaffData; }
                set
                {
                    if (_StaffData != value)
                    {
                        _StaffData = value;

                        if (_StaffData.ThumbnailImage != null)
                            StaffThumbNail = _StaffData.ThumbnailImage.Path;

                        StaffName = _StaffData.Name;

                        StaffNameFurigana = _StaffData.Kana;

                        if (!string.IsNullOrEmpty(_StaffData.Career) && StaffName != shimeinasi)
                        {
                            StaffCareerVisible = true;
                            StaffCareer = _StaffData.Career;
                        }

                        if (string.IsNullOrEmpty(_StaffData.Summary) && StaffName != shimeinasi)
                        {
                            StaffSummaryVisible = true;
                            StaffSummary = _StaffData.Summary;
                        }
                        if ((bool)_StaffData.IsFavorite)
                        {
                            FavoriteImage = "BigFavoIconOn.png";
                        }
                        //else
                        //{
                        //	FavoriteImage = "BigFavoIconOff.png";
                        //}

                        if (StaffName != shimeinasi)
                        {
                            DetailVisible = true;
                        }

                        OnPropertyChanged("DetailVisible");
                        OnPropertyChanged("StaffThumbNail");
                        OnPropertyChanged("StaffName");
                        OnPropertyChanged("StaffNameFurigana");
                        OnPropertyChanged("StaffCareer");
                        OnPropertyChanged("StaffSummary");
                        OnPropertyChanged("FavoriteImage");
                    }
                }
            }

            private bool _DetailVisible;
            public bool DetailVisible
            {
                get { return _DetailVisible; }
                set
                {
                    if (_DetailVisible != value)
                    {
                        _DetailVisible = value;
                        OnPropertyChanged("DetailVisible");
                    }
                }
            }

            public double WhiteBtnWidth { get; set; }
            public double WhiteBtnHight { get; set; }

            private double _StaffListItemHeight;
            public double StaffListItemHeight
            {
                get { return _StaffListItemHeight; }
                set
                {
                    if (_StaffListItemHeight != value)
                    {
                        _StaffListItemHeight = value;
                        OnPropertyChanged("StaffListItemHeight");
                    }
                }
            }

            private string _StaffThumbNail;
            public string StaffThumbNail
            {
                get { return _StaffThumbNail; }
                set
                {
                    if (_StaffThumbNail != value)
                    {
                        _StaffThumbNail = value;
                        OnPropertyChanged("StaffThumbNail");
                    }
                }
            }

            private double _ThumbNailSize;
            public double ThumbNailSize
            {
                get { return _ThumbNailSize; }
                set
                {
                    if (_ThumbNailSize != value)
                    {
                        _ThumbNailSize = value;
                        OnPropertyChanged("ThumbNailSize");
                    }
                }
            }

            private ImageSource _FavoriteImage;
            public ImageSource FavoriteImage
            {
                get { return _FavoriteImage; }
                set
                {
                    if (_FavoriteImage != value)
                    {
                        _FavoriteImage = value;
                        OnPropertyChanged("FavoriteImage");
                    }
                }
            }

            private string _StaffName;
            public string StaffName
            {
                get { return _StaffName; }
                set
                {
                    if (_StaffName != value)
                    {
                        _StaffName = value;
                        OnPropertyChanged("StaffName");
                    }
                }
            }

            private string _StaffNameFurigana;
            public string StaffNameFurigana
            {
                get { return _StaffNameFurigana; }
                set
                {
                    if (_StaffNameFurigana != value)
                    {
                        _StaffNameFurigana = value;
                        OnPropertyChanged("StaffNameFurigana");
                    }
                }
            }


            public string StaffCareer { get; set; }

            private string _StaffSummary;
            public string StaffSummary
            {
                get { return _StaffSummary; }
                set
                {
                    if (_StaffSummary != value)
                    {
                        _StaffSummary = value;
                        OnPropertyChanged("StaffSummary");
                    }
                }
            }

            private bool _StaffSummaryVisible;
            public bool StaffSummaryVisible
            {
                get { return _StaffSummaryVisible; }
                set
                {
                    if (_StaffSummaryVisible != value)
                    {
                        _StaffSummaryVisible = value;
                        OnPropertyChanged("StaffSummaryVisible");
                    }
                }
            }

            private bool _StaffCareerVisible;
            public bool StaffCareerVisible
            {
                get { return _StaffCareerVisible; }
                set
                {
                    if (_StaffCareerVisible != value)
                    {
                        _StaffCareerVisible = value;
                        OnPropertyChanged("StaffCareerVisible");
                    }
                }
            }
            private Command _StaffDetailCommand;
            public Command StaffDetailCommand
            {
                get { return _StaffDetailCommand; }
                set
                {
                    if (_StaffDetailCommand != value)
                    {
                        _StaffDetailCommand = value;
                        OnPropertyChanged("StaffDetailCommand");
                    }
                }
            }

            private Command _SelectedStaffCommand;
            public Command SelectedStaffCommand
            {
                get { return _SelectedStaffCommand; }
                set
                {
                    if (_SelectedStaffCommand != value)
                    {
                        _SelectedStaffCommand = value;
                        OnPropertyChanged("SelectedStaffCommand");
                    }
                }
            }




            async Task StaffDetail()
            {

                //if (App.ProcessManager.CanInvoke())
                //{
                try
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await App.customNavigationPage.PushAsync(new StaffInformationPage(1, (int)StaffData.StaffId, (int)ReservationContent.SalonId, ReservationContent.StoreName));
                    });
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("ex :" + ex);
                }
                //}
                //App.ProcessManager.OnComplete();

            }

            async Task ReservationRegistPage()
            {

                ReservationContent.StaffId = StaffData.StaffId;
                ReservationContent.StaffName = StaffData.Name;
                //アカウント登録済みか確認
                var isRegisted = await FileManager.CheckFileAsync("Account", "accountInfo");
                //ゲストの場合
                if (!isRegisted)
                {
                    System.Diagnostics.Debug.WriteLine("isregisted = false");
                    var page = new AccountRegistration(3, null, ReservationContent);
                    Device.BeginInvokeOnMainThread(async () =>
                                    {
                                        await App.customNavigationPage.PushAsync(page);
                                        await page.DisplayAlert(null, "施術予約を行う場合、" + System.Environment.NewLine + "フリガナ、電話番号、性別の登録も必要となります。", "OK");
                                    });
                    //await App.customNavigationPage.PushAsync(new AccountRegistration(3, null, ReservationContent));
                    App.customNavigationPage.IsRunning = false;
                    App.ProcessManager.OnComplete();
                    return;

                }
                //登録済みの場合ReservationContentに格納する。
                var accountData = await FileManager.ReadJsonFileAsync<AccountInfo>("Account", "accountInfo");
                if (accountData != null)
                {
                    ReservationContent.Account = accountData;
                }
                //アカウント情報が読み込めなかった場合、やり直してもらう。
                else
                {
                    await App.Current.MainPage.DisplayAlert("アカウント情報が読み込めませんでした。", "アカウント登録を確認してください。", "OK");
                    return;
                }

                System.Diagnostics.Debug.WriteLine("date  :" + ReservationContent.Date.ToString("D") + ReservationContent.Date.ToString("ddd"));

                //性別が未登録だった場合は改めて性別の登録を促す。
                if (ReservationContent.Account.gender == 0 ||
                    string.IsNullOrEmpty(ReservationContent.Account.kana) ||
                    string.IsNullOrEmpty(ReservationContent.Account.tel))
                {
                    var page = new AccountRegistration(3, null, ReservationContent);
                    Device.BeginInvokeOnMainThread(async () =>
                                    {
                                        await App.customNavigationPage.PushAsync(page);
                                        await page.DisplayAlert(null, "施術予約を行う場合、" + System.Environment.NewLine + "フリガナ、電話番号、性別の登録も必要となります。", "OK");
                                    });
                    App.customNavigationPage.IsRunning = false;
                    App.ProcessManager.OnComplete();
                    return;
                }

                try
                {
                    var parameters = new Dictionary<string, string> { };
                    parameters.Add("salonId", ReservationContent.SalonId.ToString());
                    var json = await APIManager.GET("reservation__point", parameters);
                    var res = JsonManager.Deserialize<ResponseReservationPoint>(json);

                    var detailPage = new ReservationRegist(ReservationContent, res);

                    await App.customNavigationPage.PushAsync(detailPage);

                }
                catch (Exception ex)
                {

                    System.Diagnostics.Debug.WriteLine("ex :" + ex);
                }



            }




            async Task ReservationSchedulePage()
            {
                string apiName = "reservation_schedule";
                var param = new Dictionary<string, string>(){
                    { "deviceId", Config.Instance.Data.deviceId },
                    {"salonId", ReservationContent.SalonId.ToString()},
                    {"staffId", StaffData.StaffId.ToString()},
                    {"couponId",ReservationContent. CouponId.ToString()},
                    {"date", DependencyService.Get<IDateTimeService>().GetNow().ToString("d",new System.Globalization.CultureInfo("ja-JP"))}
                    };
                ReservationContent.StaffId = StaffData.StaffId;
                ReservationContent.StaffName = StaffData.Name;

                foreach (var n in param)
                {
                    System.Diagnostics.Debug.WriteLine("key :" + n.Key);
                    System.Diagnostics.Debug.WriteLine("value :" + n.Value);
                }
                System.Diagnostics.Debug.WriteLine("content menuid :" + ReservationContent.CouponContent);
                System.Diagnostics.Debug.WriteLine("content couponid :" + ReservationContent.CouponId);

                var res = await APIManager.GET(apiName, param);
                var scheduleContent = JsonManager.Deserialize<ResponseReservationSchedule>(res);
                System.Diagnostics.Debug.WriteLine("json : " + res);


                await App.customNavigationPage.Navigation.PushAsync(new ReservationSchedule(scheduleContent, ReservationContent));

            }


            async Task ReservationCouponMenuPage()
            {
                try
                {
                    string action = "reservation_menulist";
                    var parameters = new Dictionary<string, string> {
                            { "deviceId", Config.Instance.Data.deviceId },
                            { "staffId", StaffData.StaffId.ToString() },
                            {"salonId", ReservationContent.SalonId.ToString()},
                        };
                    ReservationContent.StaffId = StaffData.StaffId;
                    ReservationContent.StaffName = StaffData.Name;

                    var apiRet = await APIManager.Post(action, parameters);
                    if (apiRet != null)
                    {
                        var response = JsonConvert.DeserializeObject<ResponseReservationMenu>(apiRet);
                        if (response != null)
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                await App.customNavigationPage.PushAsync(new ReservationMenuList(response, ReservationContent));
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
            //}
        }
    }
}
