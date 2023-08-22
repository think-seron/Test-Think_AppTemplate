using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
	public class CodeLoginViewModel : ViewModelBase
	{
		public CodeLoginViewModel()
		{
			ScreenSizeScale = ScaleManager.Scale;
			CustomEntryCode = new CustomEntryCellViewModel();
			CustomEntryCode.EntryIsEnabled = true;
			CustomEntryCode.LabelText = "引き継ぎコード";
			CustomEntryCode.Placeholder = "入力してください";
			CustomNavibarBC = new CustomNavigationBarViewModel(null, CustomNavigationBarViewModel.LeftBtnKinds.BackBtn, CustomNavigationBarViewModel.RightBtnKinds.None, null);
			CustomNavibarBC.LeftBtnClicked = new Command(() => { ScreenTransition(); });
		}

		public Command LoginCommand { get; set; }
		public double ScreenSizeScale { get; set; }
		public CustomEntryCellViewModel CustomEntryCode { get; set; }

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
