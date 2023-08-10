using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Newtonsoft.Json;
using IO.Swagger.Model;
using System.IO;

using Plugin.Media;

namespace Think_App
{
    public partial class MyBeautyBlogListPage : ContentPage
    {
        MyBeautyBlogListPageViewModel myBeautyBlogListPageViewModel;
        ObservableCollection<MyBeautyBlogModel> itemsSources;
        int index;
        //List<InlineResponse20013DataMyBeautyBlogList> contents = null;
        ResponseMyBeautyBlogList contents = null;
        int? selectingCtgId = null;
        MediaPluginManager _mediaPluginManager;

        public MyBeautyBlogListPage()
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "");

            myBeautyBlogListPageViewModel = new MyBeautyBlogListPageViewModel();
            App.customNavigationPage.IsRunning = true;
            _mediaPluginManager = new MediaPluginManager();
            // ---------------GridViewのサイズ指定とitem追加-------------------
            myBeautyBlogListPageViewModel.GridViewRect = new Rectangle(0, 87, 1, ScaleManager.ScreenHeight - 152);
            myBeautyBlogListPageViewModel.ItemHeight = 100 * ScaleManager.Scale;
            myBeautyBlogListPageViewModel.ItemWidth = 100 * ScaleManager.Scale;
            myBeautyBlogListPageViewModel.ColumnSpacing = 5 * ScaleManager.Scale;
            myBeautyBlogListPageViewModel.RowSpacing = 10 * ScaleManager.Scale;
            myBeautyBlogListPageViewModel.MyBlogPlusListViewRect = new Rectangle(
                17,
                87 + myBeautyBlogListPageViewModel.ItemHeight + myBeautyBlogListPageViewModel.RowSpacing,
                201,
                98);

            //if (Device.RuntimePlatform == Device.Android)
            //{
            //	// GridView androidの表示iosとずれているので調整
            //	// android RowSpacingとColumunSpacingうまく動いていない可能性あり
            //	myBeautyBlogListPageViewModel.ItemWidth *= ScaleManager.AndroidDensity;
            //	myBeautyBlogListPageViewModel.ItemHeight *= ScaleManager.AndroidDensity;
            //	myBeautyBlogListPageViewModel.ColumnSpacing *= ScaleManager.AndroidDensity;
            //	myBeautyBlogListPageViewModel.RowSpacing *= ScaleManager.AndroidDensity;
            //	// Densityかけただけでは横が切れてしまう
            //	myBeautyBlogListPageViewModel.ItemWidth *= 1.1;
            //}
            itemsSources = new ObservableCollection<MyBeautyBlogModel>();

            this.Appearing += (sender, e) =>
            {
                itemsSources.Clear();
                itemsSources.Add(new MyBeautyBlogModel()
                {
                    GridViewLabelRect = new Rectangle(0.5, 1, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize),
                    ImgSouce = "icon_plus.png",
                    DateStr = "",
                    GridViewImgRect = new Rectangle(0.5, 0.5, 0.23, 0.23),
                    BtnIsVisible = false,
                    GridViewBgColor = ColorList.colorMain
                });
                index = 0;
                Task.Run(async () =>
                {
                    await GetMyBeautyBlogList();
                });
            };
            // ------------------------------------------------------------

            ObservableCollection<MyBlogMenuSelectList> items = new ObservableCollection<MyBlogMenuSelectList>();
            items.Add(new MyBlogMenuSelectList("ギャラリーから写真を選択"));
            items.Add(new MyBlogMenuSelectList("写真を撮影する"));
            myBeautyBlogListPageViewModel.MyBlogPlusListViewIsVisible = false;
            this.MyBlogPlusListView.ItemsSource = items;

            ObservableCollection<MyBlogMenuSelectList> ctgItems = new ObservableCollection<MyBlogMenuSelectList>();
            ctgItems.Add(new MyBlogMenuSelectList("全て"));
            ctgItems.Add(new MyBlogMenuSelectList("ヘア"));
            ctgItems.Add(new MyBlogMenuSelectList("ネイル"));
            ctgItems.Add(new MyBlogMenuSelectList("まつエク"));
            ctgItems.Add(new MyBlogMenuSelectList("エステ"));
            ctgItems.Add(new MyBlogMenuSelectList("その他"));
            MyBlogCtgListView.ItemsSource = ctgItems;
            myBeautyBlogListPageViewModel.CtgText = "全て";

            this.GrdView.FlowItemsSource = itemsSources;
            this.BindingContext = myBeautyBlogListPageViewModel;

