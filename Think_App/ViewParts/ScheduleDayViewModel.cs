using System;
using Xamarin.Forms;
using Think_App;
using System.Threading.Tasks;
namespace Think_App
{
	public class ScheduleDayViewModel : ViewModelBase
	{
		public ScheduleDayViewModel()
		{
			//DayText = day;

			//SetDayType(dayOfWeekType);
			//SetSizes();
		}

		public async void ViewUpdate(string day, int dayOfWeekType, string dayOfWeek)
		{
			DayText = day.Substring(6, 2);

			DayOfWeekText = dayOfWeek;

			System.Diagnostics.Debug.WriteLine(" day text :" + day + "  dayofweek : " + dayOfWeek + "    dayofweekType  :" + dayOfWeekType);
			SetDayType(dayOfWeekType);
			SetSizes();
		}
		const int weekday = 1, saturday = 2, sunday = 3, holiday = 4;

		void SetDayType(int dayOfWeekType)
		{
			switch (dayOfWeekType)
			{
				case weekday:
					DayTextColor = ColorList.colorReservationFontColor;
					break;
				case saturday:
					DayTextColor = ColorList.colorScheduleSaturday;
					break;
				case sunday:
				case holiday:
					DayTextColor = ColorList.colorScheduleSunday;
					break;
				default:
					break;
			}
		}



		void SetSizes()
		{
			DayWidth = ScaleManager.ScreenWidth / 5.0;
			//30.0;
			DayFontSize = 18.0;
		}
		private string _DayText;
		public string DayText
		{
			get { return _DayText; }
			set
			{
				if (_DayText != value)
				{
					_DayText = value;
					OnPropertyChanged("DayText");
				}
			}
		}

		private string _DayOfWeekText;
		public string DayOfWeekText
		{
			get { return _DayOfWeekText; }
			set
			{
				if (_DayOfWeekText != value)
				{
					_DayOfWeekText = value;
					OnPropertyChanged("DayOfWeekText");
				}
			}
		}

		private double _DayWidth;
		public double DayWidth
		{
			get { return _DayWidth; }
			set
			{
				if (_DayWidth != value)
				{
					_DayWidth = value;
					OnPropertyChanged("DayWidth");
				}
			}
		}

		//private double _DayHeight;
		//public double DayHeight
		//{
		//	get { return _DayHeight; }
		//	set
		//	{
		//		if (_DayHeight != value)
		//		{
		//			_DayHeight = value;
		//			OnPropertyChanged("DayHeight");
		//		}
		//	}
		//}

		private double _DayFontSize;
		public double DayFontSize
		{
			get { return _DayFontSize; }
			set
			{
				if (_DayFontSize != value)
				{
					_DayFontSize = value;
					OnPropertyChanged("DayFontSize");
				}
			}
		}

		private Color _DayTextColor;
		public Color DayTextColor
		{
			get { return _DayTextColor; }
			set
			{
				if (_DayTextColor != value)
				{
					_DayTextColor = value;
					OnPropertyChanged("DayTextColor");
				}
			}
		}
	}
}