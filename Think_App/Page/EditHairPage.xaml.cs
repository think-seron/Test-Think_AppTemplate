using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IO.Swagger.Model;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
namespace Think_App
{
    public partial class EditHairPage : BackCustomizeContentPage
    {
        const int _defaultColorR = 159;
        const int _defaultColorG = 128;
        const int _defaultColorB = 115;
        const double _defaltStrokeWidth = 50.0;
        const double _minStrokeWidth = 10.0;
        const double _maxStrokeWidth = 150.0;
        const double _minHairImgWidthRate = 0.5;
        const double _maxHairImgWidthRate = 1.5;
        const double _minHairImgHeightRate = 0.5;
        const double _maxHairImgHeightRate = 1.5;
        const double _minHeight = 1;
        const double _maxHeight = 10000;
        const double _closeBtnBaseFontSize = 18;
        const double _closeBtnBaseWidth = 153;
        const double _closeBtnBaseHeight = 39.87;
        const double _saveComleteViewBaseFontSize = 20;
        const double _saveComleteViewBaseButtonFontSize = 18;
        const double _saveComleteViewBaseButtonWidth = 216;
        const double _saveComleteViewBaseButtonHeight = 42;
        // 画像を保存する際の、短辺の最大値。
        const double _maxImageLength = 1024;

        Rect _hairImgRect;
        string _infoFilename, _hairFilename, _faceFilename, _imageFilename;

        bool _isGallery;
        Rect _viewImageRange;

        bool SelectColorViewUpdated;
        bool IsHairRectEditted;

        bool IsSaving;
        bool IsRunning;

        EditHairPageModel Model { get; set; }

        public EditHairPage(Rect hairImgRect, ImageSource baseImageSource, ImageSource hairImageSource,
                            bool isGallery = false, double scale = 1.0, double translationX = 0.0, double translationY = 0.0,
                            Rect? viewImageRange = null)
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "");
            var decisionToolBarItem = new ToolbarItem()
            {
                Text = "保存",
                Command = new Command(OnDecideEdit),
            };

            this.ToolbarItems.Add(decisionToolBarItem);

            Model = new EditHairPageModel()
            {
                CloseBtnFontSize = _closeBtnBaseFontSize * ScaleManager.Scale,
                CloseBtnWidth = _closeBtnBaseWidth * ScaleManager.Scale,
                CloseBtnHeight = _closeBtnBaseHeight * ScaleManager.Scale
            };

            _isGallery = isGallery;
            if (isGallery)
            {
                // PhotoImgを非表示に
                Model.PhotoImgVisible = false;
                // GalleryImgを表示に
                Model.GalleryImgVisible = true;
                // GalleryImgにイメージソース設定
                Model.GalleryImgSource = baseImageSource;
                // Scale, Translation を更新
                Model.GalleryImgScale = scale;
                Model.GalleryImgTranslationX = translationX;
                Model.GalleryImgTranslationY = translationY;
                _viewImageRange = viewImageRange ?? Rect.Zero;
            }
            else
            {
                // PhotoImgを表示に
                Model.PhotoImgVisible = true;
                // GalleryImgを非表示に
                Model.GalleryImgVisible = false;
                // PhotoImgにイメージソース設定
                Model.PhotoImgSource = baseImageSource;
                // Scale, Translation を更新
                Model.PhotoImgScale = scale;
                Model.PhotoImgTranslationX = translationX;
                Model.PhotoImgTranslationY = translationY;
                _viewImageRange = viewImageRange ?? Rect.Zero;
            }

            _hairImgRect = hairImgRect;
            Model.HairImgSource = hairImageSource;
            this.HairImg.DefaultRect = _hairImgRect;
            this.HairImg.StrokeWidth = _defaltStrokeWidth;
            this.HairImg.StrokeColor = Color.FromRgb(_defaultColorR, _defaultColorG, _defaultColorB);
            this.HairImg.WidthRate = 1.0;

            // 初期状態は編集不可。
            this.HairImg.IsEnabled = false;

            // 保存時に呼ばれる
            this.HairImg.GotCurrentImageSource += HairImg_GotCurrentImageSource;

