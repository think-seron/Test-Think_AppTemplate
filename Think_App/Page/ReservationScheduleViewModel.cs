using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using IO.Swagger.Model;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
namespace Think_App
{
    public class ReservationScheduleViewModel : ViewModelBase
    {

        const string emptyInfoParm = "の空き情報";
        const string salonData = "サロン";
        const string staffFree = "指名なし";

        public ReservationScheduleViewModel(ResponseReservationSchedule response, ReservationContentInfo content)
        {
            ReservationContent = content;

            ScheduleData = response;

            dateTime = DependencyService.Get<IDateTimeService>().GetNow();

            if (content.StaffName == staffFree)
            {
                SegTexts = salonData + emptyInfoParm;
                SingleFrameVisible = true;
            }
            else
            {
                SegTexts = content.StaffName + emptyInfoParm + "," + salonData + emptyInfoParm;
                SegBtnVisible = true;
            }
            SegWidth = ScaleManager.ScreenWidth - 16.0;

            dayViewmodelArray = new ScheduleDayViewModel[] { ScheduleDayFirst, ScheduleDaySecond, ScheduleDayThird, ScheduleDayFourth };
            ScheduleListItemSource = new ObservableCollection<ScheduleRowList>();

            SetCommands();

            if (response.Data == null)
                return;

            SetDayTexts(response);

            InitializeScheduleList(response);


        }

        async Task SetCommands()
        {
            UpdateScheduleComand_MonthAgo = await BaseUpdateCommand(0, -1);
            UpdateScheduleComand_daysAgo = await BaseUpdateCommand(-4);
            UpdateScheduleComand_dayslater = await BaseUpdateCommand(4);
            UpdateScheduleComand_MonthLater = await BaseUpdateCommand(0, 1);
        }

        async Task<Command> BaseUpdateCommand(int days, int month = 0)
        {
            return new Command(() =>
            {
                if (App.ProcessManager.CanInvoke())
                {
                    System.Diagnostics.Debug.WriteLine("start running");
                    App.customNavigationPage.IsRunning = true;
                    string apiName = "reservation_schedule";
                    var param = new Dictionary<string, string>(){
                    { "deviceId", Config.Instance.Data.deviceId },
                    {"salonId", ReservationContent.SalonId.ToString()},
                    {"staffId", ReservationContent.StaffId.ToString()},
                    {"date", dateTime.AddDays(days).AddMonths(month).ToString("d",new System.Globalization.CultureInfo("ja-JP"))}
                    };
                    dateTime = dateTime.AddDays(days).AddMonths(month);
                    if (ReservationContent.MenuId != 0)
                    {
                        param.Add("salonMenuId", ReservationContent.MenuId.ToString());
                    }
                    else
                    {
                        param.Add("couponId", ReservationContent.CouponId.ToString());
                    }
                    var resjsonAwaiter = APIManager.GET(apiName, param).GetAwaiter();
                    resjsonAwaiter.OnCompleted(() =>
                    {
                        var scheduleData = JsonManager.Deserialize<ResponseReservationSchedule>(resjsonAwaiter.GetResult());
                        System.Diagnostics.Debug.WriteLine("json : " + scheduleData);
                        ScheduleData = scheduleData;
                        SetDayTexts(ScheduleData);

                        InitializeScheduleList(ScheduleData);
                        App.customNavigationPage.IsRunning = false;
                        App.ProcessManager.OnComplete();
                    });
                }
            });
        }

        public DateTime dateTime { get; set; }
        public ReservationContentInfo ReservationContent { get; set; }
        public ResponseReservationSchedule ScheduleData { get; set; }
        ScheduleDayViewModel[] dayViewmodelArray;

