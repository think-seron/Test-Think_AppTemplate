//using System;
//using System.Collections.Generic;
//using IO.Swagger.Model;
//using System.Collections.ObjectModel;
//using System.Threading.Tasks;
//using Newtonsoft.Json;

//using Xamarin.Forms;

//namespace Think_App
//{
//	public partial class HistoryTopPage : ContentPage
//	{
//		HistoryTopPageViewModel historyTopPageViewModel;
//		ObservableCollection<ListViewHistoryViewModel> itemList;
//		ObservableCollection<MyBeautyBlogModel> beautyBlogItem;
//		string history_ImagePath = null;
//		public HistoryTopPage()
//		{
//			// ListViewとXlabsのGridView 共存させるの難しい(レイアウト崩れたり片方だけ表示できなかったりする)
//			// 解決策としてGridViewのItemHeight,ItemWidth,はBinding使わない

//			InitializeComponent();
//			NavigationPage.SetBackButtonTitle(this, "");

//			historyTopPageViewModel = new HistoryTopPageViewModel();
//			beautyBlogItem = new ObservableCollection<MyBeautyBlogModel>();
//			itemList = new ObservableCollection<ListViewHistoryViewModel>();

//            historyTopPageViewModel.BlogImageSize = ScaleManager.SizeSet(100);

//			this.Appearing += (sender, e) =>
//			{
//				App.customNavigationPage.IsRunning = true;
//				beautyBlogItem = new ObservableCollection<MyBeautyBlogModel>();
//				itemList = new ObservableCollection<ListViewHistoryViewModel>();
//				Task.Run(async () =>
//				{
//					string json;
//					Dictionary<string, string> parameters = new Dictionary<string, string> { { "index", "0" } };
//					json = await APIManager.GET("history_treatmentlist", parameters);
//					if (json == null)
//					{
//						Device.BeginInvokeOnMainThread(() =>
//						{
//							DependencyService.Get<IToast>().Show("通信エラー");
//						});
//					}
//					else
//					{
//						try
//						{
//							var response = JsonConvert.DeserializeObject<ResponseTreatmentHistoryList>(json);
//							System.Diagnostics.Debug.WriteLine(" response :" + response.Data);
//							// ------------------施術履歴表示---------------------------
//							if (response.Data.List != null && response.Data.List.Count > 0)
//							{
//								int loop = 0;
//								foreach (var val in response.Data.List)
//								{
//									List<string> historyThumbnailList = new List<string>();
//									loop++;
//									if (val.ThumbnailImage != null && val.ThumbnailImage.Count > 0)
//									{
//										history_ImagePath = val.ThumbnailImage[0].Path;
//										foreach (var path in val.ThumbnailImage)
//										{
//											historyThumbnailList.Add(path.Path);
//										}
//									}
//									else
//									{
//										history_ImagePath = "noimage.png";
//										historyThumbnailList.Add(history_ImagePath);
//									}

//									itemList.Add(new ListViewHistoryViewModel()
//									{
//										WidthSize = ScaleManager.ScreenWidth,
//										Source = history_ImagePath,
//										thumbnailList = historyThumbnailList,
//										StoreName = val.SalonName,
//										Time = val.DateStr,
//										StoreNameSize = ScaleManager.SizeSet(13),
//										TreatmentHistoryId = val.TreatmentHistoryId,
//										Stylist = val.Stylist,
//										TreatmentDescription = val.TreatmentDescription
//									});
//									if (loop >= 3)
//									{
//										break;
//									}
//								}
//							}
//							else
//							{
//								itemList.Add(new ListViewHistoryViewModel()
//								{
//									Source = "",
//									WidthSize = ScaleManager.ScreenWidth,
//									StoreName = "施術履歴はありません",
//									StoreNameSize = ScaleManager.SizeSet(13)
//								});
//							}
//						}
//						catch (Exception ex)
//						{
//							//通信などエラー時に空表示にする？
//							//System.Diagnostics.Debug.WriteLine("ex  :" + ex);
//							//itemList.Add(new ListViewHistoryViewModel()
//							//{
//							//	Source = "",
//							//	WidthSize = ScaleManager.ScreenWidth,
//							//	StoreName = "施術履歴はありません",
//							//	StoreNameSize = 13 * ScaleManager.Scale
//							//});
//						}
//						if (itemList.Count > 3)
//						{
//							historyTopPageViewModel.HistoryButtonIsVisible = true;
//						}
//						else
//						{
//							historyTopPageViewModel.HistoryButtonIsVisible = false;
//						}

