using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using IO.Swagger.Model;
namespace Think_App
{
	public class ListVIewHistoryCellViewModel : ViewModelBase
	{
		public ListVIewHistoryCellViewModel(int otherState, InlineResponse20013DataTreatmentHistoryList listdata = null)
		{
			SetSizes();
			UpdateItems(otherState, listdata);
		}

		public void UpdateItems(int otherState, InlineResponse20013DataTreatmentHistoryList listdata)
		{
			switch (otherState)
			{
				case 1:
					if (listdata.ThumbnailImage != null && listdata.ThumbnailImage.Count > 0)
					{
						Source = listdata.ThumbnailImage[0].Path;
					}
					else
					{
						Source = "noimage.png";
					}

					StoreName = listdata.SalonName;

					Time = listdata.DateStr;

					TreatmentHistoryId = listdata.TreatmentHistoryId;
					Stylist = listdata.Stylist;
					TreatmentDescription = listdata.TreatmentDescription;
					break;
				case 2:
					Source = "";
					StoreName = "施術履歴はありません";
					break;
				default:
					Source = "";
					StoreName = "読み込めませんでした";
					break;
			}


		}

		public bool CanSelected { get; set; }

		void SetSizes()
		{
			StoreNameSize = ScaleManager.SizeSet(13);
			ImgSize = ScaleManager.SizeSet(56);
			HeightSize = ScaleManager.SizeSet(84);
		}

		public double HeightSize { get; set; }
		public double ImgSize { get; set; }

		private string _Source;
		public string Source
		{
			get { return _Source; }
			set
			{
				if (_Source != value)
				{
					_Source = value;
					OnPropertyChanged("Source");
				}
			}
		}
		private string _StoreName;
		public string StoreName
		{
			get { return _StoreName; }
			set
			{
				if (_StoreName != value)
				{
					_StoreName = value;
					OnPropertyChanged("StoreName");
				}
			}
		}
		private string _Time;
		public string Time
		{
			get { return _Time; }
			set
			{
				if (_Time != value)
				{
					_Time = value;
					OnPropertyChanged("Time");
				}
			}
		}

		public double StoreNameSize { get; set; }
		//public int TreatmentHistoryId { get; set; }
		public string TreatmentHistoryId { get; set; }
		public string Stylist { get; set; }
		public string TreatmentDescription { get; set; }
		public ObservableCollection<string> thumbnailList { get; set; }
	}
}