            this.GrdView.FlowItemTapped += async (sender, e) =>
            {
                if (e.Item == null ||
                !(e.Item is MyBeautyBlogModel bilog)
                || bilog == null) return;

                if (bilog.ImgSouce == "icon_plus.png")
                {

                    if (Device.RuntimePlatform == Device.iOS)
                    {

                        var ret = await DisplayActionSheet(null, "キャンセル", null, "ギャラリーから写真を選択", "写真を撮影する");

                        if (ret == "ギャラリーから写真を選択")
                        {

                            if (!App.ProcessManager.CanInvoke())
                            {
                                return;
                            }
                            var permissionResult = await _mediaPluginManager.PickPhotoAvailable();
                            if (!permissionResult) return;
                            SelectGallery();
                        }
                        else if (ret == "写真を撮影する")
                        {
                            if (!App.ProcessManager.CanInvoke())
                            {
                                return;
                            }
                            var permissionResult = await _mediaPluginManager.CheckPermission(Plugin.Permissions.Abstractions.Permission.Camera);
                            if (!permissionResult) return;
                            SelectTakePicture();
                        }
                    }
                    else if (Device.RuntimePlatform == Device.Android)
                    {
                        if (myBeautyBlogListPageViewModel.MyBlogPlusListViewIsVisible == true)
                        {
                            myBeautyBlogListPageViewModel.MyBlogPlusListViewIsVisible = false;
                        }
                        else
                        {
                            myBeautyBlogListPageViewModel.MyBlogPlusListViewIsVisible = true;
                        }
                    }
                }
                else
                {
                    // すでに登録済みのmy美Log選択
                    await App.customNavigationPage.PushAsync(new MyBeautyBlogInfoPage(bilog));
                }
            };


            // androidのみで下記使用
            this.MyBlogPlusListView.ItemSelected += async (sender, e) =>
            {
                if (!App.ProcessManager.CanInvoke())
                {
                    return;
                }
                if (((MyBlogMenuSelectList)e.SelectedItem).MyBlogMenuSelectListText == "ギャラリーから写真を選択")
                {
                    myBeautyBlogListPageViewModel.MyBlogPlusListViewIsVisible = false;
                    this.MyBlogPlusListView.SelectedItem = null;
                    var permissionResult = await _mediaPluginManager.CheckPermission(Plugin.Permissions.Abstractions.Permission.MediaLibrary);
                    if (!permissionResult) return;
                    SelectGallery();
                }
                else
                {
                    myBeautyBlogListPageViewModel.MyBlogPlusListViewIsVisible = false;
                    this.MyBlogPlusListView.SelectedItem = null;
                    var permissionResult = await _mediaPluginManager.CheckPermission(Plugin.Permissions.Abstractions.Permission.Camera);
                    if (!permissionResult) return;
                    SelectTakePicture();
                }
            };

            this.CtgSelectBtn.Clicked += (sender, e) =>
            {
                if (myBeautyBlogListPageViewModel.CtgListIsVisible)
                {
                    myBeautyBlogListPageViewModel.CtgListIsVisible = false;
                }
                else
                {
                    myBeautyBlogListPageViewModel.CtgListIsVisible = true;
                }
            };

