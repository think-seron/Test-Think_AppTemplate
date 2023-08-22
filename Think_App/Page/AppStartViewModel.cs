using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
	public class AppStartViewModel : ViewModelBase
	{
		public AppStartViewModel()
		{
			ScreenSizeScale = ScaleManager.Scale;
			StartCommand = new Command(() => ScreenTransition(new ConsentPage()));
			CustomNavibarBC = new CustomNavigationBarViewModel(null, CustomNavigationBarViewModel.LeftBtnKinds.None, CustomNavigationBarViewModel.RightBtnKinds.None,null);

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
		public Command StartCommand { get; set; }
		public double ScreenSizeScale { get; set; }	
	}
}
