using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
    public partial class AppStart : ContentPage
    {
        public AppStart()
        {
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);
            NavigationPage.SetBackButtonTitle(this, "");
        }
        protected override bool OnBackButtonPressed()
        {
            return true;
        }
        async void OnLabelClicked(object sender, EventArgs e)
        {
            if (App.ProcessManager.CanInvoke())
            {
                await App.customNavigationPage.PushAsync(new LoginMethodSelect());
                App.ProcessManager.OnComplete();

            }
        }
    }
}
