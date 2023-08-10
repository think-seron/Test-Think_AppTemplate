using System;
using Xamarin.Forms;
using Think_App.iOS;
using UIKit;
using Foundation;

[assembly: Dependency(typeof(DeviceInfoService_iOS))]
namespace Think_App.iOS
{
	public class DeviceInfoService_iOS : IDeviceInfoService
	{
		public string GetDeviceCode()
		{
			try
			{
				return UIDevice.CurrentDevice.IdentifierForVendor.AsString();
			}
			catch
			{
				return null;
			}
		}

		public string GetNativeVersion()
		{

			try
			{
				return UIDevice.CurrentDevice.SystemVersion;
			}
			catch
			{
				return null;
			}
		}

		public string GetDeviceName()
		{
			try
			{
				return DeviceModel.GetModelName();
			}
			catch
			{
				return null;
			}
		}

		public string GetAppVersion()
		{
			try
			{
				return NSBundle.MainBundle.InfoDictionary["CFBundleVersion"].ToString();
			}
			catch
			{
				return null;
			}
		}
		public DeviceInfoService_iOS()
		{
		}
	}
}
