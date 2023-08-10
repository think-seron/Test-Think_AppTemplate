using System;
using Android.Graphics;

namespace Think_App.Droid
{
	public static class BitmapConverter
	{
		public static Bitmap FromBytes(byte[] bytes)
		{
			Bitmap bitmap = null;
			try
			{
				if (bytes != null)
				{
					bitmap = BitmapFactory.DecodeByteArray(bytes, 0, bytes.Length);
				}
			}
			catch (Exception)
			{
				bitmap = null;
			}

			return bitmap;
		}
	}
}