            this.MyBlogCtgListView.ItemSelected += (sender, e) =>
            {
                if (App.ProcessManager.CanInvoke())
                {
                    myBeautyBlogListPageViewModel.CtgText = ((MyBlogMenuSelectList)e.SelectedItem).MyBlogMenuSelectListText;
                    if (myBeautyBlogListPageViewModel.CtgText == "ヘア")
                    {
                        selectingCtgId = 1;
                    }
                    else if (myBeautyBlogListPageViewModel.CtgText == "ネイル")
                    {
                        selectingCtgId = 2;
                    }
                    else if (myBeautyBlogListPageViewModel.CtgText == "まつエク")
                    {
                        selectingCtgId = 3;
                    }
                    else if (myBeautyBlogListPageViewModel.CtgText == "エステ")
                    {
                        selectingCtgId = 4;
                    }
                    else if (myBeautyBlogListPageViewModel.CtgText == "その他")
                    {
                        selectingCtgId = 0;
                    }
                    else
                    {
                        selectingCtgId = null;
                    }

                    this.MyBlogCtgListView.SelectedItem = null;
                    myBeautyBlogListPageViewModel.CtgListIsVisible = false;

                    itemsSources.Clear();
                    //itemsSources.Add(new MyBeautyBlogModel(null, true)); // +ボタン
                    itemsSources.Add(new MyBeautyBlogModel()
                    {
                        GridViewLabelRect = new Rectangle(0.5, 1, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize),
                        ImgSouce = "icon_plus.png",
                        DateStr = "",
                        GridViewImgRect = new Rectangle(0.5, 0.5, 0.23, 0.23),
                        BtnIsVisible = false,
                        GridViewBgColor = ColorList.colorMain
                    });
                    if (contents.Data.List != null)
                    {
                        foreach (var val in contents.Data.List)
                        {
                            if (val.ThumbnailImage != null && val.Category == selectingCtgId || selectingCtgId == null)
                            {
                                //itemsSources.Add(new MyBeautyBlogModel(val, true));
                                itemsSources.Add(new MyBeautyBlogModel()
                                {
                                    BtnIsVisible = false,
                                    MyBeautyBlogId = (int)val.MyBeautyBlogId,
                                    ImgSouce = val.ThumbnailImage.Path,
                                    DateStr = val.Date.Replace("-", "/"),
                                    DateStringrShort = val.Date.Replace("-", "/").Substring(0, 10),
                                    CategoryValue = val.Category.Value,
                                    Title = val.Title,
                                    Description = val.Description,
                                    GridViewImgRect = new Rectangle(0, 0, 1, 1),
                                    GridViewLabelRect = new Rectangle(0.5, 1, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize),
                                    GridViewLabelFontSize = 12 * ScaleManager.Scale,
                                    GridViewLabelHeightRequest = 14 * ScaleManager.Scale,
                                    GridViewLabelWidthRequest = 69 * ScaleManager.Scale
                                });
                            }
                        }
                    }
                    App.ProcessManager.OnComplete();
                }
            };
        }

        /*
		// もっと見るボタン用 処理 現状使わないからコメント中
		void AddMoreLookBtn()
		{
			//itemsSources.Add(new MyBeautyBlogModel(null, false, false, true));
			itemsSources.Add(new MyBeautyBlogModel(null, false, false, true));
			itemsSources.Add(new MyBeautyBlogModel(null, false, true, false));
		}

		void delMoreLookBtn()
		{
			for (var i = 0; i < 2; i++)
			{
				itemsSources.RemoveAt(itemsSources.Count - 1);
			}
		}

		async void MoreLookBtnClick(object sender, EventArgs e)
		{
			delMoreLookBtn();
			index++;
			GetMyBeautyBlogList();
		}
		*/

        async Task GetMyBeautyBlogList()
        {
            // もっと見るボタン用 処理 現状使わないからコメント中
            //var parameters = new Dictionary<string, string> { { "index", index.ToString() } };
            //var json = await APIManager.GET("mybeautyblog_list", parameters);
            var json = await APIManager.GET("mybeautyblog_list");
            if (json == null)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    DependencyService.Get<IToast>().Show("通信エラー");
                });
            }
            else
            {
                contents = JsonConvert.DeserializeObject<ResponseMyBeautyBlogList>(json);
                if (contents.Data.List != null)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        foreach (var val in contents.Data.List)
                        {
                            if (val.ThumbnailImage != null && val.Category == selectingCtgId || selectingCtgId == null)
                            {
                                //itemsSources.Add(new MyBeautyBlogModel(val, true));
                                itemsSources.Add(new MyBeautyBlogModel()
                                {
                                    BtnIsVisible = false,
                                    MyBeautyBlogId = (int)val.MyBeautyBlogId,
                                    ImgSouce = val.ThumbnailImage.Path,
                                    DateStr = val.Date.Replace("-", "/"),
                                    DateStringrShort = val.Date.Replace("-", "/").Substring(0, 10),
                                    CategoryValue = val.Category.Value,
                                    Title = val.Title,
                                    Description = val.Description,
                                    GridViewImgRect = new Rectangle(0, 0, 1, 1),
                                    GridViewLabelRect = new Rectangle(0.5, 1, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize),
                                    GridViewLabelFontSize = 12 * ScaleManager.Scale,
                                    GridViewLabelHeightRequest = 14 * ScaleManager.Scale,
                                    GridViewLabelWidthRequest = 69 * ScaleManager.Scale
                                });
                            }
                        }
                        // もっと見るボタン用 処理 現状使わないからコメント中
                        //if (response.Data.IsEnd == false)
                        //{
                        //	AddMoreLookBtn();
                        //}
                        this.GrdView.FlowItemsSource = itemsSources;
                    });
                }
            }
            Device.BeginInvokeOnMainThread(() =>
            {
                App.customNavigationPage.IsRunning = false;
            });
        }

        async void SelectGallery()
        {


            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("写真へのアクセスができません", "設定画面から写真へのアクセスを有効にしてください。", "閉じる");
                App.ProcessManager.OnComplete();
                return;
            }

            Plugin.Media.Abstractions.MediaFile file;
            Device.BeginInvokeOnMainThread(async () =>
            {
                file = await CrossMedia.Current.PickPhotoAsync();
                if (file == null)
                {
                    App.ProcessManager.OnComplete();
                    return;
                }

                App.customNavigationPage.IsRunning = true;

                await Task.Run(() =>
                {
                    var imageSouce = DependencyService.Get<IImageService>().GetOrientationAdjustedImageSourceReduction(file.Path);

                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await App.customNavigationPage.PushAsync(new MyBeautyBlogEditPage(imageSouce));
                    });
                    App.ProcessManager.OnComplete();
                });
            });
        }

        async void SelectTakePicture()
        {
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                App.ProcessManager.OnComplete();
                await DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            Device.BeginInvokeOnMainThread(async () =>
            {
                var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    DefaultCamera = Plugin.Media.Abstractions.CameraDevice.Rear,
                    AllowCropping = false // トリミングするかどうか
                });

                if (file == null)
                {
                    return;
                }
                App.customNavigationPage.IsRunning = true;

                await Task.Run(() =>
                {
                    var imageSouce = DependencyService.Get<IImageService>().GetOrientationAdjustedImageSourceReduction(file.Path);

                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await App.customNavigationPage.PushAsync(new MyBeautyBlogEditPage(imageSouce));
                    });
                    App.ProcessManager.OnComplete();
                });
            });

            App.ProcessManager.OnComplete();
        }

    }
}
