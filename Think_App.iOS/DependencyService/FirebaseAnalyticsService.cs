using System;
using System.Collections.Generic;
using Firebase.Analytics;
using Foundation;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
[assembly: Dependency(typeof(Think_App.iOS.FirebaseAnalyticsService))]

namespace Think_App.iOS
{
    public class FirebaseAnalyticsService:IGAService
    {
        public void Track_App_Page(String PageNameToTrack)
        {
            Analytics.SetScreenNameAndClass(PageNameToTrack, null);
        }

        public void Track_App_Event(String GAEventCategory, String EventToTrack)
        {
            var keys = new List<NSString>();
            var values = new List<NSString>();

            keys.Add(new NSString(GAEventCategory));
            values.Add(new NSString(EventToTrack));

            var parametersDictionary = NSDictionary<NSString, NSObject>.FromObjectsAndKeys(values.ToArray(), keys.ToArray(), keys.Count);

            Analytics.LogEvent("AppEvent", parametersDictionary);
        }

        public void Track_App_Exception(String ExceptionMessageToTrack, Boolean isFatalException)
        {
            var keys = new List<NSString>();
            var values = new List<NSString>();

            keys.Add(new NSString(ExceptionMessageToTrack));
            values.Add(new NSString(isFatalException.ToString()));

            var parametersDictionary = NSDictionary<NSString, NSObject>.FromObjectsAndKeys(values.ToArray(), keys.ToArray(), keys.Count);

            Analytics.LogEvent("AppException", parametersDictionary);
        }
    }
}
