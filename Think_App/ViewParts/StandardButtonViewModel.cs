using System;
using Think_App;
using Xamarin.Forms;
namespace Think_App
{
	public class StandardButtonViewModel : ViewModelBase
	{
		public StandardButtonViewModel(string btnText, Command btnCommand = null)
		{
			BtnText = btnText;
			BtnClickedCommand = btnCommand;
			ScaleSize = ScaleManager.Scale;
		}
		public double ScaleSize { get; set; }
		private Command _BtnClickedCommand;
		public Command BtnClickedCommand
		{
			get { return _BtnClickedCommand; }
			set
			{
				if (_BtnClickedCommand != value)
				{
					_BtnClickedCommand = value;
					OnPropertyChanged("BtnClickedCommand");
				}
			}
		}
		private string _BtnText;
		public string BtnText
		{
			get { return _BtnText; }
			set
			{
				if (_BtnText != value)
				{
					_BtnText = value;
					OnPropertyChanged("BtnText");
				}
			}
		}

	}
}