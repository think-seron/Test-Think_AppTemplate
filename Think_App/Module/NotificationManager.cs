using System;
using Xamarin.Forms;
using IO.Swagger.Model;
namespace Think_App
{
	public class NotificationManager
	{
        public const string body = "body",
        notificationType = "pushType",
        title = "title";
		public const int news = 1,
						coupon = 2,
						reservation_today = 3,
						reservation_yesterday = 4,
						message = 5;
	}
}
