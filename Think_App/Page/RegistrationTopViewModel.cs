using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
	public class RegistrationTopViewModel : ViewModelBase
	{
		public RegistrationTopViewModel()
		{
			ScreenSizeScale = ScaleManager.Scale;

			SignUpCommand = new Command(() => ScreenTransition(new AccountRegistration(1)));
			SNSCommand = new Command(() => ScreenTransition(new SnsAccountSelect(1)));
			//CustomNavibarBC = new CustomNavigationBarViewModel(null, CustomNavigationBarViewModel.LeftBtnKinds.BackBtn, CustomNavigationBarViewModel.RightBtnKinds.None,null);
			//CustomNavibarBC.BatchVisible = true;
			//CustomNavibarBC.LeftBtnClicked = new Command(() => {
			//	ScreenTransition();
			//});
			ButtonWidth = 224 * ScaleManager.Scale;
			ButtonHeight = 50 * ScaleManager.Scale;
			ButtonFontSize = 18 * ScaleManager.Scale;
		}

		public Command SignUpCommand { get; set; }
		public Command SNSCommand { get; set; }
		public double ScreenSizeScale { get; set; }

		public double ButtonWidth { get; set; }
		public double ButtonHeight { get; set; }
		public double ButtonFontSize { get; set; }
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
