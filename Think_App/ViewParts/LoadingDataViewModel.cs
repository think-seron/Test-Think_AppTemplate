using System;
using Xamarin.Forms;

namespace Think_App
{
	public class LoadingDataViewModel : ViewModelBase
	{
        private string _loadingMessageLblText;
        public string LoadingMessageLblText
        {
            get => _loadingMessageLblText;
            set
            {
                if (_loadingMessageLblText == value) return;
                _loadingMessageLblText = value;
                OnPropertyChanged(nameof(LoadingMessageLblText));
            }
        }

        double _CommonFontSize;
		public double CommonFontSize
		{
			get
			{
				return _CommonFontSize;
			}
			set
			{
				SetProperty(ref _CommonFontSize, value);
			}
		}
	}
}
