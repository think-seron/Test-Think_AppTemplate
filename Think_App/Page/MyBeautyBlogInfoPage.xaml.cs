using System;
using System.Collections.Generic;
using Plugin.Media;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
    public partial class MyBeautyBlogInfoPage : ContentPage
    {

        MyBeautyBlogInfoPageViewModel myBeautyBlogInfoPageViewModel;
        public MyBeautyBlogModel myBeautyBlogModel;

        public MyBeautyBlogInfoPage(MyBeautyBlogModel model)
        {
            InitializeComponent();

            NavigationPage.SetBackButtonTitle(this, "");
            System.Diagnostics.Debug.WriteLine("MyBeautyBlogInfoPage :");
            myBeautyBlogModel = model;
            myBeautyBlogInfoPageViewModel = new MyBeautyBlogInfoPageViewModel(model);
            // ----------------------androidのみで下記使用--------------------------
            ObservableCollection<MyBlogMenuSelectList> items = new ObservableCollection<MyBlogMenuSelectList>();
            items.Add(new MyBlogMenuSelectList("編集する"));
            items.Add(new MyBlogMenuSelectList("削除する"));
            myBeautyBlogInfoPageViewModel.MyBlogPlusListViewIsVisible = false;
            this.MyBlogPlusListView.ItemsSource = items;
            this.MyBlogPlusListView.ItemSelected += async (sender, e) =>
            {
                if (!App.ProcessManager.CanInvoke())
                {
                    return;
                }
                if (((MyBlogMenuSelectList)e.SelectedItem).MyBlogMenuSelectListText == "編集する")
                {
                    myBeautyBlogInfoPageViewModel.MyBlogPlusListViewIsVisible = false;
                    this.MyBlogPlusListView.SelectedItem = null;
                    await App.customNavigationPage.PushAsync(new MyBeautyBlogEditPage(null, myBeautyBlogModel, this));
                }
                else
                {
                    myBeautyBlogInfoPageViewModel.MyBlogPlusListViewIsVisible = false;
                    this.MyBlogPlusListView.SelectedItem = null;
                    ShowModal();
                }
                App.ProcessManager.OnComplete();
            };
            //-----------------------------------------------------------------
            this.BindingContext = myBeautyBlogInfoPageViewModel;
        }

        public async Task UpdateData(MyBeautyBlogModel model)
        {
            myBeautyBlogModel = model;
            myBeautyBlogInfoPageViewModel.UpdateVIewModel(model);
        }

        async void EditClick(object sender, EventArgs e)
        {
            // TODO Xamarin.Forms.Device.RuntimePlatform is no longer supported. Use Microsoft.Maui.Devices.DeviceInfo.Platform instead. For more details see https://learn.microsoft.com/en-us/dotnet/maui/migration/forms-projects#device-changes
            if (Device.RuntimePlatform == Device.iOS)
            {
                var ret = await DisplayActionSheet(null, "キャンセル", null, "編集する", "削除する");
                if (ret == "編集する")
                {
                    if (!App.ProcessManager.CanInvoke())
                    {
                        return;
                    }
                    await App.customNavigationPage.PushAsync(new MyBeautyBlogEditPage(null, myBeautyBlogModel, this));
                }
                else if (ret == "削除する")
                {
                    if (!App.ProcessManager.CanInvoke())
                    {
                        return;
                    }
                    ShowModal();
                }
                App.ProcessManager.OnComplete();
            }
            else if (Device.RuntimePlatform == Device.Android)
            {
                if (myBeautyBlogInfoPageViewModel.MyBlogPlusListViewIsVisible == true)
                {
                    myBeautyBlogInfoPageViewModel.MyBlogPlusListViewIsVisible = false;
                }
                else
                {
                    myBeautyBlogInfoPageViewModel.MyBlogPlusListViewIsVisible = true;
                }
            }
        }

        async void ShowModal()
        {
            // お気に入りスタッフを解除モーダル
            var modalView = new ModalView();
            modalView.modalViewViewModel.ModalLabelTxt = "投稿を削除しますか？";
            modalView.modalViewViewModel.NomalModalLabelRect = new Rect(0.5, 0.4, 1, AbsoluteLayout.AutoSize);
            modalView.modalViewViewModel.SelectBtnLayoutBounds = new Rect(0.9, 0.6, 1, AbsoluteLayout.AutoSize);
            modalView.modalViewViewModel.YesButtonTxt = "削除する";
            modalView.modalViewViewModel.NoButtonTxt = "削除しない";
            modalView.yesButton.Clicked += async (ysender, ye) =>
            {
                if (App.ProcessManager.CanInvoke())
                {
                    // 削除
                    var parameters = new Dictionary<string, string> {
                        { "myBeautyBlogId", myBeautyBlogModel.MyBeautyBlogId.ToString() }
                    };
                    var apiRet = await APIManager.Post("mybeautyblog__remove", parameters);
                    if (apiRet != null)
                    {
                        DependencyService.Get<IToast>().Show("投稿を削除しました");

                        var historyTop = App.customNavigationPage.Navigation.NavigationStack.Where((arg) => arg.Id == App.HistoryTopId);

                        if (historyTop.FirstOrDefault() != null)
                        {
                            System.Diagnostics.Debug.WriteLine(" could  get home and the id  :");
                            var historyTop_page = historyTop.First() as HistoryTop;

                            var historyTop_bc = historyTop_page.BindingContext as HistoryTopViewModel;
                            historyTop_bc.SetBeautyLogItems();
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine(" could not get home and the id  :");
                        }
                        await Task.Delay(250);
                        await App.customNavigationPage.PopAsync();
                    }
                    else
                    {
                        DependencyService.Get<IToast>().Show("通信エラー");
                    }
                    await DialogManager.Instance.HideView();
                    App.ProcessManager.OnComplete();

                }
            };

            modalView.noButton.Clicked += async (nsender, ne) =>
            {
                if (App.ProcessManager.CanInvoke())
                {
                    await DialogManager.Instance.HideView();
                    App.ProcessManager.OnComplete();
                }
            };
            await DialogManager.Instance.ShowDialogView(modalView);
            App.ProcessManager.OnComplete();
        }
    }
}
