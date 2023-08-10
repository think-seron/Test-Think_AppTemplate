using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;
namespace Think_App.iOS
{
	public static class ImageSourceExtension
	{
		public static async Task<UIImage> ToUIImageAsync(this ImageSource self)
		{
			IImageSourceHandler handler;

			if (self is UriImageSource)
			{
				handler = new ImageLoaderSourceHandler();
			}
			else if (self is FileImageSource)
			{
				handler = new FileImageSourceHandler();
			}
			else if (self is StreamImageSource)
			{
				handler = new StreamImagesourceHandler();
			}
			else
			{
				return null;
			}

			var uiimage = await handler.LoadImageAsync(self);
			return uiimage;
		}
	}
}
