using System;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
	public class NoticePageViewModel : ViewModelBase
	{
		public NoticePageViewModel()
		{
			//CustomNavibarBC = new CustomNavigationBarViewModel("お知らせ", CustomNavigationBarViewModel.LeftBtnKinds.BackBtn, CustomNavigationBarViewModel.RightBtnKinds.None, null);
			//CustomNavibarBC.LeftBtnClicked = new Command(() => { ScreenTransition(); });
		}

		public string NoticeContents { get; set; }
		public Rect ScrollViewRect { get; set; }
		public string NoticeTitle { get; set; }
		public double NoticeTitleFontSize { get; set; }
		public double NoticeContentsFontSize { get; set; }

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
