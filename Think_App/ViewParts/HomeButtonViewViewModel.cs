using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using IO.Swagger.Model;
namespace Think_App
{
    public class HomeButtonViewViewModel : ViewModelBase
    {
        public HomeButtonViewViewModel()
        {
        }

        public HomeButtonViewViewModel(int menuid, string btntext, string imagesource, ObservableCollection<InlineResponse2002DataNotificationList> notificationList)
        {
            if (menuid == 0)
            {
                SetNullData();

                return;
            }

            ViewVisible = true;
            MenuID = menuid;

            BtnText = btntext;
            //臨時対応 fontを強制的にスケーリング
            //BtnFontSize = 14.0 * ScaleManager.Scale;
            BtnFontSize = ReturnFontSize();
            //* ScaleManager.Scale;

            BtnTextColor = ColorList.colorWhite;

            BtnImage = imagesource;
            //ImageSource.FromUri(new Uri(imagesource));
            if (notificationList != null)
            {
                NotificationList = notificationList;
            }

            BtnImageWidth = BtnImageHeight = ReturnImageWidth();

            BtnBgColor = ColorList.colorHomeMenuBtn;

            BtnImgLayout = CustomButton.LayoutPosition.Top;

        }

        //public void SetView(int? menuid, string btntext, string imagesource, ObservableCollection<NotificationListData> notificationList)
        //{
        //	if (menuid == 0||menuid==null)
        //	{
        //		SetNullData();

        //		return;
        //	}

        //	ViewVisible = true;
        //	MenuID = (int)menuid;

        //	BtnText = btntext;

        //	//BtnFontSize = 14.0 * ScaleManager.Scale;
        //	BtnFontSize = ReturnFontSize();
        //	//* ScaleManager.Scale;

        //	BtnTextColor = ColorList.colorWhite;

        //	BtnImage = imagesource;
        //	//ImageSource.FromUri(new Uri(imagesource));
        //	if (notificationList != null)
        //	{
        //		NotificationList = notificationList;
        //	}

        //	BtnImageWidth = 32;

        //	BtnImageHeight = 32;

        //	BtnBgColor = ColorList.colorHomeMenuBtn;

        //	BtnImgLayout = CustomButton.LayoutPosition.Top;
        //}

        //public void SetView(HomeData.MenuListData data, Command command=null) {
        //	if (data.menuId == 0)
        //	{
        //		SetNullData();

        //		return;
        //	}

        //	ViewVisible = true;
        //	MenuID = data.menuId;

        //	BtnText = data.name;
        //	BtnFontSize = ReturnFontSize();

        //	//BtnFontSize = 14.0 * ScaleManager.Scale;
        //	//* ScaleManager.Scale;

        //	BtnTextColor = ColorList.colorWhite;


        //	//ImageSource.FromUri(new Uri(imagesource));
        //	if (data.notificationList != null)
        //	{
        //		NotificationList = data.notificationList;
        //	}

        //	BtnImageWidth = 32 * ScaleManager.Scale;

        //	BtnImageHeight = 32 * ScaleManager.Scale;

        //	BtnBgColor = ColorList.colorHomeMenuBtn;

        //	BtnImgLayout = CustomButton.LayoutPosition.Top;

        //	BtnCommand = command;
        //}

        public void SetView(InlineResponse2002DataMenuList data, Command command = null)
        {
            if (data.MenuId == 0)
            {
                SetNullData();

                return;
            }

            ViewVisible = true;

            MenuID = (int)data.MenuId;

            BtnText = data.Name;
            BtnFontSize = ReturnFontSize();

            //BtnFontSize = 14.0 * ScaleManager.Scale;
            //* ScaleManager.Scale;

            BtnTextColor = ColorList.colorWhite;

            //var img = data.IconImage.Path;

            BtnImage = data.IconImage.Path;
            //BtnImage = "http://s3-ap-northeast-1.amazonaws.com/rcyoyakuappapi.sipss.jp/images/IconReservation.png";
            SetNotifi(data);

            BtnImageHeight = BtnImageWidth = ReturnImageWidth();

            BtnBgColor = ColorList.colorHomeMenuBtn;

            BtnImgLayout = CustomButton.LayoutPosition.Top;

            BtnCommand = command;

            BtnSize = GetBtnSize();
        }
        double itempadding = 3.0;

        private double GetBtnSize()
        {
            var sliderHieght = ScaleManager.ScreenWidth * 0.5625;
            var gridHeight = (ScaleManager.ScreenHeight - sliderHieght - 60.0) * 0.318;

            var gridWidth = ScaleManager.ScreenWidth;

            System.Diagnostics.Debug.WriteLine("gridHeight :" + gridHeight);
            System.Diagnostics.Debug.WriteLine("gridWidth :" + gridWidth);

            var sizeFromW = (gridWidth - 5.0d * itempadding) / 4.0d;
            var sizeFromH = (gridHeight - 3.0d * itempadding) / 2.0d;
            var itemSize = Math.Min(sizeFromH, sizeFromW);
            System.Diagnostics.Debug.WriteLine("itemSize :" + itemSize);
            return itemSize;
        }

        public HomeButtonViewViewModel(InlineResponse2002DataMenuList data)
        {

            if (data.MenuId == 0 || data.MenuId == null)
            {
                SetNullData();

                return;
            }

            ViewVisible = true;
            MenuID = (int)data.MenuId;

            BtnText = data.Name;

            //BtnFontSize = 14.0 * ScaleManager.Scale;
            BtnFontSize = ReturnFontSize();
            //* ScaleManager.Scale;

            BtnTextColor = ColorList.colorWhite;
            //var img = data.IconImage as IO.Swagger.Model.Image;

            //BtnImage = img.Path;

            BtnImage = data.IconImage.Path;

            //BtnImage = "http://s3-ap-northeast-1.amazonaws.com/rcyoyakuappapi.sipss.jp/images/IconReservation.png";
            //ImageSource.FromUri(new Uri(imagesource));

            SetNotifi(data);
            BtnImageHeight = BtnImageWidth = ReturnImageWidth();

            BtnBgColor = ColorList.colorHomeMenuBtn;

            BtnImgLayout = CustomButton.LayoutPosition.Top;
        }

