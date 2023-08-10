using System;
using Xamarin.Forms;

namespace Think_App
{
	public class GalleryViewCellModel : ViewModelBase
	{
		ImageSource _GVCThumbnailImgSource;
		public ImageSource GVCThumbnailImgSource
		{
			get
			{
				return _GVCThumbnailImgSource;
			}
			set
			{
				SetProperty(ref _GVCThumbnailImgSource, value);
			}
		}

		double _GVCThumbnailImgDSWidth;
		public double GVCThumbnailImgDSWidth
		{
			get
			{
				return _GVCThumbnailImgDSWidth;
			}
			set
			{
				SetProperty(ref _GVCThumbnailImgDSWidth, value);
			}
		}

		double _GVCThumbnailImgDSHeight;
		public double GVCThumbnailImgDSHeight
		{
			get
			{
				return _GVCThumbnailImgDSHeight;
			}
			set
			{
				SetProperty(ref _GVCThumbnailImgDSHeight, value);
			}
		}

		Color _GVCThumbnailImgBGColor;
		public Color GVCThumbnailImgBGColor
		{
			get
			{
				return _GVCThumbnailImgBGColor;
			}
			set
			{
				SetProperty(ref _GVCThumbnailImgBGColor, value);
			}
		}

		public int Index { get; set; }
	}
}
