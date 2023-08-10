using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace Think_App.Droid
{
	public static class DoubleExtension
	{
		public static double ToDpFromPixel(this double px)
		{
			return Forms.Context.FromPixels(px);
		}

		public static double ToDpFromPixel(this float px)
		{
			return ToDpFromPixel((double)px);
		}

		public static double ToDpFromPixel(this int px)
		{
			return ToDpFromPixel((double)px);
		}

		public static double ToPixelFromDp(this double dp)
		{
			return (double)Forms.Context.ToPixels(dp);
		}
	}
}
