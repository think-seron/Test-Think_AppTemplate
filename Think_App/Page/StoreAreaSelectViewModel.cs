using System;
using Xamarin.Forms;

namespace Think_App
{
	public class StoreAreaSelectViewModel : ViewModelBase
	{
		public StoreAreaSelectViewModel()
		{
			//ScreenSizeScale = ScaleManager.Scale;
			//CustomNavibarBC = new CustomNavigationBarViewModel("店舗選択", CustomNavigationBarViewModel.LeftBtnKinds.BackBtn, CustomNavigationBarViewModel.RightBtnKinds.None, null);
			//CustomNavibarBC.LeftBtnClicked = new Command(() => { ScreenTransition(); });
		}

		//public double ScreenSizeScale { get; set; }
		public Rectangle ListViewRect { get; set; }
		//public Rectangle BottomLabelRect { get; set; }
		public string LeftLabel { get; set; }
		public string RightLabel { get; set; }
		public string HooterLabel { get; set; }
		public double HooterWidth { get; set; }

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
