using System;
using Xamarin.Forms;

namespace Think_App
{
	public class StaffInformationPageViewModel : ViewModelBase
	{
		public StaffInformationPageViewModel(bool canReserv)
		{
			//CustomNavibarBC = new CustomNavigationBarViewModel("スタッフ情報", CustomNavigationBarViewModel.LeftBtnKinds.BackBtn, CustomNavigationBarViewModel.RightBtnKinds.None, null);
			//CustomNavibarBC.LeftBtnClicked = new Command(() => { ScreenTransition(); });
            StaffImgWidth = ScaleManager.SizeSet(135);
            StaffImgHeight = ScaleManager.SizeSet(135);
			//StaffNameWidth = 150 * ScaleManager.Scale;
            ReserveBtnWidth = ScaleManager.SizeSet(168);
            ReserveBtnHeight = ScaleManager.SizeSet(36);
			//ReserveBtnShadowWidth = 167 * ScaleManager.Scale;
			//ReserveBtnShadowHeight = 34 * ScaleManager.Scale;
            StaffNameFontSize = ScaleManager.SizeSet(18);
            StaffNameKanaFontSize = ScaleManager.SizeSet(10);
            StaffCareerFontSize = ScaleManager.SizeSet(12);
            MessageFontSize = ScaleManager.SizeSet(14);
            GoodImageFontSize = ScaleManager.SizeSet(14);
			BtnRect = new Rectangle(0, 0, 1, 1); // 値は固定値だがページ遷移した際のボタンの拡縮が気になるのでBindingしている

            //予約できる場合は表示
            //できなければ非表示
            ReservBtnVisible = canReserv;
		}

        public bool ReservBtnVisible { get; set; }

		public Rectangle BtnRect { get; set; }
		public double StaffNameFontSize { get; set; }
		public double StaffNameKanaFontSize { get; set; }
		public double StaffCareerFontSize { get; set;}
		public double MessageFontSize { get; set; }
		public double GoodImageFontSize { get; set; }

		public string StaffImgSouce { get; set; }
		public double StaffImgWidth { get; set; }
		public double StaffImgHeight { get; set; }
		public string StaffNameTxt { get; set; }
		public double StaffNameWidth { get; set; }
		public string StaffNameKana { get; set; }
		public string StaffCareer { get; set; }
		public string StaffComment { get; set; }
		public double ReserveBtnWidth { get; set; }
		public double ReserveBtnHeight { get; set; }
		//public double ReserveBtnShadowWidth { get; set; }
		//public double ReserveBtnShadowHeight { get; set; }
		public string Message { get; set; }

		public bool BtnIsVisible { get; set;}

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
		private string isFavorite;
		public string IsFavorite
		{
			get { return isFavorite; }
			set
			{
				if (isFavorite != value)
				{
					isFavorite = value;
					OnPropertyChanged("IsFavorite");
				}
			}
		}






		public string GoodImage { get; set; }
		public string GoodTechnic { get; set; }
		//public double ScreenWidth { get; set; }

		public Rectangle ScrollViewRect { get; set; }
	}
}
