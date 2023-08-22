using System;
using Android.App;
using Android.Content;
using Firebase.Messaging;
using Android.Media;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Android;
using Android.OS;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App.Droid
{
    [Service(Exported = true)]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class FireBaseMassagingService : FirebaseMessagingService
    {
        public override void OnNewToken(string token)
        {
            base.OnNewToken(token);
            System.Diagnostics.Debug.WriteLine("new token :" + token);
            Task.Run( () => { DeviceTokenInfo.Instance.DeviceToken = token; });
        }


        public static string ChannelId = "SalonInfo";
        public override void OnMessageReceived(RemoteMessage message)
        {
            base.OnMessageReceived(message);

            SendNotification(message);
        }

        private void SendNotification(RemoteMessage message)
        {
            try
            {
                string body = "新しいお知らせ";

                string title = " ";

                int notificationType;
                try
                {
                    var data = message.Data;
                    foreach (var n in message.Data)
                    {
                        switch (n.Key)
                        {
                            case NotificationManager.body:
                                body = n.Value;
                                break;
                            case NotificationManager.notificationType:
                                notificationType = Int32.Parse(n.Value);
                                break;
                            case NotificationManager.title:
                                title = n.Value;
                                break;
                            default:
                                break;

                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("ex  :" + ex);
                }

                var intent = new Intent(this, typeof(MainActivity));
                intent.AddFlags(ActivityFlags.ClearTop);

                var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.OneShot | PendingIntentFlags.Immutable);

                var Vibratelong = new long[] { 0, 400, 100, 200, 100, 200 };

                var defaultSoundUri = RingtoneManager.GetDefaultUri(RingtoneType.Notification);
                var notificationBuilder = new Notification.Builder(this)

                                                          .SetContentTitle(title)
                                                          .SetContentText(body)
                                                          .SetVibrate(Vibratelong)
                                                          //.SetContentText(body)
                                                          .SetAutoCancel(true)
                                                          .SetContentIntent(pendingIntent);
                if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                    notificationBuilder.SetSmallIcon(Resource.Drawable.icon_notification);
                else
                    notificationBuilder.SetSmallIcon(Resource.Drawable.icon);
                if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
                {
                    notificationBuilder.SetChannelId(ChannelId);
                }
                var notificationManager = Android.App.NotificationManager.FromContext(this);
                notificationManager.Notify(0, notificationBuilder.Build());
            }
            catch (Exception ex)
            {
                Android.Util.Log.Debug(" Notification Tag : ", "  push notificatin build  Exception: " + ex.StackTrace);
            }
        }
    }
}