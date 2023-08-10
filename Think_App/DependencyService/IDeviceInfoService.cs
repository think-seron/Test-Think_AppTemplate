using System;
namespace Think_App
{
	public interface IDeviceInfoService
	{
		string GetDeviceCode();
		string GetNativeVersion();
		string GetDeviceName();
		string GetAppVersion();
	}
}
