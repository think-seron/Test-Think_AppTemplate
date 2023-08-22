using System;
using Android.App;
using Android.Support.V4.App;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

[assembly: Dependency(typeof(Think_App.Droid.NotificationService))]
namespace Think_App.Droid
{
    public class NotificationService : INotificationService
    {
        public bool GetNotificationSetting()
        {
            var notificationManagerCompat = NotificationManagerCompat.From(MainActivity.context);
            return notificationManagerCompat.AreNotificationsEnabled();
        }
    }
}
