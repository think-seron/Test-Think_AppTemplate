using System;
using Xamarin.Forms;

namespace Think_App
{
	public class CouponInfoPageViewModel : ViewModelBase
	{
		public CouponInfoPageViewModel()
		{
			//ScreenSizeScale = ScaleManager.Scale;
			//CustomNavibarBC = new CustomNavigationBarViewModel("クーポン", CustomNavigationBarViewModel.LeftBtnKinds.BackBtn, CustomNavigationBarViewModel.RightBtnKinds.None, null);
			//CustomNavibarBC.LeftBtnClicked = new Command(() => { ScreenTransition(); });
		}

		//public double ScreenSizeScale { get; set; }
		public Rectangle GridRect { get; set; }
		//public Rectangle ButtonRect { get; set; }
		//public string ImageSouce { get; set; }
		public ImageSource ImageSouce { get; set; }
		public string CouponTitle { get; set; }
		public string Type { get; set; }
		public string ShopName { get; set; }
		public string OperationContent { get; set; }
		public string DiscountContent { get; set; }
		public string TermsOfUse { get; set; }
		public string SpatialCondition { get; set; }
		public string Description { get; set; }
		public double CouponTitleFontSize { get; set; }
		public double ShopNameFontSize { get; set;}
		public double OperationContentFontSize { get; set; }
		public double DiscountContentFontSize { get; set; }
		public double TermsOfUseFontSize { get; set; }
		public double SpatialConditionFontSize { get; set; }
		public double DescriptionFontSize { get; set; }
		public double ImageWidth { get; set; }
		public double ImageHeght { get; set; }
		public double ButtonWidth { get; set; }
		public double ButtonHeight { get; set; }
		public bool BtnIsVisebld { get; set; }
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
