using System;
namespace Think_App
{
	public class SavadDataSelectedPageModel : ViewModelBase
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
	}
}
