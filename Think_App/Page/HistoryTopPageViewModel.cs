using System;
using System.Collections.ObjectModel;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
	public class HistoryTopPageViewModel : ViewModelBase
	{
		public HistoryTopPageViewModel()
		{
			WidthSize = ScaleManager.ScreenWidth;
            DateLabelHeight = ScaleManager.SizeSet(14);
            DateLabelWidth = ScaleManager.SizeSet(69);
            BlogImageFontSize = ScaleManager.SizeSet(11);
			HistoryLabelFontSize = ScaleManager.SizeSet(14);
		}

        private bool image1ShadowIsVisible;
        public bool Image1ShadowIsVisible
        {
            get
            {
                return image1ShadowIsVisible;
            }
            set
            {
                if (image1ShadowIsVisible != value)
                {
                    image1ShadowIsVisible = value;
                    OnPropertyChanged("Image1ShadowIsVisible");
                }
            }
        }

		private bool image2ShadowIsVisible;
		public bool Image2ShadowIsVisible
		{
			get
			{
				return image2ShadowIsVisible;
			}
			set
			{
				if (image2ShadowIsVisible != value)
				{
					image2ShadowIsVisible = value;
					OnPropertyChanged("Image2ShadowIsVisible");
				}
			}
		}

		private bool image3ShadowIsVisible;
		public bool Image3ShadowIsVisible
		{
			get
			{
				return image3ShadowIsVisible;
			}
			set
			{
				if (image3ShadowIsVisible != value)
				{
					image3ShadowIsVisible = value;
					OnPropertyChanged("Image3ShadowIsVisible");
				}
			}
		}

        public double DateLabelWidth { get; set; }
        public double DateLabelHeight { get; set; }

        private string image1DateStringrShort;
        public string Image1DateStringrShort
        {
            get
            {
                return image1DateStringrShort;
            }
            set
            {
                if (image1DateStringrShort != value)
                {
                    image1DateStringrShort = value;
                    OnPropertyChanged("Image1DateStringrShort");
                }
            }
        }

		private string image2DateStringrShort;
		public string Image2DateStringrShort
		{
			get
			{
				return image2DateStringrShort;
			}
			set
			{
				if (image2DateStringrShort != value)
				{
					image2DateStringrShort = value;
					OnPropertyChanged("Image2DateStringrShort");
				}
			}
		}

		private string image3DateStringrShort;
		public string Image3DateStringrShort
		{
			get
			{
				return image3DateStringrShort;
			}
			set
			{
				if (image3DateStringrShort != value)
				{
					image3DateStringrShort = value;
					OnPropertyChanged("Image3DateStringrShort");
				}
			}
		}

        private string image1Souce;
        public string Image1Souce
        {
            get
            {
                return image1Souce;
            }
            set
            {
                if (image1Souce != value)
                {
                    image1Souce = value;
                    OnPropertyChanged("Image1Souce");
                }
            }
        }

		private string image2Souce;
		public string Image2Souce
		{
			get
			{
				return image2Souce;
			}
			set
			{
				if (image2Souce != value)
				{
					image2Souce = value;
					OnPropertyChanged("Image2Souce");
				}
			}
		}

		private string image3Souce;
		public string Image3Souce
		{
			get
			{
				return image3Souce;
			}
			set
			{
				if (image3Souce != value)
				{
					image3Souce = value;
					OnPropertyChanged("Image3Souce");
				}
			}
		}

		public double HistoryLabelFontSize { get; set; }
        public double BlogImageFontSize { get; set; }
        public double BlogImageSize { get; set; }
		public double WidthSize { get; set; }
		public double ListViewHeight { get; set; }
		public bool HistoryButtonIsVisible { get; set; }
		public double ListViewRowHeight { get; set; }

		private ObservableCollection<ListViewHistoryViewModel> historyItemSouce;
		public ObservableCollection<ListViewHistoryViewModel> HistoryItemSouce
		{
			get
			{
				return historyItemSouce;
			}
			set
			{
				if (historyItemSouce != value)
				{
					historyItemSouce = value;
					OnPropertyChanged("HistoryItemSouce");
				}
			}
		}

		private Rect noBlogTextRectangle;
		public Rect NoBlogTextRectangle
		{
			get
			{
				return noBlogTextRectangle;
			}
			set
			{
				if (noBlogTextRectangle != value)
				{
					noBlogTextRectangle = value;
					OnPropertyChanged("NoBlogTextRectangle");
				}
			}
		}

		private string blogButonTxt;
		public string BlogButonTxt
		{
			get
			{
				return blogButonTxt;
			}
			set
			{
				if (blogButonTxt != value)
				{
					blogButonTxt = value;
					OnPropertyChanged("BlogButonTxt");
				}
			}
		}


	}
}
