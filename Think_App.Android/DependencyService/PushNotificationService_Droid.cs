using System;
using System.Threading.Tasks;
using Firebase.Iid;
using Firebase.Messaging;
using Think_App;
using Think_App.Droid;
using Xamarin.Android;
using Xamarin.Forms;
[assembly: Dependency(typeof(PushNotificationService_Droid))]
namespace Think_App.Droid
{
    [Obsolete]
    public class PushNotificationService_Droid : IPushNotificationService
	{
		public void InitPushNotification()
		{
			InitializeNotification();
		}
		public static void InitializeNotification()
		{
//#if DEBUG
//			Task.Run(() =>
//			{
//				var instanceid = FirebaseInstanceId.Instance;
//				instanceid.DeleteInstanceId();
//				var new_token = instanceid.GetToken(MainActivity.context.GetString(Resource.String.gcm_defaultSenderId), Firebase.Messaging.FirebaseMessaging.InstanceIdScope);
//				Android.Util.Log.Debug("TAG", "{0} {1}", instanceid.Token, new_token);
//				System.Diagnostics.Debug.WriteLine("new_token" + new_token);
//				//if (App.customNavigationPage != null)
//				//	Device.BeginInvokeOnMainThread(() =>
//				//								   App.customNavigationPage.CurrentPage.DisplayAlert("new_token", new_token, "ok"));
//				FirebaseMessaging.Instance.SubscribeToTopic("all");
//			});
//#else
//			Task.Run(() =>
//			{
//				var instanceid = FirebaseInstanceId.Instance;
//				//ここで取得してしまってもすぐにリフレッシュで再取得する。ここでtokenは取得するべきではない。
//				//var new_token = instanceid.GetToken(GetString(Resource.String.gcm_defaultSenderId), Firebase.Messaging.FirebaseMessaging.InstanceIdScope);
//				//DeviceInfo.DeviceToken = new_token;
//				FirebaseMessaging.Instance.SubscribeToTopic("all");
//			});
//#endif
		}
	}
}
