using System;
using Xamarin.Forms;
namespace Think_App
{
	public class StoreListPageViewModel : ViewModelBase
	{
		public StoreListPageViewModel()
		{
			//CustomNavibarBC = new CustomNavigationBarViewModel("店舗一覧", CustomNavigationBarViewModel.LeftBtnKinds.BackBtn, CustomNavigationBarViewModel.RightBtnKinds.None, null);
			//CustomNavibarBC.LeftBtnClicked = new Command(() => { ScreenTransition(); });
		}
		public Rectangle ListViewRect { get; set; }

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
