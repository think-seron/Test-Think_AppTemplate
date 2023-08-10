using System;
using UIKit;
namespace Think_App.iOS
{
	public static class DoubleExtension
	{
		public static double Clamp(this double self, double min, double max)
		{
			return Math.Min(max, Math.Max(min, self));
		}

		public static nfloat Clamp(this nfloat self, nfloat min, nfloat max)
		{
			return (nfloat)Clamp((double)self, (double)min, (double)max);
		}

		public static double ToPixelFromDp(this double dp)
		{
			var scale = (int)UIScreen.MainScreen.Scale;
			return dp * scale;
		}

		public static nfloat ToPixelFromDp(this nfloat dp)
		{
			var scale = (int)UIScreen.MainScreen.Scale;
			return dp * scale;
		}

		public static double ToDpFromPixel(this double px)
		{
			var scale = (int)UIScreen.MainScreen.Scale;
			try
			{
				return px / scale;
			}
			catch
			{
				return 0.0;
			}
		}
		public static nfloat ToDpFromPixel(this nfloat px)
		{
			var scale = (int)UIScreen.MainScreen.Scale;
			try
			{
				return px / scale;
			}
			catch
			{
				return 0.0f;
			}
		}
	}
}