            if (EnableBackButtonOverride)
            {
                this.CustomBackButtonAction = async () =>
                {
                    // リセット直後に「戻る」をタップした時に落ちることがあるので防ぐ。
                    if (IsRunning)
                    {
                        return;
                    }
                    IsRunning = true;

                    if (IsSaving)
                    {
                        // セーブ中に呼ばれることがある。何もしない。
                        System.Diagnostics.Debug.WriteLine("セーブ中です。機能を封じます！");
                        IsRunning = false;
                        return;
                    }

                    if (this.HairImg.IsEditted || IsHairRectEditted)
                    {
                        // 編集済みの場合はリセット動作
                        await ResetAsync();
                    }
                    else
                    {
                        // 編集がない場合はページを閉じる。
                        System.Diagnostics.Debug.WriteLine("ページを閉じます。");
                        await Navigation.PopAsync();
                    }

                    IsRunning = false;
                };
            }

            this.UpBtn.Clicked += UpBtn_Clicked;
            this.DownBtn.Clicked += DownBtn_Clicked;
            this.LeftBtn.Clicked += LeftBtn_Clicked;
            this.RightBtn.Clicked += RightBtn_Clicked;
            this.FacePlusBtn.Clicked += FacePlusBtn_Clicked;
            this.FacePlusBtn.LongTapping += FacePlusBtn_Clicked;
            this.FaceMinusBtn.Clicked += FaceMinusBtn_Clicked;
            this.FaceMinusBtn.LongTapping += FaceMinusBtn_Clicked;

            this.SelectPenBtn.Clicked += SelectPenBtn_Clicked;
            this.CloseBtn.Clicked += CloseBtn_Clicked;

            this.SelectColorView.ColorSelected += SelectColorView_ColorSelected;

            this.HairThicknessSlider.ValueChanged += HairThicknessSlider_ValueChanged;
            this.StrokeThicknessSlider.ValueChanged += StrokeThicknessSlider_ValueChanged;

            // 初回更新
            UpdateHairImagePosition(true);
            InitSelectColorView();
            UpdateCurrentColorCircle();
            UpdateHairThicknessSlider();
            UpdateStrokeThicnessSlider();

            this.SelectColorView.SizeChanged += (sender, e) =>
            {
                if (SelectColorViewUpdated)
                {
                    return;
                }

                if (this.SelectColorView.Width > 0 && this.SelectColorView.Height > 0)
                {
                    this.SelectColorView.UpdateGradationList();
                    SelectColorViewUpdated = true;
                }
            };

