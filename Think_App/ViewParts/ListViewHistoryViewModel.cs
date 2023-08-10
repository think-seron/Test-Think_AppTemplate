using System;
using IO.Swagger.Model;
using System.Collections.Generic;

namespace Think_App
{
	public class ListViewHistoryViewModel : ViewModelBase
	{
		public ListViewHistoryViewModel()
		{
			//if (list != null)
			//{
			//	WidthSize = ScaleManager.ScreenWidth;
			//	//Source = "loginBgImg.png";
			//	Source = list.ThumbnailImage[0].Path;
			//	//ImgTimestamp = list.ThumbnailImage.Timestamp;
			//	thumbnailList = list.ThumbnailImage;
			//	StoreName = list.SalonName;
			//	Time = list.DateStr;
			//	StoreNameSize = 13 * ScaleManager.Scale;
			//	TreatmentHistoryId = list.TreatmentHistoryId;
			//	Stylist = list.Stylist;
			//	TreatmentDescription = list.TreatmentDescription;
			//}
			//else
			//{
			//	Source = "";
			//	WidthSize = ScaleManager.ScreenWidth;
			//	StoreName = "施術履歴はありません";
			//	StoreNameSize = 13 * ScaleManager.Scale;
			//}
			ImgSize = ScaleManager.SizeSet(56);
			HeightSize = ScaleManager.SizeSet(84);
		}

		public double HeightSize { get; set; }
		public double ImgSize { get; set; }
		public double WidthSize { get; set; }
		public string Source { get; set; }
		//public string ImgTimestamp { get; set; }
		public string StoreName { get; set; }
		public string Time { get; set; }
		public double StoreNameSize { get; set; }
		//public int TreatmentHistoryId { get; set; }
		public string TreatmentHistoryId { get; set; }
		public string Stylist { get; set; }
		public string TreatmentDescription { get; set; }
		public List<string> thumbnailList { get; set; }
	}
}
