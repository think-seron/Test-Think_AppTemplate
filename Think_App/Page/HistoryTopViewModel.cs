using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using IO.Swagger.Model;
using Xamarin.Forms;
namespace Think_App
{
    public class HistoryTopViewModel : ViewModelBase
    {
        public HistoryTopViewModel(bool bilogAvailable)
        {
            BilogAvailable = bilogAvailable;

            HistoryItemSouce = new ObservableCollection<ListVIewHistoryCellViewModel>();
            SetHistoryList();
            BeautyBlogItem1 = new BeautyBlogTileItemViewModel();
            BeautyBlogItem2 = new BeautyBlogTileItemViewModel();
            BeautyBlogItem3 = new BeautyBlogTileItemViewModel();

            BeautyItems = new BeautyBlogTileItemViewModel[] { BeautyBlogItem1, BeautyBlogItem2, BeautyBlogItem3 };

            SetBeautyLogItems();

            SetCommand();
        }

        public void SetHistorySizes()
        {
            ListViewHeight = HistoryItemSouce.Count * HistoryItemSouce[0].HeightSize;
            BeautyBlogHeight = ScaleManager.SizeSet(130);

        }

        public void SetCommand()
        {
            HistoryBtnClicked = new Command(async () =>
            {
                if (App.ProcessManager.CanInvoke())
                {
                    await App.customNavigationPage.PushAsync(new HistoryListPage());
                    App.ProcessManager.OnComplete();
                }
            });
            BeautyBlogBtnClicked = new Command(async () =>
            {
                if (App.ProcessManager.CanInvoke())
                {
                    await App.customNavigationPage.PushAsync(new MyBeautyBlogListPage());
                    //await App.customNavigationPage.PushAsync(new MyBeautyBlogListNewPage { BindingContext = new MyBeautyBlogListNewPageViewModel() });
                    App.ProcessManager.OnComplete();
                }
            });
        }

        public void SetHistoryList()
        {
            Task.Run(async () =>
            {
                Dictionary<string, string> parameters = new Dictionary<string, string> { { "index", "0" } };
                try
                {
                    var json = await APIManager.GET("history_treatmentlist", parameters);

                    if (json == null)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            DependencyService.Get<IToast>().Show("通信エラー");
                            Device.BeginInvokeOnMainThread(() =>
                                                           HistoryItemSouce.Add(new ListVIewHistoryCellViewModel(0)));
                        });
                    }
                    else
                    {
                        var response = JsonManager.Deserialize<ResponseTreatmentHistoryList>(json);
                        System.Diagnostics.Debug.WriteLine(" response History:" + response.Data);

                        if (response.Data.List != null && response.Data.List.Count > 0)
                        {
                            int loop = 0;
                            foreach (var val in response.Data.List)
                            {
                                loop++;
                                Device.BeginInvokeOnMainThread(() =>
                                                               HistoryItemSouce.Add(new ListVIewHistoryCellViewModel(1, val)));

                                if (loop >= 3)
                                {
                                    break;
                                }
                            }
                            if (response.Data.List.Count > 3)
                            {
                                HistoryBtnVisible = true;
                            }
                        }
                        else
                        {
                            Device.BeginInvokeOnMainThread(() =>
                                                           HistoryItemSouce.Add(new ListVIewHistoryCellViewModel(2)));
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("ex : " + ex);
                    Device.BeginInvokeOnMainThread(() =>
                                                           HistoryItemSouce.Add(new ListVIewHistoryCellViewModel(0)));

                }
                Device.BeginInvokeOnMainThread(() =>
                                               SetHistorySizes());
            });
        }