            // Binding
            this.BindingContext = Model;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // 強制アップデート
            this.HairImg.ForceUpdate();
        }

        void UpdateHairImagePosition(bool init = false)
        {
            AbsoluteLayout.SetLayoutBounds(this.HairImg, _hairImgRect);

            if (!init && !IsHairRectEditted)
            {
                // ポジション及び幅・高さの編集フラグが立っていなければ立てる。
                IsHairRectEditted = true;
            }
        }

        void UpdateCurrentColorCircle()
        {
            this.CurrentColorCircle.Color = this.HairImg.StrokeColor;
        }

        void UpdateHairThicknessSlider()
        {
            this.HairThicknessSlider.Value = CalcSliderValue(this.HairImg.WidthRate, _minHairImgWidthRate, _maxHairImgWidthRate, this.HairThicknessSlider.Minimum, this.HairThicknessSlider.Maximum);
        }

        void UpdateStrokeThicnessSlider()
        {
            this.StrokeThicknessSlider.Value = CalcSliderValue(this.HairImg.StrokeWidth, _minStrokeWidth, _maxStrokeWidth, this.StrokeThicknessSlider.Minimum, this.StrokeThicknessSlider.Maximum);
        }

        void InitSelectColorView()
        {
            this.SelectColorView.AddGradation(Color.FromRgb(159, 128, 115), Color.FromRgb(38, 22, 16));
            this.SelectColorView.AddGradation(Color.FromRgb(147, 134, 113), Color.FromRgb(29, 24, 14));
            this.SelectColorView.AddGradation(Color.FromRgb(158, 134, 108), Color.FromRgb(35, 23, 13));
            this.SelectColorView.AddGradation(Color.FromRgb(163, 135, 76), Color.FromRgb(41, 25, 12));
            this.SelectColorView.AddGradation(Color.FromRgb(146, 139, 98), Color.FromRgb(33, 25, 12));
            this.SelectColorView.AddGradation(Color.FromRgb(139, 138, 134), Color.FromRgb(22, 21, 13));
            this.SelectColorView.AddGradation(Color.FromRgb(114, 114, 150), Color.FromRgb(17, 17, 24));
            this.SelectColorView.AddGradation(Color.FromRgb(145, 128, 142), Color.FromRgb(24, 15, 16));
            this.SelectColorView.AddGradation(Color.FromRgb(144, 117, 140), Color.FromRgb(31, 17, 24));
            this.SelectColorView.AddGradation(Color.FromRgb(165, 108, 104), Color.FromRgb(42, 15, 11));
            this.SelectColorView.AddGradation(Color.FromRgb(167, 113, 96), Color.FromRgb(40, 14, 11));
            this.SelectColorView.AddGradation(Color.FromRgb(169, 119, 91), Color.FromRgb(43, 21, 13));
            this.SelectColorView.AddGradation(Color.FromRgb(166, 127, 84), Color.FromRgb(41, 20, 13));

            this.SelectColorView.DefaultColor = this.HairImg.StrokeColor;
        }

        void UpBtn_Clicked(object sender, EventArgs e)
        {
            _hairImgRect.Y = _hairImgRect.Y - 1;
            UpdateHairImagePosition();
        }

        void DownBtn_Clicked(object sender, EventArgs e)
        {
            _hairImgRect.Y = _hairImgRect.Y + 1;
            UpdateHairImagePosition();
        }

        void LeftBtn_Clicked(object sender, EventArgs e)
        {
            _hairImgRect.X = _hairImgRect.X - 1;
            UpdateHairImagePosition();
        }

        void RightBtn_Clicked(object sender, EventArgs e)
        {
            _hairImgRect.X = _hairImgRect.X + 1;
            UpdateHairImagePosition();
        }

        void FacePlusBtn_Clicked(object sender, EventArgs e)
        {
            if (_hairImgRect.Height > _maxHeight - 2)
            {
                // 高さはこれ以上上げることができない。
                return;
            }

            // 高さを上げる
            var oldHeight = _hairImgRect.Height;
            var height = _hairImgRect.Height + 2;
            // 古い高さの半分だけ上にずらす。
            var dif = (height - _hairImgRect.Height) / 2.0;
            _hairImgRect.Y = _hairImgRect.Y - dif;
            _hairImgRect.Height = height;

            if (this.HairImg.IsEditted)
            {
                if (oldHeight > 0)
                {
                    // 髪の毛のストローク情報を再計算する。
                    this.HairImg.RecalclateStrokes(1.0, height / oldHeight);
                }
            }

            UpdateHairImagePosition();

        }

        void FaceMinusBtn_Clicked(object sender, EventArgs e)
        {
            if (_hairImgRect.Height < _minHeight + 2)
            {
                // 高さはこれ以上下げることができない。
                return;
            }

            // 高さを下げる
            var oldHeight = _hairImgRect.Height;
            var height = _hairImgRect.Height - 2;
            // 古い高さの半分だけ上にずらす。
            var dif = (height - _hairImgRect.Height) / 2.0;
            _hairImgRect.Y = _hairImgRect.Y - dif;
            _hairImgRect.Height = height;

            if (this.HairImg.IsEditted)
            {
                if (oldHeight > 0)
                {
                    // 髪の毛のストローク情報を再計算する。
                    this.HairImg.RecalclateStrokes(1.0, height / oldHeight);
                }
            }

            UpdateHairImagePosition();
        }

        void HairThicknessSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            var val = e.NewValue;
            // 全体における割合を計算
            this.HairImg.WidthRate = val / (this.HairThicknessSlider.Maximum - this.HairThicknessSlider.Minimum) + _minHairImgWidthRate;
            // 新しい幅設定
            var width = this.HairImg.DefaultRect.Width * this.HairImg.WidthRate;
            // 古い幅との差の半分だけ左にずらす。
            var dif = (width - _hairImgRect.Width) / 2.0;
            _hairImgRect.X = _hairImgRect.X - dif;
            var oldWidth = _hairImgRect.Width;
            _hairImgRect.Width = width;

            if (oldWidth.Equals(width))
            {
                // 幅が変更されないので何もしない。
                return;
            }

            if (this.HairImg.IsEditted)
            {
                if (oldWidth > 0)
                {
                    // 髪の毛のストローク情報を再計算する。
                    this.HairImg.RecalclateStrokes(width / oldWidth, 1.0);
                }
            }

            UpdateHairImagePosition();
        }

        void StrokeThicknessSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            var val = e.NewValue;
            // 全体における割合を計算
            var rate = val / (this.StrokeThicknessSlider.Maximum - this.StrokeThicknessSlider.Minimum);
            // 新しいペン幅設定
            var strokeWidth = _minStrokeWidth + (_maxStrokeWidth - _minStrokeWidth) * rate;
            this.HairImg.StrokeWidth = strokeWidth;
        }

        void SelectColorView_ColorSelected(object sender, SelectColorView.ColorEventArgs e)
        {
            this.HairImg.StrokeColor = e.Color;
            UpdateCurrentColorCircle();
        }

        void SelectPenBtn_Clicked(object sender, EventArgs e)
        {
            this.SelectPenController.IsVisible = true;
            this.SelectPenBtn.IsEnabled = false;
            var navPage = this.Parent as CustomNavigationPage;
            if (navPage != null)
            {
                navPage.UpdateShadow(true);
            }
        }

        void CloseBtn_Clicked(object sender, EventArgs e)
        {
            this.SelectPenController.IsVisible = false;
            this.SelectPenBtn.IsEnabled = true;
            var navPage = this.Parent as CustomNavigationPage;
            if (navPage != null)
            {
                navPage.UpdateShadow(false);
            }

            if (!this.HairImg.IsEnabled)
            {
                // もし編集不可になっていれば、編集を有効にしておく。
                this.HairImg.IsEnabled = true;
            }
        }

        async Task ResetAsync()
        {
            if (!App.ProcessManager.CanInvoke())
            {
                return;
            }

            // モーダルページ作成
            var modalView = new ModalView();
            modalView.modalViewViewModel.ModalLabelTxt = "編集をリセットしますか？";
            modalView.modalViewViewModel.YesButtonTxt = "リセットする";
            modalView.modalViewViewModel.NoButtonTxt = "リセットしない";
            modalView.modalViewViewModel.NomalModalLabelRect = new Rect(0.5, 0.45, 1, AbsoluteLayout.AutoSize);
            modalView.modalViewViewModel.SelectBtnLayoutBounds = new Rect(0.9, 0.73, 1, AbsoluteLayout.AutoSize);

            modalView.yesButton.Clicked += async (sender, e) =>
            {
                // リセット動作。
                System.Diagnostics.Debug.WriteLine("リセットします。");
                this.HairImg.Clear();
                _hairImgRect = this.HairImg.DefaultRect;
                UpdateHairImagePosition(true);
                UpdateHairThicknessSlider();
                IsHairRectEditted = false;

                // モーダルを閉じる
                await DialogManager.Instance.HideView();
                App.ProcessManager.OnComplete();
            };

            modalView.noButton.Clicked += async (sender, e) =>
            {
                // モーダルを閉じる
                await DialogManager.Instance.HideView();
                App.ProcessManager.OnComplete();
            };

            // モーダルを表示
            await DialogManager.Instance.ShowDialogView(modalView);
        }

        void OnDecideEdit(object obj)
        {
            if (IsSaving)
            {
                return;
            }
            // セーブ中フラグを立てておく。
            IsSaving = true;
            // すべてのコントロールを封じる。
            SwitchAllControlEnable(false);

            System.Diagnostics.Debug.WriteLine("保存が押されました。");
            // 現在の編集結果の髪型画像をリクエストする。
            this.HairImg.RequestCurrentImageSource();
        }

        async void HairImg_GotCurrentImageSource(object sender, GotCurrentImageSourceEventArgs e)
        {
            if (!IsSaving)
            {
                // セーブ中でなければ何も行わない。
                return;
            }

            // 保存用の設定。
            // 1)画像の編集情報
            // 2)髪型の画像
            // 3)本人の画像
            // 4)現在の合成画像

            // 保存のためのファイル名は現在時刻で生成する。
            var filename = StorageManager.CreateFileNameByCurrentTime();
            // 1)画像の編集情報 ファイル名
            _infoFilename = filename + ConstantManager.Suffix_EditInfo + ".txt";
            // 2)髪型の画像 ファイル名
            _hairFilename = filename + ConstantManager.Suffix_Hair + ".png";
            // 3)本人の画像 ファイル名
            _faceFilename = filename + ConstantManager.Suffix_Face + ".png";
            // 4)現在の合成画像 ファイル名
            _imageFilename = filename + ConstantManager.Suffix_Image + ".png";

            System.Diagnostics.Debug.WriteLine("保存開始。");
            // 1)画像の編集情報 の保存
            var data = new EditHairSaveData()
            {
                AppendColorImageSaveData = this.HairImg.GetSaveData(),
                HairImageRect = _hairImgRect,
                isGallery = _isGallery,
                ImageScale = _isGallery ? Model.GalleryImgScale : Model.PhotoImgScale,
                ImageTranslationX = _isGallery ? Model.GalleryImgTranslationX : Model.PhotoImgTranslationX,
                ImageTranslationY = _isGallery ? Model.GalleryImgTranslationY : Model.PhotoImgTranslationY,
                ViewImageRange = _viewImageRange
            };
            await StorageManager.UserDataWriteAsync(ConstantManager.FolderName_EditData, _infoFilename, data, false);
            System.Diagnostics.Debug.WriteLine("保存完了:{0}", _infoFilename);
            // 2)髪型の画像 の保存
            await ImageManager.SaveImageToLocalStorageAsync(this.HairImg.Source, ConstantManager.FolderName_EditData, _hairFilename, false);
            System.Diagnostics.Debug.WriteLine("保存完了:{0}", _hairFilename);
            // 3)本人の画像 の保存
            await ImageManager.SaveImageToLocalStorageAsync(GetFaceImage(), ConstantManager.FolderName_EditData, _faceFilename, false);
            System.Diagnostics.Debug.WriteLine("保存完了:{0}", _faceFilename);
            // 4)現在の合成画像 の保存
            var faceImageSource = await GetCurrentFaceImageAsync();
            var faceImageSize = await DependencyService.Get<IImageService>().GetImageSizeAsync(faceImageSource);
            bool resize = false;
            double minImageLength = 0;
            if (_maxImageLength < Math.Min(faceImageSize.Width, faceImageSize.Height))
            {
                resize = true;
                minImageLength = _maxImageLength;
                System.Diagnostics.Debug.WriteLine("合成画像リサイズ 短辺:{0}", minImageLength);
            }
            await ImageManager.SaveCombinedImageToLocalStorageAsync(faceImageSource, e.ImageSource, _hairImgRect, GetViewSize(), GetAspect(), ConstantManager.FolderName_SavedImage, _imageFilename, false, resize, minImageLength);
            System.Diagnostics.Debug.WriteLine("保存完了:{0}", _imageFilename);
            System.Diagnostics.Debug.WriteLine("すべての保存完了！");

            // セーブ完了モーダルを表示。
            await DialogManager.Instance.ShowDialogView(CreateModalPage());
        }

        ImageSource GetFaceImage()
        {
            return (_isGallery) ? this.GalleryImg.Source : this.PhotoImg.Source;
        }

        Size GetViewSize()
        {
            double width, height;
            if (_isGallery)
            {
                width = this.GalleryImg.Width;
                height = this.GalleryImg.Height;
            }
            else
            {
                width = this.PhotoImg.Width;
                height = this.PhotoImg.Height;
            }

            return new Size(width, height);
        }

        Aspect GetAspect()
        {
            return (_isGallery) ? this.GalleryImg.Aspect : this.PhotoImg.Aspect;
        }

        async Task<ImageSource> GetCurrentFaceImageAsync()
        {
            ImageSource source = null;
            if (_isGallery)
            {
                // 範囲をクロップして画像を返す。
                var service = DependencyService.Get<IImageService>();
                source = await service.GetCroppedImageSourceAsync(this.GalleryImg.Source, _viewImageRange);
            }
            else
            {
                if (_viewImageRange == Rect.Zero)
                {
                    // 単純にPhotoImgに今設定されている画像を返すだけです。
                    source = this.PhotoImg.Source;
                }
                else
                {
                    // 範囲をクロップして画像を返す。
                    var service = DependencyService.Get<IImageService>();
                    source = await service.GetCroppedImageSourceAsync(this.PhotoImg.Source, _viewImageRange);
                }
            }

            return source;
        }

        async Task OnSendMessageButtonClickedAsync()
        {
            if (!App.ProcessManager.CanInvoke())
            {
                return;
            }

            var json = await APIManager.GET("home");
            try
            {
                var param = JsonManager.Deserialize<ResponseHome>(json);

                // モーダルを閉じる。
                await DialogManager.Instance.HideView();
                // ホームへ遷移。
                await this.Navigation.PushAsync(new Home(param)
                {
                    ForceGoMessagePage = true,
                    ImageForMessage = await ImageManager.LoadImageFromLocalStorageAsync(ConstantManager.FolderName_SavedImage, _imageFilename)
                }, false);

                // セーブ中フラグをオフ。
                IsSaving = false;
                // すべてのコントロールを復帰。
                SwitchAllControlEnable(true);

                // プロセス終了
                App.ProcessManager.OnComplete();
            }
            catch
            {
                DependencyService.Get<IToast>().Show("通信エラー");
                // プロセス終了
                App.ProcessManager.OnComplete();
            }
        }

        async Task OnEndButtonClickedAsync()
        {
            if (!App.ProcessManager.CanInvoke())
            {
                return;
            }

            var json = await APIManager.GET("home");
            try
            {
                var param = JsonManager.Deserialize<ResponseHome>(json);

                // モーダルを閉じる
                await DialogManager.Instance.HideView();
                // ホームへ遷移。
                await this.Navigation.PushAsync(new Home(param));

                // セーブ中フラグをオフ。
                IsSaving = false;
                // すべてのコントロールを復帰。
                SwitchAllControlEnable(true);

                // プロセス終了
                App.ProcessManager.OnComplete();
            }
            catch
            {
                DependencyService.Get<IToast>().Show("通信エラー");
                // プロセス終了
                App.ProcessManager.OnComplete();
            }
        }

        double CalcSliderValue(double current, double min, double max, double minimum, double maximum)
        {
            double value = 0.0;
            if (max - min > 0)
            {
                value = (maximum - minimum) * (current - min) / (max - min) - minimum;
            }
            return value;
        }

        void SwitchAllControlEnable(bool enable)
        {
            this.IsEnabled = enable;
            this.Wall.IsVisible = !enable;
        }

        public void ApplySavedEditData(AppendColorImageSaveData data)
        {
            this.HairImg.SetSaveData(data);

            // 表示更新
            UpdateCurrentColorCircle();
            UpdateHairThicknessSlider();
            UpdateStrokeThicnessSlider();
            this.SelectColorView.DefaultColor = this.HairImg.StrokeColor;
        }

        SaveCompletedView CreateModalPage()
        {
            // モーダルページ作成
            var modalView = new SaveCompletedView();
            var model = new SaveCompletedViewModel()
            {
                ButtonWidth = _saveComleteViewBaseButtonWidth * ScaleManager.Scale,
                ButtonHeight = _saveComleteViewBaseButtonHeight * ScaleManager.Scale,
                ButtonFontSize = _saveComleteViewBaseButtonFontSize * ScaleManager.Scale,
                InfoLblFontSize = _saveComleteViewBaseFontSize * ScaleManager.Scale,
                SendMessageCommand = new Command(async () => await OnSendMessageButtonClickedAsync()),
                EndCommand = new Command(async () => await OnEndButtonClickedAsync())
            };
            modalView.BindingContext = model;
            return modalView;
        }
    }

    public class EditHairSaveData
    {
        public AppendColorImageSaveData AppendColorImageSaveData { get; set; }
        public Rect HairImageRect { get; set; }
        public bool isGallery { get; set; }
        public double ImageScale { get; set; }
        public double ImageTranslationX { get; set; }
        public double ImageTranslationY { get; set; }
        public Rect ViewImageRange { get; set; }
    }
}