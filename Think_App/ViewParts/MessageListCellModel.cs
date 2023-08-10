using System;
using Xamarin.Forms;
namespace Think_App
{
	public class MessageListCellModel : ViewModelBase
	{
		string _MLCSalonName;
		public string MLCSalonName
		{
			get
			{
				return _MLCSalonName;
			}
			set
			{
				SetProperty(ref _MLCSalonName, value);
			}
		}

		double _MLCBalloonViewWidth;
		public double MLCBalloonViewWidth
		{
			get
			{
				return _MLCBalloonViewWidth;
			}
			set
			{
				SetProperty(ref _MLCBalloonViewWidth, value);
			}
		}

		double _MLCBalloonViewHeight;
		public double MLCBalloonViewHeight
		{
			get
			{
				return _MLCBalloonViewHeight;
			}
			set
			{
             	SetProperty(ref _MLCBalloonViewHeight, value);
			}
		}

		LayoutOptions _MLCBalloonViewHorizontalOptions;
		public LayoutOptions MLCBalloonViewHorizontalOptions
		{
			get
			{
				return _MLCBalloonViewHorizontalOptions;
			}
			set
			{
				SetProperty(ref _MLCBalloonViewHorizontalOptions, value);
			}
		}

		Color _MLCBalloonViewColor;
		public Color MLCBalloonViewColor
		{
			get
			{
				return _MLCBalloonViewColor;
			}
			set
			{
				SetProperty(ref _MLCBalloonViewColor, value);
			}
		}

		double _MLCBalloonViewTailWidth;
		public double MLCBalloonViewTailWidth
		{
			get
			{
				return _MLCBalloonViewTailWidth;
			}
			set
			{
				SetProperty(ref _MLCBalloonViewTailWidth, value);
			}
		}

		BalloonView.Direction _MLCBalloonViewTailDirection;
		public BalloonView.Direction MLCBalloonViewTailDirection
		{
			get
			{
				return _MLCBalloonViewTailDirection;
			}
			set
			{
				SetProperty(ref _MLCBalloonViewTailDirection, value);
			}
		}

		bool _MLCTextVisible;
		public bool MLCTextVisible
		{
			get
			{
				return _MLCTextVisible;
			}
			set
			{
				SetProperty(ref _MLCTextVisible, value);
			}
		}

		string _MLCMessageText;
		public string MLCMessageText
		{
			get
			{
				return _MLCMessageText;
			}
			set
			{
				SetProperty(ref _MLCMessageText, value);
			}
		}

		double _MLCMessageFontSize;
		public double MLCMessageFontSize
		{
			get
			{
				return _MLCMessageFontSize;
			}
			set
			{
				SetProperty(ref _MLCMessageFontSize, value);
			}
		}

		bool _MLCImageVisible;
		public bool MLCImageVisible
		{
			get
			{
				return _MLCImageVisible;
			}
			set
			{
				SetProperty(ref _MLCImageVisible, value);
			}
		}


        private DateTime _mLCMessageTime;
        public DateTime MLCMessageTime
        {
            get { return _mLCMessageTime; }
            set
            {
                if (_mLCMessageTime == value) return;
                _mLCMessageTime = value;
                OnPropertyChanged(nameof(MLCMessageTime));
            }
        }

        ImageSource _MLCImageSource;
		public ImageSource MLCImageSource
		{
			get
			{
				return _MLCImageSource;
			}
			set
			{
				SetProperty(ref _MLCImageSource, value);
			}
		}

		double _MLCImageDownsampleWidth;
		public double MLCImageDownsampleWidth
		{
			get
			{
				return _MLCImageDownsampleWidth;
			}
			set
			{
				SetProperty(ref _MLCImageDownsampleWidth, value);
			}
		}

		double _MLCImageDownsampleHeight;
		public double MLCImageDownsampleHeight
		{
			get
			{
				return _MLCImageDownsampleHeight;
			}
			set
			{
				SetProperty(ref _MLCImageDownsampleHeight, value);
			}
		}

		Thickness _MLCMessageMargin;
		public Thickness MLCMessageMargin
		{
			get
			{
				return _MLCMessageMargin;
			}
			set
			{
				SetProperty(ref _MLCMessageMargin, value);
			}
		}

		string _MLCDateLblText;
		public string MLCDateLblText
		{
			get
			{
				return _MLCDateLblText;
			}
			set
			{
				SetProperty(ref _MLCDateLblText, value);
			}
		}

		Thickness _MLCDateLblPadding;
		public Thickness MLCDateLblPadding
		{
			get
			{
				return _MLCDateLblPadding;
			}
			set
			{
				SetProperty(ref _MLCDateLblPadding, value);
			}
		}

		double _MLCDateLblHeight;
		public double MLCDateLblHeight
		{
			get
			{
				return _MLCDateLblHeight;
			}
			set
			{
				SetProperty(ref _MLCDateLblHeight, value);
			}
		}

		public DateTime PostedDateTime { get; set; }


        private bool _isLeftTimeVisible;
        public bool IsLeftTimeVisible
        {
            get { return _isLeftTimeVisible; }
            set
            {
                if (_isLeftTimeVisible == value) return;
                _isLeftTimeVisible = value;
                OnPropertyChanged(nameof(IsLeftTimeVisible));
            }
        }

        private bool _isRightTimeVisible;
        public bool IsRightTimeVisible
        {
            get { return _isRightTimeVisible; }
            set
            {
                if (_isRightTimeVisible == value) return;
                _isRightTimeVisible = value;
                OnPropertyChanged(nameof(IsRightTimeVisible));
            }
        }


        private Thickness _layoutPadding;
        public Thickness LayoutPadding
        {
            get { return _layoutPadding; }
            set
            {
                if (_layoutPadding == value) return;
                _layoutPadding = value;
                OnPropertyChanged(nameof(LayoutPadding));
            }
        }
    }
}