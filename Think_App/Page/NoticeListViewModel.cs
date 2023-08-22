using System;
using System.Collections.Generic;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
    public class NoticeListViewModel : ViewModelBase
    {
        public NoticeListViewModel(bool soloSalon)
        {
            //ScreenSizeScale = ScaleManager.Scale;
            //CustomNavibarBC = new CustomNavigationBarViewModel("お知らせ", CustomNavigationBarViewModel.LeftBtnKinds.BackBtn, CustomNavigationBarViewModel.RightBtnKinds.Shops, null);
            //CustomNavibarBC.LeftBtnClicked = new Command(() => { ScreenTransition(); });
            //CustomNavibarBC.RightImageBtnClicked = new Command(() => { ScreenTransition(new StoreSelect(2, null)); });

            //iosかつ複数店舗の場合
            // TODO Xamarin.Forms.Device.RuntimePlatform is no longer supported. Use Microsoft.Maui.Devices.DeviceInfo.Platform instead. For more details see https://learn.microsoft.com/en-us/dotnet/maui/migration/forms-projects#device-changes
                                                                        if (Device.RuntimePlatform == Device.iOS && !soloSalon)
                ToolbarIcon = "Icon_Home.png";
        }

        //public double ScreenSizeScale { get; set; }
        public Rect ListViewRect { get; set; }

        private string salonName;
        public string SalonName
        {
            get
            {
                return salonName;
            }
            set
            {
                if (salonName != value)
                {
                    salonName = value;
                    OnPropertyChanged("SalonName");
                }
            }
        }


        private double hooterHeight;
        public double HooterHeight
        {
            get { return hooterHeight; }
            set
            {
                if (hooterHeight != value)
                {
                    hooterHeight = value;
                    OnPropertyChanged("HooterHeight");
                }
            }
        }

        public double HooterWidth { get; set; }

        private double hooterBtnHeight;
        public double HooterBtnHeight
        {
            get { return hooterBtnHeight; }
            set
            {
                if (hooterBtnHeight != value)
                {
                    hooterBtnHeight = value;
                    OnPropertyChanged("HooterBtnHeight");
                }
            }
        }

        private bool hooterIsVisible;
        public bool HooterIsVisible
        {
            get { return hooterIsVisible; }
            set
            {
                if (hooterIsVisible != value)
                {
                    hooterIsVisible = value;
                    if (hooterIsVisible == false)
                    {
                        this.HooterBtnHeight = 0;
                        this.HooterHeight = 0;
                    }
                    else
                    {
                        this.HooterBtnHeight = 36;
                        this.HooterHeight = 44;
                    }
                    OnPropertyChanged("HooterIsVisible");
                }
            }
        }
        private string _ToolbarIcon;
        public string ToolbarIcon
        {
            get { return _ToolbarIcon; }
            set
            {
                if (_ToolbarIcon != value)
                {
                    _ToolbarIcon = value;
                    OnPropertyChanged("ToolbarIcon");
                }
            }
        }
        private Command _ToolbarItemsClick;
        public Command ToolbarItemsClick
        {
            get { return _ToolbarItemsClick; }
            set
            {
                if (_ToolbarItemsClick != value)
                {
                    _ToolbarItemsClick = value;
                    OnPropertyChanged("ToolbarItemsClick");
                }
            }
        }
    }
}
