using System;
using UIKit;
using UserNotifications;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

[assembly: Dependency(typeof(Think_App.iOS.NotificationService))]
namespace Think_App.iOS
{
	public class NotificationService : INotificationService
	{
		public static bool isRegisteredForRemoteNotifications;
		public bool GetNotificationSetting()
		{
			bool ret = false;
			if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
			{
				UNUserNotificationCenter.Current.GetNotificationSettings((UNNotificationSettings obj) =>
				{
					if (obj.AuthorizationStatus == UNAuthorizationStatus.Authorized)
					{
						ret = true;
					}
				});
			}
			else
			{
				if (UIApplication.SharedApplication.IsRegisteredForRemoteNotifications)
				{
					ret = true;
				}
			}
			return ret;
		}
	}
}