//						historyTopPageViewModel.ListViewRowHeight = ScaleManager.SizeSet(84);
//						double rectHeight = historyTopPageViewModel.ListViewRowHeight * itemList.Count;
//						System.Diagnostics.Debug.WriteLine("rectHeight" + rectHeight);
//						historyTopPageViewModel.ListViewHeight = historyTopPageViewModel.ListViewRowHeight * itemList.Count;

//						Device.BeginInvokeOnMainThread(() =>
//						{
//							historyTopPageViewModel.HistoryItemSouce = itemList;
//						});
//						// -------------------------------------------------------------


//						// ----------------------My美Log表示----------------------------
//						json = await APIManager.GET("mybeautyblog_list", parameters);
//						if (json == null)
//						{
//							Device.BeginInvokeOnMainThread(() =>
//							{
//								DependencyService.Get<IToast>().Show("通信エラー");
//							});
//						}
//						else
//						{
//							var resp = JsonConvert.DeserializeObject<ResponseMyBeautyBlogList>(json);
//							//var oneGridSize = this.GrdView.ItemWidth + this.GrdView.ColumnSpacing + this.GrdView.ColumnSpacing / 2;
//							//var dispCount = (int)Math.Floor(ScaleManager.ScreenWidth / oneGridSize);
//							//System.Diagnostics.Debug.WriteLine("ScaleManager.ScreenWidth" + ScaleManager.ScreenWidth);
//							//System.Diagnostics.Debug.WriteLine("this.GrdView.ItemWidth dispCount" + dispCount );

//							Device.BeginInvokeOnMainThread(() =>
//							{
//                                historyTopPageViewModel.Image1ShadowIsVisible = false;
//                                historyTopPageViewModel.Image2ShadowIsVisible = false;
//                                historyTopPageViewModel.Image3ShadowIsVisible = false;
//                                historyTopPageViewModel.Image1Souce = "";
//                                historyTopPageViewModel.Image2Souce = "";
//                                historyTopPageViewModel.Image3Souce = "";
//                                historyTopPageViewModel.Image1DateStringrShort = "";
//                                historyTopPageViewModel.Image2DateStringrShort = "";
//                                historyTopPageViewModel.Image3DateStringrShort = "";
//								if (resp.Data.List != null && resp.Data.List.Count > 0)
//								{
//									int loop = 0;
//									foreach (var val in resp.Data.List)
//									{
//										loop++;
//                                        if (loop == 1)
//                                        {
//                                            historyTopPageViewModel.Image1ShadowIsVisible = true;
//                                            historyTopPageViewModel.Image1Souce = val.ThumbnailImage.Path;
//                                            historyTopPageViewModel.Image1DateStringrShort = val.Date.Replace("-", "/").Substring(0, 10);
//                                        }
//                                        else if(loop == 2)
//                                        {
//                                            historyTopPageViewModel.Image2ShadowIsVisible = true;
//                                            historyTopPageViewModel.Image2Souce = val.ThumbnailImage.Path;
//                                            historyTopPageViewModel.Image2DateStringrShort = val.Date.Replace("-", "/").Substring(0, 10);
//                                        }
//                                        else if(loop == 3)
//                                        {
//                                            historyTopPageViewModel.Image3ShadowIsVisible = true;
//                                            historyTopPageViewModel.Image3Souce = val.ThumbnailImage.Path;
//                                            historyTopPageViewModel.Image3DateStringrShort = val.Date.Replace("-", "/").Substring(0, 10);
//                                        }
//										beautyBlogItem.Add(new MyBeautyBlogModel()
//										{
//											BtnIsVisible = false,
//											MyBeautyBlogId = (int)val.MyBeautyBlogId,
//											ImgSouce = val.ThumbnailImage.Path,
//											DateStr = val.Date.Replace("-", "/"),
//											DateStringrShort = val.Date.Replace("-", "/").Substring(0, 10),
//											CategoryValue = val.Category.Value,
//											Title = val.Title,
//											Description = val.Description
//										});
//										if (loop >= 3)
//										{
//											break;
//										}
//									}
//								}

