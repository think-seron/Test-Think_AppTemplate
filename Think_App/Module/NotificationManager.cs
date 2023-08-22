using System;
using IO.Swagger.Model;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
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
