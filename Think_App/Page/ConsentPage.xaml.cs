using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IO.Swagger.Model;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
    public partial class ConsentPage : ContentPage
    {
        ConsentPageViewModel consentPageViewModel;
        bool GotHtml;
        public ConsentPage()
        {
            InitializeComponent();

            NavigationPage.SetBackButtonTitle(this, "");
            App.customNavigationPage.IsRunning = true;

            consentPageViewModel = new ConsentPageViewModel();

            this.BindingContext = consentPageViewModel;

            SetWebViewSource();

            this.CheckBox.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName != CheckBoxView.IsCheckedProperty.PropertyName) return;
                if (this.CheckBox.IsChecked && GotHtml)
                    consentPageViewModel.ButtonEnable = true;
                else
                    consentPageViewModel.ButtonEnable = false;
            };
        }

        async void SetWebViewSource()
        {
            GotHtml = await Task.Run(async () =>
            {
                var resp = await APIManager.GET("html_text");
                if (resp != null)
                {
                    var deserializeJson = JsonManager.Deserialize<ResponseHtmlText>(resp);
                    if (deserializeJson != null && deserializeJson.Data != null && deserializeJson.Data.PrivacyPolicy != null && deserializeJson.Data.TermsOfService != null)
                    {
                        string str = deserializeJson.Data.PrivacyPolicy + deserializeJson.Data.TermsOfService;
                        var source = new HtmlWebViewSource
                        {
                            Html = str
                        };
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            consentPageViewModel.Source = source;
                            App.customNavigationPage.IsRunning = false;
                            CheckBox.IsEnabled = true;
                        });
                        return true;
                    }
                    else
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            DependencyService.Get<IToast>().Show("通信エラー");
                            App.customNavigationPage.IsRunning = false;
                        });
                        return false;
                    }
                }
                Device.BeginInvokeOnMainThread(() => App.customNavigationPage.IsRunning = false);
                return false;
            });
        }
    }
}
