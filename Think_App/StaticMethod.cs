using System;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Newtonsoft.Json;
using IO.Swagger.Model;

namespace Think_App
{
    public class StaticMethod
    {
        public StaticMethod()
        {
        }

        public static async Task<string> AccountReg(string nameTxt, string kanaTxt, string telTxt, string mailTxt, int genderFlg)
        {
            nameTxt = nameTxt.Trim();
            if (!string.IsNullOrEmpty(kanaTxt))
            {
                kanaTxt = kanaTxt.Trim();
                var kana = Regex.Replace(kanaTxt, @"\s", "");
                if (!Regex.IsMatch(kana, @"^[\p{IsKatakana}\u31F0-\u31FF\u3099-\u309C\uFF65-\uFF9F]+$"))
                {
                    return "フリガナを正しく入力してください";
                }
            }
            if (!string.IsNullOrEmpty(telTxt))
            {
                telTxt = telTxt.Trim();
                telTxt = telTxt.Replace("ー", "-");
                if (!Regex.IsMatch(telTxt, @"^[0-9]+[0-9-]+[0-9]$"))
                {
                    return "電話番号を正しく入力してください";
                }
            }

            if (!String.IsNullOrEmpty(mailTxt))
            {
                //mailTxt = mailTxt.Trim();
                //if (!Regex.IsMatch(mailTxt, @"\A[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}\z", RegexOptions.IgnoreCase))
                //{
                //	return "メールアドレスを正しく入力してください";
                //}
            }

            //if (genderFlg != 1 && genderFlg != 2)
            //{
            //	return "性別を選択してください";
            //}


            AccountInfo acntInfo = new AccountInfo()
            {
                name = nameTxt,
                kana = kanaTxt,
                tel = telTxt,
                email = mailTxt,
                gender = genderFlg
            };
            bool ret = await FileManager.WriteJsonFileAsync("Account", "accountInfo", acntInfo);
            if (ret)
            {
                return "OK";
            }

            return "NG";
        }


