using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
	public class LoginMethodSelectViewModel : ViewModelBase
	{
		public LoginMethodSelectViewModel()
		{
			ScreenSizeScale = ScaleManager.Scale;
			SNSCommand = new Command(async() =>
			{

				App.customNavigationPage.IsRunning = true;
				await System.Threading.Tasks.Task.Delay(1000);
				ScreenTransition(new SnsAccountSelect(2));
				App.customNavigationPage.IsRunning = false;
			});
			CodeCommand = new Command(() => ScreenTransition(new CodeLogin()));
			CustomNavibarBC = new CustomNavigationBarViewModel(null, CustomNavigationBarViewModel.LeftBtnKinds.BackBtn, CustomNavigationBarViewModel.RightBtnKinds.None, null);
			CustomNavibarBC.LeftBtnClicked = new Command(() => { ScreenTransition(); });

		}

		public Command SNSCommand { get; set; }
		public Command CodeCommand { get; set; }
		public double ScreenSizeScale { get; set; }	

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
