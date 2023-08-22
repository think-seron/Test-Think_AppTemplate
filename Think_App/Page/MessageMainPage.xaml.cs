using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Diagnostics;

using Plugin.Media;
using System.Collections.Generic;
using IO.Swagger.Model;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
    public enum AvailableImageStatus
    {
        Neutral = 0,
        AddHairSimulation = 1,
    }
    public partial class MessageMainPage : ContentPage
    {
        const double _imageSourceViewHeight = 216;
        const uint _defaultDuration = 250;
        const double _refreshTimeLineIntervalSeconds = 10;
        const double _balloonTailWidth = 15.88;
        const double _inputMessageViewHeightSingle = 45;
        const double _inputMessageViewHeightMulti = 66;
        // 画像を送信する際の、短辺の最大値。
        const double _maxImageLength = 1024;

        double _maxBalloonWidth;
        CancellationTokenSource _ctsReset;
        CancellationTokenSource _ctsSlide;

        public bool IsKeyboardShown { get; set; }

        MessageMainPageModel Model { get; set; }

        private int? SalonId { get; set; }
        private string SalonName { get; set; }

        private bool IsTimerContinue { get; set; }
        private bool IsInitCompleted { get; set; }

        private int LastMessageId { get; set; }
        private int FirstMessageId { get; set; }
        private bool IsGettableOlderMessages { get; set; }
        private bool IsOlderRunning { get; set; }
        private bool IsRefreshRunning { get; set; }

        private MessageEditor.Lines InputtingLines { get; set; }
        private double UnderInputMessageViewHeight { get; set; }

        private bool IsInBottom { get; set; }
        private bool IsUpdateInBottomFlagDisable { get; set; }

        private bool IsAnimating { get; set; }
        private bool IsSlided { get; set; }

        private ImageSource SendImageSource { get; set; }

        private DateTime InittedDate { get; set; }

        public bool IsSwitchingLines
        {
            get
            {
                return this.InputMessageView.IsSwitchingLines;
            }
        }

        bool soloSalon;
        AvailableImageStatus _availableImageState;
        public MessageMainPage(int? salonId = null, string salonName = null, ImageSource imageSource = null, bool _soloSalon = false, AvailableImageStatus availableImageState = AvailableImageStatus.Neutral)
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "");
            _availableImageState = availableImageState;

            // 現在時刻を取得
            InittedDate = DependencyService.Get<IDateTimeService>().GetNow();

            // 最大バルーン幅の設定
            var screenWidth = DependencyService.Get<IScreenService>().GetScreenWidth();
            _maxBalloonWidth = screenWidth - 124 - 17;

            // サロンId、サロン名
            SalonId = salonId;
            SalonName = salonName;
            soloSalon = _soloSalon;
            // 送るべきイメージソース
            SendImageSource = imageSource;

            // メッセージIdは未読み込みで -1 で初期化
            LastMessageId = -1;

            InputtingLines = MessageEditor.Lines.Single;

            this.MessageList.Margin = new Thickness(0, 0, 0, _inputMessageViewHeightSingle);
            this.InputControlContainer.TranslationY = _imageSourceViewHeight;

            this.InputMessageView.LinesChanged += (__, e) =>
            {
                InputtingLines = e;
                UpdateMessageListMargin();
            };
            this.InputMessageView.ImageButtonClicked += (__, _) =>
            {
                // 常に瞬間的に表示する。
                SlideContent(false, _imageSourceViewHeight, 0);
            };
            this.InputMessageView.SendButtonClicked += async (__, e) =>
            {
                // メッセージを送信する。
                await SendMessageAsync(e);
            };
            this.InputMessageView.Focused += async (sender, e) =>
            {
                // コンテンツ表示を引っ込める。
                SlideContent(true, 0, 0);

                // TODO Xamarin.Forms.Device.RuntimePlatform is no longer supported. Use Microsoft.Maui.Devices.DeviceInfo.Platform instead. For more details see https://learn.microsoft.com/en-us/dotnet/maui/migration/forms-projects#device-changes
                if (Device.RuntimePlatform == Device.Android)
                {
                    IsUpdateInBottomFlagDisable = true;
                    this.InputMessageView.InputDummyText();
                }
                await Task.Delay(300);
                Device.BeginInvokeOnMainThread(() =>
                                               this.MessageList.ScrollToBottom());
            };
            this.InputMessageView.DummyTextInputted += async (sender, e) =>
            {
                await this.InputMessageView.ClearDummyTextAsync();
                IsUpdateInBottomFlagDisable = false;
            };
            this.MessageList.Scrolled += async (sender, e) =>
            {
                if (e.IsTop)
                {
                    Debug.WriteLine("スクロール: 一番上");
                }
                if (e.IsBottom && !IsUpdateInBottomFlagDisable)
                {
                    Debug.WriteLine("スクロール: 一番下");
                }
                if (IsInBottom && !e.IsBottom && !IsUpdateInBottomFlagDisable)
                {
                    Debug.WriteLine("スクロール: 一番下を離れた");
                }

                // 一番下にいるかどうかのフラグ更新。
                if (!IsUpdateInBottomFlagDisable)
                {
                    IsInBottom = e.IsBottom;
                }

                if (e.IsTop)
                {
                    // 一番上にスクロールしたので、古いデータの取得。
                    await UpdateOlderMessagesAsync();
                }
            };

            if (!soloSalon)
            {
                this.ToolbarItems.Add(new ToolbarItem()
                {
                    Icon = "Icon_Home.png",
                    Command = new Command(async () =>
                    {
                        if (!App.ProcessManager.CanInvoke())
                            return;
                        // 店舗選択ページへ遷移
                        var page = new SelectSalonMessagePage();
                        page.SalonSelected += OnSalonSelected;

                        // TODO Xamarin.Forms.Device.RuntimePlatform is no longer supported. Use Microsoft.Maui.Devices.DeviceInfo.Platform instead. For more details see https://learn.microsoft.com/en-us/dotnet/maui/migration/forms-projects#device-changes
                        if (Device.RuntimePlatform == Device.iOS)
                            App.customNavigationPage.IsBadgeVisble = false;

                        await this.Navigation.PushAsync(page);
                        App.ProcessManager.OnComplete();
                    })
                });
            }


            //android6.0以上のためのパーミッション確認
            bool res = int.TryParse(Config.Instance.Data.nativeVersion.Substring(0, 1), out version);
            if (!res)
                return;
            // TODO Xamarin.Forms.Device.RuntimePlatform is no longer supported. Use Microsoft.Maui.Devices.DeviceInfo.Platform instead. For more details see https://learn.microsoft.com/en-us/dotnet/maui/migration/forms-projects#device-changes
            if (Device.RuntimePlatform == Device.Android && version >= 6)
            {
                DependencyService.Get<IScanerPermissionService>().Call();
            }




        }
        int version;
        void UpdateMessageListMargin()
        {
            this.MessageList.Margin = (InputtingLines == MessageEditor.Lines.Multi)
                ? new Thickness(0, 0, 0, _inputMessageViewHeightMulti + UnderInputMessageViewHeight)
                : new Thickness(0, 0, 0, _inputMessageViewHeightSingle + UnderInputMessageViewHeight);
            if (IsInBottom)
            {
                // 最新アイテムが見えている状況の場合、自動的にスクロールを最終位置に移動します。
                this.MessageList.ScrollToBottom();
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (IsInitCompleted)
            {
                System.Diagnostics.Debug.WriteLine("IsInitCompleted true");
                // 定期更新タイマーを再開する。
                PreStartRefreshTimeLineTimer();
                StartRefreshTimeLineTimer();
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("IsInitCompleted false");
                // 初期化
                Init(SalonId, SalonName);
            }
            if (!soloSalon)
                SetBadge();
        }


        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            // 定期更新タイマーを終了する。
            EndRefreshTimeLineTimer();

            // 送るべきイメージソースのクリア
            SendImageSource = null;
        }

        async void OnSalonSelected(object sender, SelectSalonMessagePage.SalonInfo info)
        {
            if (SalonId == null || SalonId.Value != info.SalonId)
            {
                // サロンId、サロン名更新
                SalonId = info.SalonId;
                SalonName = info.SalonName;
                Debug.WriteLine("SalonId:{0} Salon名:{1} に更新しました。", info.SalonId, info.SalonName);
                IsInitCompleted = false;
            }

            try
            {
                if (this.Navigation.NavigationStack[this.Navigation.NavigationStack.Count - 1] == sender)
                {
                    // 店舗選択ページをポップする。
                    await this.Navigation.PopAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        void Init(int? salonId, string salonName)
        {
            App.customNavigationPage.IsRunning = true;

            // まず表示周りをリセットする。
            this.InputMessageView.ClearText();
            UnderInputMessageViewHeight = 0;
            this.MessageList.Margin = new Thickness(0, 0, 0, _inputMessageViewHeightSingle);
            this.InputControlContainer.TranslationY = _imageSourceViewHeight;
            this.BindingContext = null;

            Model = new MessageMainPageModel();
            System.Diagnostics.Debug.WriteLine("Model  new");


            Task.Run(async () =>
            {
                var selectImageSourceViewModel = new SelectImageSourceViewModel()
                {
                    PhotoButtonClickedCommand = new Command(OnPhotoButtonClicked),
                    CameraButtonClickedCommand = new Command(OnCameraButtonClicked),
                    CatalogButtonClickedCommand = new Command(OnCatalogButtonClicked),
                    SimulationButtonClickedCommand = new Command(OnSimulationButtonClicked),
                    SimulationBtnVisible = _availableImageState == AvailableImageStatus.Neutral
                                                                                       ? false
                                                                                       : true,
                };
                Model.SelectImageSourceViewModel = selectImageSourceViewModel;
                Model.PageTappedCommand = new Command(OnPageTapped);
                Model.PageTappedEnable = false;
                Model.SelectImageSourceViewHeight = _imageSourceViewHeight;
                Model.SelectImageSourceViewModel = selectImageSourceViewModel;

                // APIからデータ読み込み
                if (salonId == null)
                {
                    // 本来あり得ないことだが・・・。
                    Model.MessageListViewItemsSource = new ObservableCollection<MessageListCellModel>();
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        this.BindingContext = Model;
                        App.customNavigationPage.IsRunning = false;
                    });
                }
                else
                {

                    Model.MessageListViewItemsSource = new ObservableCollection<MessageListCellModel>();

                    var id = salonId.Value;
                    var postData = new Dictionary<string, string>()
                    {
                        {"salonId", id.ToString()},
                        {"messageId", "0"}
                    };
                    var json = await APIManager.GET("message_list", postData);
                    try
                    {
                        var response = JsonManager.Deserialize<ResponseMessageList>(json);

                        // ここからUIスレッド
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            // 古いメッセージが取得可能かのフラグを得る。
                            IsGettableOlderMessages = !response.Data.IsLast.Value;
                            // 古いほうから順番にソースに追加していくので逆順。
                            if (response.Data.MessageList != null && response.Data.MessageList.Count > 0)
                            {
                                DateTime oldDate = DateTime.MinValue;
                                DateTime newDate;
                                for (int i = response.Data.MessageList.Count - 1; i >= 0; --i)
                                {
                                    var messageDetail = response.Data.MessageList[i];
                                    // 日付を取得。
                                    newDate = DateManager.ConvertMessageDateStringToDateTime(messageDetail.Date);
                                    bool showDate = !DateManager.EqualsAsDay(newDate, oldDate);

                                    var cellModel = await CreateMessageListCellModelAsync(messageDetail.MessageType.Value,
                                                                                          messageDetail.Message,
                                                                                          messageDetail.Image,
                                                                                          messageDetail.IsSalon.Value,
                                                                                          SalonName,
                                                                                          newDate,
                                                                                          showDate);
                                    Model.MessageListViewItemsSource.Add(cellModel);
                                    if (i == 0)
                                    {
                                        // 最新のメッセージIdを取得
                                        LastMessageId = messageDetail.MessageId.Value;
                                    }
                                    if (i == response.Data.MessageList.Count - 1)
                                    {
                                        // 一番古いアイテムのメッセージIDを保持
                                        FirstMessageId = messageDetail.MessageId.Value;
                                    }
                                    oldDate = newDate;
                                }
                            }

                            this.BindingContext = Model;
                            App.customNavigationPage.IsRunning = false;

                            // スクロールを一番下に移動(アニメーションなし)
                            if (response.Data.MessageList != null && response.Data.MessageList.Count > 0)
                            {
                                this.MessageList.ScrollToBottom();
                            }
                            IsInBottom = true;
                            // ここまで来たら、定期更新タイマーをスタートする。
                            IsInitCompleted = true;
                            PreStartRefreshTimeLineTimer();
                            StartRefreshTimeLineTimer();
                        });
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            this.BindingContext = Model;
                            App.customNavigationPage.IsRunning = false;
                            DependencyService.Get<IToast>().Show("通信エラー");
                        });
                    }

                    if (SendImageSource != null)
                    {
                        // ここで強制的にイメージを投稿する。
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            Debug.WriteLine("イメージを投稿します。");
                            await SendImageAsync(SendImageSource);
                        });
                    }
                }
            });

        }
        async void SetBadge()
        {
            try
            {

                var jsonNotRead = await APIManager.GET("check_badge");
                var responseNotRead = JsonManager.Deserialize<ResponseCheckBatch>(jsonNotRead);
                if (responseNotRead != null)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        if ((bool)(responseNotRead.Data.MessageNotification) && this.ToolbarItems != null)
                        {
                            if (Device.RuntimePlatform == Device.Android)
                            {
                                //this.RightToolbarItem.Icon = "Icon_HomeAndBadge.png";
                                this.ToolbarItems[0].Icon = "Icon_HomeAndBadge";
                            }
                            else
                            {
                                App.customNavigationPage.IsBadgeVisble = true;
                            }

                        }
                        else
                        {

                            if (Device.RuntimePlatform == Device.Android)
                            {
                                //this.RightToolbarItem.Icon = "Icon_Home.png";
                                this.ToolbarItems[0].Icon = "Icon_Home";
                            }
                            else
                            {
                                App.customNavigationPage.IsBadgeVisble = false;
                            }
                        }
                    });
                }
            }
            catch (Exception ex)
            {

                //this.RightToolbarItem.Icon = "Icon_Home.png";
                System.Diagnostics.Debug.WriteLine(" check badge ex :" + ex);
            }
        }
        async Task<Size> CalculateImageBalloonSizeAsync(ImageSource source)
        {
            var size = Size.Zero;

            try
            {
                // 幅は最大サイズにとる。
                size.Width = _maxBalloonWidth;
                // 画像のサイズ取得。
                var imageSize = await DependencyService.Get<IImageService>().GetImageSizeAsync(source);
                // 画像の幅に対する高さの割合を求める。
                var scale = imageSize.Height / imageSize.Width;
                // 表示画像幅。バルーン幅からパディングとしっぽの幅を引く。
                var imageWidth = size.Width - 10 * 2 - _balloonTailWidth;
                // 表示画像高さ。幅から求める。
                var imageHeight = imageWidth * scale;
                // 高さ。画像高さにパディングを足す。
                size.Height = imageHeight + 10 * 2;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return size;
        }

        Size CalculateMessageBalloonSize(ref string message, double fontSize)
        {
            var size = Size.Zero;

            try
            {
                // 行ごとにmessageを分割
                string[] delimiter = { Environment.NewLine };
                var lines = message.Split(delimiter, StringSplitOptions.None);

                double maxLendth = 0;
                foreach (var line in lines)
                {
                    // 各行の幅を調べる。
                    var length = DependencyService.Get<ITextService>().CalculateTextWidth(line, fontSize);
                    if (maxLendth < length)
                    {
                        // 最大幅更新
                        maxLendth = length;
                    }
                }

                // 幅はテキスト幅にパディングとしっぽの幅を足す。ただし最大バルーンサイズを超えない。
                size.Width = Math.Min(maxLendth + 12 * 2 + _balloonTailWidth, _maxBalloonWidth);
                // テキスト幅。バルーン幅からパディングとしっぽの幅を引く。
                var textWidth = size.Width - 12 * 2 - _balloonTailWidth;
                // このテキスト幅に合わせて、テキストの各行の再分割を行う。
                // 一旦メッセージを空にする。
                var messageList = new List<string>();
                foreach (var line in lines)
                {
                    // この行の幅を調べる。
                    var length = DependencyService.Get<ITextService>().CalculateTextWidth(line, fontSize);
                    if (length < textWidth + 0.01)
                    {
                        // テキスト幅の範囲内ならそのまま追加。
                        messageList.Add(line);
                    }
                    else
                    {
                        // テキスト幅を上回った場合、テキスト幅に収まる範囲で文字を分割。
                        int startIndex = 0;
                        int count = 1;
                        while (startIndex + count <= line.Length)
                        {
                            var str = line.Substring(startIndex, count);
                            var strLength = DependencyService.Get<ITextService>().CalculateTextWidth(str, fontSize);
                            if (textWidth < strLength + 0.01)
                            {
                                // 一文字減らした文字数でギリギリ収まる。
                                messageList.Add(line.Substring(startIndex, count - 1));

                                // countを1に戻し、startIndexを文字数ぶん進める。
                                startIndex += count - 1;
                                count = 1;
                            }
                            else
                            {
                                // カウントを進める。
                                count++;
                            }
                        }

                        if (startIndex < line.Length)
                        {
                            // 残りの文字を新しい行にすべてつぎ込む。
                            messageList.Add(line.Substring(startIndex));
                        }
                    }
                }

                // メッセージリストをメッセージに直す。
                message = "";
                for (int i = 0; i < messageList.Count; ++i)
                {
                    message += messageList[i];
                    if (i < messageList.Count - 1)
                    {
                        message += Environment.NewLine;
                    }
                }

                // テキスト高さを求める。
                var textHeight = DependencyService.Get<ITextService>().CalculateTextHeight(message, fontSize);
                // 高さはテキスト高さにパディングを足す
                size.Height = textHeight + 10 * 2;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return size;
        }

        public void SlideContent(bool isReset, double dY, uint duration)
        {
            if (isReset && !IsSlided)
            {
                return;
            }

            Task.Run(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (isReset)
                    {
                        _ctsReset = new CancellationTokenSource();
                        if (_ctsSlide != null)
                        {
                            _ctsSlide.Cancel();
                        }
                        await ResetSlideContentAsync(0);
                        IsSlided = false;
                    }
                    else
                    {
                        _ctsSlide = new CancellationTokenSource();
                        if (_ctsReset != null)
                        {
                            _ctsReset.Cancel();
                        }
                        await SlideContentAsync(dY, duration);
                        IsSlided = true;
                    }
                });
            });
        }

        public async Task SlideContentAsync(double dY, uint duration)
        {
            IsAnimating = true;
            try
            {
                if (_ctsSlide != null)
                {
                    _ctsSlide.Token.ThrowIfCancellationRequested();
                }
                await Task.Delay((int)duration);
                this.InputControlContainer.TranslationY = _imageSourceViewHeight - dY;
                UnderInputMessageViewHeight = dY;
                UpdateMessageListMargin();
                if (_ctsSlide != null)
                {
                    _ctsSlide.Token.ThrowIfCancellationRequested();
                }
            }
            catch (OperationCanceledException ex)
            {
                Debug.WriteLine("SlideContentAsync was Cencelled.:{0}", ex);
            }

            if (_ctsSlide != null)
            {
                _ctsSlide.Dispose();
                _ctsSlide = null;
            }
            IsAnimating = false;
        }

        public async Task ResetSlideContentAsync(uint duration)
        {
            IsAnimating = true;
            try
            {
                if (_ctsReset != null)
                {
                    _ctsReset.Token.ThrowIfCancellationRequested();
                }
                await Task.Delay((int)duration);
                this.InputControlContainer.TranslationY = _imageSourceViewHeight;
                UnderInputMessageViewHeight = 0;
                UpdateMessageListMargin();
                if (_ctsReset != null)
                {
                    _ctsReset.Token.ThrowIfCancellationRequested();
                }
            }
            catch (OperationCanceledException ex)
            {
                Debug.WriteLine("ResetSlideContentAsync was Cencelled.:{0}", ex);
                this.InputControlContainer.TranslationY = _imageSourceViewHeight;
                this.InputControlContainer.WidthRequest = this.InputControlContainer.Width;
            }

            if (_ctsReset != null)
            {
                _ctsReset.Dispose();
                _ctsReset = null;
            }
            IsAnimating = false;
        }

        void OnPageTapped()
        {
            if (IsAnimating)
            {
                return;
            }

            Debug.WriteLine("Page Tapped");
            // TODO Xamarin.Forms.Device.RuntimePlatform is no longer supported. Use Microsoft.Maui.Devices.DeviceInfo.Platform instead. For more details see https://learn.microsoft.com/en-us/dotnet/maui/migration/forms-projects#device-changes
            if (Device.RuntimePlatform == Device.iOS && Device.Idiom == DeviceIdiom.Phone)
            {
                if (this.InputMessageView.IsFocused)
                {
                    this.InputMessageView.Unfocus();
                }
            }

            SlideContent(true, 0, _defaultDuration);
        }

        async void OnPhotoButtonClicked()
        {
            Debug.WriteLine("写真 が押されました。");

            if (!App.ProcessManager.CanInvoke())
            {
                return;
            }

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                Debug.WriteLine("写真のピッカーに対応していません！");
                await DisplayAlert("写真へのアクセスができません", "設定画面から写真へのアクセスを有効にしてください。", "閉じる");
                App.ProcessManager.OnComplete();
                return;
            }

            // コンテンツ表示を引っ込める。
            SlideContent(true, 0, 0);

            // 写真ピッカーの呼び出しと写真の取得。
            var file = await CrossMedia.Current.PickPhotoAsync();

            if (file == null)
            {
                App.ProcessManager.OnComplete();
                return;
            }

            Debug.WriteLine("File Path:{0}", file.Path);

            var service = DependencyService.Get<IImageService>();
            var imageSource = service.GetOrientationAdjustedImageSource(file.Path);

            // ファイルピッカーの画像が大きすぎる場合は縮小する。
            var imageSize = await service.GetImageSizeAsync(imageSource);
            if (_maxImageLength < Math.Min(imageSize.Width, imageSize.Height))
            {
                double newWidth, newHeight;
                if (imageSize.Height < imageSize.Width)
                {
                    newHeight = _maxImageLength;
                    newWidth = newHeight * imageSize.Width / imageSize.Height;
                }
                else
                {
                    newWidth = _maxImageLength;
                    newHeight = newWidth * imageSize.Height / imageSize.Width;
                }
                Debug.WriteLine("ファイルピッカーリサイズ");
                Debug.WriteLine("幅:{0} 高さ{1} => 幅:{2} 高さ:{3}", imageSize.Width, imageSize.Height, newWidth, newHeight);
                imageSource = await service.ResizeAsync(imageSource, newWidth, newHeight);
            }

            // モーダルページ作成
            var modalView = new ModalView();
            modalView.modalViewViewModel.ImageSource = imageSource;
            modalView.modalViewViewModel.ImageAspect = Aspect.AspectFill;
            modalView.modalViewViewModel.ImageWidth = 270;
            modalView.modalViewViewModel.ImageHeight = 270;
            modalView.modalViewViewModel.ModalLabelTxt = "この写真でよろしいですか？";
            modalView.modalViewViewModel.YesButtonTxt = "はい";
            modalView.modalViewViewModel.NoButtonTxt = "いいえ";
            // TODO Xamarin.Forms.Device.RuntimePlatform is no longer supported. Use Microsoft.Maui.Devices.DeviceInfo.Platform instead. For more details see https://learn.microsoft.com/en-us/dotnet/maui/migration/forms-projects#device-changes
            double posY = (Device.RuntimePlatform == Device.Android) ? 0.2 : 0.31;
            modalView.modalViewViewModel.ImageRect = new Rect(0.5, posY, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize);
            modalView.modalViewViewModel.NomalModalLabelRect = new Rect(0.5, 0.71, 1, AbsoluteLayout.AutoSize);
            modalView.modalViewViewModel.SelectBtnLayoutBounds = new Rect(0.9, 0.77, 1, AbsoluteLayout.AutoSize);

            modalView.yesButton.Clicked += async (sender, e) =>
            {
                // モーダルを閉じる
                await DialogManager.Instance.HideView();
                App.ProcessManager.OnComplete();

                // 選択したイメージを送る。
                await SendImageAsync(imageSource);
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

        async void OnCameraButtonClicked()
        {
            Debug.WriteLine("カメラ が押されました。");

            if (!CrossMedia.Current.IsCameraAvailable || !DependencyService.Get<ICameraService>().IsCameraAvailable())
            {
                Debug.WriteLine("カメラが使用できません");
                return;
            }

            if (!App.ProcessManager.CanInvoke())
            {
                return;
            }

            // コンテンツ表示を引っ込める。
            SlideContent(true, 0, 0);

            // 撮影ページに遷移する。
            var page = new TakePhotoMessagePage();
            page.TakenPhotoCompleted += OnTakenPhotoCompleted;

            // TODO Xamarin.Forms.Device.RuntimePlatform is no longer supported. Use Microsoft.Maui.Devices.DeviceInfo.Platform instead. For more details see https://learn.microsoft.com/en-us/dotnet/maui/migration/forms-projects#device-changes
            if (Device.RuntimePlatform == Device.iOS)
                App.customNavigationPage.IsBadgeVisble = false;

            await this.Navigation.PushAsync(page);

            App.ProcessManager.OnComplete();
        }

        async void OnTakenPhotoCompleted(object sender, ImageSource e)
        {
            // この時点では撮影ページがまだpushされているのでpopする。
            try
            {
                if (this.Navigation.NavigationStack[this.Navigation.NavigationStack.Count - 1] == sender)
                {
                    Debug.WriteLine("撮影ページをpopします。");
                    await this.Navigation.PopAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            // 写真を送信する。
            await SendImageAsync(e);
        }

        async void OnCatalogButtonClicked()
        {
            Debug.WriteLine("ヘアカタログ が押されました。");

            if (SalonId == null)
            {
                // 本来あり得ないが・・・。
                Debug.WriteLine("サロンが選ばれていません。");
            }

            if (!App.ProcessManager.CanInvoke())
            {
                return;
            }

            // コンテンツ表示を引っ込める。
            SlideContent(true, 0, 0);

            // ヘアカタログページに遷移する。
            var page = new HairCatalogSelectionPage(SalonId.Value, SalonName);
            page.HairSelected += OnHairSelected;

            // TODO Xamarin.Forms.Device.RuntimePlatform is no longer supported. Use Microsoft.Maui.Devices.DeviceInfo.Platform instead. For more details see https://learn.microsoft.com/en-us/dotnet/maui/migration/forms-projects#device-changes
            if (Device.RuntimePlatform == Device.iOS)
                App.customNavigationPage.IsBadgeVisble = false;

            await this.Navigation.PushAsync(page);

            App.ProcessManager.OnComplete();
        }

        async void OnHairSelected(object sender, ImageSource imageSource)
        {
            // 選択したイメージを送る。
            await SendImageAsync(imageSource);
        }

        async void OnSimulationButtonClicked()
        {
            Debug.WriteLine("ヘアシミュ が押されました。");

            if (!App.ProcessManager.CanInvoke())
            {
                return;
            }

            // コンテンツ表示を引っ込める。
            SlideContent(true, 0, 0);

            // ヘアシミュ画像保存ページに遷移する。
            var page = new SavedDataGalleryPage(SavedDataGalleryPage.Purpose.SelectImage);
            page.ImageSelected += OnHairSimulationImageSelected;

            // TODO Xamarin.Forms.Device.RuntimePlatform is no longer supported. Use Microsoft.Maui.Devices.DeviceInfo.Platform instead. For more details see https://learn.microsoft.com/en-us/dotnet/maui/migration/forms-projects#device-changes
            if (Device.RuntimePlatform == Device.iOS)
                App.customNavigationPage.IsBadgeVisble = false;

            await this.Navigation.PushAsync(page);

            App.ProcessManager.OnComplete();
        }

        async void OnHairSimulationImageSelected(object sender, ImageSource imageSource)
        {
            // 選択したイメージを送る。
            await SendImageAsync(imageSource);
        }

        async Task SendMessageAsync(string message)
        {
            if (SalonId == null)
            {
                // サロンが選ばれていないので、送信不可。
                Debug.WriteLine("サロンが選ばれていません！");
                return;
            }

            if (string.IsNullOrEmpty(message))
            {
                // メッセージがないので送信不可。
                Debug.WriteLine("送信対象のメッセージがありません！");
                return;
            }

            var checkMessage = message.Replace(Environment.NewLine, "");
            if (string.IsNullOrEmpty(checkMessage))
            {
                // 改行コードしかない場合はサーバーで弾かれる。
                Debug.WriteLine("不正な文字列です！");
                return;
            }

            // メッセージを送信する。
            var postData = new Dictionary<string, string>()
            {
                {"message", message},
                {"messageType", "1"},
                {"salonId", SalonId.Value.ToString()}
            };
            var json = await APIManager.Post("message_regist", postData);
            if (json == null)
            {
                // 通信失敗
                DependencyService.Get<IToast>().Show("通信エラー");
            }
            else
            {
                // 通信成功
                if (IsInitCompleted)
                {
                    // 初期化が終了している場合
                    // 入力欄をクリア
                    this.InputMessageView.ClearText();
                    // 画面更新。
                    await RefreshTimeLineAsync(SalonId.Value, LastMessageId);
                }
                else
                {
                    // 初期化が終了していない場合
                    // 入力欄をクリア
                    this.InputMessageView.ClearText();
                    // 初期化。
                    Init(SalonId.Value, SalonName);
                }
            }
        }

        async Task SendImageAsync(ImageSource imageSource)
        {
            if (SalonId == null)
            {
                // サロンが選ばれていないので、送信不可。
                Debug.WriteLine("サロンが選ばれていません！");
                return;
            }

            var imageSize = await DependencyService.Get<IImageService>().GetImageSizeAsync(imageSource);
            bool resize = false;
            double minLength = 0;
            if (_maxImageLength < Math.Min(imageSize.Width, imageSize.Height))
            {
                // リサイズする。
                resize = true;
                minLength = _maxImageLength;
                Debug.WriteLine("送信時リサイズ");
                Debug.WriteLine("幅:{0} 高さ:{1} => 短辺:{2}", imageSize.Width, imageSize.Height, minLength);
            }

            var imageBytes = await DependencyService.Get<IImageService>().ConvertImageSourceToBytesAsync(imageSource, resize, minLength);
            if (imageBytes == null)
            {
                // バイト列への変換に失敗したので、送信不可。
                Debug.WriteLine("バイト列への変換に失敗しました！");
                return;
            }

            // 写真を送信する。
            var postData = new Dictionary<string, string>()
            {
                {"messageType", "2"},
                {"salonId", SalonId.Value.ToString()}
            };
            var json = await APIManager.PostBytes("message_regist", "imageData", imageBytes, postData);
            if (json == null)
            {
                // 通信失敗
                DependencyService.Get<IToast>().Show("通信エラー");
            }
            else
            {
                // 通信成功
                if (IsInitCompleted)
                {
                    // 初期化が終了している場合
                    // 画面更新。
                    await RefreshTimeLineAsync(SalonId.Value, LastMessageId);
                }
                else
                {
                    // 初期化が終了していない場合
                    // 初期化。
                    Init(SalonId.Value, SalonName);
                }
            }
        }

        void PreStartRefreshTimeLineTimer()
        {
            // フラグを立てる。
            IsTimerContinue = true;
        }

        void StartRefreshTimeLineTimer()
        {
            // TODO Xamarin.Forms.Device.StartTimer is no longer supported. Use Microsoft.Maui.Dispatching.DispatcherExtensions.StartTimer instead. For more details see https://learn.microsoft.com/en-us/dotnet/maui/migration/forms-projects#device-changes
            Device.StartTimer(TimeSpan.FromSeconds(_refreshTimeLineIntervalSeconds), () =>
            {
                Task.Run(() =>
                {
                    if (SalonId == null)
                    {
                        return;
                    }

                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        Debug.WriteLine("タイムラインの定期更新を行います。");
                        await RefreshTimeLineAsync(SalonId.Value, LastMessageId);
                        Debug.WriteLine("タイムラインの定期更新が終了しました。");
                        if (IsTimerContinue)
                        {
                            StartRefreshTimeLineTimer();
                        }
                    });
                });
                // この関数を呼び出すことで毎回タイマーを生成し直しているので、タイマー自体は止める。
                return false;
            });
        }

        void EndRefreshTimeLineTimer()
        {
            // フラグを下ろす。
            IsTimerContinue = false;
        }

        async Task RefreshTimeLineAsync(int salonId, int lastMessageId)
        {
            if (IsRefreshRunning || IsOlderRunning)
            {
                return;
            }
            IsRefreshRunning = true;
            Debug.WriteLine("タイムライン更新開始します。");
            var postData = new Dictionary<string, string>()
            {
                {"salonId", salonId.ToString()},
                {"messageId", lastMessageId.ToString()}
            };
            var json = await APIManager.GET("message_newlist", postData);
            try
            {
                var response = JsonManager.Deserialize<ResponseMessageNewList>(json);
                if (response.Data.MessageList != null && response.Data.MessageList.Count > 0)
                {
                    DateTime oldDate, newDate;

                    var count = Model.MessageListViewItemsSource.Count;
                    if (count > 0)
                    {
                        // １つ以上メッセージが画面にあるとき
                        var model = Model.MessageListViewItemsSource[count - 1];
                        // modelから投稿日時を取得する。
                        oldDate = model.PostedDateTime;
                    }
                    else
                    {
                        oldDate = DateTime.MinValue;
                    }

                    // 古いほうから順番にソースに追加していくので逆順。
                    for (int i = response.Data.MessageList.Count - 1; i >= 0; --i)
                    {
                        var messageDetail = response.Data.MessageList[i];
                        // メッセージから日付を取得。
                        newDate = DateManager.ConvertMessageDateStringToDateTime(messageDetail.Date);
                        bool showDate = !DateManager.EqualsAsDay(newDate, oldDate);

                        var cellModel = await CreateMessageListCellModelAsync(messageDetail.MessageType.Value,
                                                                              messageDetail.Message,
                                                                              messageDetail.Image,
                                                                              messageDetail.IsSalon.Value,
                                                                              SalonName,
                                                                              DateManager.ConvertMessageDateStringToDateTime(messageDetail.Date),
                                                                              showDate);
                        Model.MessageListViewItemsSource.Add(cellModel);
                        if (i == 0)
                        {
                            // 最新のメッセージIdを取得
                            LastMessageId = messageDetail.MessageId.Value;
                        }
                        oldDate = newDate;
                    }
                }

                // 日付が変わっていた場合、"今日"表示の更新
                UpdateTodayDateString();

                if (IsInBottom)
                {
                    // 最新アイテムが見えている状況の場合、自動的にスクロールを最終位置に移動します。
                    this.MessageList.ScrollToBottom();
                }
                Debug.WriteLine("タイムライン更新終了しました。");
            }
            catch (Exception ex)
            {
                // いちいち画面にエラー表示は出しません。
                Debug.WriteLine("タイムライン更新エラー！:{0}", ex);
            }

            IsRefreshRunning = false;
        }

        async Task UpdateOlderMessagesAsync()
        {
            if (IsRefreshRunning || IsOlderRunning)
            {
                return;
            }

            if (IsGettableOlderMessages)
            {
                // 一番上のアイテムが出現した時、かつ過去のデータが存在する時、そのアイテムより前のアイテムを取得する。

                if (SalonId == null)
                {
                    // あり得ないことだが・・・。
                    return;
                }

                Debug.WriteLine("以前のデータを取得します。");

                App.customNavigationPage.IsRunning = true;
                IsOlderRunning = true;

                var id = SalonId.Value;
                var postData = new Dictionary<string, string>()
                {
                    {"salonId", id.ToString()},
                    {"messageId", FirstMessageId.ToString()}
                };
                var json = await APIManager.GET("message_list", postData);
                try
                {
                    var response = JsonManager.Deserialize<ResponseMessageList>(json);

                    // 古いメッセージが取得可能かのフラグを得る。
                    IsGettableOlderMessages = !response.Data.IsLast.Value;
                    if (response.Data.MessageList != null && response.Data.MessageList.Count > 0 && Model.MessageListViewItemsSource.Count > 0)
                    {
                        DateTime currentDate, olderDate;

                        // 新しいメッセージから順にindex = 0にinsertしていく。
                        for (int i = 0; i < response.Data.MessageList.Count; ++i)
                        {
                            var messageDetail = response.Data.MessageList[i];
                            // メッセージから日付を取得。
                            olderDate = DateManager.ConvertMessageDateStringToDateTime(messageDetail.Date);
                            currentDate = Model.MessageListViewItemsSource[0].PostedDateTime;

                            if (DateManager.EqualsAsDay(currentDate, olderDate))
                            {
                                // 日付が同じ場合、index = 0のセルの日付表示をオフにする。
                                Model.MessageListViewItemsSource[0].MLCDateLblPadding = new Thickness(0, 0, 0, 0);
                                Model.MessageListViewItemsSource[0].MLCDateLblHeight = 0;
                                Model.MessageListViewItemsSource[0].MLCDateLblText = null;
                            }
                            var cellModel = await CreateMessageListCellModelAsync(messageDetail.MessageType.Value,
                                                                                  messageDetail.Message,
                                                                                  messageDetail.Image,
                                                                                  messageDetail.IsSalon.Value,
                                                                                  SalonName,
                                                                                  olderDate,
                                                                                  true);
                            Model.MessageListViewItemsSource.Insert(0, cellModel);

                            if (i == response.Data.MessageList.Count - 1)
                            {
                                // 一番古いアイテムのメッセージIDを保持
                                FirstMessageId = messageDetail.MessageId.Value;
                            }

                            currentDate = olderDate;
                        }
                    }

                    // 日付が変わっていた場合、"今日"表示の更新
                    UpdateTodayDateString();

                    App.customNavigationPage.IsRunning = false;

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    App.customNavigationPage.IsRunning = false;
                    DependencyService.Get<IToast>().Show("通信エラー");
                }

                IsOlderRunning = false;
            }
        }

        async Task<MessageListCellModel> CreateMessageListCellModelAsync(int messageType, string message, ImageSource image, bool isSalon, string salonName, DateTime dateTime, bool showDate = false)
        {
            // バルーンサイズ,コンテンツに与えるマージンは先に計算しておく
            var balloonSize = Size.Zero;
            Thickness messageMargin = new Thickness(0, 0, 0, 0);
            if (messageType == 1)
            {
                // テキストメッセージ
                balloonSize = CalculateMessageBalloonSize(ref message, 14.0);
                messageMargin.Left = isSalon ? 12 + _balloonTailWidth : 12;
                messageMargin.Top = 10;
                messageMargin.Right = 0;
                messageMargin.Bottom = 0;
            }
            else if (messageType == 2)
            {
                // 画像
                balloonSize = await CalculateImageBalloonSizeAsync(image);
                messageMargin.Left = isSalon ? 10 + _balloonTailWidth : 10;
                messageMargin.Top = 10;
                messageMargin.Right = isSalon ? 10 : 10 + _balloonTailWidth;
                messageMargin.Bottom = 10;
            }

            var model = new MessageListCellModel()
            {
                MLCDateLblPadding = showDate ? new Thickness(0, 41, 0, 0) : new Thickness(0, 0, 0, 0),
                MLCDateLblText = showDate ? CreateDateString(dateTime) : null,
                MLCDateLblHeight = showDate ? 46 : 0,
                MLCSalonName = isSalon ? salonName : string.Empty,
                MLCTextVisible = (messageType == 1),
                MLCMessageText = messageType == 1 ? message : string.Empty,
                MLCMessageFontSize = 14.0,
                MLCImageVisible = (messageType == 2),
                MLCImageSource = image,
                MLCImageDownsampleWidth = messageType == 2 ? (balloonSize.Width - 10 * 2 - _balloonTailWidth) * DependencyService.Get<IScreenService>().GetScreenScale() : 0,
                MLCImageDownsampleHeight = messageType == 2 ? (balloonSize.Height - 10 * 2) * DependencyService.Get<IScreenService>().GetScreenScale() : 0,
                MLCBalloonViewWidth = balloonSize.Width,
                MLCBalloonViewHeight = balloonSize.Height,
                MLCBalloonViewHorizontalOptions = isSalon ? LayoutOptions.Start : LayoutOptions.End,
                MLCMessageMargin = messageMargin,
                MLCBalloonViewColor = ColorList.colorWhite,
                MLCBalloonViewTailWidth = _balloonTailWidth,
                MLCBalloonViewTailDirection = isSalon ? BalloonView.Direction.Left : BalloonView.Direction.Right,
                PostedDateTime = dateTime,
                IsLeftTimeVisible = !isSalon,
                IsRightTimeVisible = isSalon,
                LayoutPadding = isSalon ? new Thickness(17, 0, 0, 0) : new Thickness(0, 0, 17, 0),
            };

            return model;
        }

        string CreateDateString(DateTime date, bool forceGetDateString = false)
        {
            string dateString = null;
            var now = DependencyService.Get<IDateTimeService>().GetNow();
            if (!forceGetDateString && DateManager.EqualsAsDay(date, now))
            {
                // 今日の場合はテキスト「今日」
                dateString = "今日";
            }
            else
            {
                // 今日より前の場合は「00/00」の形で日付を表示する。
                dateString = date.ToString("MM/dd");
            }

            return dateString;
        }

        void UpdateTodayDateString()
        {
            var now = DependencyService.Get<IDateTimeService>().GetNow();
            if (DateManager.EqualsAsDay(InittedDate, now))
            {
                // 初期化した日と今が同じ日の場合は何もしない。
                return;
            }

            if (Model.MessageListViewItemsSource != null && Model.MessageListViewItemsSource.Count > 0)
            {
                for (int i = Model.MessageListViewItemsSource.Count - 1; i >= 0; --i)
                {
                    var item = Model.MessageListViewItemsSource[i];
                    if (item.MLCDateLblText == "今日" && !DateManager.EqualsAsDay(item.PostedDateTime, now))
                    {
                        // "今日"と表示されているのを、その日の日付表示に修正。
                        Model.MessageListViewItemsSource[i].MLCDateLblText = CreateDateString(item.PostedDateTime, true);
                        break;
                    }
                }
            }
        }
    }
}
