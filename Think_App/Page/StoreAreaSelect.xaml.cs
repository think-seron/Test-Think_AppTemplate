using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;
using IO.Swagger.Model;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
	public partial class StoreAreaSelect : ContentPage
	{
		StoreAreaSelectViewModel storeAreaSelectViewModel;

		int lastPageNumber;
		// lastPageNumberについて
		// 1: 新規登録画面からの遷移
		// 2: ホーム画面からの遷移
		// 3: 予約からの遷移
		public StoreAreaSelect(int param, ResponseSalonRegionList responseSalonRegionList)
		{
			InitializeComponent();
			NavigationPage.SetBackButtonTitle(this, "");

			lastPageNumber = param;
			storeAreaSelectViewModel = new StoreAreaSelectViewModel();

			if (param == 1)
			{
				this.Title = "店舗選択";
				storeAreaSelectViewModel.LeftLabel = "エリア選択";
				storeAreaSelectViewModel.RightLabel = "※ 後で変更できます";
				storeAreaSelectViewModel.HooterLabel = "お気に入りの店舗がある地域を選択してください。";
			}
			else if (param == 2)
			{
				this.Title = "店舗一覧";
				storeAreaSelectViewModel.LeftLabel = "エリア一覧";
				storeAreaSelectViewModel.HooterLabel = "店舗地域を選択してください。";
			}
			else if (param == 3)
			{
				this.Title = "店舗一覧";
				storeAreaSelectViewModel.LeftLabel = "エリア一覧";
				storeAreaSelectViewModel.HooterLabel = "店舗地域を選択してください。";
			}

			List<ListViewLabelViewModel> itemList = new List<ListViewLabelViewModel>();

			var regionList = responseSalonRegionList.Data.List;
			foreach (var val in regionList)
			{
				//for (var i = 0; i < 5; i++)
				//{
				//itemList.Add(new ListViewLabelViewModel(val));
				itemList.Add(new ListViewLabelViewModel()
				{
					LabelText = val.Name,
					regionId = val.RegionId
				});
				//}
			}

			int rectHeight = 48 * itemList.Count + 38;
			int heightAgust = 111;
			// TODO Xamarin.Forms.Device.RuntimePlatform is no longer supported. Use Microsoft.Maui.Devices.DeviceInfo.Platform instead. For more details see https://learn.microsoft.com/en-us/dotnet/maui/migration/forms-projects#device-changes
			if (Device.RuntimePlatform == Device.Android)
			{
				heightAgust = heightAgust - 9;
			}
			if ((ScaleManager.ScreenHeight - heightAgust) < rectHeight)
			{
				storeAreaSelectViewModel.ListViewRect = new Rect(0, 46, 1, (ScaleManager.ScreenHeight - heightAgust));
			}
			else
			{
				storeAreaSelectViewModel.ListViewRect = new Rect(0, 46, 1, rectHeight);
			}

			storeAreaSelectViewModel.HooterWidth = ScaleManager.ScreenWidth;

			this.ListView.ItemsSource = itemList;
			this.BindingContext = storeAreaSelectViewModel;


			this.ListView.ItemSelected += async (sender, e) =>
			{
				if (App.ProcessManager.CanInvoke())
				{
					if (lastPageNumber == 1)
					{
						App.customNavigationPage.IsRunning = true;
						await Navigation.PushAsync(new StoreSelect(1, ((ListViewLabelViewModel)e.SelectedItem).regionId));
						App.customNavigationPage.IsRunning = false;
					}
					else if (lastPageNumber == 2)
					{
						await Navigation.PushAsync(new StoreListPage(((ListViewLabelViewModel)e.SelectedItem).regionId));
					}
					else
					{
						await Navigation.PushAsync(new StoreSelect(4, ((ListViewLabelViewModel)e.SelectedItem).regionId));
					}
					this.ListView.SelectedItem = null;
					App.ProcessManager.OnComplete();
				}
			};
		}
	}
}