//								if (resp.Data.List != null && beautyBlogItem.Count > 0)
//								{
//									historyTopPageViewModel.NoBlogTextRectangle = new Rectangle(0, 0, 0, 0);
//									if (resp.Data.List != null && resp.Data.List.Count > 3)
//									{
//										historyTopPageViewModel.BlogButonTxt = "もっと見る";
//									}
//									else
//									{
//										historyTopPageViewModel.BlogButonTxt = "My美Logへ";
//									}
//								}
//								else
//								{
//									historyTopPageViewModel.NoBlogTextRectangle = new Rectangle(0, 0, 1, 1);
//									historyTopPageViewModel.BlogButonTxt = "My美Logへ";
//								}
//							});
//							// -------------------------------------------------------------
//						}
//						Device.BeginInvokeOnMainThread(() =>
//						{
//                            this.BindingContext = historyTopPageViewModel;
//							App.customNavigationPage.IsRunning = false;
//						});
//					}
//				});
//			};

//			this.ListView.ItemSelected += async (sender, e) =>
//			{
//				if (App.ProcessManager.CanInvoke())
//				{
//					if (((ListViewHistoryViewModel)e.SelectedItem).StoreName == "施術履歴はありません")
//					{
//						App.ProcessManager.OnComplete();
//						return;
//					}
//					await App.customNavigationPage.PushAsync(new HistoryDetailPage(((ListViewHistoryViewModel)e.SelectedItem)));
//					this.ListView.SelectedItem = null;
//					App.ProcessManager.OnComplete();
//				}
//			};

//			this.HistoryButton.Clicked += async (sender, e) =>
//			{
//				if (App.ProcessManager.CanInvoke())
//				{
//					await App.customNavigationPage.PushAsync(new HistoryListPage());
//					App.ProcessManager.OnComplete();
//				}
//			};

//			this.BeautyBlogButton.Clicked += async (sender, e) =>
//			{
//				if (App.ProcessManager.CanInvoke())
//				{
//					await App.customNavigationPage.PushAsync(new MyBeautyBlogListPage());
//					App.ProcessManager.OnComplete();
//				}
//			};
//		}

//		void OnImage1Clicked(object sender, EventArgs e)
//		{
//			if (App.ProcessManager.CanInvoke())
//            {
//				if (beautyBlogItem.Count < 1)
//				{
//					App.ProcessManager.OnComplete();
//					return;
//				}
//                App.customNavigationPage.PushAsync(new MyBeautyBlogInfoPage(beautyBlogItem[0]));
//                App.ProcessManager.OnComplete();
//            }
//		}
//		void OnImage2Clicked(object sender, EventArgs e)
//		{
//			if (App.ProcessManager.CanInvoke())
//			{
//				if (beautyBlogItem.Count < 2)
//				{
//					App.ProcessManager.OnComplete();
//					return;
//				}
//				App.customNavigationPage.PushAsync(new MyBeautyBlogInfoPage(beautyBlogItem[1]));
//				App.ProcessManager.OnComplete();
//			}
//		}
//        void OnImage3Clicked(object sender, EventArgs e)
//        {
//			if (App.ProcessManager.CanInvoke())
//			{
//                if (beautyBlogItem.Count < 3)
//                {
//                    App.ProcessManager.OnComplete();
//                    return;
//                }
//				App.customNavigationPage.PushAsync(new MyBeautyBlogInfoPage(beautyBlogItem[2]));
//				App.ProcessManager.OnComplete();
//			}
//        }
//	}
//}
