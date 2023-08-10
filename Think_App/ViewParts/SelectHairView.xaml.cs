using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using IO.Swagger.Model;

namespace Think_App
{
    public class HairStyleSelectedEventArgs : EventArgs
    {
        public string Url { get; set; }
        public double Scale { get; set; }
        public double ShiftX { get; set; }
        public double SHiftY { get; set; }
    }

    public partial class SelectHairView : ContentView
    {
        public event EventHandler InitViewCompleted = delegate { };
        public event EventHandler<HairStyleSelectedEventArgs> HairStyleSelected = delegate { };

        static readonly Color UnSelectedTextColor = Color.FromHex("#9B9B9B");
        static readonly Color SelectedTextColor = ColorList.colorWhite;

        // 定数定義
        const int _maxColumns = 5;
        const double _rowSpacing = 0;
        const double _columnSpacing = 0;
        const double _marginLeft = 10;
        const double _marginTop = 9;
        const double _marginRight = 10;
        const double _marginBottom = 9;

        double _screenScale;
        double _screenWidth;

        public SelectHairView()
        {
            InitializeComponent();

            // ポジション変更をキャッチ
            this.SelectHairUnitCarouselView.PropertyChanged += SelectHairUnitCarouselView_PropertyChanged;

            // スクリーンスケールとスクリーン幅を求めておく。
            var service = DependencyService.Get<IScreenService>();
            _screenScale = service.GetScreenScale();
            _screenWidth = service.GetScreenWidth();

            Task.Run(() => Device.BeginInvokeOnMainThread(async () => this.BindingContext = await CreateModel()));

            this.BindingContextChanged += OnBindingContextChanged;
        }

