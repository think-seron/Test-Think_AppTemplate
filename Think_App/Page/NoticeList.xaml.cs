using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using IO.Swagger.Model;
using System.Collections.ObjectModel;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
	public partial class NoticeList : ContentPage
	{
		public NoticeListViewModel noticeListViewModel;
		bool? isEnd;
		int footerHeightNum = 44;
		Dictionary<string, string> parameters;
		ObservableCollection<ListViewNoticeViewModel> itemList;
		public string salonName;
		public int? salonId;
		bool soloSalon;
		public NoticeList(string _salonName, int? _salonId = null, bool _soloSalon = false)
		{
			InitializeComponent();
			NavigationPage.SetBackButtonTitle(this, "");
			//			ToolbarItems.Add(new ToolbarItem("ChangeStore", "Icon_Home.png", () =>
			//{
			//	Navigation.PushAsync(new StoreSelect(2, null, this));
			//}));
			salonName = _salonName;
			salonId = _salonId;
			soloSalon = _soloSalon;
			App.customNavigationPage.IsRunning = true;
			GetData();

			this.ListView.ItemSelected += async (sender, e) =>
			{
				if (App.ProcessManager.CanInvoke())
				{
					await Navigation.PushAsync(new NoticePage(((ListViewNoticeViewModel)e.SelectedItem).LabelText, ((ListViewNoticeViewModel)e.SelectedItem).Description, ((ListViewNoticeViewModel)e.SelectedItem).NoticeId, ((ListViewNoticeViewModel)e.SelectedItem).IsRead));
					this.ListView.SelectedItem = null;
					App.ProcessManager.OnComplete();
				}
			};

			if (!soloSalon)
			{
				ToolbarItems.Add(new ToolbarItem("ChangeStore", "Icon_Home.png", async () =>
				{
					if (!App.ProcessManager.CanInvoke())
						return;
					var page = new StoreSelect(2, null, this);
					// TODO Xamarin.Forms.Device.RuntimePlatform is no longer supported. Use Microsoft.Maui.Devices.DeviceInfo.Platform instead. For more details see https://learn.microsoft.com/en-us/dotnet/maui/migration/forms-projects#device-changes
					if (Device.RuntimePlatform == Device.iOS)
						App.customNavigationPage.IsBadgeVisble = false;
					await App.customNavigationPage.PushAsync(page);
					App.ProcessManager.OnComplete();
				}));
				this.Appearing += (sender, e) =>
				{
					SetIcon();
				};
			}


		}

		//protected override void OnAppearing()
		//{
		//    base.OnAppearing();
		//}
		async void SetIcon()
		{
			try
			{
				var jsonNotRead = await APIManager.GET("check_badge");
				var responseNotRead = JsonManager.Deserialize<ResponseCheckBatch>(jsonNotRead);
				if (responseNotRead != null && !soloSalon)
				{

					if ((bool)(responseNotRead.Data.NewsNotification))
					{
						Device.BeginInvokeOnMainThread(() =>
						{
							if (Device.RuntimePlatform == Device.Android)
							{
								ToolbarItems[0].Icon = "Icon_HomeAndBadge.png";
							}
							else
							{
								App.customNavigationPage.IsBadgeVisble = true;
							}
						});

					}
					else
					{
						Device.BeginInvokeOnMainThread(() =>
						{
							if (Device.RuntimePlatform == Device.Android)
							{
								ToolbarItems[0].Icon = "Icon_Home.png";
							}
							else
							{
								App.customNavigationPage.IsBadgeVisble = false;
							}
						});
					}
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine("ex :" + ex);
			}

		}

		public void GetData()
		{
			itemList = new ObservableCollection<ListViewNoticeViewModel>();
			Task.Run(async () =>
			{
				noticeListViewModel = new NoticeListViewModel(soloSalon);

				noticeListViewModel.SalonName = salonName;

				string json;
				if (salonId == null)
				{
					parameters = new Dictionary<string, string> { { "index", "0" } };
					json = await APIManager.GET("notice_list", parameters);
				}
				else
				{
					parameters = new Dictionary<string, string> { { "index", "0" }, { "salonId", salonId.ToString() } };
					json = await APIManager.GET("notice_list", parameters);
				}
				if (json == null)
				{
					Device.BeginInvokeOnMainThread(() =>
					{
						this.BindingContext = noticeListViewModel;
						DependencyService.Get<IToast>().Show("通信エラー");
					});
				}
				else
				{
					var response = JsonConvert.DeserializeObject<ResponseNoticeList>(json);
					var regionList = response.Data.List;
					isEnd = response.Data.IsEnd;
					if (regionList != null)
					{
						foreach (var val in regionList)
						{
							//itemList.Add(new ListViewNoticeViewModel(val));
							bool isRead = false;
							if (val.IsRead != null)
							{
								isRead = (bool)val.IsRead;
							}
							itemList.Add(new ListViewNoticeViewModel()
							{
								NoticeId = val.NoticeId,
								LabelText = val.Title,
								Summary = val.Summary,
								Description = val.Description,
								IsRead = isRead,
								LabelFontSize = ScaleManager.SizeSet(14)
							});
						}
					}
					int rectHeight = 84 * itemList.Count;
					noticeListViewModel.HooterIsVisible = false;
					int heightAgust = 111;
					// TODO Xamarin.Forms.Device.RuntimePlatform is no longer supported. Use Microsoft.Maui.Devices.DeviceInfo.Platform instead. For more details see https://learn.microsoft.com/en-us/dotnet/maui/migration/forms-projects#device-changes
					if (Device.RuntimePlatform == Device.Android)
					{
						heightAgust = heightAgust - 9;
					}
					if ((ScaleManager.ScreenHeight - heightAgust) < rectHeight)
					{
						noticeListViewModel.ListViewRect = new Rect(0, 46, 1, (ScaleManager.ScreenHeight - heightAgust));
						if (isEnd == false)
						{
							noticeListViewModel.HooterIsVisible = true;
						}
					}
					else
					{
						noticeListViewModel.ListViewRect = new Rect(0, 46, 1, rectHeight);
					}

					noticeListViewModel.HooterWidth = ScaleManager.ScreenWidth;

					Device.BeginInvokeOnMainThread(() =>
					{
						this.ListView.ItemsSource = itemList;
						this.BindingContext = noticeListViewModel;
						App.customNavigationPage.IsRunning = false;
					});
				}

				////単独店舗の場合は以下の処理は不要
				//if (soloSalon)
				//    return;

				//if (Device.RuntimePlatform == Device.iOS)
				//{
				//    Device.BeginInvokeOnMainThread(() =>
				//    {
				//        noticeListViewModel.ToolbarIcon = "Icon_Home.png";
				//    });
				//}

				//noticeListViewModel.ToolbarItemsClick = new Command(async () =>
				//{
				//    if (!App.ProcessManager.CanInvoke())
				//        return;
				//    var page = new StoreSelect(2, null, this);
				//    if (Device.RuntimePlatform == Device.iOS)
				//        App.customNavigationPage.IsBadgeVisble = false;
				//    await App.customNavigationPage.PushAsync(page);
				//    App.ProcessManager.OnComplete();
				//});

			});
		}

		async void FooterBtnClick(object sender, EventArgs e)
		{
			if (App.ProcessManager.CanInvoke())
			{
				int indexNum = int.Parse(parameters["index"]);
				indexNum++;
				parameters["index"] = indexNum.ToString();
				if (salonId != null)
				{
					parameters["salonId"] = salonId.ToString();
				}
				var json = await APIManager.GET("notice_list", parameters);
				if (json == null)
				{
					Device.BeginInvokeOnMainThread(() =>
					{
						this.BindingContext = noticeListViewModel;
						DependencyService.Get<IToast>().Show("通信エラー");
					});
				}
				else
				{
					var response = JsonConvert.DeserializeObject<ResponseNoticeList>(json);
					var regionList = response.Data.List;
					isEnd = response.Data.IsEnd;
					if (regionList != null)
					{
						foreach (var val in regionList)
						{
							//itemList.Add(new ListViewNoticeViewModel(val));
							bool isRead = false;
							if (val.IsRead != null)
							{
								isRead = (bool)val.IsRead;
							}
							itemList.Add(new ListViewNoticeViewModel()
							{
								NoticeId = val.NoticeId,
								LabelText = val.Title,
								Summary = val.Summary,
								Description = val.Description,
								IsRead = isRead,
								LabelFontSize = ScaleManager.SizeSet(14)
							});
						}
						noticeListViewModel.ListViewRect = new Rect(0, 46, 1, 84 * itemList.Count);
					}
					if (isEnd == true)
					{
						noticeListViewModel.HooterIsVisible = false;
					}

					Device.BeginInvokeOnMainThread(() =>
					{
						this.BindingContext = noticeListViewModel;
					});
				}
				App.ProcessManager.OnComplete();
			}
		}
		protected override void OnDisappearing()
		{
			base.OnDisappearing();

			// TODO Xamarin.Forms.Device.RuntimePlatform is no longer supported. Use Microsoft.Maui.Devices.DeviceInfo.Platform instead. For more details see https://learn.microsoft.com/en-us/dotnet/maui/migration/forms-projects#device-changes
			if (Device.RuntimePlatform == Device.iOS && App.customNavigationPage.IsBadgeVisble == true)
				App.customNavigationPage.IsBadgeVisble = false;
		}

	}
}
