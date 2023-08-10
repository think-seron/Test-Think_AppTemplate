using System;
using AssetsLibrary;
using UIKit;

namespace Think_App.iOS
{
	public static class AssetsLibraryExtention
	{
		public static UIImageOrientation ToUIImageOrientation(this ALAssetOrientation self)
		{
			UIImageOrientation uio = default(UIImageOrientation);
			if (self == ALAssetOrientation.Down)
			{
				uio = UIImageOrientation.Down;
			}
			else if (self == ALAssetOrientation.DownMirrored)
			{
				uio = UIImageOrientation.DownMirrored;
			}
			else if (self == ALAssetOrientation.Left)
			{
				uio = UIImageOrientation.Left;
			}
			else if (self == ALAssetOrientation.LeftMirrored)
			{
				uio = UIImageOrientation.LeftMirrored;
			}
			else if (self == ALAssetOrientation.Right)
			{
				uio = UIImageOrientation.Right;
			}
			else if (self == ALAssetOrientation.RightMirrored)
			{
				uio = UIImageOrientation.RightMirrored;
			}
			else if (self == ALAssetOrientation.Up)
			{
				uio = UIImageOrientation.Up;
			}
			else if (self == ALAssetOrientation.UpMirrored)
			{
				uio = UIImageOrientation.UpMirrored;
			}			
			return uio;
		}
	}
}
