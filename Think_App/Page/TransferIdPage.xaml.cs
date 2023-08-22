using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
    public partial class TransferIdPage : ContentPage
    {
        TransferIdPageViewModel transferIdPageViewModel;

        public TransferIdPage()
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "");

            transferIdPageViewModel = new TransferIdPageViewModel();
            Task.Run(async () =>
            {
                var deviceInfo = await FileManager.ReadJsonFileAsync<DeviceInfo>("Account", "deviceInfo");
                if (deviceInfo != null)
                {
                    transferIdPageViewModel.transferId = deviceInfo.TransferID;
                }
                Device.BeginInvokeOnMainThread(() =>
                {
                    this.BindingContext = transferIdPageViewModel;
                });
            });

            this.CodeCopyBtn.Clicked += (sender, e) =>
            {
                DependencyService.Get<IClipboardService>().CopyToClipboard(transferIdPageViewModel.transferId);
                DependencyService.Get<IToast>().Show("コピーしました");
            };
        }
    }
}