        private void SelectHairUnitCarouselView_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == CarouselLayout.SelectedIndexProperty.PropertyName)
                UpdateSelection();
        }

        public async Task<SelectHairViewModel> CreateModel()
        {
            var gender = await GenderManager.LoadGenderAsync();

            // Todo 現在はGET
            var json = await APIManager.GET("wig_all");
            try
            {
                var response = JsonManager.Deserialize<ResponseWigAll>(json);
                var data = response.Data;

                var _SelectHairViewModel = new SelectHairViewModel();
                _SelectHairViewModel.SelectHairUnitViewModelSets = new List<SelectHairViewModel.SelectHairUnitViewModelSet>();

                var infoList = (gender == Gender.Female) ? data.WomanWigList : null;
                if (infoList == null)
                {
                    infoList = (gender == Gender.Male) ? data.ManWigList : null;
                }

                if (infoList == null || infoList.Count == 0)
                {
                    return null;
                }

                for (int i = 0; i < infoList.Count; ++i)
                {
                    var info = infoList[i];

                    // のちに生成するグリッドのタイルサイズを計算しておく。
                    double tileSize = (_screenWidth - _marginLeft - _marginRight - _columnSpacing * (_maxColumns - 1)) / _maxColumns;
                    var source = new List<SelectHairCellModel>();
                    if (info.WigList != null)
                    {
                        for (int j = 0; j < info.WigList.Count; ++j)
                        {
                            var wig = info.WigList[j];
                            var path = await ImageManager.GetLocalStorageFullPathByUrlAsync(wig.Image.Path, true);
                            var cellModel = new SelectHairCellModel()
                            {
                                SHCBackImgSource = "hair_back_image.png",
                                SHCHairImgMargin = new Thickness(6, 6, 6, 6),
                                SHCHairImgDSWidth = tileSize * _screenScale,
                                SHCHairImgDSHeight = tileSize * _screenScale,
                                SHCHairImgBGColor = Color.Transparent,
                                SHCSelectedMarkMargin = new Thickness(3, 3, 3, 3),
                                SHCSelectedMarkBorderThickness = 3.0,
                                SHCSelectedMarkFillColor = Color.Transparent,
                                SHCSelectedMarkStrokeColor = ColorList.colorMain,
                                SHCSelectedMarkRadiusRate = 0.0,
                                SHCSelectedMarkVisible = false,
                                Url = wig.Image.Path,
                                Scale = wig.Scale == null ? 1.0 : (double)wig.Scale.Value,
                                ShiftX = wig.ShiftX == null ? 0.0 : (double)wig.ShiftX.Value,
                                SHiftY = wig.ShiftY == null ? 0.0 : (double)wig.ShiftY.Value
                            };
                            if (string.IsNullOrEmpty(path))
                            {
                                cellModel.SHCHairImgSource = new UriImageSource() { Uri = new Uri(wig.Image.Path) };
                            }
                            else
                            {
                                cellModel.SHCHairImgSource = new FileImageSource() { File = path };
                            }
                            source.Add(cellModel);
                        }
                    }

                    var model = new SelectHairUnitViewModel()
                    {
                        GridMaxColumns = _maxColumns,
                        GridMargin = new Thickness(_marginLeft, _marginTop, _marginRight, _marginBottom),
                        GridRowSpacing = _rowSpacing,
                        GridColumnSpacing = _columnSpacing,
                        GridTileHeight = tileSize,
                        GridItemsSource = source,
                    };

                    _SelectHairViewModel.SelectHairUnitViewModelSets.Add(new SelectHairViewModel.SelectHairUnitViewModelSet()
                    {
                        Model = model,
                        HairStyleName = info.HairStyle
                    });
                }

                return _SelectHairViewModel;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                // 通信に失敗している。
                DependencyService.Get<IToast>().Show("通信エラー");
                return null;
            }
        }

        async void OnBindingContextChanged(object sender, EventArgs e)
        {
            if (this.BindingContext != null && this.BindingContext is SelectHairViewModel)
            {
                await InitView(this.BindingContext as SelectHairViewModel);
                if (InitViewCompleted != null)
                {
                    InitViewCompleted(this, EventArgs.Empty);
                }
            }
        }

        async Task InitView(SelectHairViewModel model)
        {
            var source = new ObservableCollection<CaroucelViewModel>();
            for (int i = 0; i < model.SelectHairUnitViewModelSets.Count; ++i)
            {
                var modelSet = model.SelectHairUnitViewModelSets[i];
                var label = new Label()
                {
                    Text = modelSet.HairStyleName,
                    FontSize = 14,
                    VerticalTextAlignment = TextAlignment.Center,
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    BackgroundColor = Color.Transparent
                };
                this.HairTypeSelector.ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = new GridLength(1, GridUnitType.Star)
                });
                this.HairTypeSelector.Children.Add(label, i, 0);

                // SelectHairUnitViewを作成し、modelをバインディング。
                var view = new SelectHairUnitView() { BindingContext = modelSet.Model };
                view.HairStyleSelected += OnHairStyleSelected;
                await view.UpdateView();
                source.Add(new CaroucelViewModel() { ViewContent = view });
            }
            this.SelectHairUnitCarouselView.ItemsSource = source;
            // 初期表示更新
            UpdateSelection();
        }

        void OnHairStyleSelected(object sender, HairStyleSelectedEventArgs e)
        {
            Debug.WriteLine("HairStyleUrl:{0} が選ばれました。", e.Url);
            UpdateHairStyleSelection(e.Url);
            if (HairStyleSelected != null)
            {
                HairStyleSelected(this, e);
            }
        }

        void UpdateSelection()
        {
            for (int i = 0; i < this.HairTypeSelector.Children.Count; ++i)
            {
                try
                {
                    var lbl = this.HairTypeSelector.Children[i] as Label;
                    // 文字色の更新。
                    lbl.TextColor = (i == this.SelectHairUnitCarouselView.SelectedIndex) ? SelectedTextColor : UnSelectedTextColor;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
        }

        public void UpdateHairStyleSelection(string url)
        {
            // 全髪型を走査して、選択状態の更新をViewModelから行う。
            var model = this.BindingContext as SelectHairViewModel;
            if (model != null)
            {
                foreach (var modelSet in model.SelectHairUnitViewModelSets)
                {
                    foreach (var cellModel in modelSet.Model.GridItemsSource)
                    {
                        cellModel.SHCSelectedMarkVisible = (cellModel.Url == url);
                    }
                }
            }
        }

        public class CaroucelViewModel : ViewModelBase
        {
            public View ViewContent { get; set; }
        }
    }
}
