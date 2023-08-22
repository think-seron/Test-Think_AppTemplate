using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using IO.Swagger.Model;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
	public partial class FavoriteStoreReg : ContentPage
	{
		FavoriteStoreRegViewModel favoriteStoreRegViewModel;
		public FavoriteStoreReg()
		{
			InitializeComponent();
			NavigationPage.SetBackButtonTitle(this, "");
			NavigationPage.SetHasBackButton(this, false);
			SetTask();
		}

		async Task SetTask()
		{

			favoriteStoreRegViewModel = new FavoriteStoreRegViewModel();
			string deviceId = Config.Instance.Data.deviceId;
			System.Diagnostics.Debug.WriteLine(" device id :" + deviceId);

			var parameters = new Dictionary<string, string> { { "deviceId", deviceId.ToString() } };
			var json = await APIManager.GET("salon_regionlist", parameters);
			if (json == null)
			{
				Device.BeginInvokeOnMainThread(() =>
				{
					DependencyService.Get<IToast>().Show("通信エラー");
				});
			}
			else
			{
				var response = JsonConvert.DeserializeObject<ResponseSalonRegionList>(json);

				if (response.Data != null)
				{
					favoriteStoreRegViewModel.RegStartCommand = new Command(() => Navigation.PushAsync(new StoreAreaSelect(1, response)));
				}
				else
				{
					favoriteStoreRegViewModel.RegStartCommand = new Command(() => Navigation.PushAsync(new StoreSelect(1, null)));
				}

				await Task.Delay(500);
				favoriteStoreRegViewModel.BtnIsEnabled = true;
			}
			Device.BeginInvokeOnMainThread(() =>
			{
				App.customNavigationPage.IsRunning = false;
				this.BindingContext = favoriteStoreRegViewModel;
			});
		}
		protected override bool OnBackButtonPressed()
		{
			return true;
		}
	}
}
