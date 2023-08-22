using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using IO.Swagger.Model;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
    public partial class OpeningSetting : ContentPage
    {
        OpeningSettingViewModel openingSettingViewModel;

        public OpeningSetting(string json)
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "");

            openingSettingViewModel = new OpeningSettingViewModel();

            // ----------------------------------------------------
            // 既存の設定もしくはデフォルトの設定情報を取得し表示に反映
            var resp = JsonConvert.DeserializeObject<ResponseSettingPublish>(json);
            if (resp.Data.Myblog == true)
            {
                openingSettingViewModel.CustomSwitchCellBlog.SwitchIsToggled = true;
            }
            else
            {
                // デフォルト
                openingSettingViewModel.CustomSwitchCellBlog.SwitchIsToggled = false;
            }
            // ----------------------------------------------------
            this.BindingContext = openingSettingViewModel;
            App.customNavigationPage.IsRunning = false;

            this.BlogSwitchCell.nSwitch.Toggled += async (sender, e) =>
            {
                if (App.ProcessManager.CanInvoke())
                {
                    Dictionary<string, string> param = new Dictionary<string, string> { { "deviceId", Config.Instance.Data.deviceId } };
                    if (e.Value)
                    {
                        param.Add("myblog", "1");
                    }
                    else
                    {
                        param.Add("myblog", "0");
                    }
                    var respJson = await APIManager.Post("setting_publish_regist", param);
                    if (respJson == null)
                    {
                        // 通信失敗時
                        if (e.Value)
                        {
                            openingSettingViewModel.CustomSwitchCellBlog.SwitchIsToggled = false;
                        }
                        else
                        {
                            openingSettingViewModel.CustomSwitchCellBlog.SwitchIsToggled = true;
                        }
                        DependencyService.Get<IToast>().Show("通信エラー");
                    }
                    App.ProcessManager.OnComplete();
                }
            };
        }
    }
}
