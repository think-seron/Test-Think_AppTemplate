using System;
using System.Collections.Generic;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
    public class CouponListPageViewModel : ViewModelBase
    {
        public CouponListPageViewModel(bool soloSalon)
        {
            // TODO Xamarin.Forms.Device.RuntimePlatform is no longer supported. Use Microsoft.Maui.Devices.DeviceInfo.Platform instead. For more details see https://learn.microsoft.com/en-us/dotnet/maui/migration/forms-projects#device-changes
            if (Device.RuntimePlatform == Device.iOS && !soloSalon)
                ToolbarIcon = "Icon_Home.png";
        }
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

        //private CustomNavigationBarViewModel _CustomNavibarBC;
        //public CustomNavigationBarViewModel CustomNavibarBC
        //{
        //	get { return _CustomNavibarBC; }
        //	set
        //	{
        //		if (_CustomNavibarBC != value)
        //		{
        //			_CustomNavibarBC = value;
        //			OnPropertyChanged("CustomNavibarBC");
        //		}
        //	}
        //}
    }
}