        public void SetBeautyLogItems()
        {
            Task.Run(async () =>
            {
                Dictionary<string, string> parameters = new Dictionary<string, string> { { "index", "0" } };
                try
                {
                    var json = await APIManager.GET("mybeautyblog_list", parameters);
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

                        var resp = JsonManager.Deserialize<ResponseMyBeautyBlogList>(json);
                        System.Diagnostics.Debug.WriteLine("SetBeautyLogItems    resp  :" + resp);
                        if (resp.Data.List != null && resp.Data.List.Count > 0)
                        {
                            int loop = 0;
                            BeautyBlogNotEmpty = true;
                            foreach (var val in resp.Data.List)
                            {
                                await BeautyItems[loop].UpdateData(val);
                                loop++;
                                if (loop >= 3)
                                {
                                    break;
                                }
                            }
                            while (loop < 3)
                            {

                                await BeautyItems[loop].ClearData();

                                loop++;
                            }

                            if (resp.Data.List.Count > 3)
                            {
                                BlogButonTxt = "もっと見る";
                            }
                            else
                            {
                                BlogButonTxt = "My美Logへ";
                            }
                            BeautyBlogEmptyText = null;
                        }
                        else
                        {
                            BeautyBlogNotEmpty = false;
                            BeautyBlogEmptyText = "My美Logが登録されていません";
                            BlogButonTxt = "My美Logへ";
                        }
                    }
                }
                catch (Exception ex)
                {
                    BeautyBlogNotEmpty = false;
                    System.Diagnostics.Debug.WriteLine("ex bilog :" + ex);
                    BeautyBlogEmptyText = "My美Logのデータが読み込めませんでした。";
                }
            });
        }

        private bool _BilogAvailable;
        public bool BilogAvailable
        {
            get => _BilogAvailable;
            set
            {
                if (_BilogAvailable != value)
                {
                    _BilogAvailable = value;
                    OnPropertyChanged(nameof(BilogAvailable));
                }
            }
        }

        private string _BeautyBlogEmptyText;
        public string BeautyBlogEmptyText
        {
            get { return _BeautyBlogEmptyText; }
            set
            {
                if (_BeautyBlogEmptyText != value)
                {
                    _BeautyBlogEmptyText = value;
                    OnPropertyChanged("BeautyBlogEmptyText");
                }
            }
        }
        private string _BlogButonTxt;
        public string BlogButonTxt
        {
            get { return _BlogButonTxt; }
            set
            {
                if (_BlogButonTxt != value)
                {
                    _BlogButonTxt = value;
                    OnPropertyChanged("BlogButonTxt");
                }
            }
        }

        public BeautyBlogTileItemViewModel[] BeautyItems;



        private double _ListViewHeight;
        public double ListViewHeight
        {
            get { return _ListViewHeight; }
            set
            {
                if (_ListViewHeight != value)
                {
                    _ListViewHeight = value;
                    OnPropertyChanged("ListViewHeight");
                }
            }
        }

        private bool _HistoryBtnVisible;
        public bool HistoryBtnVisible
        {
            get { return _HistoryBtnVisible; }
            set
            {
                if (_HistoryBtnVisible != value)
                {
                    _HistoryBtnVisible = value;
                    OnPropertyChanged("HistoryBtnVisible");
                }
            }
        }

        private ObservableCollection<ListVIewHistoryCellViewModel> _HistoryItemSouce;
        public ObservableCollection<ListVIewHistoryCellViewModel> HistoryItemSouce
        {
            get { return _HistoryItemSouce; }
            set
            {
                if (_HistoryItemSouce != value)
                {
                    _HistoryItemSouce = value;
                    OnPropertyChanged("HistoryItemSouce");
                }
            }
        }

        private ListVIewHistoryCellViewModel _HisotyrSelectedItem;
        public ListVIewHistoryCellViewModel HisotyrSelectedItem
        {
            get { return _HisotyrSelectedItem; }
            set
            {
                //if (_HisotyrSelectedItem != value)
                //{
                _HisotyrSelectedItem = value;

                SelectHistory(_HisotyrSelectedItem);

                _HisotyrSelectedItem = null;
                OnPropertyChanged("HisotyrSelectedItem");
                //}
            }
        }
        void SelectHistory(ListVIewHistoryCellViewModel item)
        {
            if (App.ProcessManager.CanInvoke())
            {
                System.Diagnostics.Debug.WriteLine("selected ");
                if (item.StoreName == "施術履歴はありません")
                {
                    App.ProcessManager.OnComplete();
                    return;
                }

                //var detailItem = new ListViewHistoryViewModel()
                //{
                //	StoreName = item.StoreName,
                //	Time = item.Time,
                //	StoreNameSize = item.StoreNameSize,
                //	TreatmentHistoryId = item.TreatmentHistoryId,
                //	Stylist = item.Stylist,
                //	TreatmentDescription = item.TreatmentDescription,
                //};
                Device.BeginInvokeOnMainThread(async () =>
                                               await App.customNavigationPage.PushAsync(new HistoryDetailPage(item)));
                App.ProcessManager.OnComplete();
            }
        }

        private Command _HistoryBtnClicked;
        public Command HistoryBtnClicked
        {
            get { return _HistoryBtnClicked; }
            set
            {
                if (_HistoryBtnClicked != value)
                {
                    _HistoryBtnClicked = value;
                    OnPropertyChanged("HistoryBtnClicked");
                }
            }
        }

        private double _BeautyBlogHeight;
        public double BeautyBlogHeight
        {
            get { return _BeautyBlogHeight; }
            set
            {
                if (_BeautyBlogHeight != value)
                {
                    _BeautyBlogHeight = value;
                    OnPropertyChanged("BeautyBlogHeight");
                }
            }
        }

        private bool _BeautyBlogEmpty;
        public bool BeautyBlogEmpty
        {
            get { return _BeautyBlogEmpty; }
            set
            {
                if (_BeautyBlogEmpty != value)
                {
                    _BeautyBlogEmpty = value;
                    System.Diagnostics.Debug.WriteLine("BeautyBlogEmpty  :" + _BeautyBlogEmpty);
                    OnPropertyChanged("BeautyBlogEmpty");
                }
            }
        }


        private bool _BeautyBlogNotEmpty;
        public bool BeautyBlogNotEmpty
        {
            get { return _BeautyBlogNotEmpty; }
            set
            {
                _BeautyBlogNotEmpty = value;
                BeautyBlogEmpty = !value;
                OnPropertyChanged("BeautyBlogNotEmpty");
                OnPropertyChanged("BeautyBlogEmpty");
            }
        }

        private BeautyBlogTileItemViewModel _BeautyBlogItem1;
        public BeautyBlogTileItemViewModel BeautyBlogItem1
        {
            get { return _BeautyBlogItem1; }
            set
            {
                if (_BeautyBlogItem1 != value)
                {
                    _BeautyBlogItem1 = value;
                    OnPropertyChanged("BeautyBlogItem1");
                }
            }
        }
        private BeautyBlogTileItemViewModel _BeautyBlogItem2;
        public BeautyBlogTileItemViewModel BeautyBlogItem2
        {
            get { return _BeautyBlogItem2; }
            set
            {
                if (_BeautyBlogItem2 != value)
                {
                    _BeautyBlogItem2 = value;
                    OnPropertyChanged("BeautyBlogItem2");
                }
            }
        }

        private BeautyBlogTileItemViewModel _BeautyBlogItem3;
        public BeautyBlogTileItemViewModel BeautyBlogItem3
        {
            get { return _BeautyBlogItem3; }
            set
            {
                if (_BeautyBlogItem3 != value)
                {
                    _BeautyBlogItem3 = value;
                    OnPropertyChanged("BeautyBlogItem3");
                }
            }
        }



        private Command _BeautyBlogBtnClicked;
        public Command BeautyBlogBtnClicked
        {
            get { return _BeautyBlogBtnClicked; }
            set
            {
                if (_BeautyBlogBtnClicked != value)
                {
                    _BeautyBlogBtnClicked = value;
                    OnPropertyChanged("BeautyBlogBtnClicked");
                }
            }
        }

        public Command HistoryTapCommand => new Command((obj) =>
        {
            if (!(obj is ListVIewHistoryCellViewModel item)) return;
            SelectHistory(item);
        });
    }
}