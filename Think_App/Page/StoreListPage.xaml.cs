using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using System.Threading.Tasks;
using IO.Swagger.Model;
using Newtonsoft.Json;

namespace Think_App
{
	public partial class StoreListPage : ContentPage
	{
		StoreListPageViewModel storeListPageViewModel;
		int? regionId;

		public StoreListPage(int? _regionId)
		{
			InitializeComponent();
			NavigationPage.SetBackButtonTitle(this, "");
			regionId = _regionId;

			storeListPageViewModel = new StoreListPageViewModel();

			// ------------------------------------------------------
			// 選択した地域のサロンの一覧データを取得しitemListにadd
			this.Appearing += (sender, e) =>
			{
				App.customNavigationPage.IsRunning = true;
				List<ListViewStoreViewModel> itemList = new List<ListViewStoreViewModel>();
				Task.Run(async () =>
				{
					string json;
					if (regionId == null)
					{
						var parameters = new Dictionary<string, string> { };
						json = await APIManager.GET("salon_list", parameters);
					}
					else
					{
						var parameters = new Dictionary<string, string> { { "regionId", regionId.ToString() } };
						json = await APIManager.GET("salon_list", parameters);
					}
					if (json == null)
					{
						Device.BeginInvokeOnMainThread(() =>
						{
							DependencyService.Get<IToast>().Show("通信エラー");
						});
					}
					else
					{
						var responseSalonRegionList = JsonConvert.DeserializeObject<ResponseSalonList>(json);
						var salonList = responseSalonRegionList.Data.List;
						foreach (var val in salonList)
						{
							string iconSouce;
							if (val.IsFavorite == true)
							{
								iconSouce = "BigFavoIconOn.png";
							}
							else
							{
								iconSouce = "BigFavoIconOff.png";
							}
							//itemList.Add(new ListViewStoreViewModel(val));
							itemList.Add(new ListViewStoreViewModel()
							{
								ImageSouce = val.ThumbnailImage.Path,
								StoreName = val.Name,
								StoreAddress = val.Address,
								StoreTel = val.Tel,
								BusinessHours = val.BusinessHours,
								FavoIconSouce = iconSouce,
								SalonID = val.SalonId
							});
						}
					}

					int rectHeight = 183 * itemList.Count;
					int heightAgust = 111;
					if (Device.RuntimePlatform == Device.Android)
					{
						heightAgust = heightAgust - 9;
					}
					if ((ScaleManager.ScreenHeight - heightAgust) < rectHeight)
					{
						storeListPageViewModel.ListViewRect = new Rectangle(0, 46, 1, (ScaleManager.ScreenHeight - heightAgust));
					}
					else
					{
						storeListPageViewModel.ListViewRect = new Rectangle(0, 46, 1, rectHeight);
					}

					Device.BeginInvokeOnMainThread(() =>
					{
						this.ListView.ItemsSource = itemList;
						this.BindingContext = storeListPageViewModel;
						App.customNavigationPage.IsRunning = false;
					});
				});
			};

			this.ListView.ItemSelected += async (sender, e) =>
			{
				if (App.ProcessManager.CanInvoke())
				{
					await Navigation.PushAsync(new StoreInformationPage(((ListViewStoreViewModel)e.SelectedItem).SalonID));
					this.ListView.SelectedItem = null;
					App.ProcessManager.OnComplete();
				}
			};
		}
	}
}
