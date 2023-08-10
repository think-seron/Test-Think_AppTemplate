using System;
namespace Think_App
{
	public class ReservationContentInfo
	{
		public ReservationContentInfo()
		{
		}

		public int? SalonId { get; set; }

		public int? StaffId { get; set; }

		public int? MenuId { get; set; }

		public int? CouponId { get; set; }

		public string StaffName { get; set; }

		public string StoreName { get; set; }

		public string MenuName { get; set;}

		public string CouponContent { get; set; }

		public DateTime Date { get; set; }

		public bool IsRedicide { get; set; }

		public AccountInfo Account { get; set; }
	}
}
