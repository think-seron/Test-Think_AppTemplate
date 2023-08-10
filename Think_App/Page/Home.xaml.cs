using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Xamarin.Forms;
using IO.Swagger.Model;
using Think_App;
namespace Think_App
{
	public partial class Home : CustomHomePage
	{
		public HomeViewModel homeViewModel;
		//public Guid PageId;
		double gridHeight, gridWidth, itemsize;
		bool OnTimer;

		public class SalonInfo
		{
			public int SalonId { get; private set; }
			public string SalonName { get; private set; }
			public SalonInfo(int salonId, string salonName)
			{
				SalonId = salonId;
				SalonName = salonName;
			}
		}

		public bool ForceGoMessagePage { private get; set; }
		public SalonInfo SalonInfoForMessage { private get; set; }
		public ImageSource ImageForMessage { private get; set; }

		public Home(ResponseHome response)
		{
			if (Device.RuntimePlatform == Device.Android)
			{
				GASCall.Track_App_Page("Android_Home");
			}
			else
			{
				GASCall.Track_App_Page("iOS_Home");
			}

			InitializeComponent();
			NavigationPage.SetBackButtonTitle(this, "");
			homeViewModel = new HomeViewModel(response);

			App.HomeId = Id;
			this.BindingContext = homeViewModel;

			this.SizeChanged += (sender, e) =>
			{
				homeViewModel.ThumnNailSize = this.HomeInfoContainer.Height - 11.0 * 2.0;
			};

			BtnGrid.HeightRequest = (ScaleManager.ScreenHeight - homeViewModel.SliderHeight - 65.0) * 0.318;
			OnTimer = true;
			this.Appearing += (sender, e) =>
			{
				if (homeViewModel.CarouselItem != null || homeViewModel.CarouselItem.Count > 0)
				{
					Device.StartTimer(TimeSpan.FromSeconds(8.0), () =>
					{

                    //System.Diagnostics.Debug.WriteLine("time_ct :"+)
                    if (this.Carousel.SelectedIndex == homeViewModel.CarouselItem.Count - 1)
						{
							this.Carousel.SelectedIndex = 0;
						}
						else
						{
							try
							{
								this.Carousel.SelectedIndex++;
							}
							catch
							{

							}
						}
						return OnTimer;
					});
				}

				if (ForceGoMessagePage)
				{
					System.Diagnostics.Debug.WriteLine("メッセージページを開いて画像を投稿します。");
					int? salonId = null;
					string salonName = null;
					if (SalonInfoForMessage != null)
					{
						System.Diagnostics.Debug.WriteLine("指定店舗に投稿します。");
						salonId = SalonInfoForMessage.SalonId;
						salonName = SalonInfoForMessage.SalonName;
					}
					else
					{
						// 指定がない場合はホーム店舗
						System.Diagnostics.Debug.WriteLine("ホーム店舗に投稿します。");
						salonId = homeViewModel.HomeResponseData.Data.HomeSalonInfo.SalonId;
						salonName = homeViewModel.HomeResponseData.Data.HomeSalonInfo.Name;
					}
					System.Diagnostics.Debug.WriteLine("SalonId:{0}, SalonName:{1}", salonId, salonName);
					bool soloSalon = homeViewModel.HomeResponseData.Data.SalonCount > 1 ? false : true;
					this.Navigation.PushAsync(new MessageMainPage(salonId,
																  salonName,
																  ImageForMessage, soloSalon, homeViewModel.GetAvailableImageStatus()), false);
					// 情報のクリア
					ForceGoMessagePage = false;
					SalonInfoForMessage = null;
					ImageForMessage = null;
				}
			};


			this.Disappearing += (sender, e) =>
			{
				OnTimer = false;
				homeViewModel.ConfigVisible = false;
			};
			//pushedが遷移仕切った場合ならばここでremove処理が可能？
			//App.customNavigationPage.Pushed += (sender, e) => {

			//};

			//appearingでstackのremoveを行おうとした場合、navigationの処理順の制約でerrorがでてしまう。
			//if (App.customNavigationPage.Navigation.NavigationStack.Count > 0)
			//	foreach (var n in App.customNavigationPage.Navigation.NavigationStack)
			//	{
			//		if (n.Id != this.Id)
			//		{
			//			System.Diagnostics.Debug.WriteLine("  page title   " + n.Title);
			//			System.Diagnostics.Debug.WriteLine("  page id   " + n.Id);
			//			App.customNavigationPage.Navigation.RemovePage(n);
			//		}
			//	}

		}
		protected override bool OnBackButtonPressed()
		{
			return true;
		}
	}
}