        void SetDayTexts(ResponseReservationSchedule response)
        {
            if (response.Data != null)
            {
                for (int i = 0; i < response.Data.SalonDailyScheduleList.Count; i++)
                {
                    dayViewmodelArray[i].ViewUpdate(response.Data.SalonDailyScheduleList[i].HourScheduleList[0].Date, (int)response.Data.SalonDailyScheduleList[i].DayType, response.Data.SalonDailyScheduleList[i].DayOfWeek);
                }

                SelectedDate = response.Data.SalonDailyScheduleList[0].HourScheduleList[0].Date.Substring(0, 4) + "年" + response.Data.SalonDailyScheduleList[0].HourScheduleList[0].Date.Substring(4, 2) + "月";
            }

        }
        int ct = 0;
        int i;
        void InitializeScheduleList(ResponseReservationSchedule response)
        {
            ScheduleListItemSource = new ObservableCollection<ScheduleRowList>();
            Task.Run(() =>
                    {
                        if (SegSelectedIndex == 0 && SegBtnVisible == true)
                        {
                            ReservationContent.IsRedicide = false;
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                for (i = 0; i < response.Data.StaffDailyScheduleList[0].HourScheduleList.Count; i++)
                                {
                                    var item = new ScheduleRowList()
                                    {
                                        ListItem1 = new ScheduleListItemViewModel(response.Data.StaffDailyScheduleList[0].HourScheduleList[i], ReservationContent, 1),
                                        ListItem2 = new ScheduleListItemViewModel(response.Data.StaffDailyScheduleList[0].HourScheduleList[i], ReservationContent, 2),
                                        ListItem3 = new ScheduleListItemViewModel(response.Data.StaffDailyScheduleList[1].HourScheduleList[i], ReservationContent, 2),
                                        ListItem4 = new ScheduleListItemViewModel(response.Data.StaffDailyScheduleList[2].HourScheduleList[i], ReservationContent, 2),
                                        ListItem5 = new ScheduleListItemViewModel(response.Data.StaffDailyScheduleList[3].HourScheduleList[i], ReservationContent, 2)
                                    };
                                    ScheduleListItemSource.Add(item);
                                }
                            });
                            return;
                        }
                        if (SegBtnVisible == false)
                            ReservationContent.IsRedicide = false;
                        else
                            ReservationContent.IsRedicide = true;
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            for (i = 0; i < response.Data.SalonDailyScheduleList[0].HourScheduleList.Count; i++)
                            {
                                var item = new ScheduleRowList()
                                {
                                    ListItem1 = new ScheduleListItemViewModel(response.Data.SalonDailyScheduleList[0].HourScheduleList[i], ReservationContent, 1),
                                    ListItem2 = new ScheduleListItemViewModel(response.Data.SalonDailyScheduleList[0].HourScheduleList[i], ReservationContent, 2),
                                    ListItem3 = new ScheduleListItemViewModel(response.Data.SalonDailyScheduleList[1].HourScheduleList[i], ReservationContent, 2),
                                    ListItem4 = new ScheduleListItemViewModel(response.Data.SalonDailyScheduleList[2].HourScheduleList[i], ReservationContent, 2),
                                    ListItem5 = new ScheduleListItemViewModel(response.Data.SalonDailyScheduleList[3].HourScheduleList[i], ReservationContent, 2)
                                };
                                ScheduleListItemSource.Add(item);
                            }
                        });
                    });
        }

        private string _SelectedDate;
        public string SelectedDate
        {
            get { return _SelectedDate; }
            set
            {
                if (_SelectedDate != value)
                {
                    _SelectedDate = value;
                    OnPropertyChanged("SelectedDate");
                }
            }
        }

        private double _GridWidthtRequest;
        public double GridWidthtRequest
        {
            get { return _GridWidthtRequest; }
            set
            {
                if (_GridWidthtRequest != value)
                {
                    _GridWidthtRequest = value;
                    OnPropertyChanged("GridWidthtRequest");
                }
            }
        }

        private double _GridHeightRequest;
        public double GridHeightRequest
        {
            get { return _GridHeightRequest; }
            set
            {
                if (_GridHeightRequest != value)
                {
                    _GridHeightRequest = value;
                    OnPropertyChanged("GridHeightRequest");
                }
            }
        }

        private string _SegTexts;
        public string SegTexts
        {
            get
            {
                return _SegTexts;
            }
            set
            {
                if (_SegTexts != value)
                {
                    _SegTexts = value;
                    OnPropertyChanged("SegTexts");
                }
            }
        }


        private double _SegWidth;
        public double SegWidth
        {
            get { return _SegWidth; }
            set
            {
                if (_SegWidth != value)
                {
                    _SegWidth = value;
                    OnPropertyChanged("SegWidth");
                }
            }
        }
        private bool _SegBtnVisible;
        public bool SegBtnVisible
        {
            get { return _SegBtnVisible; }
            set
            {
                if (_SegBtnVisible != value)
                {
                    _SegBtnVisible = value;
                    OnPropertyChanged("SegBtnVisible");
                }
            }
        }
        private bool _SingleFrameVisible;
        public bool SingleFrameVisible
        {
            get { return _SingleFrameVisible; }
            set
            {
                if (_SingleFrameVisible != value)
                {
                    _SingleFrameVisible = value;
                    OnPropertyChanged("SingleFrameVisible");
                }
            }
        }

        private int _SegSelectedIndex;
        public int SegSelectedIndex
        {
            get { return _SegSelectedIndex; }
            set
            {
                if (_SegSelectedIndex != value)
                {
                    _SegSelectedIndex = value;
                    System.Diagnostics.Debug.WriteLine("update  schedule");
                    InitializeScheduleList(ScheduleData);

                    OnPropertyChanged("SegSelectedIndex");
                }
            }
        }

        private ScheduleDayViewModel _ScheduleDayNull;
        public ScheduleDayViewModel ScheduleDayNull
        {
            get { return _ScheduleDayNull; }
            set
            {
                if (_ScheduleDayNull != value)
                {
                    _ScheduleDayNull = value;
                    OnPropertyChanged("ScheduleDayNull");
                }
            }
        }

        private ScheduleDayViewModel _ScheduleDayFirst;
        public ScheduleDayViewModel ScheduleDayFirst
        {
            get
            {
                if (_ScheduleDayFirst == null)
                {
                    _ScheduleDayFirst = new ScheduleDayViewModel();
                }
                return _ScheduleDayFirst;
            }
            set
            {
                if (_ScheduleDayFirst == null)
                {
                    _ScheduleDayFirst = new ScheduleDayViewModel();
                }
                if (_ScheduleDayFirst != value)
                {
                    _ScheduleDayFirst = value;
                    OnPropertyChanged("ScheduleDayFirst");
                }
            }
        }

        private ScheduleDayViewModel _ScheduleDaySecond;
        public ScheduleDayViewModel ScheduleDaySecond
        {
            get
            {
                if (_ScheduleDaySecond == null)
                {
                    _ScheduleDaySecond = new ScheduleDayViewModel();
                }
                return _ScheduleDaySecond;
            }
            set
            {
                if (_ScheduleDaySecond == null)
                {
                    _ScheduleDaySecond = new ScheduleDayViewModel();
                }
                if (_ScheduleDaySecond != value)
                {
                    _ScheduleDaySecond = value;
                    OnPropertyChanged("ScheduleDaySecond");
                }
            }
        }

        private ScheduleDayViewModel _ScheduleDayThird;
        public ScheduleDayViewModel ScheduleDayThird
        {
            get
            {
                if (_ScheduleDayThird == null)
                {
                    _ScheduleDayThird = new ScheduleDayViewModel();
                }
                return _ScheduleDayThird;
            }
            set
            {
                if (_ScheduleDayThird == null)
                {
                    _ScheduleDayThird = new ScheduleDayViewModel();
                }
                if (_ScheduleDayThird != value)
                {
                    _ScheduleDayThird = value;
                    OnPropertyChanged("ScheduleDayThird");
                }
            }
        }

        private ScheduleDayViewModel _ScheduleDayFourth;
        public ScheduleDayViewModel ScheduleDayFourth
        {
            get
            {
                if (_ScheduleDayFourth == null)
                {
                    _ScheduleDayFourth = new ScheduleDayViewModel();
                }
                return _ScheduleDayFourth;
            }
            set
            {
                if (_ScheduleDayFourth == null)
                {
                    _ScheduleDayFourth = new ScheduleDayViewModel();
                }
                if (_ScheduleDayFourth != value)
                {
                    _ScheduleDayFourth = value;
                    OnPropertyChanged("ScheduleDayFourth");
                }
            }
        }


        private Command _UpdateScheduleComand_MonthAgo;
        public Command UpdateScheduleComand_MonthAgo
        {
            get { return _UpdateScheduleComand_MonthAgo; }
            set
            {
                if (_UpdateScheduleComand_MonthAgo != value)
                {
                    _UpdateScheduleComand_MonthAgo = value;
                    OnPropertyChanged("UpdateScheduleComand_MonthAgo");
                }
            }
        }

        private Command _UpdateScheduleComand_MonthLater;
        public Command UpdateScheduleComand_MonthLater
        {
            get { return _UpdateScheduleComand_MonthLater; }
            set
            {
                if (_UpdateScheduleComand_MonthLater != value)
                {
                    _UpdateScheduleComand_MonthLater = value;
                    OnPropertyChanged("UpdateScheduleComand_MonthLater");
                }
            }
        }

        private Command _UpdateScheduleComand_daysAgo;
        public Command UpdateScheduleComand_daysAgo
        {
            get { return _UpdateScheduleComand_daysAgo; }
            set
            {
                if (_UpdateScheduleComand_daysAgo != value)
                {
                    _UpdateScheduleComand_daysAgo = value;
                    OnPropertyChanged("UpdateScheduleComand_daysAgo");
                }
            }
        }

        private Command _UpdateScheduleComand_dayslater;
        public Command UpdateScheduleComand_dayslater
        {
            get { return _UpdateScheduleComand_dayslater; }
            set
            {
                if (_UpdateScheduleComand_dayslater != value)
                {
                    _UpdateScheduleComand_dayslater = value;
                    OnPropertyChanged("UpdateScheduleComand_dayslater");
                }
            }
        }


        double size = 44.0;

        private double _GridItemHeight;
        public double GridItemHeight
        {
            get { return _GridItemHeight; }
            set
            {
                if (_GridItemHeight != value)
                {
                    _GridItemHeight = value;
                    OnPropertyChanged("GridItemHeight");
                }
            }
        }

        private double _GridItemWidth;
        public double GridItemWidth
        {
            get { return _GridItemWidth; }
            set
            {
                if (_GridItemWidth != value)
                {
                    _GridItemWidth = value;
                    OnPropertyChanged("GridItemWidth");
                }
            }
        }


        private double _GridColumnSpacing;
        public double GridColumnSpacing
        {
            get { return _GridColumnSpacing; }
            set
            {
                if (_GridColumnSpacing != value)
                {
                    _GridColumnSpacing = value;
                    OnPropertyChanged("GridColumnSpacing");
                }
            }
        }


        private double _GridRowSpacing;
        public double GridRowSpacing
        {
            get { return _GridRowSpacing; }
            set
            {
                if (_GridRowSpacing != value)
                {
                    _GridRowSpacing = value;
                    OnPropertyChanged("GridRowSpacing");
                }
            }
        }


        private double _TimeListHeight;
        public double TimeListHeight
        {
            get { return _TimeListHeight; }
            set
            {
                if (_TimeListHeight != value)
                {
                    _TimeListHeight = value;
                    OnPropertyChanged("TimeListHeight");
                }
            }
        }

        private ObservableCollection<ScheduleRowList> _ScheduleListItemSource;
        public ObservableCollection<ScheduleRowList> ScheduleListItemSource
        {
            get
            {
                if (_ScheduleListItemSource == null)
                {
                    _ScheduleListItemSource = new ObservableCollection<ScheduleRowList>();
                }
                return _ScheduleListItemSource;
            }
            set
            {
                if (_ScheduleListItemSource == null)
                {
                    _ScheduleListItemSource = new ObservableCollection<ScheduleRowList>();
                }
                if (_ScheduleListItemSource != value)
                {
                    _ScheduleListItemSource = value;
                    OnPropertyChanged("ScheduleListItemSource");
                }
            }
        }
        public class ScheduleRowList : ViewModelBase
        {
            public ScheduleRowList()
            {
            }
            private ScheduleListItemViewModel _ListItem1;
            public ScheduleListItemViewModel ListItem1
            {
                get { return _ListItem1; }
                set
                {
                    if (_ListItem1 != value)
                    {
                        _ListItem1 = value;
                        OnPropertyChanged("ListItem1");
                    }
                }
            }
            private ScheduleListItemViewModel _ListItem2;
            public ScheduleListItemViewModel ListItem2
            {
                get { return _ListItem2; }
                set
                {
                    if (_ListItem2 != value)
                    {
                        _ListItem2 = value;
                        OnPropertyChanged("ListItem2");
                    }
                }
            }

            private ScheduleListItemViewModel _ListItem3;
            public ScheduleListItemViewModel ListItem3
            {
                get { return _ListItem3; }
                set
                {
                    if (_ListItem3 != value)
                    {
                        _ListItem3 = value;
                        OnPropertyChanged("ListItem3");
                    }
                }
            }
            private ScheduleListItemViewModel _ListItem4;
            public ScheduleListItemViewModel ListItem4
            {
                get { return _ListItem4; }
                set
                {
                    if (_ListItem4 != value)
                    {
                        _ListItem4 = value;
                        OnPropertyChanged("ListItem4");
                    }
                }
            }
            private ScheduleListItemViewModel _ListItem5;
            public ScheduleListItemViewModel ListItem5
            {
                get { return _ListItem5; }
                set
                {
                    if (_ListItem5 != value)
                    {
                        _ListItem5 = value;
                        OnPropertyChanged("ListItem5");
                    }
                }
            }
        }
    }

    public static class ListManager
    {
        static ObservableCollection<ReservationScheduleViewModel.ScheduleRowList> _list;
        public static ObservableCollection<ReservationScheduleViewModel.ScheduleRowList> list
        {
            get
            {
                if (_list == null)
                    _list = new ObservableCollection<ReservationScheduleViewModel.ScheduleRowList>();

                return _list;
            }
        }

        private static ResponseReservationSchedule _Response;
        public static ResponseReservationSchedule Response
        {
            get { return _Response; }
            set
            {
                if (_Response != value)
                {
                    _Response = value;
                }
            }
        }

        static int i;
        public static void CreateList(ResponseReservationSchedule response, ReservationContentInfo ReservationContent)
        {

            Task.Run(() =>
            {
                list.Clear();
                for (i = 0; i < response.Data.StaffDailyScheduleList[0].HourScheduleList.Count; i++)
                {
                    var item = new ReservationScheduleViewModel.ScheduleRowList()
                    {
                        ListItem1 = new ScheduleListItemViewModel(response.Data.StaffDailyScheduleList[0].HourScheduleList[i], ReservationContent, 1),
                        ListItem2 = new ScheduleListItemViewModel(response.Data.StaffDailyScheduleList[0].HourScheduleList[i], ReservationContent, 2),
                        ListItem3 = new ScheduleListItemViewModel(response.Data.StaffDailyScheduleList[1].HourScheduleList[i], ReservationContent, 2),
                        ListItem4 = new ScheduleListItemViewModel(response.Data.StaffDailyScheduleList[2].HourScheduleList[i], ReservationContent, 2),
                        ListItem5 = new ScheduleListItemViewModel(response.Data.StaffDailyScheduleList[3].HourScheduleList[i], ReservationContent, 2)
                    };
                    list.Add(item);
                }
            });
        }
    }
}