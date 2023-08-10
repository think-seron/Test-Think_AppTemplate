using System;
using Java.Text;
using Java.Util;
using Xamarin.Forms;

[assembly: Dependency(typeof(Think_App.Droid.DateTimeService))]
namespace Think_App.Droid
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime GetNow()
        {
            // Android 14未満
            if ((int)Android.OS.Build.VERSION.SdkInt < 34)
                return DateTime.Now;

            var currentTime = Calendar.Instance.Time;
            var currentTimeString = new SimpleDateFormat("yyyy/MM/dd HH:mm:ss").Format(currentTime);
            return DateTime.Parse(currentTimeString);
        }

        public DateTime GetToday()
        {
            // Android 14未満
            if ((int)Android.OS.Build.VERSION.SdkInt < 34)
                return DateTime.Today;

            var now = GetNow();
            return now.Date;
        }
    }
}

