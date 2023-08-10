using System;
using System.Linq;
using IO.Swagger.Model;
using Xamarin.Forms;

namespace Think_App
{
    public class PushNotificationSettingViewModel : ViewModelBase
    {
        //ホームメニュー
        public const int Reservation = 1, News = 2, Coupon = 3, Message = 4, StoreList = 5, History = 6, HairSimulation = 7, Bolg = 8;

        public PushNotificationSettingViewModel(ResponseHome responseHome)
        {
            BdgReserveBefore1day = new CustomSwitchCellViewModel()
            {
                LabelText = "予約通知（来店1日前）",
                ViewVisible = true,
            };
            BdgReserveBeforeHour = new CustomSwitchCellViewModel()
            {
                LabelText = "予約通知（来店N時間前）",
                ViewVisible = true,
            };
            BdgNotice = new CustomSwitchCellViewModel();
            if (responseHome.Data.MenuList.FirstOrDefault((x) => x.MenuId == News) != null)
            {
                BdgNotice.LabelText = "お知らせの更新";
                BdgNotice.ViewVisible = true;
            }
            BdgTicket = new CustomSwitchCellViewModel();
            if (responseHome.Data.MenuList.FirstOrDefault((x) => x.MenuId == Coupon) != null)
            {
                BdgTicket.LabelText = "クーポンの追加";
                BdgTicket.ViewVisible = true;
            }
            BdgMessage = new CustomSwitchCellViewModel();
            if (responseHome.Data.MenuList.FirstOrDefault((x) => x.MenuId == Message) != null)
            {
                BdgMessage.LabelText = "新規メッセージ";
                BdgMessage.ViewVisible = true;
            }
        }

        public CustomSwitchCellViewModel BdgReserveBefore1day { get; set; }
        public CustomSwitchCellViewModel BdgReserveBeforeHour { get; set; }
        public CustomSwitchCellViewModel BdgNotice { get; set; }
        public CustomSwitchCellViewModel BdgTicket { get; set; }
        public CustomSwitchCellViewModel BdgMessage { get; set; }
    }
}