        // サロンのお気に入りの状態を処理
        async public static void SalonFavoriteChange(object sender, int id)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string> { { "deviceId", Config.Instance.Data.deviceId } };
            //if (souce.File == "BigFavoIconOn.png")
            if ((FileImageSource)((Xamarin.Forms.Image)sender).Source == "BigFavoIconOn.png")
            {
                var jsonSalonFavo = await APIManager.GET("salon_favoritelist");
                var responseSalonFavo = JsonConvert.DeserializeObject<ResponseFavoriteSalonList>(jsonSalonFavo);
                var favoSalonCount = responseSalonFavo.Data.List.Count;
                // お気に入り店舗が1つ以上あるかどうか
                if (favoSalonCount <= 1)
                {
                    var modalView = new ModalView();
                    modalView.modalViewViewModel.ModalLabelTxt = "お気に入り店舗は" + Environment.NewLine + "必ず１つ以上選択してください。";
                    modalView.modalViewViewModel.NomalModalLabelRect = new Rectangle(0.5, 0.4, 1, AbsoluteLayout.AutoSize);
                    modalView.modalViewViewModel.OKBtnLayoutBounds = new Rectangle(0.5, 0.6, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize);
                    var currentApp = Xamarin.Forms.Application.Current;
                    modalView.okButton.Clicked += async (okSender, okE) =>
                    {
                        await DialogManager.Instance.HideView();
                        if (currentApp != Xamarin.Forms.Application.Current)
                        {
                            throw new InvalidOperationException("Application.Current changed");
                        }
                    };
                    await DialogManager.Instance.ShowDialogView(modalView);
                    App.ProcessManager.OnComplete();
                    return;
                }

                var json = await APIManager.GET("home", parameters);
                if (json != null)
                {
                    // お気に入り登録解除しようとした店舗がホーム店舗かどうかを確認
                    string action = "salon_favorite_regist";
                    parameters.Add("salonId", id.ToString());
                    parameters.Add("isFavorited", "false");
                    var response = JsonConvert.DeserializeObject<ResponseHome>(json);
                    var homeSalonInfo = response.Data.HomeSalonInfo;
                    if (id == homeSalonInfo.SalonId)
                    {
                        var modalView = new ModalView();
                        modalView.modalViewViewModel.ModalLabelTxt = "こちらの店舗は" + Environment.NewLine + "ホーム店舗に登録されています。" + Environment.NewLine + "お気に入りを解除しますか？";
                        modalView.modalViewViewModel.NomalModalLabelRect = new Rectangle(0.5, 0.4, 1, AbsoluteLayout.AutoSize);
                        modalView.modalViewViewModel.SelectBtnLayoutBounds = new Rectangle(0.9, 0.6, 1, AbsoluteLayout.AutoSize);
                        modalView.modalViewViewModel.YesButtonTxt = "はい";
                        modalView.modalViewViewModel.NoButtonTxt = "いいえ";

                        //var currentApp = Xamarin.Forms.Application.Current;
                        modalView.yesButton.Clicked += async (yesSender, yesE) =>
                        {
                            if (App.ProcessManager.CanInvoke())
                            {
                                var ret = await APIManager.Post(action, parameters);
                                if (ret != null)
                                {
                                    ((Xamarin.Forms.Image)sender).Source = "BigFavoIconOff.png";
                                    DependencyService.Get<IToast>().Show("お気に入り店舗から削除しました");
                                    //ホーム店舗解除
                                    //parameters = new Dictionary<string, string> { { "deviceId", Config.Instance.Data.deviceId } };
                                    //parameters.Add("salonId", id.ToString());
                                    //var resp = await APIManager.Post("salon_home_regist", parameters);
                                    //ホーム店舗登録
                                    //if (resp != null)
                                    //{
                                    int? newHomeSalonId = null;
                                    foreach (var val in responseSalonFavo.Data.List)
                                    {
                                        if (val.SalonId.ToString() == id.ToString())
                                        {
                                            continue;
                                        }
                                        newHomeSalonId = val.SalonId;
                                        parameters["salonId"] = newHomeSalonId.ToString();
                                        var resp = await APIManager.Post("salon_home_regist", parameters);
                                        if (resp == null)
                                        {
                                            DependencyService.Get<IToast>().Show("通信エラー");
                                        }

                                        var res = JsonManager.Deserialize<ResponseHome>(resp);

                                        var home = App.customNavigationPage.Navigation.NavigationStack.Where((arg) => arg.Id == App.HomeId);
                                        if (home.FirstOrDefault() != null)
                                        {
                                            System.Diagnostics.Debug.WriteLine(" could  get home and the id  :");
                                            var home_page = home.First() as Home;

                                            var home_bc = home_page.BindingContext as HomeViewModel;
                                            home_bc.UpdateHomeSalon(res);
                                        }
                                        else
                                        {
                                            System.Diagnostics.Debug.WriteLine(" could not get home and the id  :");
                                        }


                                        break;
                                    }

                                    //}
                                    //else
                                    //{
                                    //	DependencyService.Get<IToast>().Show("通信エラー");
                                    //}
                                }
                                else
                                {
                                    DependencyService.Get<IToast>().Show("通信エラー");
                                }
                                await DialogManager.Instance.HideView();
                                //if (currentApp != Xamarin.Forms.Application.Current)
                                //{
                                //	throw new InvalidOperationException("Application.Current changed");
                                //}
                                App.ProcessManager.OnComplete();
                            }
                        };
                        modalView.noButton.Clicked += async (noSender, noE) =>
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
                    else
                    {
                        var resp = await APIManager.Post(action, parameters);
                        if (resp != null)
                        {
                            ((Xamarin.Forms.Image)sender).Source = "BigFavoIconOff.png";
                            DependencyService.Get<IToast>().Show("お気に入り店舗から削除しました");
                        }
                        else
                        {
                            DependencyService.Get<IToast>().Show("通信エラー");
                        }
                        App.ProcessManager.OnComplete();
                    }
                }
                else
                {
                    DependencyService.Get<IToast>().Show("通信エラー");
                    App.ProcessManager.OnComplete();
                }
            }
            else
            {
                string action = "salon_favorite_regist";
                parameters.Add("salonId", id.ToString());
                parameters.Add("isFavorited", "true");
                var resp = await APIManager.Post(action, parameters);
                if (resp != null)
                {
                    ((Xamarin.Forms.Image)sender).Source = "BigFavoIconOn.png";
                    DependencyService.Get<IToast>().Show("お気に入り店舗に登録しました");
                }
                else
                {
                    DependencyService.Get<IToast>().Show("通信エラー");
                }
                App.ProcessManager.OnComplete();
            }
        }

