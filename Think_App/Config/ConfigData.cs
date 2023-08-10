using System;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace Think_App
{
	public class ConfigData
	{
		//設定系のデータはここにまとめたい
		public ConfigData()
		{
			
		}

        const string SalonId = "@@@SalonId"; 

        public string appId {
			get { return SalonId;}
		}


		//サーバーから採番された端末固有のID
		//これがなかったら登録画面に遷移させてしまう。
		DeviceInfo deviceInfo;
		string _deviceId;
		public string deviceId {
			get {
				if (_deviceId == null)
				{
					//読み込み処理
					//_deviceId = "test";
					 var res = Task.Run(async () =>
					{
						deviceInfo = await FileManager.ReadJsonFileAsync<DeviceInfo>("Account", "deviceInfo");

						if (deviceInfo == null)
						{
							System.Diagnostics.Debug.WriteLine("deviceInfo == null:");
							return null;
						}
						else if (deviceInfo != null && deviceInfo.ID == null)
						{
							System.Diagnostics.Debug.WriteLine("deviceInfo != null  && deviceInfo.ID == null:");
							return null;

						}
						//else if (_deviceId != null)
						//{
						//	System.Diagnostics.Debug.WriteLine("_deviceId != null :");

						//	return null;
						//}
						System.Diagnostics.Debug.WriteLine("deviceInfo.ID:"+deviceInfo.ID);
 						return deviceInfo.ID;

						//System.Diagnostics.Debug.WriteLine("_deviceId != null:" + deviceInfo.ID);
						//System.Diagnostics.Debug.WriteLine("_deviceId" + _deviceId);
					});
					_deviceId = res.Result;
					System.Diagnostics.Debug.WriteLine("_deviceId:"+_deviceId);
					return _deviceId;
					// diviceIDが取れるまで待つ
					//var timespan = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
					//var nowTime = (uint)timespan.TotalSeconds;
					//var limitTime = nowTime + 30;

					//while (true)
					//{
					//	//timespan = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
					//	//nowTime = (uint)timespan.TotalSeconds;
					//	if (deviceInfo == null)
					//	{
					//		System.Diagnostics.Debug.WriteLine("deviceInfo == null:");
					//		break;
					//	}
					//	else if (deviceInfo != null && deviceInfo.ID == null)
					//	{
					//		System.Diagnostics.Debug.WriteLine("deviceInfo != null  && deviceInfo.ID == null:");

					//		break;
					//	}
					//	else if (_deviceId != null)
					//	{
					//		System.Diagnostics.Debug.WriteLine("_deviceId != null :");

					//		break;
					//	}

					//	//else if (limitTime < nowTime)
					//	//{
					//	//	// タイムアウト
					//	//	break;
					//	//}
					//}
				}
				else
				{
					return _deviceId;
				}
			}
			set {
				if (_deviceId != value && _deviceId != null)
				{
					_deviceId = value;
					//書き込み・保存処理
					//Task.Run(async () =>
					//{
					//	await StaticMethod.DeviceIDReg(_deviceId);
					//});
				}
			}
		}

		//機種名
		public string deviceName {
			get
			{
				return DependencyService.Get<IDeviceInfoService>().GetDeviceName();
			}
		}
		//OSVersion
		public string nativeVersion
		{
			get
			{
				return DependencyService.Get<IDeviceInfoService>().GetNativeVersion();
			}
		}
		//アプリバージョン
		public string appVersion
		{
			get
			{
				return DependencyService.Get<IDeviceInfoService>().GetAppVersion();
			}
		}
		//プラットフォーム
		public int platform
		{
			get
			{
#if DEBUG
				if (Device.RuntimePlatform == Device.iOS)
				{
					return 3;
				}
				else if (Device.RuntimePlatform == Device.Android)
				{
					return 4;
				}
				else {
					return 5;
				}
#else
				if (Device.RuntimePlatform == Device.iOS)
				{
					return 1;
				}
				else if (Device.RuntimePlatform == Device.Android)
				{
					return 2;
				}
				else{
					return 5;
				}
#endif
			}
		}
	}
}
