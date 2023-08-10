using System;
using Xamarin.Forms;

namespace Think_App
{
	public class StoreInformationPageViewModel : ViewModelBase
	{
		public StoreInformationPageViewModel()
		{
			//CustomNavibarBC = new CustomNavigationBarViewModel("店舗一覧", CustomNavigationBarViewModel.LeftBtnKinds.BackBtn, CustomNavigationBarViewModel.RightBtnKinds.None, null);
			//CustomNavibarBC.LeftBtnClicked = new Command(() => { ScreenTransition(); });

            StoreImgWidth = ScaleManager.SizeSet(135);
            StoreImgHeight = ScaleManager.SizeSet(135);
			FavoriteIconWidth = 27.63 * ScaleManager.Scale;
			FavoriteIconHeight = 25 * ScaleManager.Scale;
            TelBtnWidth = ScaleManager.SizeSet(168);
            TelBtnHeight = ScaleManager.SizeSet(36);
			BtnImgSize = TelBtnHeight / 2;
			//TelBtnShadowWidth = TelBtnWidth + 7;
			//TelBtnShadowHeight = TelBtnHeight + 7;
			BoxViewWidth = ScaleManager.ScreenWidth;
            StoreNameFontSize = ScaleManager.SizeSet(18);
            StoreAddrFontSize = ScaleManager.SizeSet(14);
		}

		public double StoreNameFontSize { get; set; }
		public double StoreAddrFontSize { get; set; }
		public double StoreImgWidth { get; set; }
		public double StoreImgHeight { get; set; }
		public string StoreImgSouce { get; set; }
		public double FavoriteIconWidth { get; set; }
		public double FavoriteIconHeight { get; set; }

		private string favoIconSouce;
		public string FavoIconSouce
		{
			get { return favoIconSouce; }
			set
			{
				if (favoIconSouce != value)
				{
					favoIconSouce = value;
					OnPropertyChanged("FavoIconSouce");
				}
			}
		}

		public string StoreNameTxt { get; set; }
		public double TelBtnWidth { get; set; }
		public double TelBtnHeight { get; set; }
		//public double TelBtnShadowWidth { get; set; }
		//public double TelBtnShadowHeight { get; set; }
		public double BoxViewWidth { get; set; }
		public string StoreMessage { get; set; }
		public string StoreBusinessHour { get; set; }
		public string StoreAddress { get; set; }
		public string StoreTelNumber { get; set; }
		public double BtnImgSize { get; set; }

		//public Rectangle ListViewRect { get; set; }

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
