using System;
using System.Collections.Generic;
using IO.Swagger.Model;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;

namespace Think_App
{
	public partial class HistoryListPage : ContentPage
	{
		ObservableCollection<ListVIewHistoryCellViewModel> itemList;
		HistoryListPageViewModel historyListPageViewModel;
		bool? isEnd;
		int index;
		Dictionary<string, string> parameters;

		public HistoryListPage()
		{
			InitializeComponent();
			NavigationPage.SetBackButtonTitle(this, "");
			App.customNavigationPage.IsRunning = true;

			historyListPageViewModel = new HistoryListPageViewModel();
			itemList = new ObservableCollection<ListVIewHistoryCellViewModel>();

			index = 0;
			parameters = new Dictionary<string, string> { { "index", index.ToString() } };
			DataGet();
			App.customNavigationPage.IsRunning = false;
		}


		async void DataGet()
		{
			var json = await APIManager.GET("history_treatmentlist", parameters);
			if (json == null)
			{
				Device.BeginInvokeOnMainThread(() =>
				{
					DependencyService.Get<IToast>().Show("通信エラー");
				});
			}
			else
			{
				var response = JsonConvert.DeserializeObject<ResponseTreatmentHistoryList>(json);
				isEnd = response.Data?.IsEnd;

				if (response.Data == null || response.Data.List == null)
					itemList.Add(new ListVIewHistoryCellViewModel(3));
				else if (response.Data.List.Count == 0)
					itemList.Add(new ListVIewHistoryCellViewModel(2));
				else if (response.Data.List.Any())
					foreach (var n in response.Data.List)
						itemList.Add(new ListVIewHistoryCellViewModel(1, n));
				else
				{
					itemList.Add(new ListVIewHistoryCellViewModel(3));
					return;
				}

				int rectHeight = (int)historyListPageViewModel.ListViewRowHeight * itemList.Count;
				historyListPageViewModel.HooterIsVisible = false;
				int heightAgust = 111;
				if (Device.RuntimePlatform == Device.Android)
				{
					heightAgust = heightAgust - 9;
				}
				if ((ScaleManager.ScreenHeight - heightAgust) < rectHeight)
				{
					historyListPageViewModel.ListViewRect = new Rectangle(0, 46, 1, (ScaleManager.ScreenHeight - heightAgust));
					if (isEnd == false)
					{
						historyListPageViewModel.HooterIsVisible = true;
					}
				}
				else
				{
					historyListPageViewModel.ListViewRect = new Rectangle(0, 46, 1, rectHeight);
				}
			}
			Device.BeginInvokeOnMainThread(() =>
			{
				this.ListView.ItemsSource = itemList;
				this.BindingContext = historyListPageViewModel;
			});
		}


		void FooterBtnClick(object sender, EventArgs e)
		{
			if (App.ProcessManager.CanInvoke())
			{
				int indexNum = int.Parse(parameters["index"]);
				indexNum++;
				parameters["index"] = indexNum.ToString();
				DataGet();
				App.ProcessManager.OnComplete();
			}
		}
	}
}
