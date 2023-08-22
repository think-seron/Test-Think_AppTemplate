using System;
using UIKit;
using Think_App.iOS;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

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
