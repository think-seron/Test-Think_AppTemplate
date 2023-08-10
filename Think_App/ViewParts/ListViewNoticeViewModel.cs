using System;
using IO.Swagger.Model;

namespace Think_App
{
	public class ListViewNoticeViewModel : ViewModelBase
	{
		//public ListViewNoticeViewModel(string str, bool imgFlg)
		//{
		//	LabelText = str;
		//	DispIcon = imgFlg;
		//}

		public ListViewNoticeViewModel()
		{
			//NoticeId = list.NoticeId;
			//LabelText = list.Title;
			//Summary = list.Summary;
			//Description = list.Description;
			//if (list.IsRead != null)
			//{
			//	IsRead = (bool)list.IsRead;
			//}
			//LabelFontSize = 14 * ScaleManager.Scale;
            BatchSize = ScaleManager.SizeSet(14);
		}

		public string Summary { get; set; }
		public int? NoticeId { get; set; }
		public string Description { get; set; }
		public string LabelText { get; set; }
		public double LabelFontSize { get; set; }
		public double BatchSize { get; set; }

		private bool batchIsVisible;
		public bool BatchIsVisible
		{
			get
			{
				return batchIsVisible;
			}
			set
			{
				batchIsVisible = value;
                OnPropertyChanged("BatchIsVisible");
			}
		}

		private bool isRead;
		public bool IsRead
		{
			get
			{
				return isRead;
			}
			set
			{
				//if (isRead != value)
				//{
				//	isRead = value;
				//	IsReadChanged(isRead);
				//}
				isRead = value;
                IsReadChanged(isRead);
			}
		}


		void IsReadChanged(bool isread)
		{
			if (!isread)
			{
				batchIsVisible = true;
			}
			else
			{
				batchIsVisible = false;
			}
			OnPropertyChanged("BatchIsVisible");
		}
	}
}
