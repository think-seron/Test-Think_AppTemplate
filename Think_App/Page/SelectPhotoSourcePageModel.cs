using System;
using System.Collections.Generic;
using Xamarin.Forms;
namespace Think_App
{
	public class SelectPhotoSourcePageModel : ViewModelBase
	{
		double _ScreenSizeScale;
		public double ScreenSizeScale
		{
			get
			{
				return _ScreenSizeScale;
			}
			set
			{
				SetProperty(ref _ScreenSizeScale, value);
			}
		}
		public Command TakePhotoBtnCommand { get; set; }
		public Command SelectFromGalleryBtnCommand { get; set; }
		public Command UseLastPhotoBtnCommand { get; set; }

		double _InfoLblFontSize;
		public double InfoLblFontSize
		{
			get
			{
				return _InfoLblFontSize;
			}
			set
			{
				SetProperty(ref _InfoLblFontSize, value);
			}
		}

		bool _TakePhotoBtnEnable;
		public bool TakePhotoBtnEnable
		{
			get
			{
				return _TakePhotoBtnEnable;
			}
			set
			{
				SetProperty(ref _TakePhotoBtnEnable, value);
			}
		}

		bool _UseLastPhotoBtnEnable;
		public bool UseLastPhotoBtnEnable
		{
			get
			{
				return _UseLastPhotoBtnEnable;
			}
			set
			{
				SetProperty(ref _UseLastPhotoBtnEnable, value);
			}
		}

		public List<FadeImageView.FadeInfo> BGFadeImageViewInfoList { get; set; }
	}
}
