using System;
using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace Think_App
{
	public class HistoryDetailPageViewModel : ViewModelBase
	{
		public HistoryDetailPageViewModel(ListVIewHistoryCellViewModel list)
		{
			CarouselItem = new ObservableCollection<CarouselImageSouce>();

			if (list.thumbnailList != null && list.thumbnailList.Count > 0)
			{
				foreach (var val in list.thumbnailList)
				{
					CarouselItem.Add(new CarouselImageSouce() { CarouselImage = val });
				}
			}
			else {
					CarouselItem.Add(new CarouselImageSouce() { CarouselImage = "no_image_detail.png"});
			}

			DateTxt = list.Time;
			StoreNameTxt = list.StoreName;
			StylistTxt = list.Stylist;
			TreatmentDescription = list.TreatmentDescription;
			DateTxtSize = ScaleManager.SizeSet(18);
			StylistTxtSize = ScaleManager.SizeSet(14);
		}

		public double StylistTxtSize { get; set; }
		public double DateTxtSize { get; set; }
		public string DateTxt { get; set; }
		public string StoreNameTxt { get; set; }
		public string StylistTxt { get; set; }
		public string TreatmentDescription { get; set; }

		private ObservableCollection<CarouselImageSouce> carouselItem;
		public ObservableCollection<CarouselImageSouce> CarouselItem
		{
			get
			{
				if (carouselItem == null)
				{
					carouselItem = new ObservableCollection<CarouselImageSouce>();
				}
				return carouselItem;
			}
			set
			{
				if (carouselItem == null)
				{
					carouselItem = new ObservableCollection<CarouselImageSouce>();
				}
				if (carouselItem != value)
				{
					carouselItem = value;
					OnPropertyChanged("CarouselItem");
				}
			}
		}

		public class CarouselImageSouce
		{
			public string CarouselImage { get; set; }
		}
	}
}
