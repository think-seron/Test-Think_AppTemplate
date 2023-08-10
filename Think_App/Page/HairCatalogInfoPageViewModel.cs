using System;
using System.Linq;
using System.Threading.Tasks;
using IO.Swagger.Model;
using Xamarin.Forms;

namespace Think_App
{
    public class HairCatalogInfoPageViewModel : ViewModelBase
    {
        public HairCatalogInfoPageViewModel(bool isStaffNameVisible = false)
        {
            //CustomNavibarBC = new CustomNavigationBarViewModel("ヘアカタログ", CustomNavigationBarViewModel.LeftBtnKinds.BackBtn, CustomNavigationBarViewModel.RightBtnKinds.None, null);
            //CustomNavibarBC.LeftBtnClicked = new Command(() => { ScreenTransition(); });
            ScreenSizeScale = ScaleManager.Scale;
            LabelFontSize = 14 * ScaleManager.Scale;
            ButtonWidth = 177 * ScaleManager.Scale;
            ButtonHeight = 36 * ScaleManager.Scale;
            BtnFontSize = 12 * ScaleManager.Scale;
            StaffNameLblFontSize = 14 * ScaleManager.Scale;
            StaffNameLblOpacity = isStaffNameVisible ? 1 : 0;
            StaffNameLblHeight = 36 * ScaleManager.Scale;
            SetMessageBtnVisible();
        }

        private async void SetMessageBtnVisible()
        {
            var json = await APIManager.GET("home");

            var response = JsonManager.Deserialize<ResponseHome>(json);
            if (response == null
                || response.Data == null
                || response.Data.MenuList == null
                || !response.Data.MenuList.Any((x) => x.MenuId == 4))
                MessageSendBtnVisible = false;
            else
                MessageSendBtnVisible = true;
        }

        public bool MessageSendBtnVisible { get; set; }
        public double BtnFontSize { get; set; }
        public double ScreenSizeScale { get; set; }
        public string BtnTxt { get; set; }
        //public string Souce { get; set; }
        public ImageSource Souce { get; set; }
        public string LabelTxt { get; set; }
        public double LabelFontSize { get; set; }
        public double ButtonWidth { get; set; }
        public double ButtonHeight { get; set; }
        public double ImageWidth { get; set; }
		public double ImageHeight { get; set;}
        public double StaffNameLblFontSize { get; set; }
        public string StaffNameLblTxt { get; set; }
        public double StaffNameLblOpacity { get; set; }
        public double StaffNameLblHeight { get; set; }
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
