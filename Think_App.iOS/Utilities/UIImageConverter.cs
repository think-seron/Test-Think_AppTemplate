using System;
using Foundation;
using UIKit;

namespace Think_App.iOS
{
	public static class UIImageConverter
	{
		public static UIImage FromBytes(byte[] bytes)
		{
			UIImage uiimage = null;
			try
			{
				uiimage = new UIImage(NSData.FromArray(bytes));
			}
			catch (Exception)
			{
				uiimage = null;
			}

			return uiimage;
		}
	}
}
