using System;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using IO.Swagger.Model;

namespace Think_App
{
    public partial class HairCatalogSelectionPage : ContentPage
    {
        public event EventHandler<ImageSource> HairSelected = delegate { };

        // 横並びの最大カラム数
        const int _maxColumns = 3;
        // グリッド表示のパディング
        const double _paddingLeft = 27;
        const double _paddingRight = 27;
        const double _paddingBottom = 10;
        // グリッド表示のカラムスペーシング
        const double _columnSpacing = 11;
        // HairCatalogListPageにおける最終ページフラグ
        const int _lastPageFlg = 3;

        int _salonId;
        string _salonName;

        HairCatalogSelectionPageModel Model { get; set; }
        bool IsInitted { get; set; }

        public HairCatalogSelectionPage(int salonId, string salonName)
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "");
            _salonId = salonId;
            _salonName = salonName;

            // 初期化
            Init();

            this.BindingContextChanged += async (sender, e) =>
            {
                if (Model.WomanHairStyleItemsSource != null)
                {
                    // タイルの生成。
                    await this.WomanView.BuildTiles();
                }
                if (Model.ManHairStyleItemsSource != null)
                {
                    // タイルの生成。
                    await this.ManView.BuildTiles();
                }

                // Contentの表示を復帰。
                this.Content.IsVisible = true;

                // プログレス非表示
                App.customNavigationPage.IsRunning = false;
            };
        }

        void Init()
        {
            // いったんContentを非表示にしないと、ゴミが見える。
            this.Content.IsVisible = false;
            Model = new HairCatalogSelectionPageModel();

            // プログレス表示
            App.customNavigationPage.IsRunning = true;

            var postData = new Dictionary<string, string>()
            {
                {"salonId", _salonId.ToString()}
            };
            var awaiter = APIManager.GET("salon_hair_list", postData).GetAwaiter();
            awaiter.OnCompleted(() =>
            {
                try
                {
                    var json = awaiter.GetResult();
                    var response = JsonManager.Deserialize<ResponseSalonHairList>(json);
                    var data = response.Data;

                    var screenWidth = DependencyService.Get<IScreenService>().GetScreenWidth();

                    Task.Run(() =>
                    {
                        // グリッドの設定
                        Model.HairStyleViewPadding = new Thickness(_paddingLeft, 0, _paddingRight, _paddingBottom);
                        Model.HairStyleViewColumnSpacing = _columnSpacing;
                        Model.HairStyleViewMaxColumns = _maxColumns;
                        var tileWidth = (screenWidth - _paddingLeft - _paddingRight - (_maxColumns - 1) * _columnSpacing) / _maxColumns;
                        Model.HairStyleViewTileHeight = tileWidth + 30;

                        if (data.WomanHairStyleList != null && data.WomanHairStyleList.Count > 0)
                        {
                            var source = new List<HairCatalogSelectionViewCellModel>();
                            foreach (var hairStyle in data.WomanHairStyleList)
                            {
                                source.Add(new HairCatalogSelectionViewCellModel()
                                {
                                    HCSVCImageSource = hairStyle.ThumbnailImage.Path,
                                    HCSVCImageHeight = new GridLength(tileWidth, GridUnitType.Absolute),
                                    HCSVCHairStyleNameLblText = hairStyle.Name,
                                    HairStyleId = hairStyle.HairStyleId.Value
                                });
                            }
                            Model.WomanHairStyleItemsSource = source;
                            Model.WomanHairStyleSelectedCommand = new Command(OnSelectedWomanHairStyle);
                        }

                        if (data.ManHairStyleList != null && data.ManHairStyleList.Count > 0)
                        {
                            var source = new List<HairCatalogSelectionViewCellModel>();
                            foreach (var hairStyle in data.ManHairStyleList)
                            {
                                source.Add(new HairCatalogSelectionViewCellModel()
                                {
                                    HCSVCImageSource = hairStyle.ThumbnailImage.Path,
                                    HCSVCImageHeight = new GridLength(tileWidth, GridUnitType.Absolute),
                                    HCSVCHairStyleNameLblText = hairStyle.Name,
                                    HairStyleId = hairStyle.HairStyleId.Value
                                });
                            }
                            Model.ManHairStyleItemsSource = source;
                            Model.ManHairStyleSelectedCommand = new Command(OnSelectedManHairStyle);
                        }

                        Model.WomanHairStyleVisible = (Model.WomanHairStyleItemsSource != null);
                        Model.ManHairStyleVisible = (Model.ManHairStyleItemsSource != null);
                        Model.SeparatorVisible = (Model.WomanHairStyleItemsSource != null) && (Model.ManHairStyleItemsSource != null);

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            this.BindingContext = Model;
                        });
                    });
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        // トースト表示
                        DependencyService.Get<IToast>().Show("通信エラー");
                        // プログレス非表示
                        App.customNavigationPage.IsRunning = false;
                    });
                }
            });
        }

        async void OnSelectedWomanHairStyle(object obj)
        {
            if (!App.ProcessManager.CanInvoke())
            {
                return;
            }

            var model = obj as HairCatalogSelectionViewCellModel;
            if (model != null)
            {
                await GoHairCatalogListPageAsync(model.HairStyleId, model.HCSVCHairStyleNameLblText, GetSexTypeAsNumber(Gender.Woman));
            }

            App.ProcessManager.OnComplete();
        }

        async void OnSelectedManHairStyle(object obj)
        {
            if (!App.ProcessManager.CanInvoke())
            {
                return;
            }

            var model = obj as HairCatalogSelectionViewCellModel;
            if (model != null)
            {
                await GoHairCatalogListPageAsync(model.HairStyleId, model.HCSVCHairStyleNameLblText, GetSexTypeAsNumber(Gender.Man));
            }

            App.ProcessManager.OnComplete();
        }

        async Task GoHairCatalogListPageAsync(int hairStyleId, string hairName, int sexType)
        {
            var page = new HairCatalogListPage(_lastPageFlg, hairStyleId, hairName, sexType, _salonId, _salonName);
            page.HairSelected += OnHairSelected;
            await this.Navigation.PushAsync(page);
        }

        async void OnHairSelected(object sender, ImageSource e)
        {
            try
            {
                if (this.Navigation.NavigationStack.Last() is HairCatalogListPage)
                {
                    if (HairSelected != null)
                    {
                        // HairCatalogListPageをポップ。
                        await this.Navigation.PopAsync(false);
                        HairSelected(this, e);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }

        enum Gender
        {
            Woman,
            Man
        }

        int GetSexTypeAsNumber(Gender gender)
        {
            if (gender == Gender.Man)
            {
                // 男性
                return 2;
            }
            else
            {
                // 女性
                return 1;
            }
        }
    }
}
