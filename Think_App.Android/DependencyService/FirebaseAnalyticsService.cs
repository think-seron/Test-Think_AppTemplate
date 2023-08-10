using System;
using Android.App;
using Android.Content;
using Android.OS;
using Firebase.Analytics;
using Xamarin.Forms;
[assembly: Dependency(typeof(Think_App.Droid.FirebaseAnalyticsService))]
namespace Think_App.Droid
{
    public class FirebaseAnalyticsService : IGAService
    {
        private static FirebaseAnalyticsService _fAService { get; set; } = new FirebaseAnalyticsService();
        private FirebaseAnalytics Analytics { get; set; }
        private Activity Activity { get; set; }

        public static void CreateInstance(Activity activity)
        {
            _fAService.Activity = activity;
            try
            {
                _fAService.Analytics = FirebaseAnalytics.GetInstance(activity);
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("ex :" + ex);
            }

        }
        public void Track_App_Page(String PageNameToTrack)
        {
            if (_fAService == null) return;
            var analytics = _fAService.Analytics;
            var activity = _fAService.Activity;
            analytics.SetCurrentScreen(activity, PageNameToTrack, null);
        }

        public void Track_App_Event(String GAEventCategory, String EventToTrack)
        {
            if (_fAService == null) return;
            var analytics = _fAService.Analytics;
            var bundle = new Bundle();
            bundle.PutString(GAEventCategory, EventToTrack);
            analytics.LogEvent("AppEvent", bundle);
            System.Diagnostics.Debug.WriteLine("************AppEvent************ ;" + GAEventCategory);
        }

        public void Track_App_Exception(String ExceptionMessageToTrack, Boolean isFatalException)
        {
            if (_fAService == null) return;
            var analytics = _fAService.Analytics;
            var bundle = new Bundle();
            bundle.PutString(ExceptionMessageToTrack, isFatalException.ToString());
            analytics.LogEvent("AppException", bundle);
            System.Diagnostics.Debug.WriteLine("AppException ;" + ExceptionMessageToTrack);
        }
    }
}
