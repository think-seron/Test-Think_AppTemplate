using System;
using System.Collections.Generic;
using IO.Swagger.Model;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace Think_App
{
	public partial class HairCatalogInfoPage : ContentPage
	{
		public event EventHandler<ImageSource> HairSelected = delegate { };

		HairCatalogInfoPageViewModel hairCatalogInfoPageViewModel;

		// lastPageFlgについて(仮)
		// 1: 予約画面からの遷移
		// 2: 店舗情報からの遷移
		// 3: メッセージからの遷移
		public HairCatalogInfoPage(int lastPageFlg, HairStyleInfo hairStyleInfo)
		{
			InitializeComponent();
			NavigationPage.SetBackButtonTitle(this, "");

			// ---------------------------------------------
			// 選択したヘアカタログの情報を取得し表示
			var isStaffNameVisible = (lastPageFlg == 3);
			hairCatalogInfoPageViewModel = new HairCatalogInfoPageViewModel(isStaffNameVisible);
			hairCatalogInfoPageViewModel.StaffNameLblTxt = "ヘアスタイル担当：" + hairStyleInfo.StaffName;
			hairCatalogInfoPageViewModel.LabelTxt = hairStyleInfo.Description;
			hairCatalogInfoPageViewModel.Souce = hairStyleInfo.Souce;
			// ---------------------------------------------
			hairCatalogInfoPageViewModel.ImageWidth = ScaleManager.ScreenWidth;
			hairCatalogInfoPageViewModel.ImageHeight = hairCatalogInfoPageViewModel.ImageWidth;

			if (lastPageFlg == 2 || lastPageFlg == 3)
			{
				hairCatalogInfoPageViewModel.BtnTxt = "この髪型をメッセージで送る";
			}
			this.BindingContext = hairCatalogInfoPageViewModel;
			App.customNavigationPage.IsRunning = false;

			this.MessageSendBtn.Clicked += async (sender, e) =>
			{
				if (lastPageFlg == 3)
				{
					// メッセージへ投稿するため、投稿するイメージソースを通知するとともに、このページのポップを要求する。
					if (HairSelected != null)
					{
						HairSelected(this, hairStyleInfo.Souce);
					}
				}
				else if (App.ProcessManager.CanInvoke())
				{
					App.customNavigationPage.IsRunning = true;
					var json = await APIManager.GET("home");
					try
					{
						var param = JsonManager.Deserialize<ResponseHome>(json);
						//var souce = ImageSource.FromUri(new Uri(hairStyleInfo.Souce));
						// ホームへ遷移。
						await this.Navigation.PushAsync(new Home(param)
						{
							ForceGoMessagePage = true,
							SalonInfoForMessage = new Home.SalonInfo(hairStyleInfo.StoreId, hairStyleInfo.StoreName),
							//ImageForMessage = souce
							ImageForMessage = hairStyleInfo.Souce
						}, false);
						// プロセス終了
						App.ProcessManager.OnComplete();
						App.customNavigationPage.IsRunning = false;
					}
					catch
					{
						DependencyService.Get<IToast>().Show("通信エラー");
						// プロセス終了
						App.ProcessManager.OnComplete();
						App.customNavigationPage.IsRunning = false;

					}
				}
			};
		}
	}
}