        // スタッフのお気に入りの状態を処理
        async public static void StaffFavoriteChange(object sender, FileImageSource souce, Dictionary<string, string> parameters, int salonID, int staffID, System.Collections.IEnumerable itemSouce = null)
        {
            // すでにお気に入りスタッフを登録しているかどうかを調べる
            Dictionary<string, string> staffListparam = new Dictionary<string, string> { { "salonId", salonID.ToString() } };
            var json = await APIManager.GET("staff_list", staffListparam);
            int? favoritedStaffId = null;
            if (json != null)
            {
                var respStaffList = JsonConvert.DeserializeObject<ResponseStaffList>(json);
                foreach (var val in respStaffList.Data.List)
                {
                    if (val.IsFavorite == true)
                    {
                        favoritedStaffId = val.StaffId;
                    }
                }

                string action = "staff_favorite_regist";
                if (souce.File == "BigFavoIconOn.png")
                {
                    parameters.Add("isFavorited", "false");
                    parameters.Add("staffId", staffID.ToString());

                    // お気に入りスタッフを解除モーダル
                    var modalView = new ModalView();
                    modalView.modalViewViewModel.ModalLabelTxt = "お気に入りスタッフを" + Environment.NewLine + "解除しますか？";
                    modalView.modalViewViewModel.NomalModalLabelRect = new Rectangle(0.5, 0.4, 1, AbsoluteLayout.AutoSize);
                    modalView.modalViewViewModel.SelectBtnLayoutBounds = new Rectangle(0.9, 0.6, 1, AbsoluteLayout.AutoSize);
                    modalView.modalViewViewModel.YesButtonTxt = "はい";
                    modalView.modalViewViewModel.NoButtonTxt = "いいえ";

                    modalView.yesButton.Clicked += async (ysender, ye) =>
                    {
                        if (App.ProcessManager.CanInvoke())
                        {
                            var ret = await APIManager.Post(action, parameters);

                            if (ret != null)
                            {
                                if (itemSouce != null)
                                {
                                    ((ListViewStaffStoreViewModel)((Xamarin.Forms.Image)sender).BindingContext).BdContext.FavoIconSouce = "BigFavoIconOff.png";
                                    ((ListViewStaffStoreViewModel)((Xamarin.Forms.Image)sender).BindingContext).BdContext.IsFavorite = false;
                                }
                                else
                                {
                                    ((Xamarin.Forms.Image)sender).Source = "BigFavoIconOff.png";
                                }
                                DependencyService.Get<IToast>().Show("お気に入りスタッフから削除しました");

                                await UpdateHome();
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
                else
                {
                    if (favoritedStaffId != null)
                    {
                        // すでにお気に入りスタッフがいる場合
                        parameters.Add("isFavorited", "false");
                        parameters.Add("staffId", favoritedStaffId.ToString());

                        //お気に入りスタッフ変更モーダル
                        var modalView = new ModalView();
                        modalView.modalViewViewModel.ModalLabelTxt = "お気に入りスタッフを" + Environment.NewLine + "変更しますか？";
                        modalView.modalViewViewModel.NomalModalLabelRect = new Rectangle(0.5, 0.4, 1, AbsoluteLayout.AutoSize);
                        modalView.modalViewViewModel.SelectBtnLayoutBounds = new Rectangle(0.9, 0.6, 1, AbsoluteLayout.AutoSize);
                        modalView.modalViewViewModel.YesButtonTxt = "はい";
                        modalView.modalViewViewModel.NoButtonTxt = "いいえ";

                        modalView.yesButton.Clicked += async (ysender, ye) =>
                        {
                            if (App.ProcessManager.CanInvoke())
                            {
                                var result = await APIManager.Post(action, parameters);
                                if (result != null)
                                {
                                    if (itemSouce != null)
                                    {
                                        foreach (ListViewStaffStoreViewModel val in itemSouce)
                                        {
                                            if (staffID == val.StaffID)
                                            {
                                                val.IsFavorite = true;
                                                val.FavoIconSouce = "BigFavoIconOn.png";
                                            }
                                            else
                                            {
                                                val.IsFavorite = false;
                                                val.FavoIconSouce = "BigFavoIconOff.png";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        ((Xamarin.Forms.Image)sender).Source = "BigFavoIconOn.png";
                                    }
                                    // アイコンクリックしたスタッフをお気に入りに登録
                                    parameters["isFavorited"] = "true";
                                    parameters["staffId"] = staffID.ToString();
                                    result = await APIManager.Post(action, parameters);
                                    if (result != null)
                                    {
                                        //((Xamarin.Forms.Image)sender).Source = "BigFavoIconOn.png";
                                        DependencyService.Get<IToast>().Show("お気に入りスタッフを変更しました。");

                                        await UpdateHome();

                                    }
                                    else
                                    {
                                        DependencyService.Get<IToast>().Show("通信エラー");
                                    }
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
                        App.ProcessManager.OnComplete();
                        await DialogManager.Instance.ShowDialogView(modalView);
                    }
                    else
                    {
                        // アイコンクリックしたスタッフをお気に入りに登録
                        parameters.Add("isFavorited", "true");
                        parameters["staffId"] = staffID.ToString();
                        var ret = await APIManager.Post(action, parameters);
                        if (ret != null)
                        {
                            if (itemSouce != null)
                            {
                                ((ListViewStaffStoreViewModel)((Xamarin.Forms.Image)sender).BindingContext).BdContext.FavoIconSouce = "BigFavoIconOn.png";
                                ((ListViewStaffStoreViewModel)((Xamarin.Forms.Image)sender).BindingContext).BdContext.IsFavorite = true;
                            }
                            else
                            {
                                ((Xamarin.Forms.Image)sender).Source = "BigFavoIconOn.png";
                            }
                            DependencyService.Get<IToast>().Show("お気に入りスタッフに登録しました");

                            await UpdateHome();
                        }
                        else
                        {
                            DependencyService.Get<IToast>().Show("通信エラー");
                        }
                        App.ProcessManager.OnComplete();
                    }
                }

            }
            else
            {
                DependencyService.Get<IToast>().Show("通信エラー");
                App.ProcessManager.OnComplete();
            }
            //string action = "staff_favorite_regist";
            //if (souce.File == "BigFavoIconOn.png")
            //{
            //	parameters.Add("isFavorited", "false");
            //	var ret = await APIManager.Post(action, parameters);
            //	if (ret != null)
            //	{
            //		((Xamarin.Forms.Image)sender).Source = "BigFavoIconOff.png";
            //		DependencyService.Get<IToast>().Show("お気に入りスタッフから削除しました");
            //		return true;
            //	}
            //}
            //else
            //{
            //	parameters.Add("isFavorited", "true");
            //	var ret = await APIManager.Post(action, parameters);
            //	if (ret != null)
            //	{
            //		((Xamarin.Forms.Image)sender).Source = "BigFavoIconOn.png";
            //		DependencyService.Get<IToast>().Show("お気に入りスタッフに登録しました");
            //		return true;
            //	}
            //}
        }

        static async Task UpdateHome()
        {
            var json = await APIManager.GET("home");

            var res = JsonManager.Deserialize<ResponseHome>(json);

            var home = App.customNavigationPage.Navigation.NavigationStack.Where((arg) => arg.Id == App.HomeId);
            if (home.FirstOrDefault() != null)
            {
                System.Diagnostics.Debug.WriteLine(" could  get home and the id  :");
                var home_page = home.First() as Home;

                var home_bc = home_page.BindingContext as HomeViewModel;
                home_bc.UpdateHomeSalon(res);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine(" could not get home and the id  :");
            }
        }

        public static async Task<string> DeviceIDReg(string id, string transferId)
        {
            DeviceInfo deviceInfo = new DeviceInfo()
            {
                ID = id,
                TransferID = transferId
            };

            Config.Instance.Data.deviceId = id;

            bool ret = await FileManager.WriteJsonFileAsync("Account", "deviceInfo", deviceInfo);
            if (ret)
            {
                return "OK";
            }

            return "NG";
        }
    }
}
