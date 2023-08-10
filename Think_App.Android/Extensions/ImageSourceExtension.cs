using System;
using System.Threading.Tasks;
using Android.Graphics;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace Think_App.Droid
{
	public static class ImageSourceExtension
	{
		public static async Task<Bitmap> ToBitmapAsync(this ImageSource self)
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

			var bitmap = await handler.LoadImageAsync(self, Forms.Context);
			return bitmap;			
		}
	}
}
