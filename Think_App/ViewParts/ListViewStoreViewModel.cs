using System;
using IO.Swagger.Model;

namespace Think_App
{
	public class ListViewStoreViewModel : ViewModelBase
	{
		//public ListViewStoreViewModel(InlineResponse2005DataList list)
		public ListViewStoreViewModel()
		{
			//ImageSouce = list.ThumbnailImage.Path;
			//StoreName = list.Name;
			//StoreAddress = list.Address;
			//StoreTel = list.Tel;
			//BusinessHours = list.BusinessHours;
			//if (list.IsFavorite == true)
			//{
			//	FavoIconSouce = "BigFavoIconOn.png";
			//}
			//else
			//{
			//	FavoIconSouce = "BigFavoIconOff.png";
			//}
			//SalonID = list.SalonId;
			BdContext = this;
            StoreAddressFontSize = ScaleManager.SizeSet(12);
            StoreNameFontSize = ScaleManager.SizeSet(18);
            ThumbnailSize = ScaleManager.SizeSet(135);
		}

        public double ThumbnailSize { get; set; }
		public int? SalonID { get; set; }
		public string StoreAddress { get; set; }
		public string ImageSouce { get; set; }
		public string StoreName { get; set; }
		public string StoreTel { get; set; }
		public string BusinessHours { get; set; }
		public double StoreAddressFontSize { get; set; }
		public double StoreNameFontSize { get; set; }
		public ListViewStoreViewModel BdContext { get; set; }

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
