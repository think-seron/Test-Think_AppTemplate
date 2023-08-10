using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;
using Newtonsoft.Json;
using IO.Swagger.Model;
using System.Threading.Tasks;
using Plugin.Media;
using System.Collections.ObjectModel;
using System.Linq;

namespace Think_App
{
    public partial class MyBeautyBlogEditPage : ContentPage
    {
        MyBeautyBlogEditPageViewModel myBeautyBlogEditPageViewModel;
        MyBeautyBlogInfoPage myBeautyBlogInfoPage;
        MyBeautyBlogModel oldModel;
        public MyBeautyBlogEditPage(ImageSource imageSouce, MyBeautyBlogModel model = null, MyBeautyBlogInfoPage page = null)
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "");
            myBeautyBlogEditPageViewModel = new MyBeautyBlogEditPageViewModel();
            oldModel = model;
            myBeautyBlogInfoPage = page;

            var dp = new DatePicker();
            if (model == null)
            {
                myBeautyBlogEditPageViewModel.ImgSouce = imageSouce;
                // 初期表示の曜日用に使用
                DayOfWeekCheck(dp);
            }
            else
            {
                myBeautyBlogEditPageViewModel.ImgSouce = model.ImgSouce;
                try
                {
                    var yearStr = model.DateStr.Substring(0, 4);
                    var monthStr = model.DateStr.Substring(5, 2);
                    var dayStr = model.DateStr.Substring(8, 2);
                    // 初期表示に使用
                    dp.Date = new DateTime(int.Parse(yearStr), int.Parse(monthStr), int.Parse(dayStr));
                    //DP.Date = new DateTime(int.Parse(yearStr), int.Parse(monthStr), int.Parse(dayStr));
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("error ::" + e);
                }
                DayOfWeekCheck(dp);

                if (model.CategoryValue == 1)
                {
                    myBeautyBlogEditPageViewModel.HairBtnCheck = true;
                }
                else if (model.CategoryValue == 2)
                {
                    myBeautyBlogEditPageViewModel.NailBtnCheck = true;
                }
                else if (model.CategoryValue == 3)
                {
                    myBeautyBlogEditPageViewModel.EyelashBtnCheck = true;
                }
                else if (model.CategoryValue == 4)
                {
                    myBeautyBlogEditPageViewModel.EstheticBtnCheck = true;
                }
                myBeautyBlogEditPageViewModel.EntryTxt = model.Title;
                myBeautyBlogEditPageViewModel.CustomEditorTxt = model.Description;
                myBeautyBlogEditPageViewModel.myBeautyBlogId = model.MyBeautyBlogId;
            }

            // DatePickerのMargin Binding効かなかったので値直指定
            //DP.Margin = new Thickness(0, 0, 0, 0);
            //if (Device.RuntimePlatform == Device.iOS)
            //{
            //	DP.Margin = new Thickness(0, 7.5, 0, 7.5);
            //}

