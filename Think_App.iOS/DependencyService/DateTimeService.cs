using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
[assembly: Dependency(typeof(Think_App.iOS.DateTimeService))]
namespace Think_App.iOS
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime GetNow() => DateTime.Now;

        public DateTime GetToday() => DateTime.Today;
    }
}

