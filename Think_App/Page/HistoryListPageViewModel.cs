using System;
using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace Think_App
{
	public class HistoryListPageViewModel : ViewModelBase
	{
		public HistoryListPageViewModel()
		{
			ListViewRowHeight = ScaleManager.SizeSet(84);
		}

		public double ListViewRowHeight { get; set; }
		public Rectangle ListViewRect { get; set; }

		private ObservableCollection<ListViewHistoryViewModel> historyItemSouce;
		public ObservableCollection<ListViewHistoryViewModel> HistoryItemSouce
		{
			get
			{
				return historyItemSouce;
			}
			set
			{
				if (historyItemSouce != value)
				{
					historyItemSouce = value;
					OnPropertyChanged("HistoryItemSouce");
				}
			}
		}

		private double hooterHeight;
		public double HooterHeight
		{
			get { return hooterHeight; }
			set
			{
				if (hooterHeight != value)
				{
				hooterHeight = value;
				OnPropertyChanged("HooterHeight");
				}
			}
		}

		public double HooterWidth { get; set; }

		private double hooterBtnHeight;
		public double HooterBtnHeight
		{
			get { return hooterBtnHeight; }
			set
			{
				if (hooterBtnHeight != value)
				{
					hooterBtnHeight = value;
					OnPropertyChanged("HooterBtnHeight");
				}
			}
		}

		private bool hooterIsVisible;
		public bool HooterIsVisible
		{
			get { return hooterIsVisible; }
			set
			{
				if (hooterIsVisible != value)
				{
					hooterIsVisible = value;
					if (hooterIsVisible == false)
					{
						this.HooterBtnHeight = 0;
						this.HooterHeight = 0;
					}
					else
					{
						this.HooterBtnHeight = 36;
						this.HooterHeight = 44;
					}
					OnPropertyChanged("HooterIsVisible");
				}
			}
		}


        public Command HistoryTapCommand => new Command(async(obj) =>
        {
            if (App.ProcessManager.CanInvoke())
			{
				if (!(obj is ListVIewHistoryCellViewModel item))
				{
					App.ProcessManager.OnComplete();
					return;
				}
                await App.customNavigationPage.PushAsync(new HistoryDetailPage((item)));
                App.ProcessManager.OnComplete();
            }
        });
    }
}
