using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Think_App
{
	public class HairSimulationStartPageModel : ViewModelBase
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
		public Command BeginBtnCommand { get; set; }
		public Command LookSavedImagesBtnCommand { get; set; }

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

		double _AttentionLblFontSize;
		public double AttentionLblFontSize
		{
			get
			{
				return _AttentionLblFontSize;
			}
			set
			{
                SetProperty(ref _AttentionLblFontSize, value);
			}
		}

		public List<FadeImageView.FadeInfo> BGFadeImageViewInfoList { get; set; }
	}
}
