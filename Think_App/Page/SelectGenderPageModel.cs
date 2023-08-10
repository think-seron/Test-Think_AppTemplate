using System;
using System.Collections.Generic;
using Xamarin.Forms;
namespace Think_App
{
	public class SelectGenderPageModel : ViewModelBase
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
		public Command FemaleBtnCommand { get; set; }
		public Command MaleBtnCommand { get; set; }

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

		bool _FemaleBtnEnable;
		public bool FemaleBtnEnable
		{
			get
			{
				return _FemaleBtnEnable;
			}
			set
			{
				SetProperty(ref _FemaleBtnEnable, value);
			}
		}

		bool _MaleBtnEnable;
		public bool MaleBtnEnable
		{
			get
			{
				return _MaleBtnEnable;
			}
			set
			{
				SetProperty(ref _MaleBtnEnable, value);
			}
		}

		public List<FadeImageView.FadeInfo> BGFadeImageViewInfoList { get; set; }
	}
}
