using System;
using Xamarin.Forms;
using UIKit;
using Think_App.iOS;

[assembly: Dependency(typeof(ScreenService))]
namespace Think_App.iOS
{
	public class ScreenService : IScreenService
	{
		public int GetScreenWidth()
		{
			var width = (int)UIScreen.MainScreen.Bounds.Width;
			return width;
		}

		public int GetScreenHeight()
		{
			var height = (int)UIScreen.MainScreen.Bounds.Height;
			return height;
		}

		public double GetScreenScale()
		{
			return (double)UIScreen.MainScreen.Scale;
		}

		public int GetStatusBarHeight()
		{
			return (int)UIApplication.SharedApplication.StatusBarFrame.Size.Height;
		}

		public bool IsPortrait()
		{
			var orientation = UIApplication.SharedApplication.StatusBarOrientation;
			return (orientation == UIInterfaceOrientation.Portrait || orientation == UIInterfaceOrientation.PortraitUpsideDown);
		}
	}
}
