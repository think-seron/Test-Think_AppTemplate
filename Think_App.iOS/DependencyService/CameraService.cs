using System;
using Xamarin.Forms;
using UIKit;
using Think_App.iOS;

[assembly: Dependency(typeof(CameraService))]
namespace Think_App.iOS
{
	public class CameraService : ICameraService
	{
		public bool IsCameraAvailable()
		{
			try
			{
				return UIImagePickerController.IsSourceTypeAvailable(UIImagePickerControllerSourceType.Camera);
			}
			catch
			{
				return false;
			}
		}
	}
}
