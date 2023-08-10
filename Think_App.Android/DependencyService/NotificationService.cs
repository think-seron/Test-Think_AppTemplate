using System;
using Xamarin.Forms;
using Android.App;
using Android.Support.V4.App;

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
