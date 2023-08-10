using System;
namespace Think_App
{
	public class CustomSwitchCellViewModel : ViewModelBase
	{
		public CustomSwitchCellViewModel()
		{
		}

		private bool switchIsToggled;
		public bool SwitchIsToggled
		{
			get
			{
				return switchIsToggled;
			}
			set
			{
				if (switchIsToggled != value)
				{
					switchIsToggled = value;
					OnPropertyChanged("SwitchIsToggled");
				}
			}
		}

		private string labelText;
		public string LabelText
		{
			get
			{
				return labelText;
			}
			set
			{
				if (labelText != value)
				{
					labelText = value;
					OnPropertyChanged("LabelText");
				}
			}
		}

        private bool _ViewVisible;
        public bool ViewVisible
        {
            get => _ViewVisible;
            set
            {
                if (_ViewVisible != value)
                {
                    _ViewVisible = value;
                    OnPropertyChanged(nameof(ViewVisible));
                }
            }
        }
	}
}
