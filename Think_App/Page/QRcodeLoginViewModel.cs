using Xamarin.Forms;
using System;
using System.Threading.Tasks;

namespace Think_App
{
	public class QRcodeLoginViewModel : ViewModelBase
	{
		public QRcodeLoginViewModel()
		{
			ScreenSizeScale = ScaleManager.Scale;
			CustomEntryTel = new CustomEntryCellViewModel();
			CustomEntryTel.LabelText = "TEL";
			CustomEntryTel.Placeholder = "000-0000-0000";
			CustomEntryTel.EntryKeyboard = Keyboard.Numeric;
            TextSize = ScaleManager.SizeSet(12);
			CustomEntryTel.EntryIsEnabled = true;
			//CustomNavibarBC = new CustomNavigationBarViewModel(null, CustomNavigationBarViewModel.LeftBtnKinds.BackBtn, CustomNavigationBarViewModel.RightBtnKinds.None, null);
			//CustomNavibarBC.LeftBtnClicked = new Command(() => { ScreenTransition(); });
		}

		public double TextSize { get; set; }
		public double ScreenSizeScale { get; set; }
		public CustomEntryCellViewModel CustomEntryTel { get; set; }
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
