using System;
using Xamarin.Forms;

namespace Think_App
{
	public class MyBeautyBlogEditPageViewModel : ViewModelBase
	{
		public MyBeautyBlogEditPageViewModel()
		{
			ScreenWidth = ScaleManager.ScreenWidth;
            CustomEntryCellWidth = ScaleManager.ScreenWidth - 97;
            EditorHeight = 192 * ScaleManager.Scale;
			HairBtnCheck = false;
			NailBtnCheck = false;
			EyelashBtnCheck = false;
			EstheticBtnCheck = false;

			HairBtnCommand = new Command(() =>
			{
				if (HairBtnCheck)
				{
					HairBtnCheck = false;
				}
				else
				{
					HairBtnCheck = true;
					NailBtnCheck = false;
					EyelashBtnCheck = false;
					EstheticBtnCheck = false;
				}
			});

			NailBtnCommand = new Command(() =>
			{
				if (NailBtnCheck)
				{
					NailBtnCheck = false;
				}
				else
				{
					NailBtnCheck = true;
					HairBtnCheck = false;
					EyelashBtnCheck = false;
					EstheticBtnCheck = false;
				}
			});

			EyelashBtnCommand = new Command(() =>
			{
				if (EyelashBtnCheck)
				{
					EyelashBtnCheck = false;
				}
				else
				{
					EyelashBtnCheck = true;
					HairBtnCheck = false;
					NailBtnCheck = false;
					EstheticBtnCheck = false;
				}
			});

			EstheticBtnCommand = new Command(() =>
			{
				if (EstheticBtnCheck)
				{
					EstheticBtnCheck = false;
				}
				else
				{
					EstheticBtnCheck = true;
					HairBtnCheck = false;
					NailBtnCheck = false;
					EyelashBtnCheck = false;
				}
			});
		}

		private string datePickerFormat;
		public string DatePickerFormat
		{
			get
			{
				return datePickerFormat;
			}
			set
			{
				if (datePickerFormat != value)
				{
					datePickerFormat = value;
					OnPropertyChanged("DatePickerFormat");
				}
			}
		}

		public string DatePickerTxt { get; set; }
		public double ScreenWidth { get; set;}
		public double CustomEntryCellWidth { get; set; }
        public double EditorHeight { get; set; }
		public Command HairBtnCommand { get; set; }
		public Command NailBtnCommand { get; set; }
		public Command EyelashBtnCommand { get; set; }
		public Command EstheticBtnCommand { get; set; }
		public int? myBeautyBlogId { get; set; }

		private ImageSource imgSouce;
		public ImageSource ImgSouce
		{
			get
			{
				return imgSouce;
			}
			set
			{
				if (imgSouce != value)
				{
					imgSouce = value;
					OnPropertyChanged("ImgSouce");
				}
			}
		}


		private bool hairBtnCheck;
		public bool HairBtnCheck
		{
			get
			{
				return hairBtnCheck;
			}
			set
			{
				if (hairBtnCheck != value)
				{
					hairBtnCheck = value;
					OnPropertyChanged("HairBtnCheck");
				}
				if (hairBtnCheck)
				{
					HairBtnColor = ColorList.colorNeutral;
				}
				else
				{
					HairBtnColor = ColorList.colorWhite;
				}
			}
		}
		private Color hairBtnColor;
		public Color HairBtnColor
		{
			get
			{
				return hairBtnColor;
			}
			set
			{
				if (hairBtnColor != value)
				{
					hairBtnColor = value;
					OnPropertyChanged("HairBtnColor");
				}
			}
		}

		private bool nailBtnCheck;
		public bool NailBtnCheck
		{
			get
			{
				return nailBtnCheck;
			}
			set
			{
				if (nailBtnCheck != value)
				{
					nailBtnCheck = value;
					OnPropertyChanged("NailBtnCheck");
				}
				if (nailBtnCheck)
				{
					NailBtnColor = ColorList.colorNeutral;
				}
				else
				{
					NailBtnColor = ColorList.colorWhite;
				}
			}
		}
		private Color nailBtnColor;
		public Color NailBtnColor
		{
			get
			{
				return nailBtnColor;
			}
			set
			{
				if (nailBtnColor != value)
				{
					nailBtnColor = value;
					OnPropertyChanged("NailBtnColor");
				}
			}
		}

		private bool eyelashBtnCheck;
		public bool EyelashBtnCheck
		{
			get
			{
				return eyelashBtnCheck;
			}
			set
			{
				if (eyelashBtnCheck != value)
				{
					eyelashBtnCheck = value;
					OnPropertyChanged("EyelashBtnCheck");
				}
				if (eyelashBtnCheck)
				{
					EyelashBtnColor = ColorList.colorNeutral;
				}
				else
				{
					EyelashBtnColor = ColorList.colorWhite;
				}
			}
		}
		private Color eyelashBtnColor;
		public Color EyelashBtnColor
		{
			get
			{
				return eyelashBtnColor;
			}
			set
			{
				if (eyelashBtnColor != value)
				{
					eyelashBtnColor = value;
					OnPropertyChanged("EyelashBtnColor");
				}
			}
		}

		private bool estheticBtnCheck;
		public bool EstheticBtnCheck
		{
			get
			{
				return estheticBtnCheck;
			}
			set
			{
				if (estheticBtnCheck != value)
				{
					estheticBtnCheck = value;
					OnPropertyChanged("EstheticBtnCheck");
				}
				if (estheticBtnCheck)
				{
					EstheticBtnColor = ColorList.colorNeutral;
				}
				else
				{
					EstheticBtnColor = ColorList.colorWhite;
				}
			}
		}
		private Color estheticBtnColor;
		public Color EstheticBtnColor
		{
			get
			{
				return estheticBtnColor;
			}
			set
			{
				if (estheticBtnColor != value)
				{
					estheticBtnColor = value;
					OnPropertyChanged("EstheticBtnColor");
				}
			}
		}

		private string entryTxt;
		public string EntryTxt
		{
			get
			{
				return entryTxt;
			}
			set
			{
				if (entryTxt != value)
				{
					entryTxt = value;
					OnPropertyChanged("EntryTxt");
				}
			}
		}

		private string customEditorTxt;
		public string CustomEditorTxt
		{
			get
			{
				return customEditorTxt;
			}
			set
			{
				if (customEditorTxt != value)
				{
					customEditorTxt = value;
					OnPropertyChanged("CustomEditorTxt");
				}
			}
		}

		private bool myBlogPlusListViewIsVisible;
		public bool MyBlogPlusListViewIsVisible
		{
			get
			{
				return myBlogPlusListViewIsVisible;
			}
			set
			{
				if (myBlogPlusListViewIsVisible != value)
				{
					myBlogPlusListViewIsVisible = value;
					OnPropertyChanged("MyBlogPlusListViewIsVisible");
				}
			}
		}

	}
}