        //batch
        void SetNotifi(InlineResponse2002DataMenuList data)
        {
            if (data.Notification != null)
            {
                BatchVisible = (bool)data.Notification;
            }
        }

        double ReturnImageWidth()
        {
            return
                //Device.RuntimePlatform == Device.Android
                //                  ? 22 * ScaleManager.Scale :
                                  26 * iOSScale();
        }

        double iOSScale()
        {
            return ScaleManager.Scale > 1.0 ? 1.0
                                   : ScaleManager.Scale;
        }

        double ReturnFontSize()
        {
            if (ScaleManager.Scale < 1.0)
            {
                //if (Device.RuntimePlatform == Device.iOS)
                //{
                //    return 11 * ScaleManager.Scale * 0.7;
                //}
                //else
                //{
                    return 11 * ScaleManager.Scale * 0.7;
                //}
            }
            else
            {
                //if (Device.RuntimePlatform == Device.iOS)
                //{
                    return 11;
                //}
                //else
                //{
                //    return 11;
                //}
            }
        }
        void SetNullData()
        {

        }



        private int _MenuID;
        public int MenuID
        {
            get { return _MenuID; }
            set
            {
                if (_MenuID != value)
                {
                    _MenuID = value;

                    switch (_MenuID)
                    {

                        case 1:

                            BtnCommand = new Command(() =>
                           {

                               ScreenTransition(new WebViewPage(WebViewPage.webViewType.PrivacyPolicy));
                           });
                            break;


                        default:

                            BtnCommand = new Command(() =>
                           {
                               ScreenTransition(new WebViewPage(WebViewPage.webViewType.TermsOfService));
                           });
                            break;
                    }

                    OnPropertyChanged("MenuID");
                }
            }
        }


        private bool _ViewVisible;
        public bool ViewVisible
        {
            get { return _ViewVisible; }
            set
            {
                if (_ViewVisible != value)
                {
                    _ViewVisible = value;
                    OnPropertyChanged("ViewVisible");
                }
            }
        }

        private string _BtnText;
        public string BtnText
        {
            get { return _BtnText; }
            set
            {
                if (_BtnText != value)
                {
                    _BtnText = value;
                    OnPropertyChanged("BtnText");
                }
            }
        }


        private CustomButton.LayoutPosition _BtnImgLayout;
        public CustomButton.LayoutPosition BtnImgLayout
        {
            get { return _BtnImgLayout; }
            set
            {
                if (_BtnImgLayout != value)
                {
                    _BtnImgLayout = value;
                    OnPropertyChanged("BtnImgLayout");
                }
            }
        }

        public double BtnFontSize { get; set; }

        private bool _BatchVisible;
        public bool BatchVisible
        {
            get { return _BatchVisible; }
            set
            {
                if (_BatchVisible != value)
                {
                    _BatchVisible = value;
                    OnPropertyChanged("BatchVisible");
                }
            }
        }

        private Color _BtnTextColor;
        public Color BtnTextColor
        {
            get { return _BtnTextColor; }
            set
            {
                if (_BtnTextColor != value)
                {
                    _BtnTextColor = value;
                    OnPropertyChanged("BtnTextColor");
                }
            }
        }

        private ImageSource _BtnImage;
        public ImageSource BtnImage
        {
            get { return _BtnImage; }
            set
            {
                if (_BtnImage != value)
                {
                    _BtnImage = value;
                    OnPropertyChanged("BtnImage");
                }
            }
        }


        public double BtnImageWidth { get; set; }

        public double BtnImageHeight { get; set; }


        private double _BtnSize;
        public double BtnSize
        {
            get { return _BtnSize; }
            set
            {
                System.Diagnostics.Debug.WriteLine(" btn size :" + value);
                if (value > 0)
                {
                    _BtnSize = value;
                    ShadowSize = value + 3.0d;
                    OnPropertyChanged("BtnSize");
                }
            }
        }

        private double _ShadowSize;
        public double ShadowSize
        {
            get { return _ShadowSize; }
            set
            {
                if (_ShadowSize != value)
                {
                    _ShadowSize = value;

                    OnPropertyChanged("ShadowSize");
                }
            }
        }
        private Color _BtnBgColor;
        public Color BtnBgColor
        {
            get { return _BtnBgColor; }
            set
            {
                if (_BtnBgColor != value)
                {
                    _BtnBgColor = value;
                    OnPropertyChanged("BtnBgColor");
                }
            }
        }




        private ObservableCollection<InlineResponse2002DataNotificationList> _NotificationList;
        public ObservableCollection<InlineResponse2002DataNotificationList> NotificationList
        {
            get { return _NotificationList; }
            set
            {
                if (NotificationList == null)
                    NotificationList = new ObservableCollection<InlineResponse2002DataNotificationList>();

                if (_NotificationList != value)
                {
                    _NotificationList = value;
                    OnPropertyChanged("NotificationList");
                }
            }
        }
        private Command _BtnCommand;
        public Command BtnCommand
        {
            get { return _BtnCommand; }
            set
            {
                if (_BtnCommand != value)
                {
                    _BtnCommand = value;

                    OnPropertyChanged("BtnCommand");


                }
            }
        }
    }
}
