using System;
using Xamarin.Forms;

namespace Think_App
{
	public class StoreSelectViewModel : ViewModelBase
	{
		// lastPageFlgについて
		// 1: 新規登録ページからの遷移
		// 2: お知らせページからの遷移
		// 3: クーポンページからの遷移
		public StoreSelectViewModel(int lastPageFlg)
		{
			//ScreenSizeScale = ScaleManager.Scale;
			//if (lastPageFlg == 1)
			//{
				//CustomNavibarBC = new CustomNavigationBarViewModel("店舗選択", CustomNavigationBarViewModel.LeftBtnKinds.BackBtn, CustomNavigationBarViewModel.RightBtnKinds.None, null);
			//}
			//else if (lastPageFlg == 2)
			//{
				//CustomNavibarBC = new CustomNavigationBarViewModel("お知らせ", CustomNavigationBarViewModel.LeftBtnKinds.BackBtn, CustomNavigationBarViewModel.RightBtnKinds.None, null);
			//}
			//else if (lastPageFlg == 3)
			//{
				//CustomNavibarBC = new CustomNavigationBarViewModel("クーポン", CustomNavigationBarViewModel.LeftBtnKinds.BackBtn, CustomNavigationBarViewModel.RightBtnKinds.None, null);
			//}
			//CustomNavibarBC.LeftBtnClicked = new Command(() => { ScreenTransition(); });
		}

		public string TopRightLabelTxt { get; set; }
		public string TopLabelTxt { get; set; }
		//public double ScreenSizeScale { get; set; }
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
