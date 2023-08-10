using System;
using Xamarin.Forms;
namespace Think_App
{
	public class HairCatalogSelectionViewCellModel : ViewModelBase
	{
		GridLength _HCSVCImageHeight;
		public GridLength HCSVCImageHeight
		{
			get
			{
				return _HCSVCImageHeight;
			}
			set
			{
				SetProperty(ref _HCSVCImageHeight, value);
			}
		}

		ImageSource _HCSVCImageSource;
		public ImageSource HCSVCImageSource
		{
			get
			{
				return _HCSVCImageSource;
			}
			set
			{
				SetProperty(ref _HCSVCImageSource, value);
			}
		}

		string _HCSVCHairStyleNameLblText;
		public string HCSVCHairStyleNameLblText
		{
			get
			{
				return _HCSVCHairStyleNameLblText;
			}
			set
			{
				SetProperty(ref _HCSVCHairStyleNameLblText, value);
			}
		}

		public int HairStyleId { get; set; }
	}
}
