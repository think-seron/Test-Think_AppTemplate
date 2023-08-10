using Xamarin.Forms;
using System.Threading.Tasks;
using Newtonsoft.Json;
using IO.Swagger.Model;
using System.Collections.Generic;

namespace Think_App
{
	public class FavoriteStoreRegViewModel : ViewModelBase
	{
		public FavoriteStoreRegViewModel()
		{
			ScreenSizeScale = ScaleManager.Scale;
			BtnIsEnabled = false;
			//Device.BeginInvokeOnMainThread(async () =>
			//{
			//	var deviceId = Config.Instance.Data.deviceId;
			//	var parameters = new Dictionary<string, string> { { "deviceId", deviceId.ToString() } };
			//	var json = await APIManager.GET("salon_regionlist", parameters);

			//	if (json == null)
			//	{
				   
			//		   DependencyService.Get<IToast>().Show("通信エラー");
				   
			//	}
			//	else
			//	{
			//	   var response = JsonConvert.DeserializeObject<ResponseSalonRegionList>(json);
			//		if (response.Data.List.Count > 0)
			//		{
			//			RegStartCommand = new Command(() => ScreenTransition(new StoreAreaSelect(1, response)));
			//		}
			//		else
			//		{
			//			RegStartCommand = new Command(() => ScreenTransition(new StoreSelect(1, null)));
			//		}
			//		//await Task.Delay(500);
			//		BtnIsEnabled = true;
			//	}
			//});
			/*
			Task.Run(async() =>
			{
				var deviceId =  Config.Instance.Data.deviceId;
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
					if (response.Data.List.Count > 0)
					{
						RegStartCommand = new Command(() => ScreenTransition(new StoreAreaSelect(1, response)));
					}
					else
					{
						RegStartCommand = new Command(() => ScreenTransition(new StoreSelect(1, null)));
					}
					await Task.Delay(500);
					BtnIsEnabled = true;
				}
				App.customNavigationPage.IsRunning = false;
			});
			*/

			//CustomNavibarBC = new CustomNavigationBarViewModel("店舗選択", CustomNavigationBarViewModel.LeftBtnKinds.BackBtn, CustomNavigationBarViewModel.RightBtnKinds.None);
			//CustomNavibarBC.LeftBtnClicked = new Command(() => { ScreenTransition(); });
		}

		public Command RegStartCommand { get; set; }
		public double ScreenSizeScale { get; set; }

		private bool btnIsEnabled;
		public bool BtnIsEnabled
		{
			get
			{
				return btnIsEnabled;
			}
			set
			{
				if (btnIsEnabled != value)
				{
					btnIsEnabled = value;
					OnPropertyChanged("BtnIsEnabled");
				}
			}
		}


		//private CustomNavigationBarViewModel _CustomNavibarBC;
		//public CustomNavigationBarViewModel CustomNavibarBC
		//{
		//	get { return _CustomNavibarBC; }
		//	set
		//	{
		//		if (_CustomNavibarBC != value)
		//		{
		//			_CustomNavibarBC = value;
		//			OnPropertyChanged("CustomNavibarBC");
		//		}
		//	}
		//}
	}
}
