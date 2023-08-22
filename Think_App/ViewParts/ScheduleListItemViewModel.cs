using System;
using System.Threading.Tasks;
using Think_App;
using IO.Swagger.Model;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
namespace Think_App
{
    public class ScheduleListItemViewModel : ViewModelBase
    {
        public ScheduleListItemViewModel()
        {
            SetSizes();
        }
        public ScheduleListItemViewModel(InlineResponse20019DataHourScheduleList response, ReservationContentInfo content, int cellType)
        {
            SetSizes();

            Initialize(response, content, cellType);

        }

        void Initialize(InlineResponse20019DataHourScheduleList response, ReservationContentInfo content, int cellType)
        {
            CanReservation = (bool)response.CanReservation;
            ReservationContent = content;
            Data = response;
            SetCellType(cellType);
            SetCommand();
        }
        //type 1:１列目　時間表示, type 2: ２列目~5列目ボタン
        const int date = 1, btn = 2;
        void SetCellType(int type)
        {
            switch (type)
            {
                case date:
                    BtnTimeVisible = true;
                    BtnImgVisible = false;
                    TimeText = Data.Date.Substring(8, 2) + ":" + Data.Date.Substring(10, 2);

                    break;

                default:
                    BtnTimeVisible = false;
                    BtnImgVisible = true;
                    break;
            }
        }

        public async Task UpdateDate(InlineResponse20019DataHourScheduleList response)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Data = response;
                TimeText = Data.Date.Substring(8, 2) + ":" + Data.Date.Substring(10, 2);
            });
        }

        public async Task UpdateViewModel(InlineResponse20019DataHourScheduleList response)
        {
            CanReservation = (bool)response.CanReservation;
            Data = response;
        }

        void SetSizes(double size = 0.0)
        {
            size = 44.0;
            BtnWidth = size;
            BtnHeight = size - 2.0;
        }

        void SetCommand()
        {
            BtnClickedCommand = new Command(async () =>
                        {
                            if (!CanReservation)
                                return;
                            if (App.ProcessManager.CanInvoke())
                            {
                                App.customNavigationPage.IsRunning = true;

                                ReservationContent.Date = new DateTime(
                                NumFromString(Data.Date, 0, 4),//year
                                NumFromString(Data.Date, 4, 2),//month
                                NumFromString(Data.Date, 6, 2),//day
                                NumFromString(Data.Date, 8, 2),//hour
                                NumFromString(Data.Date, 10, 2),//minute
                                0);


                                //スタッフ指名なし
                                if (ReservationContent.IsRedicide && ReservationContent.StaffName != "指名なし")
                                {

                                    var dic = new Dictionary<string, string> {
                            {"salonId", ReservationContent.SalonId.ToString()},
                            {"date", ReservationContent.Date.ToString("g",new System.Globalization.CultureInfo("ja-JP"))},
                                    };
                                    if (ReservationContent.MenuId == null || ReservationContent.MenuId == 0)
                                    {
                                        dic.Add("couponId", ReservationContent.CouponId.ToString());
                                    }
                                    else
                                    {
                                        dic.Add("salonMenuId", ReservationContent.MenuId.ToString());
                                    }
                                    var json = await APIManager.GET("reservation_schedule_stafflist", dic);
                                    var res = JsonManager.Deserialize<ResponseStaffList>(json);
                                    System.Diagnostics.Debug.WriteLine("  json  ;" + json);
                                    try
                                    {
                                        await App.customNavigationPage.PushAsync(new ReservationSelectStaff(res, ReservationContent, 3));
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                    App.customNavigationPage.IsRunning = false;
                                    App.ProcessManager.OnComplete();
                                    return;

                                }


                                var isRegisted = await FileManager.CheckFileAsync("Account", "accountInfo");
                                if (!isRegisted)
                                {
                                    System.Diagnostics.Debug.WriteLine("isregisted = false");
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

                                var parameters = new Dictionary<string, string> { };
                                parameters.Add("salonId", ReservationContent.SalonId.ToString());

                                try
                                {
                                    var json = await APIManager.GET("reservation__point", parameters);
                                    var res = JsonManager.Deserialize<ResponseReservationPoint>(json);
                                    System.Diagnostics.Debug.WriteLine("  json  ;" + json);


                                    var detailPage = new ReservationRegist(ReservationContent, res);

                                    await App.customNavigationPage.PushAsync(detailPage);

                                }
                                catch (Exception ex)
                                {

                                    System.Diagnostics.Debug.WriteLine("ex :" + ex);
                                }
                                App.customNavigationPage.IsRunning = false;
                                App.ProcessManager.OnComplete();


                            }
                        });
        }
        int NumFromString(string parm, int index, int length)
        {
            return Int32.Parse(parm.Substring(index, length));
        }

        public InlineResponse20019DataHourScheduleList Data { get; set; }
        ReservationContentInfo ReservationContent { get; set; }
        private bool _CanReservation;
        public bool CanReservation
        {
            get { return _CanReservation; }
            set
            {
                _CanReservation = value;

                SetBtnStyle(_CanReservation);

                OnPropertyChanged("CanReservation");
            }
        }

        void SetBtnStyle(bool canReservation)
        {

            if (canReservation)
            {
                BtnImgSource = "Icon_ScheduleAble.png";
            }
            else
            {
                BtnImgSource = "Icon_ScheduleDisable.png";

            }
            OnPropertyChanged("BtnImgSource");
        }
        private string _TimeText;
        public string TimeText
        {
            get { return _TimeText; }
            set
            {
                if (_TimeText != value)
                {
                    _TimeText = value;
                    OnPropertyChanged("TimeText");
                }
            }
        }
        private bool _BtnTimeVisible;
        public bool BtnTimeVisible
        {
            get { return _BtnTimeVisible; }
            set
            {
                if (_BtnTimeVisible != value)
                {

                    _BtnTimeVisible = value;
                    OnPropertyChanged("BtnTimeVisible");
                }
            }
        }
        private string _BtnImgSource;
        public string BtnImgSource
        {
            get { return _BtnImgSource; }
            set
            {
                if (_BtnImgSource != value)
                {

                    _BtnImgSource = value;
                    OnPropertyChanged("BtnImgSource");
                }
            }
        }

        private bool _BtnImgVisible;
        public bool BtnImgVisible
        {
            get { return _BtnImgVisible; }
            set
            {
                if (_BtnImgVisible != value)
                {
                    _BtnImgVisible = value;
                    OnPropertyChanged("BtnImgVisible");
                }
            }
        }



        private double _BtnHeight;
        public double BtnHeight
        {
            get { return _BtnHeight; }
            set
            {
                if (_BtnHeight != value)
                {
                    _BtnHeight = value;
                    OnPropertyChanged("BtnHeight");
                }
            }
        }
        private double _BtnWidth;
        public double BtnWidth
        {
            get { return _BtnWidth; }
            set
            {
                if (_BtnWidth != value)
                {
                    _BtnWidth = value;
                    OnPropertyChanged("BtnWidth");
                }
            }
        }

        private Command _BtnClickedCommand;
        public Command BtnClickedCommand
        {
            get { return _BtnClickedCommand; }
            set
            {
                if (_BtnClickedCommand != value)
                {
                    _BtnClickedCommand = value;
                    OnPropertyChanged("BtnClickedCommand");
                }
            }
        }

    }
}