using System;
using Xamarin.Forms;
using Think_App.Droid;
using Android.OS;

[assembly: Dependency(typeof(DeviceInfoService_Droid))]
namespace Think_App.Droid
{
	public class DeviceInfoService_Droid:IDeviceInfoService
	{
		public string GetDeviceCode(){
			try
			{
				return Build.Serial;
			}
			catch
			{
				return null;
			}
		}

		public string GetNativeVersion() {
			return Build.VERSION.Release;
		}

		public string GetDeviceName() {
			try
			{
				return Build.Model;
			}
			catch {
				return null;
			}
		}


		public string GetAppVersion() {
			try
			{
				return Forms.Context.PackageManager.GetPackageInfo(Forms.Context.PackageName, 0).VersionName;
			}
			catch
			{
				return null;
			}
		}
		public DeviceInfoService_Droid()
		{
		}
	}
}
