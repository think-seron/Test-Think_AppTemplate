using Xamarin.Forms;

namespace Think_App
{
	public class ModalViewViewModel : ViewModelBase
	{
		public ModalViewViewModel()
		{
			ScreenSizeScale = ScaleManager.Scale;
		}

		private string yesButtonTxt;
		public string YesButtonTxt
		{
			get
			{
				return yesButtonTxt;
			}
			set
			{
				if (yesButtonTxt != value)
				{
					yesButtonTxt = value;
					OnPropertyChanged("YesButtonTxt");
				}
			}
		}
		private string noButtonTxt;
		public string NoButtonTxt
		{
			get
			{
				return noButtonTxt;
			}
			set
			{
				if (noButtonTxt != value)
				{
					noButtonTxt = value;
					OnPropertyChanged("NoButtonTxt");
				}
			}
		}

		public double ScreenSizeScale { get; set; }

		private string modalLabelTxt;
		public string ModalLabelTxt
		{
			get
			{
				return modalLabelTxt;
			}
			set
			{
				if (modalLabelTxt != value)
				{
					modalLabelTxt = value;
					OnPropertyChanged("ModalLabelTxt");
				}
			}
		}

		double _ModalLabelFontSize = 20;
		public double ModalLabelFontSize
		{
			get
			{
				return _ModalLabelFontSize;
			}
			set
			{
				SetProperty(ref _ModalLabelFontSize, value);
			}
		}

		private Rectangle modalBgLayoutBounds;
		public Rectangle ModalBgLayoutBounds
		{
			get
			{
				return modalBgLayoutBounds;
			}
			set
			{
				if (modalBgLayoutBounds != value)
				{
					modalBgLayoutBounds = value;
					OnPropertyChanged("ModalBgLayoutBounds");
				}
			}
		}

		private Rectangle okBtnLayoutBounds;
		public Rectangle OKBtnLayoutBounds
		{
			get
			{
				return okBtnLayoutBounds;
			}
			set
			{
				if (okBtnLayoutBounds != value)
				{
					okBtnLayoutBounds = value;
					OnPropertyChanged("OKBtnLayoutBounds");
				}
			}
		}

		private Rectangle nomalModalLabelRect;
		public Rectangle NomalModalLabelRect
		{
			get
			{
				return nomalModalLabelRect;
			}
			set
			{
				if (nomalModalLabelRect != value)
				{
					nomalModalLabelRect = value;
					OnPropertyChanged("NomalModalLabelRect");
				}
			}
		}

		private Rectangle modalViewLayoutBounds;
		public Rectangle ModalViewLayoutBounds
		{
			get
			{
				return modalViewLayoutBounds;
			}
			set
			{
				if (modalViewLayoutBounds != value)
				{
					modalViewLayoutBounds = value;
					OnPropertyChanged("ModalViewLayoutBounds");
				}
			}
		}

		private Rectangle selectBtnLayoutBounds;
		public Rectangle SelectBtnLayoutBounds
		{
			get
			{
				return selectBtnLayoutBounds;
			}
			set
			{
				if (selectBtnLayoutBounds != value)
				{
					selectBtnLayoutBounds = value;
					OnPropertyChanged("SelectBtnLayoutBounds");
				}
			}
		}

		private Rectangle imageRect;
		public Rectangle ImageRect
		{
			get
			{
				return imageRect;
			}
			set
			{
				SetProperty(ref imageRect, value);
			}
		}

		private ImageSource imageSource;
		public ImageSource ImageSource
		{
			get
			{
				return imageSource;
			}
			set
			{
				SetProperty(ref imageSource, value);
			}
		}

		private Aspect imageAspect;
		public Aspect ImageAspect
		{
			get
			{
				return imageAspect;
			}
			set
			{
				SetProperty(ref imageAspect, value);
			}
		}

		private double imageWidth;
		public double ImageWidth
		{
			get
			{
				return imageWidth;
			}
			set
			{
				SetProperty(ref imageWidth, value);
			}
		}

		private double imageHeight;
		public double ImageHeight
		{
			get
			{
				return imageHeight;
			}
			set
			{
				SetProperty(ref imageHeight, value);
			}
		}
	}
}
