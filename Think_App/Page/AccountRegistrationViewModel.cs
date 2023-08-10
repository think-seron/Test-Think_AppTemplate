using Xamarin.Forms;


namespace Think_App
{
    public class AccountRegistrationViewModel : ViewModelBase
    {
        public AccountRegistrationViewModel(int pageFlg)
        {
            ScreenSizeScale = ScaleManager.Scale;
            CustomEntryName = new CustomEntryCellViewModel();
            CustomEntryName.LabelText = "氏名*";
            CustomEntryName.Placeholder = "入力してください";
            CustomEntryName.EntryKeyboard = Keyboard.Text;

            CustomEntryKana = new CustomEntryCellViewModel();
            CustomEntryKana.LabelText = "フリガナ";
            CustomEntryKana.Placeholder = "入力してください";
            CustomEntryKana.EntryKeyboard = Keyboard.Text;

            CustomEntryTel = new CustomEntryCellViewModel();
            CustomEntryTel.LabelText = "TEL";
            CustomEntryTel.Placeholder = "000-0000-0000";
            CustomEntryTel.EntryKeyboard = Keyboard.Url;

            CustomEntryMail = new CustomEntryCellViewModel();
            CustomEntryMail.LabelText = "E-mail";
            CustomEntryMail.Placeholder = "入力してください";
            CustomEntryMail.EntryKeyboard = Keyboard.Url;

            if (pageFlg == 1 || pageFlg == 3)
            {
                RegBtnIsVisible = true;
                CustomEntryName.EntryIsEnabled = true;
                CustomEntryKana.EntryIsEnabled = true;
                CustomEntryTel.EntryIsEnabled = true;
                CustomEntryMail.EntryIsEnabled = true;
                RadioGroupIsVisible = true;
                ToolbarItemsTxt = "";
                PageTitle = "登録";
                RadioGroupBoxViewRect = new Rectangle(0, 0, 0, 0);
            }
            else if (pageFlg == 4)
            {
                RegBtnIsVisible = false;
                CustomEntryName.EntryIsEnabled = true;
                CustomEntryKana.EntryIsEnabled = true;
                CustomEntryTel.EntryIsEnabled = true;
                CustomEntryMail.EntryIsEnabled = true;
                RadioGroupIsVisible = true;
                PageTitle = "設定";
                ToolbarItemsTxt = "完了";
                RadioGroupBoxViewRect = new Rectangle(0, 0, 0, 0);
            }
            else
            {
                RegBtnIsVisible = false;
                CustomEntryName.EntryIsEnabled = false;
                CustomEntryKana.EntryIsEnabled = false;
                CustomEntryTel.EntryIsEnabled = false;
                CustomEntryMail.EntryIsEnabled = false;
                RadioGroupIsVisible = false;
                PageTitle = "設定";
                ToolbarItemsTxt = "編集";
                RadioGroupBoxViewRect = new Rectangle(0, 0, 1, 1);
            }



            //CustomNavibarBC = new CustomNavigationBarViewModel(null, CustomNavigationBarViewModel.LeftBtnKinds.BackBtn, CustomNavigationBarViewModel.RightBtnKinds.None, null);
            //CustomNavibarBC.LeftBtnClicked = new Command(() => { ScreenTransition(); });
        }

        public string PageTitle { get; set; }
        public double ScreenSizeScale { get; set; }
        public CustomEntryCellViewModel CustomEntryName { get; set; }
        public CustomEntryCellViewModel CustomEntryKana { get; set; }
        public CustomEntryCellViewModel CustomEntryTel { get; set; }
        public CustomEntryCellViewModel CustomEntryMail { get; set; }
        public Color SelectedRadioColor => Device.RuntimePlatform == Device.Android ?
            Color.FromHex("#009788") : Color.Black;

        private string toolbarItemsTxt;
        public string ToolbarItemsTxt
        {
            get
            {
                return toolbarItemsTxt;
            }
            set
            {
                if (toolbarItemsTxt != value)
                {
                    toolbarItemsTxt = value;
                    OnPropertyChanged("ToolbarItemsTxt");
                }
            }
        }


        private bool radioGroupIsVisible;
        public bool RadioGroupIsVisible
        {
            get
            {
                return radioGroupIsVisible;
            }
            set
            {
                if (radioGroupIsVisible != value)
                {
                    radioGroupIsVisible = value;
                    OnPropertyChanged("RadioGroupIsVisible");
                }
            }
        }


        private bool regBtnIsVisible;
        public bool RegBtnIsVisible
        {
            get
            {
                return regBtnIsVisible;
            }
            set
            {
                if (regBtnIsVisible != value)
                {
                    regBtnIsVisible = value;
                    OnPropertyChanged("RegBtnIsVisible");
                }
            }
        }

        // ラジオボタンを無効にするため被せているBoxViewのRectangle
        private Rectangle radioGroupBoxViewRect;
        public Rectangle RadioGroupBoxViewRect
        {
            get
            {
                return radioGroupBoxViewRect;
            }
            set
            {
                if (radioGroupBoxViewRect != value)
                {
                    radioGroupBoxViewRect = value;
                    OnPropertyChanged("RadioGroupBoxViewRect");
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
