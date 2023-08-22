using System;
using Think_App.iOS;
using UIKit;
using Foundation;
using UserNotifications;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

[assembly: Dependency(typeof(PushNotificationService_iOS))]
namespace Think_App.iOS
{
    [Obsolete]
    public class PushNotificationService_iOS : IPushNotificationService
    {
        public PushNotificationService_iOS()
        {
        }
        public void InitPushNotification()
        {
            InitializeNotification();
        }

        public static void InitializeNotification()
        {
            //if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            //{
            //	// iOS 10
            //	var authOptions = UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound;
            //	UNUserNotificationCenter.Current.RequestAuthorization(authOptions, (granted, error) =>
            //													{
            //														Console.WriteLine(granted);
            //													});

            //	// For iOS 10 display notification (sent via APNS)
            //	//UNUserNotificationCenter.Current.Delegate = this;
            //	// For iOS 10 data message (sent via FCM)
            //	//Messaging.SharedInstance.RemoteMessageDelegate = this;
            //}
            //else
            //{
            //	// iOS 9 <=
            //	var allNotificationTypes = UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound;
            //	var settings = UIUserNotificationSettings.GetSettingsForTypes(allNotificationTypes, null);
            //	UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
            //}
            //UIApplication.SharedApplication.RegisterForRemoteNotifications();

        }
    }
}
