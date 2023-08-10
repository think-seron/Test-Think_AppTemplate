using System;
using Xamarin.Forms;

namespace Think_App
{
	public class ConsentPageViewModel : ViewModelBase
	{
		public ConsentPageViewModel()
		{
			ScreenSizeScale = ScaleManager.Scale;
			ConsentCommand = new Command(() => ScreenTransition(new RegistrationTop()));
			//CustomNavibarBC = new CustomNavigationBarViewModel(null, CustomNavigationBarViewModel.LeftBtnKinds.BackBtn, CustomNavigationBarViewModel.RightBtnKinds.None, null);
			//CustomNavibarBC.LeftBtnClicked = new Command(() => {
			//	ScreenTransition();
			//});

			ButtonEnable = false;
		}
        HtmlWebViewSource _Source;
		public HtmlWebViewSource Source
        {
            get { return _Source; }
            set{
                if(_Source!=value)
                {
                    _Source = value;
                    OnPropertyChanged("Source");
                }
            }
        }

		public double ScreenSizeScale { get; set; }
		public Command ConsentCommand { get; set; }
		private bool buttonEnable;
		public bool ButtonEnable
		{
			get
			{
				return buttonEnable;
			}
			set
			{
				if (buttonEnable != value)
				{
					buttonEnable = value;
                    OnPropertyChanged("ButtonEnable");
				}
			}
		}
		private CustomNavigationBarViewModel _CustomNavibarBC;
		public CustomNavigationBarViewModel CustomNavibarBC
		{
			get { return _CustomNavibarBC; }
			set
			{
				if (_CustomNavibarBC != value)
				{
					_CustomNavibarBC = value;
					OnPropertyChanged("CustomNavibarBC");
				}
			}
		}
	}
}
