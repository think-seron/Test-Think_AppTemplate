using System;
using System.Collections.ObjectModel;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
	public class HairCatalogListPageViewModel : ViewModelBase
	{
		public HairCatalogListPageViewModel()
		{
			CustomNavibarBC = new CustomNavigationBarViewModel("ヘアカタログ", CustomNavigationBarViewModel.LeftBtnKinds.BackBtn, CustomNavigationBarViewModel.RightBtnKinds.None, null);
			CustomNavibarBC.LeftBtnClicked = new Command(() => { ScreenTransition(); });
			ScreenSizeScale = ScaleManager.Scale;
		}

		public double ScreenSizeScale { get; set; }
		public double ItemHeight { get; set; }
		public double ItemWidth { get; set; }
		public double ColumnSpacing { get; set; }
		public double RowSpacing { get; set; }
		public Rect GridViewRect { get; set; }
		public string LabelTxt { get; set; }

		private ObservableCollection<HairStyleInfo> itemsSource;
		public ObservableCollection<HairStyleInfo> ItemsSource
		{
			get { return itemsSource;}
			set
			{
				if (itemsSource != value)
				{
					itemsSource = value;
                    OnPropertyChanged("ItemsSource");
				}
			}
		}


		private CustomNavigationBarViewModel _CustomNavibarBC;
		public CustomNavigationBarViewModel CustomNavibarBC
		{
			get { return _CustomNavibarBC; }
			set
			{
				if (_CustomNavibarBC != value)
				{
					_CustomNavibarBC = value;
					OnPropertyChanged("CustomNavibarBC");
				}
			}
		}
	}
}
