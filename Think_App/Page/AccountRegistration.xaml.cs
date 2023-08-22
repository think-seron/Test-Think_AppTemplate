using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using IO.Swagger.Model;
using System.Linq;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
    public partial class AccountRegistration : ContentPage
    {
        AccountRegistrationViewModel accountRegistrationViewModel;

        // pageFlg  1: 新規登録からの遷移  2: アカウント編集画面からの遷移
        //3:予約時にまだ登録していなかった場合 4:予約登録時にアカウントを変更する場合
        int PageFlag;
        ReservationContentInfo ReservationContent;
        public AccountRegistration(int pageFlg, UserData userData = null, ReservationContentInfo reservationContent = null, ResponseQRcodeData responseData = null)
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "");
            PageFlag = pageFlg;
            if (reservationContent != null)
            {
                ReservationContent = reservationContent;
            }
            accountRegistrationViewModel = new AccountRegistrationViewModel(pageFlg);

            //ToolbarItems.Add(new ToolbarItem
            //{
            //    Text = "編集",
            //	Icon="",
            //	Command = new Command(() => { EnableTrue(); })
            //});

            //         GenderRadioBtn.ItemsSource = new[]
            //         {
            //                      "女性",
            //                      "男性",
            //	//"その他"
            //};
            //         GenderRadioBtn.Items[0].WidthRequest = 80;
            //         GenderRadioBtn.Items[1].WidthRequest = 80;
            //         GenderRadioBtn.Items[2].WidthRequest = 100;

            Task.Run(async () =>
            {
                if (userData != null)
                {
                    try
                    {
                        accountRegistrationViewModel.CustomEntryName.EntryText = userData.name;
                        accountRegistrationViewModel.CustomEntryTel.EntryText = userData.tel;
                        accountRegistrationViewModel.CustomEntryMail.EntryText = userData.email;
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            if (userData.gender == "male")
                            {
                                FemaleRadio.IsChecked = true;
                            }
                            else if (userData.gender == "female")
                            {
                                MaleRadio.IsChecked = true;
                            }
                        });
                    }
                    catch
                    {

                    }
                }


                if (responseData != null)
                {
                    accountRegistrationViewModel.CustomEntryName.EntryText = responseData.Data.Name;

                    accountRegistrationViewModel.CustomEntryKana.EntryText = responseData.Data.Kana;

                    accountRegistrationViewModel.CustomEntryTel.EntryText = responseData.Data.Tel;

                    accountRegistrationViewModel.CustomEntryMail.EntryText = responseData.Data.Mail;

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        if ((int)(responseData.Data.Sex) == 1)
                        {
                            FemaleRadio.IsChecked = true;
                        }
                        else if ((int)(responseData.Data.Sex) == 2)
                        {
                            MaleRadio.IsChecked = true;
                        }
                    });
                }

                var str = await FileManager.ReadJsonFileAsync<AccountInfo>("Account", "accountInfo");
                if (str != null)
                {
                    accountRegistrationViewModel.CustomEntryName.EntryText = str.name;
                    accountRegistrationViewModel.CustomEntryKana.EntryText = str.kana;
                    accountRegistrationViewModel.CustomEntryTel.EntryText = str.tel;
                    accountRegistrationViewModel.CustomEntryMail.EntryText = str.email;
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        if (str.gender == 1)
                        {
                            FemaleRadio.IsChecked = true;
                        }
                        else if (str.gender == 2)
                        {
                            MaleRadio.IsChecked = true;
                        }
                        //else if (str.gender == 3)
                        //{
                        //	GenderRadioBtn.Items[2].Checked = true;
                        //}
                    });
                }
                else
                {
                    //性別の未選択状態での登録が許可されたためコメント化
                    //GenderRadioBtn.Items[0].Checked = true;
                }
            });
            string action;
            this.RegBtn.Clicked += async (sender, e) =>
            {
                if (App.ProcessManager.CanInvoke())
                {
                    if (String.IsNullOrEmpty(accountRegistrationViewModel.CustomEntryName.EntryText)
                       || (reservationContent != null &&
                       (String.IsNullOrEmpty(accountRegistrationViewModel.CustomEntryKana.EntryText) ||
                           String.IsNullOrEmpty(accountRegistrationViewModel.CustomEntryTel.EntryText) ||
                           (!FemaleRadio.IsChecked && !MaleRadio.IsChecked)))
                       )
                    {
                        await this.DisplayAlert("エラー", "未入力欄があります。", "OK");
                        App.ProcessManager.OnComplete();
                        return;
                    }


                    App.customNavigationPage.IsRunning = true;
                    int genderFlg = 0;
                    if (FemaleRadio.IsChecked)// 女性
                        genderFlg = 1;
                    else if (MaleRadio.IsChecked)// 男性
                        genderFlg = 2;

                    string error = "";

                    if (!String.IsNullOrEmpty(accountRegistrationViewModel.CustomEntryMail.EntryText))
                    {
                        accountRegistrationViewModel.CustomEntryMail.EntryText = accountRegistrationViewModel.CustomEntryMail.EntryText.Trim();
                    }
                    var awaiterAccountReg = StaticMethod.AccountReg(
                        accountRegistrationViewModel.CustomEntryName.EntryText,
                        accountRegistrationViewModel.CustomEntryKana.EntryText,
                        accountRegistrationViewModel.CustomEntryTel.EntryText,
                        accountRegistrationViewModel.CustomEntryMail.EntryText,
                        genderFlg
                    ).GetAwaiter();

                    awaiterAccountReg.OnCompleted(async () =>
                    {
                        error = awaiterAccountReg.GetResult();
                        if (error != "OK")
                        {
                            await this.DisplayAlert("エラー", error, "OK");
                            App.ProcessManager.OnComplete();
                            App.customNavigationPage.IsRunning = false;
                            return;
                        }

                        // POST処理
                        if (PageFlag == 1)
                        {
                            action = "account__regist";
                        }
                        else
                        {
                            action = "account__update";

                        }

                        var parameters = new Dictionary<string, string> {
                            { "deviceCode", Config.Instance.deviceCode },
                            { "name", accountRegistrationViewModel.CustomEntryName.EntryText?.Trim() },
                            { "phonetic", accountRegistrationViewModel.CustomEntryKana.EntryText?.Trim() },
                            { "tel", accountRegistrationViewModel.CustomEntryTel.EntryText?.Trim()?.Replace("ー","-") },
                            { "sex", genderFlg.ToString() },
                        };

                        if (userData != null && !string.IsNullOrEmpty(userData.line_id))
                        {
                            parameters.Add("lineId", userData.line_id);
                        }
                        else if (userData != null && !string.IsNullOrEmpty(userData.fb_id))
                        {
                            parameters.Add("facebookId", userData.fb_id);
                        }
                        else if (userData != null && !string.IsNullOrEmpty(userData.google_id))
                        {
                            parameters.Add("googleId", userData.google_id);
                        }
                        else if (userData != null && !string.IsNullOrEmpty(userData.apple_id))
                        {
                            parameters.Add("appleId", userData.apple_id);
                        }


                        if (!string.IsNullOrEmpty(accountRegistrationViewModel.CustomEntryMail.EntryText))
                        {
                            parameters.Add("mail", accountRegistrationViewModel.CustomEntryMail.EntryText);
                        }
                        var apiRet = await APIManager.Post(action, parameters);

                        if (apiRet != null)
                        {
                            //scheduleからアカウント登録に飛んだ場合。
                            if (PageFlag == 3)
                            {
                                System.Diagnostics.Debug.WriteLine("PageFlag :" + PageFlag);
                                //予約時に性別が未登録だった場合は顧客登録がSHIPSSではできないため、エラーを返す必要がある。
                                if (genderFlg == 0)
                                {
                                    await DisplayAlert("予約登録時には性別の登録が必要です。", null, "OK");

                                    App.customNavigationPage.IsRunning = false;
                                    App.ProcessManager.OnComplete();
                                    return;
                                }

                                System.Diagnostics.Debug.WriteLine("genderFlg :" + genderFlg);
                                ReservationContent.Account = new AccountInfo()
                                {
                                    name = accountRegistrationViewModel.CustomEntryName.EntryText?.Trim(),
                                    kana = accountRegistrationViewModel.CustomEntryKana.EntryText?.Trim(),
                                    tel = accountRegistrationViewModel.CustomEntryTel.EntryText?.Trim(),

                                };

                                //point取得
                                var param = new Dictionary<string, string> { };
                                param.Add("salonId", ReservationContent.SalonId.ToString());

                                try
                                {
                                    var json = await APIManager.GET("reservation__point", param);
                                    var res = JsonManager.Deserialize<ResponseReservationPoint>(json);
                                    System.Diagnostics.Debug.WriteLine("  json  ;" + json);


                                    var detailPage = new ReservationRegist(ReservationContent, res);

                                    //detailPage.BindingContext = new ReservationDetailViewModel(ReservationContent, res);
                                    Device.BeginInvokeOnMainThread(async () =>
                                    {
                                        await App.customNavigationPage.PushAsync(detailPage);
                                    });
                                }
                                catch (Exception ex)
                                {
                                    error = "NG";
                                    System.Diagnostics.Debug.WriteLine("ex :" + ex);
                                }
                                await Task.Delay(500);
                                App.customNavigationPage.IsRunning = false;
                                App.ProcessManager.OnComplete();
                                return;
                            }
                            //ここまでPageFlag==3

                            //ここからPageFlag
                            var responseJson = JsonConvert.DeserializeObject<ResponseAccount>(apiRet);
                            if (responseJson != null && responseJson.Status == 0)
                            {
                                //error = await StaticMethod.DeviceIDReg(responseJson.Data.DeviceId);
                                var awaiter = StaticMethod.DeviceIDReg(responseJson.Data.DeviceId, responseJson.Data.TransferId).GetAwaiter();
                                awaiter.OnCompleted(async () =>
                                {
                                    error = awaiter.GetResult();
                                    if (error == "OK")
                                    {
                                        Config.Instance.Data.deviceId = responseJson.Data.DeviceId;

                                        await MovePage();

                                    }
                                    else
                                    {
                                        System.Diagnostics.Debug.WriteLine(" get result error");
                                    }
                                });
                            }
                            else
                            {
                                error = "NG";
                            }
                        }
                        else
                        {
                            error = "NG";
                        }

                        if (error == "NG")
                        {
                            await this.DisplayAlert("エラー", "アカウントの登録に失敗しました。再度登録してください。", "OK");
                        }
                        await Task.Delay(500);
                        App.customNavigationPage.IsRunning = false;
                        App.ProcessManager.OnComplete();
                    });
                }

            };
            this.BindingContext = accountRegistrationViewModel;
        }

        //登録店舗が１店舗だった場合はhome画面へ遷移させる。
        async Task MovePage()
        {
            var salonListJson = await APIManager.Post("salon_list", new Dictionary<string, string>());
            if (salonListJson == null)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    DependencyService.Get<IToast>().Show("通信エラー");
                });
            }
            else
            {
                var responseSalonRegionList = JsonConvert.DeserializeObject<ResponseSalonList>(salonListJson);
                var salonListCount = responseSalonRegionList.Data.List.Count;
                System.Diagnostics.Debug.WriteLine("List Count :" + salonListCount);

                //1店舗じゃない場合は選択させる
                if (salonListCount != 1)
                {
                    await App.customNavigationPage.PushAsync(new FavoriteStoreReg());
                }
                //１店舗ならばホームに登録させて遷移
                else
                {
                    string action = "salon_home_regist";
                    var parameters = new Dictionary<string, string> {
                            { "deviceId", Config.Instance.Data.deviceId },
                        { "salonId", responseSalonRegionList.Data.List[0].SalonId.ToString() }
                        };
                    var homeRegistJson = await APIManager.Post(action, parameters);
                    if (!string.IsNullOrEmpty(homeRegistJson))
                    {
                        var json = await APIManager.GET("home");
                        var param = JsonManager.Deserialize<ResponseHome>(json);
                        DeviceTokenManager.PostAndRegistDeviceToken(DeviceTokenInfo.Instance.DeviceToken);
                        await App.customNavigationPage.PushAsync(new Home(param));
                    }
                    else
                    {
                        await this.DisplayAlert("エラー", "登録に失敗しました。再度登録してください。", "OK");
                        return;
                    }

                }

            }
        }


        async void ToolbarItemsClick(object sender, EventArgs e)
        {
            if (this.ToolbarItems.Text == "編集")
            {
                accountRegistrationViewModel.CustomEntryName.EntryIsEnabled = true;
                accountRegistrationViewModel.CustomEntryKana.EntryIsEnabled = true;
                accountRegistrationViewModel.CustomEntryTel.EntryIsEnabled = true;
                accountRegistrationViewModel.CustomEntryMail.EntryIsEnabled = true;
                accountRegistrationViewModel.RadioGroupIsVisible = true;
                this.ToolbarItems.Text = "完了";
                accountRegistrationViewModel.RadioGroupBoxViewRect = new Rect(0, 0, 0, 0);
            }
            else if (this.ToolbarItems.Text == "完了")
            {

                if (String.IsNullOrEmpty(accountRegistrationViewModel.CustomEntryName.EntryText) 
                    || (ReservationContent != null &&
                       (String.IsNullOrEmpty(accountRegistrationViewModel.CustomEntryKana.EntryText) ||
                           String.IsNullOrEmpty(accountRegistrationViewModel.CustomEntryTel.EntryText) ||
                           (!FemaleRadio.IsChecked && !MaleRadio.IsChecked)))
                   )
                {
                    await this.DisplayAlert("エラー", "未入力欄があります。", "OK");
                    App.ProcessManager.OnComplete();
                    return;
                }

                int genderFlg = 0;
                if (FemaleRadio.IsChecked)// 女性
                    genderFlg = 1;
                else if (MaleRadio.IsChecked)// 男性
                    genderFlg = 2;

                if (!String.IsNullOrEmpty(accountRegistrationViewModel.CustomEntryMail.EntryText))
                {
                    accountRegistrationViewModel.CustomEntryMail.EntryText = accountRegistrationViewModel.CustomEntryMail.EntryText.Trim();
                }
                string ret = await StaticMethod.AccountReg(
                    accountRegistrationViewModel.CustomEntryName.EntryText,
                    accountRegistrationViewModel.CustomEntryKana.EntryText,
                    accountRegistrationViewModel.CustomEntryTel.EntryText,
                    accountRegistrationViewModel.CustomEntryMail.EntryText,
                    genderFlg
                );
                if (ret == "OK")
                {
                    // POST処理
                    string action = "account__update";
                    var parameters = new Dictionary<string, string> {
                        { "deviceId", Config.Instance.Data.deviceId },
                        { "name", accountRegistrationViewModel.CustomEntryName.EntryText.Trim() },
                        { "phonetic", accountRegistrationViewModel.CustomEntryKana.EntryText?.Trim() },
                        { "tel", accountRegistrationViewModel.CustomEntryTel.EntryText?.Trim()?.Replace("ー","-") },
                        { "sex", genderFlg.ToString() }
                    };
                    if (!string.IsNullOrEmpty(accountRegistrationViewModel.CustomEntryMail.EntryText))
                    {
                        parameters.Add("mail", accountRegistrationViewModel.CustomEntryMail.EntryText);
                    }
                    var apiRet = await APIManager.Post(action, parameters);

                    if (apiRet != null)
                    {
                        var responseJson = JsonConvert.DeserializeObject<ResponseBase>(apiRet);
                        if (responseJson != null)
                        {
                            accountRegistrationViewModel.CustomEntryName.EntryIsEnabled = false;
                            accountRegistrationViewModel.CustomEntryKana.EntryIsEnabled = false;
                            accountRegistrationViewModel.CustomEntryTel.EntryIsEnabled = false;
                            accountRegistrationViewModel.CustomEntryMail.EntryIsEnabled = false;
                            accountRegistrationViewModel.RadioGroupIsVisible = false;
                            this.ToolbarItems.Text = "編集";
                            accountRegistrationViewModel.RadioGroupBoxViewRect = new Rect(0, 0, 1, 1);


                            if (PageFlag == 4)
                            {
                                var home = App.customNavigationPage.Navigation.NavigationStack.Where((arg) => arg.Id == App.ReservationDetailId);
                                if (home.FirstOrDefault() != null)
                                {
                                    System.Diagnostics.Debug.WriteLine(" could  get home and the id  :");
                                    var home_page = home.First() as ReservationRegist;

                                    var home_bc = home_page.BindingContext as ReservationRegistViewModel;
                                    home_bc.UpdateUserSettings(
                                    accountRegistrationViewModel.CustomEntryName.EntryText?.Trim(),
                                    accountRegistrationViewModel.CustomEntryKana.EntryText?.Trim(),
                                    accountRegistrationViewModel.CustomEntryTel.EntryText?.Trim().Replace("ー", "-")
                                    );
                                }


                                await App.customNavigationPage.PopAsync();
                            }
                        }
                        else
                        {
                            await this.DisplayAlert("エラー", "アカウントの登録に失敗しました。再度登録してください。", "OK");
                        }
                    }
                    else
                    {
                        await this.DisplayAlert("エラー", "アカウントの登録に失敗しました。再度登録してください。", "OK");
                    }
                }
                else
                {
                    await this.DisplayAlert("エラー", "アカウントの登録に失敗しました。再度登録してください。", "OK");
                }
            }
        }

        //private void AnsPickerCheckedChanged(object sender, int e)
        //{
        //    var radio = sender as CustomRadioButton;
        //    if (radio == null || radio.Id == -1)
        //    {
        //        return;
        //    }
        //}

        /*
        async void OnLabelClicked(object sender, EventArgs e)
        {
            if (App.ProcessManager.CanInvoke())
            {
                // POST処理
                string action = "account_guest_regist";
                var parameters = new Dictionary<string, string> {
                    { "deviceCode", Config.Instance.deviceCode }
                };
                var apiRet = await APIManager.Post(action, parameters);

                if (apiRet != null)
                {
                    var responseJson = JsonConvert.DeserializeObject<ResponseAccount>(apiRet);
                    if (responseJson != null)
                    {
                        await Navigation.PushAsync(new FavoriteStoreReg());
                    }
                    else
                    {
                        await this.DisplayAlert("エラー", "通信に失敗しました。再度やり直してください。", "OK");
                    }
                }
                App.ProcessManager.OnComplete();
            }
        }
        */
    }

}