            // ----------------------androidのみで下記使用--------------------------
            ObservableCollection<MyBlogMenuSelectList> items = new ObservableCollection<MyBlogMenuSelectList>();
            items.Add(new MyBlogMenuSelectList("ギャラリーから写真を選択"));
            items.Add(new MyBlogMenuSelectList("写真を撮影する"));
            myBeautyBlogEditPageViewModel.MyBlogPlusListViewIsVisible = false;
            this.MyBlogPlusListView.ItemsSource = items;
            this.MyBlogPlusListView.ItemSelected += (sender, e) =>
            {
                if (!App.ProcessManager.CanInvoke())
                {
                    return;
                }
                if (((MyBlogMenuSelectList)e.SelectedItem).MyBlogMenuSelectListText == "ギャラリーから写真を選択")
                {
                    myBeautyBlogEditPageViewModel.MyBlogPlusListViewIsVisible = false;
                    this.MyBlogPlusListView.SelectedItem = null;
                    SelectGallery();
                }
                else
                {
                    myBeautyBlogEditPageViewModel.MyBlogPlusListViewIsVisible = false;
                    this.MyBlogPlusListView.SelectedItem = null;
                    SelectTakePicture();
                }
            };
            //-----------------------------------------------------------------
            App.customNavigationPage.IsRunning = false;
            this.BindingContext = myBeautyBlogEditPageViewModel;
        }


        async void compClick(object sender, EventArgs e)
        {
            if (!App.ProcessManager.CanInvoke())
                return;
            Device.BeginInvokeOnMainThread(() =>
                                           App.customNavigationPage.IsRunning = true);
            //var model = new MyBeautyBlogModel();
            int category;

            if (myBeautyBlogEditPageViewModel.HairBtnCheck)
            {
                category = 1;
            }
            else if (myBeautyBlogEditPageViewModel.NailBtnCheck)
            {
                category = 2;
            }
            else if (myBeautyBlogEditPageViewModel.EyelashBtnCheck)
            {
                category = 3;
            }
            else if (myBeautyBlogEditPageViewModel.EstheticBtnCheck)
            {
                category = 4;
            }
            else
            {
                category = 0;
            }

            string action = "mybeautyblog__regist";
            var imgService = DependencyService.Get<IImageService>();
            //var awaiter = imgService.ConvertImageSourceToBytesAsync(((FileImageSource)myBeautyBlogEditPageViewModel.ImgSouce).File).GetAwaiter();
            var awaiter = imgService.ConvertImageSourceToBytesAsync(myBeautyBlogEditPageViewModel.ImgSouce).GetAwaiter();
            awaiter.OnCompleted(async () =>
            {
                var imgbyte = awaiter.GetResult();
                var parameters = new Dictionary<string, string> {
                    { "category", category.ToString() }
					//{ "imageData", ((FileImageSource)myBeautyBlogEditPageViewModel.ImgSouce).File }
					//{ "imageData", imgbyte.ToString() }
				};
                if (myBeautyBlogEditPageViewModel.myBeautyBlogId != null)
                {
                    parameters.Add("myBeautyBlogId", myBeautyBlogEditPageViewModel.myBeautyBlogId.ToString());
                }
                if (!string.IsNullOrEmpty(myBeautyBlogEditPageViewModel.DatePickerTxt))
                {
                    parameters.Add("date", myBeautyBlogEditPageViewModel.DatePickerTxt.Replace(" ", ""));
                    //model.DateStr = myBeautyBlogEditPageViewModel.DatePickerTxt;
                }
                if (!string.IsNullOrEmpty(myBeautyBlogEditPageViewModel.EntryTxt))
                {
                    parameters.Add("title", myBeautyBlogEditPageViewModel.EntryTxt);
                    //model.Title = myBeautyBlogEditPageViewModel.EntryTxt;
                }
                if (!string.IsNullOrEmpty(myBeautyBlogEditPageViewModel.CustomEditorTxt))
                {
                    parameters.Add("description", myBeautyBlogEditPageViewModel.CustomEditorTxt);
                    //model.Description = myBeautyBlogEditPageViewModel.CustomEditorTxt;
                }

                //var apiRet = await APIManager.Post(action, parameters);
                var apiRet = await APIManager.PostBytes(action, "imageData", imgbyte, parameters);

                if (apiRet != null)
                {
                    var index = apiRet.IndexOf("status\":", StringComparison.Ordinal);
                    var statusNum = apiRet.Substring(index + 8, 1);
                    // status:0 → 成功
                    if (statusNum == "0")
                    {
                        var responseJson = JsonConvert.DeserializeObject<ResponseBase>(apiRet);
                        if (responseJson != null)
                        {
                            //model.CategoryValue = category;
                            //model.ImgSouce = ((FileImageSource)myBeautyBlogEditPageViewModel.ImgSouce).File;
                            //await App.customNavigationPage.PushAsync(new MyBeautyBlogInfoPage(model));

                            //await App.customNavigationPage.PushAsync(new MyBeautyBlogListPage());
                            //if (myBeautyBlogInfoPage != null)
                            //{
                            //	                               Navigation.RemovePage(myBeautyBlogInfoPage); // このページに戻らないように削除
                            ////await Task.Delay(500); // 少しページ戻るの待たせないと前のページが見えてしまう
                            //}
                            Dictionary<string, string> dic = new Dictionary<string, string> { { "index", "0" } };

                            var json = await APIManager.GET("mybeautyblog_list", dic);
                            if (json == null)
                            {
                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    DependencyService.Get<IToast>().Show("通信エラー");
                                });
                            }
                            else
                            {
                                System.Diagnostics.Debug.WriteLine("json :" + json);
                                var contents = JsonConvert.DeserializeObject<ResponseMyBeautyBlogList>(json);
                                if (contents.Data.List != null)
                                {
                                    System.Diagnostics.Debug.WriteLine("(contents.Data.List != null) :" + contents.Data.List);
                                    var item = contents.Data.List.Where((arg) => (int)arg.MyBeautyBlogId == oldModel.MyBeautyBlogId);
                                    System.Diagnostics.Debug.WriteLine("item :" + item);
                                    try
                                    {
                                        var data = item.FirstOrDefault();
                                        System.Diagnostics.Debug.WriteLine("data :" + data);
                                        Task.Run(() =>
                                        {

                                            var editedItem = new MyBeautyBlogModel()
                                            {
                                                BtnIsVisible = false,
                                                MyBeautyBlogId = (int)(data.MyBeautyBlogId),
                                                ImgSouce = data.ThumbnailImage.Path,
                                                DateStr = data.Date.Replace("-", "/"),
                                                DateStringrShort = data.Date.Replace("-", "/").Substring(0, 10),
                                                CategoryValue = data.Category.Value,
                                                Title = data.Title,
                                                Description = data.Description
                                            };
                                            myBeautyBlogInfoPage.UpdateData(editedItem);

                                        });
                                    }
                                    catch (Exception ex)
                                    {
                                        System.Diagnostics.Debug.WriteLine("ex :" + ex);
                                    }

                                }

                            }



                            Task.Run(() =>
                            {
                                var historyTop = App.customNavigationPage.Navigation.NavigationStack.Where((arg) => arg.Id == App.HistoryTopId);

                                if (historyTop.FirstOrDefault() != null)
                                {
                                    System.Diagnostics.Debug.WriteLine(" could  get home and the id  :");
                                    var historyTop_page = historyTop.First() as HistoryTop;

                                    var historyTop_bc = historyTop_page.BindingContext as HistoryTopViewModel;
                                    historyTop_bc.SetBeautyLogItems();
                                }
                                else
                                {
                                    System.Diagnostics.Debug.WriteLine(" could not get home and the id  :");
                                }
                            });

                            App.customNavigationPage.PopAsync();
                        }
                        else
                        {
                            await this.DisplayAlert("エラー", "My美Logの登録に失敗しました。再度登録してください。", "OK");
                        }
                    }
                    else
                    {
                        await this.DisplayAlert("エラー", "My美Logの登録に失敗しました。再度登録してください。", "OK");
                    }
                }
                App.customNavigationPage.IsRunning = false;
                App.ProcessManager.OnComplete();
            });
        }


        private void OnSelectedDate(object sender, DateChangedEventArgs dateChangedEventArgs)
        {
            DayOfWeekCheck(((DatePicker)sender));
        }


        void DayOfWeekCheck(DatePicker dp)
        {
            string dddd = "";
            if (dp.Date.DayOfWeek == DayOfWeek.Sunday)
            {
                dddd = "日";
            }
            else if (dp.Date.DayOfWeek == DayOfWeek.Monday)
            {
                dddd = "月";
            }
            else if (dp.Date.DayOfWeek == DayOfWeek.Tuesday)
            {
                dddd = "火";
            }
            else if (dp.Date.DayOfWeek == DayOfWeek.Wednesday)
            {
                dddd = "水";
            }
            else if (dp.Date.DayOfWeek == DayOfWeek.Thursday)
            {
                dddd = "木";
            }
            else if (dp.Date.DayOfWeek == DayOfWeek.Friday)
            {
                dddd = "金";
            }
            else if (dp.Date.DayOfWeek == DayOfWeek.Saturday)
            {
                dddd = "土";
            }
            myBeautyBlogEditPageViewModel.DatePickerFormat = "yyyy / MM / dd (" + dddd + ")";

            string monthStr = dp.Date.Month.ToString();
            if (monthStr.Length <= 1)
            {
                monthStr = "0" + monthStr;
            }
            string dayStr = dp.Date.Day.ToString();
            if (dayStr.Length <= 1)
            {
                dayStr = "0" + dayStr;
            }
            //myBeautyBlogEditPageViewModel.DatePickerTxt = dp.Date.Year.ToString() + "/" + monthStr + "/" + dayStr + "(" + dddd + ")";
            myBeautyBlogEditPageViewModel.DatePickerTxt = dp.Date.Year.ToString() + "/" + monthStr + "/" + dayStr;
        }


        async void OnImageClicked(object sender, EventArgs e)
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
                    SelectGallery();
                }
                else if (ret == "写真を撮影する")
                {
                    if (!App.ProcessManager.CanInvoke())
                    {
                        return;
                    }
                    SelectTakePicture();
                }
            }
            else if (Device.RuntimePlatform == Device.Android)
            {
                if (myBeautyBlogEditPageViewModel.MyBlogPlusListViewIsVisible == true)
                {
                    myBeautyBlogEditPageViewModel.MyBlogPlusListViewIsVisible = false;
                }
                else
                {
                    myBeautyBlogEditPageViewModel.MyBlogPlusListViewIsVisible = true;
                }
            }
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
                var imageSouce = DependencyService.Get<IImageService>().GetOrientationAdjustedImageSourceReduction(file.Path);

                myBeautyBlogEditPageViewModel.ImgSouce = imageSouce;

                App.ProcessManager.OnComplete();

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
                var imageSouce = DependencyService.Get<IImageService>().GetOrientationAdjustedImageSourceReduction(file.Path);

                myBeautyBlogEditPageViewModel.ImgSouce = imageSouce;
                App.ProcessManager.OnComplete();
            });
        }


    }
}
