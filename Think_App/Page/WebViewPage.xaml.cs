using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Newtonsoft.Json;
using IO.Swagger.Model;

namespace Think_App
{
	public partial class WebViewPage : ContentPage
	{
		WebViewPageViewModel webViewPageViewModel;

		public enum webViewType
		{
			PrivacyPolicy,
			TermsOfService,
			License
		}

		public WebViewPage(webViewType type)
		{
			InitializeComponent();
			NavigationPage.SetBackButtonTitle(this, "");
			webViewPageViewModel = new WebViewPageViewModel();
			App.customNavigationPage.IsRunning = true;

			Task.Run(async () =>
			{
				var resp = await APIManager.GET("html_text");
				if (resp != null)
				{
					var deserializeJson = JsonManager.Deserialize<ResponseHtmlText>(resp);
					if (deserializeJson == null || deserializeJson.Data == null)
					{
						Device.BeginInvokeOnMainThread(() =>
						{
							DependencyService.Get<IToast>().Show("通信エラー");
						});
					}
					else
					{

						if (type == webViewType.PrivacyPolicy && deserializeJson.Data.PrivacyPolicy != null)
						{
							var source = new HtmlWebViewSource
							{
								Html = deserializeJson.Data.PrivacyPolicy
							};
							webViewPageViewModel.Source = source;
						}
						else if (type == webViewType.TermsOfService && deserializeJson.Data.TermsOfService != null)
						{
							var source = new HtmlWebViewSource
							{
								Html = deserializeJson.Data.TermsOfService
							};
							webViewPageViewModel.Source = source;
						}
						else if (type == webViewType.License && deserializeJson.Data.License != null)
                        {
							var source = new HtmlWebViewSource
							{
								Html = deserializeJson.Data.License
							};
							webViewPageViewModel.Source = source;
						}
						else
						{
							Device.BeginInvokeOnMainThread(() =>
							{
								DependencyService.Get<IToast>().Show("通信エラー");
							});
						}
					}
				}
				Device.BeginInvokeOnMainThread(() =>
				{
					App.customNavigationPage.IsRunning = false;
					this.BindingContext = webViewPageViewModel;
				});
			});
		}
	}
}
