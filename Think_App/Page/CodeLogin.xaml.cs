using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using IO.Swagger.Model;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
    public partial class CodeLogin : ContentPage
    {
        CodeLoginViewModel codeLoginViewModel;
        public CodeLogin()
        {
            InitializeComponent();

            NavigationPage.SetBackButtonTitle(this, "");

            codeLoginViewModel = new CodeLoginViewModel();
            this.BindingContext = codeLoginViewModel;

            this.LoginBtn.Clicked += async (sender, e) =>
            {
                if (App.ProcessManager.CanInvoke())
                {
                    App.customNavigationPage.IsRunning = true;
                    // 引き継ぎコードを確認してログインする処理必要
                    Dictionary<string, string> parm = new Dictionary<string, string>()
                    {
                        {"transferId", codeLoginViewModel.CustomEntryCode.EntryText}
                    };
                    var respJson = await APIManager.GET("transferId_login", parm);
                    if (respJson == null)
                    {
                        await DisplayAlert("エラー", "ログインできませんでした。", "OK");
                        App.customNavigationPage.IsRunning = false;
                        App.ProcessManager.OnComplete();
                        return;
                    }
                    var deserializeJson = JsonManager.Deserialize<ResponseLogin>(respJson);
                    string ret = await StaticMethod.AccountReg(
                        deserializeJson.Data.Name,
                        deserializeJson.Data.Phonetic,
                        deserializeJson.Data.Tel,
                        deserializeJson.Data.Mail,
                        (int)deserializeJson.Data.Sex
                    );

                    //ゲストアカウントでも引き継ぎを有効にする
                    //フリガナなどがなくても登録できるようにコメント化
                    //if (ret != "OK")
                    //{
                    //	DependencyService.Get<IToast>().Show("エラー");
                    //	App.customNavigationPage.IsRunning = false;
                    //	App.ProcessManager.OnComplete();
                    //	return;
                    //}

                    ret = await StaticMethod.DeviceIDReg(deserializeJson.Data.DeviceId, codeLoginViewModel.CustomEntryCode.EntryText);
                    if (ret != "OK")
                    {
                        DependencyService.Get<IToast>().Show("アカウントの保存に失敗しました。");
                        App.customNavigationPage.IsRunning = false;
                        App.ProcessManager.OnComplete();
                        return;
                    }

                    DependencyService.Get<IToast>().Show("ログイン完了");

                    DeviceTokenManager.PostAndRegistDeviceToken(DeviceTokenInfo.Instance.DeviceToken);

                    var json = await APIManager.GET("home");

                    var param = JsonManager.Deserialize<ResponseHome>(json);

                    System.Diagnostics.Debug.WriteLine("Home:" + json);

                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await App.customNavigationPage.PushAsync(new Home(param), false);
                    });
                    App.customNavigationPage.IsRunning = false;
                    App.ProcessManager.OnComplete();
                }
            };
        }
    }
}
