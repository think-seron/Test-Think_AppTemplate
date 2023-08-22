using System;
using Android.Content;
using Android.Content.PM;
using Think_App.Droid;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

[assembly: Dependency(typeof(CameraService))]
namespace Think_App.Droid
{
	public class CameraService : ICameraService
	{
		public bool IsCameraAvailable()
		{
			try
			{
				// フロント・リア両方の有無
				var rear = Forms.Context.PackageManager.HasSystemFeature(PackageManager.FeatureCamera);
				var front = Forms.Context.PackageManager.HasSystemFeature(PackageManager.FeatureCameraFront);
				return (rear && front);
			}
			catch
			{
				return false;
			}
		}
	}
}
