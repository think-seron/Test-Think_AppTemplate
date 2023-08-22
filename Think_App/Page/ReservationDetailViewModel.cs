using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using IO.Swagger.Model;
using System.Text.RegularExpressions;
using System.Linq;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
    public class ReservationDetailViewModel : ViewModelBase
    {
        string canCanselDateTime = null;
        public ReservationDetailViewModel(IO.Swagger.Model.ReservationDetail data, Guid reservationTopPageId)
        {
            ScreenSizeScale = ScaleManager.Scale;
            System.Diagnostics.Debug.WriteLine(" scale :" + ScreenSizeScale);
            TelBtnWidth = 168 * ScaleManager.Scale;
            TelBtnHeight = 36 * ScaleManager.Scale;
            BtnImgSize = TelBtnHeight / 2;
            ReservationTopPageId = reservationTopPageId;
            ReservationDate = data.Data.DateStr;
            EditorText = data.Data.Memo;
            ReservationStore = data.Data.SalonName;
            ReservationStyList = data.Data.StaffName;
            ReservationMenu = data.Data.MenuName;
            ReservationSource = data.Data.Source;
            canCanselDateTime = data.Data.CanCancelDate;

            if (string.IsNullOrEmpty(data.Data.CouponName))
            {
                ReservationUsingCoupon = "無";
                ReservationCouponContent = null;
                ReservationCouponContentVisible = false;
            }
            else
            {
                ReservationUsingCoupon = "有";
                ReservationCouponContent = data.Data.CouponName;
                ReservationCouponContentVisible = true;
            }

            CanselText = new FormattedString
            {
                Spans =
                {
                    new Span {  Text="予約のキャンセルは"
                            +canCanselDateTime
                            +"まで可能です。キャンセル可能期限以降にキャンセルを行いたい場合は、サロンに直接ご連絡ください。"},
                }
            };
            try
            {
                CancelBtnEnable = (bool)data.Data.CanCancelFlag;
            }
            catch
            {

            }
            SetCommand(data);
        }

        void SetCommand(IO.Swagger.Model.ReservationDetail data)
        {
            CallCommand = new Command(() =>
            {
                if (App.ProcessManager.CanInvoke())
                {
                    if (string.IsNullOrEmpty(data.Data.Tel))
                    {
                        DependencyService.Get<IToast>().Show("該当店舗の電話番号が取得できませんでした。");
                        App.ProcessManager.OnComplete();
                        return;
                    }
                    string telNum = data.Data.Tel.Replace("-", "");
                    Regex re = new Regex(@"[^0-9]");
                    var tel = re.Replace(telNum, "");
                    await Device.OpenAsync(new Uri("tel:" + tel));
                    App.ProcessManager.OnComplete();
                }
            });

            CanselCommand = new Command(async () =>
            {
                if (App.ProcessManager.CanInvoke())
                {

                    var yesCommand = new Command(async () =>
                    {

                        try
                        {
                            System.Diagnostics.Debug.WriteLine("yesbtn clicked ");
                            App.customNavigationPage.IsRunning = true;
                            if (App.ProcessManager.CanInvoke())
                            {
                                System.Diagnostics.Debug.WriteLine("yesbtn start ");
                                string action = "reservation__cancel";
                                var parameters = new Dictionary<string, string> {
                            {"deviceId", Config.Instance.Data.deviceId },
                            {"reservationId", data.Data.ReservationId},
                            {"sipssStoreId", data.Data.SipssStoreId},
                            {"sipssCompanyId", data.Data.SipssCompanyId},
                        };
                                var apiRet = await APIManager.Post(action, parameters);
                                if (apiRet != null)
                                {
                                    var response = JsonManager.Deserialize<ResponseHome>(apiRet);
                                    if (response != null)
                                    {
                                        //Task.Run(() =>
                                        //{
                                        var home = App.customNavigationPage.Navigation.NavigationStack.Where((arg) => arg.Id == App.HomeId);


                                        if (home.FirstOrDefault() != null)
                                        {
                                            var home_page = home.First() as Home;

                                            var home_bc = home_page.BindingContext as HomeViewModel;

                                            home_bc.UpdateHomeSalon(response);
                                        }


                                        //Task.Run(() =>
                                        //{
                                        var reservationTop = App.customNavigationPage.Navigation.NavigationStack.Where((arg) => arg.Id == ReservationTopPageId);
                                        if (reservationTop.FirstOrDefault() != null)
                                        {
                                            var reservationTop_Page = reservationTop.First() as ReservationTop;

                                            var reservationTop_BC = reservationTop_Page.BindingContext as ReservationTopViewModel;
                                            await reservationTop_BC.UpdateList();
                                        }

                                        if (response.Status == 0)
                                        {
                                            DependencyService.Get<IToast>().Show("キャンセルしました");
                                        }
                                    }
                                    else
                                    {
                                        DependencyService.Get<IToast>().Show("通信エラー");
                                    }
                                }
                                Device.BeginInvokeOnMainThread(async () =>
                                {
                                    await App.customNavigationPage.PopAsync();

                                    await DialogManager.Instance.HideView();

                                    App.customNavigationPage.IsRunning = false;

                                    App.ProcessManager.OnComplete();
                                });
                            }
                        }
                        catch (Exception ex)
                        {
                            App.customNavigationPage.IsRunning = false; ;
                            App.ProcessManager.OnComplete();
                            System.Diagnostics.Debug.WriteLine("ex :" + ex);
                        }
                    });

                    var noCommand = new Command(async () =>
                    {
                        await DialogManager.Instance.HideView();
                    });

                    var modalView = new ReservationDetailCanselView();

                    modalView.BindingContext = new ReservationDetailCanselViewModel(data, yesCommand, noCommand);


                    await DialogManager.Instance.ShowDialogView(modalView);
                    App.ProcessManager.OnComplete();
                }
            });
        }

        public Guid ReservationTopPageId { get; set; }

        public double ScreenSizeScale { get; set; }

        public string ReservationSource { get; set; }
        public string ReservationDate { get; set; }
        public string ReservationStore { get; set; }
        public string ReservationStyList { get; set; }
        public string ReservationMenu { get; set; }
        public string ReservationUsingCoupon { get; set; }
        public string ReservationCouponContent { get; set; }
        public string EditorText { get; set; }


        public bool ReservationCouponContentVisible { get; set; }

        private FormattedString _CanselText;
        public FormattedString CanselText
        {
            get { return _CanselText; }
            set
            {
                if (_CanselText != value)
                {
                    _CanselText = value;
                    OnPropertyChanged("CanselText");
                }
            }
        }





        public double TelBtnWidth { get; set; }
        public double TelBtnHeight { get; set; }
        public double BtnImgSize { get; set; }

        private bool _CancelBtnEnable;
        public bool CancelBtnEnable
        {
            get { return _CancelBtnEnable; }
            set
            {
                if (_CancelBtnEnable != value)
                {
                    _CancelBtnEnable = value;
                    OnPropertyChanged("CancelBtnEnable");
                }
            }
        }

        private Command _CanselCommand;
        public Command CanselCommand
        {
            get { return _CanselCommand; }
            set
            {
                if (_CanselCommand != value)
                {
                    _CanselCommand = value;
                    OnPropertyChanged("CanselCommand");
                }
            }
        }

        private Command _CallCommand;
        public Command CallCommand
        {
            get { return _CallCommand; }
            set
            {
                if (_CallCommand != value)
                {
                    _CallCommand = value;
                    OnPropertyChanged("CallCommand");
                }
            }
        }
    }
}
