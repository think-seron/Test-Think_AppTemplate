using System;
using Think_App;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
namespace Think_App
{
public class ReservationDetailCanselViewModel : ViewModelBase
	{
public ReservationDetailCanselViewModel(IO.Swagger.Model.ReservationDetail data,Command yesCommand, Command noCommand)
		{
			BtnWidth = 153 * ScaleManager.WidthScale;


			ReservationDate = data.Data.DateStr;
			ReservationStore = data.Data.SalonName;
			ReservationStyList = data.Data.StaffName;
			ReservationMenu = data.Data.MenuName;
			EditorText = data.Data.Memo;
			ReservationSource = data.Data.Source;
			if (string.IsNullOrEmpty(data.Data.CouponName))
			{
				ReservationUsingCoupon = "無";
				ReservationCouponContent = null;
				ReservationCouponContentVisible = false;
			}
			else
			{
				ReservationUsingCoupon = "有";
				ReservationCouponContent = data.Data.CouponName;
				ReservationCouponContentVisible = true;
			}

			YesCommand = yesCommand;
			NoCommand = noCommand;
		}

		public double BtnWidth { get; set; }
		public string ReservationDate { get; set; }
		public string ReservationStore { get; set; }
		public string ReservationStyList { get; set; }
		public string ReservationMenu { get; set; }
		public string ReservationUsingCoupon { get; set; }
		public string ReservationCouponContent { get; set; }
		public string EditorText { get; set; }
		public string ReservationSource { get; set; }

		public bool ReservationCouponContentVisible { get; set; }


		private Command _YesCommand;
		public Command YesCommand
		{
			get { return _YesCommand; }
			set
			{
				if (_YesCommand != value)
				{
					_YesCommand = value;
					OnPropertyChanged("YesCommand");
				}
			}
		}
		private Command _NoCommand;
		public Command NoCommand
		{
			get { return _NoCommand; }
			set
			{
				if (_NoCommand != value)
				{
					_NoCommand = value;
					OnPropertyChanged("NoCommand");
				}
			}
		}
	}
}
