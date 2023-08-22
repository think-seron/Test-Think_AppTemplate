using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using IO.Swagger.Model;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
    public class ConfigListVIewModel : ViewModelBase
    {
        private bool _bilog;
        public ResponseHome _responseHome;
        public ConfigListVIewModel(ResponseHome responseHome)
        {
            _responseHome = responseHome;
            SetList();
            SetListSize();
        }

        void SetListSize()
        {
            ListWidth = ScaleManager.ScreenWidth / 2;
            var ct = ConfigListItems.Count();
            ListHeight = 45.0 * ct + 10;

            ListShadowWidth = ListWidth + 1.0;
            ListShadowHeight = ListHeight + 2.0;

        }
        const int accountSetting = 1, transferId = 2, privacy = 3, terms = 4, releaseSetting = 5, pushNotifiSetting = 6, license = 7, deleteAccount = 8;
        void SetList()
        {
            ConfigListItems = new ObservableCollection<ConfigListItem>();
            ConfigListItems.Add(new ConfigListItem("アカウント情報", accountSetting));
            ConfigListItems.Add(new ConfigListItem("引き継ぎコード情報", transferId));
            ConfigListItems.Add(new ConfigListItem("プライバシーポリシー", privacy));
            ConfigListItems.Add(new ConfigListItem("利用規約", terms));
            ConfigListItems.Add(new ConfigListItem("ライセンス", license));
            var bilogAvailable = _responseHome.Data.HomeSalonInfo.BilogAvailable == null
                                              ? false
                                              : (bool)_responseHome.Data.HomeSalonInfo.BilogAvailable;
            if (bilogAvailable)
            {
                ConfigListItems.Add(new ConfigListItem("公開設定", releaseSetting));
            }
            ConfigListItems.Add(new ConfigListItem("プッシュ通知設定", pushNotifiSetting, _responseHome));
            ConfigListItems.Add(new ConfigListItem("退会", deleteAccount));
        }

        public double ListShadowWidth { get; set; }
        private double _ListShadowHeight;
        public double ListShadowHeight
        {
            get { return _ListShadowHeight; }
            set
            {
                if (_ListShadowHeight != value)
                {
                    _ListShadowHeight = value;
                    OnPropertyChanged("ListShadowHeight");
                }
            }
        }

        public double ListWidth { get; set; }
        private double _ListHeight;
        public double ListHeight
        {
            get { return _ListHeight; }
            set
            {
                if (_ListHeight != value)
                {
                    _ListHeight = value;
                    OnPropertyChanged("ListHeight");
                }
            }
        }

        private ObservableCollection<ConfigListItem> _ConfigListItems;
        public ObservableCollection<ConfigListItem> ConfigListItems
        {
            get
            {
                if (_ConfigListItems == null)
                {
                    _ConfigListItems = new ObservableCollection<ConfigListItem>();
                }
                return _ConfigListItems;
            }
            set
            {
                if (_ConfigListItems == null)
                {
                    _ConfigListItems = new ObservableCollection<ConfigListItem>();
                }
                if (_ConfigListItems != value)
                {
                    _ConfigListItems = value;
                    OnPropertyChanged("ConfigListItems");
                }
            }
        }

        private ConfigListItem _ConfigSelectedItem;
        public ConfigListItem ConfigSelectedItem
        {
            get { return _ConfigSelectedItem; }
            set
            {
                _ConfigSelectedItem = value;
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (_ConfigSelectedItem != null)
                        _ConfigSelectedItem.selected.Execute(null);
                    
                    OnPropertyChanged("ConfigSelectedItem");
                    ConfigSelectedItem = null;
                });
            }
        }

        public class ConfigListItem
        {
            public Command selected { get; set; }
            public ResponseHome _responseHome;
            public ConfigListItem(string title, int id, ResponseHome responseHome = null)
            {
                ItemTitle = title;
                ItemID = id;
                ItemFontColor = ColorList.colorFont;
                ItemFontSize = 14;
                _responseHome = responseHome;
                selected = new Command(async () => { await SelectedMethod(); });
            }
            public int ItemID { get; set; }
            public string ItemTitle { get; set; }
            public Color ItemFontColor { get; set; }
            public double ItemFontSize { get; set; }
            public async Task SelectedMethod()
            {

                if (!(App.customNavigationPage.CurrentPage.BindingContext is HomeViewModel homeVM)) return;
                homeVM.ConfigVisible = true;
                //DependencyService.Get<IToast>().Show(ItemTitle);
                switch (ItemID)
                {
                    case accountSetting:
                        await App.customNavigationPage.PushAsync(new AccountRegistration(2));
                        break;
                    case transferId:
                        await App.customNavigationPage.PushAsync(new TransferIdPage());
                        break;
                    case privacy:
                        await App.customNavigationPage.PushAsync(new WebViewPage(WebViewPage.webViewType.PrivacyPolicy));
                        break;
                    case terms:
                        await App.customNavigationPage.PushAsync(new WebViewPage(WebViewPage.webViewType.TermsOfService));
                        break;
                    case releaseSetting:
                        App.customNavigationPage.IsRunning = true;
                        var json = await APIManager.GET("setting_publish");
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            if (json == null)
                            {
                                DependencyService.Get<IToast>().Show("通信エラー");
                                App.customNavigationPage.IsRunning = false;
                            }
                            else
                            {
                                await App.customNavigationPage.PushAsync(new OpeningSetting(json));
                            }
                        });
                        break;
                    case pushNotifiSetting:
                        App.customNavigationPage.IsRunning = true;
                        var setting_json = await APIManager.GET("setting_notification");
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            if (setting_json == null)
                            {
                                DependencyService.Get<IToast>().Show("通信エラー");
                                App.customNavigationPage.IsRunning = false;
                            }
                            else
                            {
                                await App.customNavigationPage.PushAsync(new PushNotificationSetting(_responseHome, setting_json));
                            }
                        });
                        break;
                    case license:
                        await App.customNavigationPage.PushAsync(new WebViewPage(WebViewPage.webViewType.License));
                        break;
                    case deleteAccount:
                        var vm = new WithdrawalPageViewModel();
                        await App.customNavigationPage.PushAsync(new WithdrawalPage { BindingContext = vm });
                        break;
                    default:
                        break;
                }
            }
        }
    }
}