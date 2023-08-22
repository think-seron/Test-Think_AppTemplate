using System;
using System.Collections.ObjectModel;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
	public class MyBeautyBlogListPageViewModel : ViewModelBase
	{
		public MyBeautyBlogListPageViewModel()
		{
		}

		public Rect GridViewRect { get; set; }
		public double ItemHeight { get; set; }
		public double ItemWidth { get; set; }
		public double ColumnSpacing { get; set; }
		public double RowSpacing { get; set; }
		public Rect MyBlogPlusListViewRect { get; set; }

		//private ObservableCollection<MyBeautyBlogModel> itemsSource;
		//public ObservableCollection<MyBeautyBlogModel> ItemsSource
		//{
		//	get { return itemsSource; }
		//	set
		//	{
		//		if (itemsSource != value)
		//		{
		//			itemsSource = value;
		//			OnPropertyChanged("ItemsSource");
		//		}
		//	}
		//}
		private bool myBlogPlusListViewIsVisible;
		public bool MyBlogPlusListViewIsVisible
		{
			get
			{
				return myBlogPlusListViewIsVisible;
			}
			set
			{
				if (myBlogPlusListViewIsVisible != value)
				{
					myBlogPlusListViewIsVisible = value;
					OnPropertyChanged("MyBlogPlusListViewIsVisible");
				}
			}
		}

		private bool ctgListIsVisible;
		public bool CtgListIsVisible
		{
			get
			{
				return ctgListIsVisible;
			}
			set
			{
				if (ctgListIsVisible != value)
				{
					ctgListIsVisible = value;
					OnPropertyChanged("CtgListIsVisible");
				}
			}
		}

		private string ctgText;
		public string CtgText
		{
			get
			{
				return ctgText;
			}
			set
			{
				if (ctgText != value)
				{
					ctgText = value;
					OnPropertyChanged("CtgText");
				}
			}
		}



	}
}
