using System;
using System.IO;
using Android.Graphics;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App.Droid
{
	public static class BitmapExtension
	{
		public static ImageSource ToImageSource(this Bitmap self)
		{
			// Bitmap -> Bytes
			var bytes = self.ToBytes();
			if (bytes == null)
			{
				return null;
			}

			// Bytes -> ImageSource
			return ImageSource.FromStream(() => new MemoryStream(bytes));
		}

		public static byte[] ToBytes(this Bitmap self)
		{
			byte[] bytes = null;
			using (var stream = new MemoryStream())
			{
				self.Compress(Bitmap.CompressFormat.Png, 100, stream);
				bytes = stream.ToArray();
			}

			return bytes;
		}
	}
}
