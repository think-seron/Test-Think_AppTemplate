using System;
using System.Drawing;
using UIKit;

namespace Think_App.iOS
{
	public static class UIColorExtension
	{
		public static UIImage ToImage(this UIColor self)
		{
			var rect = new RectangleF(0, 0, 1, 1);

			UIGraphics.BeginImageContext(rect.Size);

			var context = UIGraphics.GetCurrentContext();
			context.SetFillColor(self.CGColor);
			context.FillRect(rect);

			var image = UIGraphics.GetImageFromCurrentImageContext();

			UIGraphics.EndImageContext();

			return image;
		}
	}
}
