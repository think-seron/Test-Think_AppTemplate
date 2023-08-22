using System;
using Think_App.Droid;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

[assembly: Dependency(typeof(ScreenService))]
namespace Think_App.Droid
{
	public class ScreenService : IScreenService
	{
		public int GetScreenWidth()
		{
			var display = Forms.Context.Resources.DisplayMetrics;
			return (int)(display.WidthPixels / display.Density);
		}

		public int GetScreenHeight()
		{
			var resourceId = Forms.Context.Resources.GetIdentifier("status_bar_height", "dimen", "android");
			var display = Forms.Context.Resources.DisplayMetrics;
			var height = (int)(display.HeightPixels / display.Density);
			int statusBarHeight = 0;
			if (resourceId > 0)
			{
				statusBarHeight = (int)(Forms.Context.Resources.GetDimensionPixelSize(resourceId) / display.Density);
			}
			return height - statusBarHeight;
		}

		public double GetScreenScale()
		{
			var display = Forms.Context.Resources.DisplayMetrics;
			return (double)display.Density;
		}

		public int GetStatusBarHeight()
		{
			var resourceId = Forms.Context.Resources.GetIdentifier("status_bar_height", "dimen", "android");
			var display = Forms.Context.Resources.DisplayMetrics;
			var height = (int)(display.HeightPixels / display.Density);
			int statusBarHeight = 0;
			if (resourceId > 0)
			{
				statusBarHeight = (int)(Forms.Context.Resources.GetDimensionPixelSize(resourceId) / display.Density);
			}
			return statusBarHeight;
		}

		public bool IsPortrait()
		{
			var config = Forms.Context.Resources.Configuration;
			return (config.Orientation == Android.Content.Res.Orientation.Portrait);
		}
	}
}
