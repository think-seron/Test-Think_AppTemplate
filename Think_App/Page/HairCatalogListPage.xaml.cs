using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using IO.Swagger.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using DLToolkit.Forms.Controls;
using ZXing.QrCode.Internal;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
    public partial class HairCatalogListPage : ContentPage
    {
        public event EventHandler<ImageSource> HairSelected = delegate { };

        HairCatalogListPageViewModel hairCatalogListPageViewModel;
        bool? isEnd;
        ObservableCollection<HairStyleInfo> Images;
        int hairStyleId;
        int index = 0;
        int sexType;
        int storeId;
        string storeName;
        int? staffId;
        int _lastPageFlg;
        // lastPageFlgについて(仮)
        // 1: 予約画面からの遷移
        // 2: 店舗情報からの遷移
        // 3: メッセージからの遷移
        public HairCatalogListPage(int lastPageFlg, int _hairStyleId, string hairName, int _sexType, int _storeId, string _storeName, int? _staffId = null)
        {
            InitializeComponent();
            _lastPageFlg = lastPageFlg;
            App.customNavigationPage.IsRunning = true;

            NavigationPage.SetBackButtonTitle(this, "");

            storeId = _storeId;
            storeName = _storeName;
            sexType = _sexType;
            hairStyleId = _hairStyleId;
            staffId = _staffId;
            hairCatalogListPageViewModel = new HairCatalogListPageViewModel();

            hairCatalogListPageViewModel.ItemWidth = 100 * hairCatalogListPageViewModel.ScreenSizeScale;
            hairCatalogListPageViewModel.ItemHeight = 100 * hairCatalogListPageViewModel.ScreenSizeScale;
            hairCatalogListPageViewModel.ColumnSpacing = 5 * hairCatalogListPageViewModel.ScreenSizeScale;
            hairCatalogListPageViewModel.RowSpacing = 10 * hairCatalogListPageViewModel.ScreenSizeScale;
            hairCatalogListPageViewModel.GridViewRect = new Rect(0, 40, 1, ScaleManager.ScreenHeight - 40);
            //if (Device.RuntimePlatform == Device.Android)
            //{
            //    // GridView androidの表示iosとずれているので調整
            //    // android RowSpacingとColumunSpacingうまく動いていない可能性あり
            //    hairCatalogListPageViewModel.ItemWidth *= ScaleManager.AndroidDensity;
            //    hairCatalogListPageViewModel.ItemHeight *= ScaleManager.AndroidDensity;
            //    hairCatalogListPageViewModel.ColumnSpacing *= ScaleManager.AndroidDensity;
            //    hairCatalogListPageViewModel.RowSpacing *= ScaleManager.AndroidDensity;
            //    // Densityかけただけでは横が切れてしまう
            //    hairCatalogListPageViewModel.ItemWidth *= 1.1;
            //}
            hairCatalogListPageViewModel.LabelTxt = hairName;
            Images = new ObservableCollection<HairStyleInfo>();
            // -------------------------------------------------------------
            // 選択したヘアカタログのカテゴリの情報を取得し、そのカテゴリ内のヘアカタログ画像を表示
            Task.Run(() =>
            {
                GetHairCatalogData();
            });
            // waitしないとiosで画像が表示されない
            //Task.Delay(200).Wait();
            hairCatalogListPageViewModel.ItemsSource = Images;
            this.BindingContext = hairCatalogListPageViewModel;
            App.customNavigationPage.IsRunning = false;
            GrdView.FlowItemTapped += GrdView_FlowItemTapped;

            //this.GrdView.ItemSelected += (sender, e) =>
            //{
            //	if ( ((HairStyleInfo)e.Value).Souce == null && ((HairStyleInfo)e.Value).Souce == (ImageSource)""  )
            //	{
            //		return;
            //	}

            //	if (App.ProcessManager.CanInvoke())
            //	{
            //		var page = new HairCatalogInfoPage(lastPageFlg, ((HairStyleInfo)e.Value));
            //		page.HairSelected += OnHairSelected;
            //		Navigation.PushAsync(page);
            //		App.ProcessManager.OnComplete();
            //	}
            //};
        }

        private void GrdView_FlowItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null ||
                !(e.Item is HairStyleInfo model) ||
                model.Souce == null) return;

            if (App.ProcessManager.CanInvoke())
            {
                var page = new HairCatalogInfoPage(_lastPageFlg, model);
                page.HairSelected += OnHairSelected;
                Navigation.PushAsync(page);
                App.ProcessManager.OnComplete();
            }
        }

        void AddMoreLookBtn()
        {
            Images.Add(new HairStyleInfo()
            {
                StoreId = storeId,
                StoreName = storeName,
                Souce = "",
                Description = "",
                BtnIsVisible = false
            });
            Images.Add(new HairStyleInfo()
            {
                StoreId = storeId,
                StoreName = storeName,
                Souce = "",
                Description = "",
                BtnIsVisible = false
            });
            Images.Add(new HairStyleInfo()
            {
                StoreId = storeId,
                StoreName = storeName,
                Souce = "",
                Description = "",
                BtnIsVisible = true
            });
        }

        void delMoreLookBtn()
        {
            for (var i = 0; i < 3; i++)
            {
                Images.RemoveAt(Images.Count - 1);
            }
        }

        async void MoreLookBtnClick(object sender, EventArgs e)
        {
            delMoreLookBtn();
            index++;

            GetHairCatalogData();
        }

        async void GetHairCatalogData()
        {
            var parameters = new Dictionary<string, string> { { "index", index.ToString() }, { "hairStyleId", hairStyleId.ToString() }, { "sexType", sexType.ToString() } };
            if (staffId != null)
            {
                // スタッフIdが渡されているときは、パラメータに追加
                parameters.Add("staffId", staffId.Value.ToString());
            }
            var json = await APIManager.GET("hair_list", parameters);
            if (json == null)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    //this.BindingContext = hairCatalogListPageViewModel;
                    DependencyService.Get<IToast>().Show("通信エラー");
                });
            }
            else
            {
                var response = JsonConvert.DeserializeObject<ResponseHairList>(json);
                if (response.Data.List != null)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        foreach (var val in response.Data.List)
                        {
                            if (val.ThumbnailImage != null)
                            {
                                Images.Add(new HairStyleInfo()
                                {
                                    StoreId = storeId,
                                    StoreName = storeName,
                                    //Souce = val.ThumbnailImage.Path,
                                    Souce = DependencyService.Get<IImageService>().ResizeNetImage(val.ThumbnailImage.Path),
                                    Description = val.Description,
                                    StaffName = val.StaffName,
                                    BtnIsVisible = false
                                });
                            }
                            //else
                            //{
                            //	Images.Add(new HairStyleInfo("loginBgImg.png", val.Description, false));
                            //}
                        }
                        if (response.Data.IsEnd == false)
                        {
                            AddMoreLookBtn();
                        }
                    });
                }
            }
        }

        async void OnHairSelected(object sender, ImageSource e)
        {
            try
            {
                if (this.Navigation.NavigationStack.Last() is HairCatalogInfoPage)
                {
                    if (HairSelected != null)
                    {
                        // HairCatalogInfoPageをポップ。
                        await this.Navigation.PopAsync(false);
                        HairSelected(this, e);
                        if (this.Navigation.NavigationStack.Last() == this)
                        {
                            // このページをポップ。
                            await this.Navigation.PopAsync(false);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }
    }
}
