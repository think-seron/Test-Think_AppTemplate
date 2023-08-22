using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using IO.Swagger.Model;
using System.Text.RegularExpressions;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
	public partial class StoreInformationPage : ContentPage
	{
		StoreInformationPageViewModel storeInformationPageViewModel;
		int ID;
		string lat;
		string lon;
		string salonName;
		bool soloSalon = false;
		public StoreInformationPage(int? salonId, bool _soloSalon = false)
		{
			InitializeComponent();
			soloSalon = _soloSalon;
			NavigationPage.SetBackButtonTitle(this, "");

			this.Title = "店舗情報";

			storeInformationPageViewModel = new StoreInformationPageViewModel();

			// ----------------------------------------------------
			// 選択した店舗の情報を取得し表示させる
			this.Appearing += (sender, e) =>
			{
				App.customNavigationPage.IsRunning = true;
				List<ListViewStaffStoreViewModel> itemList = new List<ListViewStaffStoreViewModel>();
				Task.Run(async () =>
				{
					var parameters = new Dictionary<string, string> { { "salonId", salonId.ToString() } };
					var jsonstr = await APIManager.GET("salon", parameters);
					if (jsonstr == null)
					{
						Device.BeginInvokeOnMainThread(() =>
						{
							DependencyService.Get<IToast>().Show("通信エラー");
						});
					}
					else
					{
						ID = (int)salonId;
						ResponseSalon responseSalon;
						responseSalon = JsonConvert.DeserializeObject<ResponseSalon>(jsonstr);

						storeInformationPageViewModel.StoreMessage = responseSalon.Data.Description;
						if (responseSalon.Data.ThumbnailImage != null)
						{
							storeInformationPageViewModel.StoreImgSouce = responseSalon.Data.ThumbnailImage.Path;
						}
						storeInformationPageViewModel.StoreNameTxt = responseSalon.Data.Name;
						salonName = responseSalon.Data.Name;
						storeInformationPageViewModel.StoreBusinessHour = responseSalon.Data.BusinessHours;
						storeInformationPageViewModel.StoreAddress = responseSalon.Data.Address;
						storeInformationPageViewModel.StoreTelNumber = responseSalon.Data.Tel;
						if (responseSalon.Data.IsFavorite == true)
						{
							storeInformationPageViewModel.FavoIconSouce = "BigFavoIconOn.png";
						}
						else
						{
							storeInformationPageViewModel.FavoIconSouce = "BigFavoIconOff.png";
						}
						var mapinfo = responseSalon.Data.MapInfo;
						lat = mapinfo.Lat;
						lon = mapinfo.Lon;

						if (responseSalon.Data.StaffList != null)
						{
							var staffList = responseSalon.Data.StaffList;
							foreach (var val in staffList)
							{
								string iconSouce;
								if (val.IsFavorite != null && val.IsFavorite == true)
								{
									iconSouce = "BigFavoIconOn.png";
								}
								else
								{
									iconSouce = "BigFavoIconOff.png";
								}

								itemList.Add(new ListViewStaffStoreViewModel()
								{
									ImageSouce = val.ThumbnailImage.Path,
									StaffName = val.Name,
									StaffKana = val.Kana,
									StaffCareer = val.Career,
									//StaffCome = val.Description,
									StaffCome = val.Summary,
									StaffID = val.StaffId,
									FavoIconSouce = iconSouce,
									salonID = (int)salonId,
									CanReserv = (bool)val.CanReservate
								});
							}
						}
					}

					//if (Device.RuntimePlatform == Device.iOS)
					//{ 
					//	if ((ScaleManager.ScreenHeight - 421) < rectHeight)
					//	{
					//		storeInformationPageViewModel.ListViewRect = new Rectangle(0, 421, 1, (ScaleManager.ScreenHeight - 421));
					//	}
					//	else
					//	{
					//		storeInformationPageViewModel.ListViewRect = new Rectangle(0, 421, 1, rectHeight);
					//	}
					//}
					//else if (Device.RuntimePlatform == Device.Android)
					//{
					//	if ((ScaleManager.ScreenHeight - 401) < rectHeight)
					//	{
					//		storeInformationPageViewModel.ListViewRect = new Rectangle(0, 401, 1, (ScaleManager.ScreenHeight - 401));
					//	}
					//	else
					//	{
					//		storeInformationPageViewModel.ListViewRect = new Rectangle(0, 401, 1, rectHeight);
					//	}
					//}

					Device.BeginInvokeOnMainThread(() =>
					{
						this.ListView.ItemsSource = itemList;
						this.BindingContext = storeInformationPageViewModel;
						App.customNavigationPage.IsRunning = false;
					});
				});
			};
			//this.ListView.ItemsSource = itemList;
			//this.BindingContext = storeInformationPageViewModel;

			this.ListView.ItemSelected += async (sender, e) =>
			{
				if (App.ProcessManager.CanInvoke())
				{
					await App.customNavigationPage.PushAsync(new StaffInformationPage(2, (int)((ListViewStaffStoreViewModel)e.SelectedItem).StaffID, (int)salonId, salonName, ((ListViewStaffStoreViewModel)e.SelectedItem).CanReserv));
					this.ListView.SelectedItem = null;
					App.ProcessManager.OnComplete();
				}
			};
		}

		void TelBtnClicked(object sender, EventArgs e)
		{
			if (App.ProcessManager.CanInvoke())
			{
				string telNum = storeInformationPageViewModel.StoreTelNumber.Replace("-", "");
				//int number = 0;
				//bool result = int.TryParse(telNum, out number);
				//if (result)
				//{
				//	Device.OpenUri(new Uri("tel:" + number));
				//}
				//else
				//{
				Regex re = new Regex(@"[^0-9]");
				var tel = re.Replace(telNum, "");
				Device.OpenAsync(new Uri("tel:" + tel));
				//}
				App.ProcessManager.OnComplete();
			}
		}

		void MapBtnClicked(object sender, EventArgs e)
		{
			if (App.ProcessManager.CanInvoke())
			{
				// TODO Xamarin.Forms.Device.RuntimePlatform is no longer supported. Use Microsoft.Maui.Devices.DeviceInfo.Platform instead. For more details see https://learn.microsoft.com/en-us/dotnet/maui/migration/forms-projects#device-changes
				if (Device.RuntimePlatform == Device.iOS)
				{
					// 参考 https://developer.apple.com/library/content/featuredarticles/iPhoneURLScheme_Reference/MapLinks/MapLinks.html
					Device.OpenUri(new Uri("http://maps.apple.com/?q=" + salonName + "&ll=" + lat + "," + lon));
				}
				else
				{
					// geo:[経度],[緯度]?q=サロン名
					// 緯度経度、もしくはサロン名だけでも地図表示できる
					Device.OpenUri(new Uri("geo:35.691193,139.709874?q=" + lat + "," + lon + "(" + salonName + ")"));
				}
				App.ProcessManager.OnComplete();
			}
		}

		async void OnStoreFavoIconClicked(object sender, EventArgs e)
		{
			if (!soloSalon && App.ProcessManager.CanInvoke())
			{
				StaticMethod.SalonFavoriteChange(sender, ID);
				//if (ret == false)
				//{
				//             DependencyService.Get<IToast>().Show("通信エラー");
				//}
				//App.ProcessManager.OnComplete();
			}
		}
	}
}
