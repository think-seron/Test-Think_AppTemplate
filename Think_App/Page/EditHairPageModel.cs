using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
	public class EditHairPageModel : ViewModelBase
	{
		ImageSource _PhotoImgSource;
		public ImageSource PhotoImgSource
		{
			get
			{
				return _PhotoImgSource;
			}
			set
			{
				SetProperty(ref _PhotoImgSource, value);
			}
		}

		bool _PhotoImgVisible;
		public bool PhotoImgVisible
		{
			get
			{
				return _PhotoImgVisible;
			}
			set
			{
				SetProperty(ref _PhotoImgVisible, value);
			}
		}

		double _PhotoImgScale;
		public double PhotoImgScale
		{
			get
			{
				return _PhotoImgScale;
			}
			set
			{
				SetProperty(ref _PhotoImgScale, value);
			}
		}

		double _PhotoImgTranslationX;
		public double PhotoImgTranslationX
		{
			get
			{
				return _PhotoImgTranslationX;
			}
			set
			{
				SetProperty(ref _PhotoImgTranslationX, value);
			}
		}

		double _PhotoImgTranslationY;
		public double PhotoImgTranslationY
		{
			get
			{
				return _PhotoImgTranslationY;
			}
			set
			{
				SetProperty(ref _PhotoImgTranslationY, value);
			}
		}

		ImageSource _GalleryImgSource;
		public ImageSource GalleryImgSource
		{
			get
			{
				return _GalleryImgSource;
			}
			set
			{
				SetProperty(ref _GalleryImgSource, value);
			}
		}

		bool _GalleryImgVisible;
		public bool GalleryImgVisible
		{
			get
			{
				return _GalleryImgVisible;
			}
			set
			{
				SetProperty(ref _GalleryImgVisible, value);
			}
		}

		double _GalleryImgScale;
		public double GalleryImgScale
		{
			get
			{
				return _GalleryImgScale;
			}
			set
			{
				SetProperty(ref _GalleryImgScale, value);
			}
		}

		double _GalleryImgTranslationX;
		public double GalleryImgTranslationX
		{
			get
			{
				return _GalleryImgTranslationX;
			}
			set
			{
               	SetProperty(ref _GalleryImgTranslationX, value);
			}
		}

		double _GalleryImgTranslationY;
		public double GalleryImgTranslationY
		{
			get
			{
				return _GalleryImgTranslationY;
			}
			set
			{
               	SetProperty(ref _GalleryImgTranslationY, value);
			}
		}

		ImageSource _HairImgSource;
		public ImageSource HairImgSource
		{
			get
			{
				return _HairImgSource;
			}
			set
			{
				SetProperty(ref _HairImgSource, value);
			}
		}

		double _CloseBtnFontSize;
		public double CloseBtnFontSize
		{
			get
			{
				return _CloseBtnFontSize;
			}
			set
			{
				SetProperty(ref _CloseBtnFontSize, value);
			}
		}

		double _CloseBtnWidth;
		public double CloseBtnWidth
		{
			get
			{
				return _CloseBtnWidth;
			}
			set
			{
				SetProperty(ref _CloseBtnWidth, value);
			}
		}

		double _CloseBtnHeight;
		public double CloseBtnHeight
		{
			get
			{
				return _CloseBtnHeight;
			}
			set
			{
				SetProperty(ref _CloseBtnHeight, value);
			}
		}
	}
}
