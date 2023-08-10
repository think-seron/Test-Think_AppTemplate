using System;
using IO.Swagger.Model;

namespace Think_App
{
	public class ListViewStaffStoreViewModel : ViewModelBase
	{
		public ListViewStaffStoreViewModel()
		{
			BdContext = this;
            StaffNameFontSize = ScaleManager.SizeSet(18);
            StaffKanaFontSize = ScaleManager.SizeSet(10);
            StaffCareerFontsize = ScaleManager.SizeSet(12);
            ThumbnailSize = ScaleManager.SizeSet(135);
		}


        public double ThumbnailSize { get; set; }
		public double StaffNameFontSize { get; set; }
		public double StaffKanaFontSize { get; set; }
		public double StaffCareerFontsize { get; set;}
		public int salonID { get; set; }

		public int? StaffID { get; set; }
		public string StaffKana { get; set; }
		public string ImageSouce { get; set; }
		public string StaffName { get; set; }
		public string StaffCareer { get; set; }
		public string StaffCome { get; set; }

        public bool CanReserv { get; set; }

		public ListViewStaffStoreViewModel BdContext { get; set; }

		private bool isFavorite;
		public bool IsFavorite
		{
			get
			{
				return isFavorite;
			}
			set
			{
				if (isFavorite != value)
				{
					isFavorite = value;
					OnPropertyChanged("IsFavorite");
				}
			}
		}

		private string favoIconSouce;
		public string FavoIconSouce
		{
			get
			{
				return favoIconSouce;
			}
			set
			{
				if (favoIconSouce != value)
				{
					favoIconSouce = value;
					OnPropertyChanged("FavoIconSouce");
				}
			}
		}
	}
}
