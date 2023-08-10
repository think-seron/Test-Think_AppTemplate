using System;
using UIKit;
using CoreGraphics;
using Xamarin.Forms;

namespace Think_App.iOS
{
	public static class UIImageExtension
	{
		public static ImageSource ToImageSource(this UIImage self)
		{
			var source = ImageSource.FromStream(() => self.AsPNG().AsStream());
			return source;
		}

		public static byte[] ToBytes(this UIImage self)
		{
			byte[] bytes = null;
			try
			{
				using (var data = self.AsPNG())
				{
					bytes = data.ToArray();
				}
			}
			catch (Exception)
			{
				bytes = null;
			}

			return bytes;
		}	

		public static UIImage Resize(this UIImage self, double width, double height)
		{
			var resizedSize = new CGSize(width, height);
			var scale = UIScreen.MainScreen.Scale;
			UIGraphics.BeginImageContextWithOptions(resizedSize, false, scale);
			self.Draw(new CGRect(0, 0, resizedSize.Width, resizedSize.Height));
			var resizedImage = UIGraphics.GetImageFromCurrentImageContext();
			UIGraphics.EndImageContext();
			return resizedImage;
		}
	}
}
