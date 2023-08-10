using System;
using System.Diagnostics;
namespace Think_App
{
	public static class DateManager
	{
		public static DateTime ConvertMessageDateStringToDateTime(string dateString)
		{
			try
			{
				int year = int.Parse(dateString.Substring(0, 4));
				int month = int.Parse(dateString.Substring(4, 2));
				int day = int.Parse(dateString.Substring(6, 2));
				int hour = int.Parse(dateString.Substring(8, 2));
				int minite = int.Parse(dateString.Substring(10, 2));
				int second = 0;

				var dateTime = new DateTime(year, month, day, hour, minite, second, DateTimeKind.Local);
				Debug.WriteLine("{0}年{1}月{2}日 {3}時{4}分{5}秒", year, month, day, hour, minite, second);

				return dateTime;
			}
			catch(Exception ex)
			{
				Debug.WriteLine("コンバート失敗:{0}", ex);
				return DateTime.MinValue;
			}
		}

		public static bool EqualsAsDay(DateTime dateTime1, DateTime dateTime2)
		{
			return (dateTime1.Year == dateTime2.Year && dateTime1.Month == dateTime2.Month && dateTime1.Day == dateTime2.Day);
		}
	}
}
