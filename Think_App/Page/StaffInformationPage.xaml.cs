using System;
using System.Collections.Generic;
using FFImageLoading.Forms;
using IO.Swagger.Model;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
	public partial class StaffInformationPage : ContentPage
	{
		StaffInformationPageViewModel staffInformationPageViewModel;
		int staffId;
		int storeID;
		string storeName;
		// lastPageFlg
		// 1: 予約画面からの遷移
		// 2: 店舗情報からの遷移
		public StaffInformationPage(int lastPageFlg, int id, int _storeID, string _storeName, bool _canReserv = false)
		{
			InitializeComponent();

			NavigationPage.SetBackButtonTitle(this, "");

			staffId = id;
			storeID = _storeID;
			storeName = _storeName;

			staffInformationPageViewModel = new StaffInformationPageViewModel(_canReserv);

			this.Title = "スタッフ情報";
			if (lastPageFlg == 1)
			{
				staffInformationPageViewModel.BtnIsVisible = false;
			}
			else if (lastPageFlg == 2)
			{
				staffInformationPageViewModel.BtnIsVisible = true;
			}

			Task.Run(async () =>
			{
				var parameters = new Dictionary<string, string> { { "staffId", staffId.ToString() } };
				var json = await APIManager.GET("staff", parameters);
				if (json == null)
				{
					Device.BeginInvokeOnMainThread(() =>
					{
						this.BindingContext = staffInformationPageViewModel;
						DependencyService.Get<IToast>().Show("通信エラー");
					});
				}
				else
				{
					var response = JsonConvert.DeserializeObject<ResponseStaff>(json);
					// スタイリストの情報を取得し表示
					staffInformationPageViewModel.StaffImgSouce = response.Data.ThumbnailImage.Path;
					staffInformationPageViewModel.StaffNameTxt = response.Data.Name;
					staffInformationPageViewModel.StaffNameKana = response.Data.Kana;
					staffInformationPageViewModel.StaffCareer = response.Data.Career;
					staffInformationPageViewModel.StaffComment = response.Data.Summary;
					staffInformationPageViewModel.Message = response.Data.Description;
					staffInformationPageViewModel.GoodImage = response.Data.GoodImagine;
					staffInformationPageViewModel.GoodTechnic = response.Data.GoodSkill;
					// スタッフのお気に入り
					if (response.Data.IsFavorite != null && response.Data.IsFavorite == true)
					{
						staffInformationPageViewModel.FavoIconSouce = "BigFavoIconOn.png";
					}
					else
					{
						staffInformationPageViewModel.FavoIconSouce = "BigFavoIconOff.png";
					}

					// TODO Xamarin.Forms.Device.RuntimePlatform is no longer supported. Use Microsoft.Maui.Devices.DeviceInfo.Platform instead. For more details see https://learn.microsoft.com/en-us/dotnet/maui/migration/forms-projects#device-changes
					if (Device.RuntimePlatform == Device.iOS)
					{
						staffInformationPageViewModel.ScrollViewRect = new Rect(0, 421, 1, ScaleManager.ScreenHeight - 421);
					}
					else
					{
						staffInformationPageViewModel.ScrollViewRect = new Rect(0, 401, 1, ScaleManager.ScreenHeight - 401);
					}

					var womanHairStyle = response.Data.WomanHairStyleList;
					var manHairStyle = response.Data.ManHairStyleList;

					Device.BeginInvokeOnMainThread(() =>
					{
						// レディースヘアカタログ表示
						if (womanHairStyle != null && womanHairStyle.Count > 0)
						{
							// ヘアカタログが何種類あるか取得
							DispHairCatalog(womanHairStyle.Count, "レディース", womanHairStyle);

							// レディースとメンズを分けるボーダーを書く
							if (manHairStyle != null && manHairStyle.Count > 0 && manHairStyle[0] != null)
							{
								var boxView = new BoxView()
								{
									Margin = new Thickness(0, 24, 0, 0),
									HeightRequest = 1,
									WidthRequest = ScaleManager.ScreenWidth,
									Color = ColorList.colorCellBoader
								};
								this.ScrollViewStackLayout.Children.Add(boxView);
							}
						}

						if (manHairStyle != null && manHairStyle.Count > 0 && manHairStyle[0] != null)
						{
							// メンズ
							// ヘアカタログが何種類あるか取得
							var count = manHairStyle.Count;
							DispHairCatalog(manHairStyle.Count, "メンズ", manHairStyle);

							var boxView = new BoxView()
							{
								Margin = new Thickness(0, 24, 0, 0),
								HeightRequest = 1,
								WidthRequest = ScaleManager.ScreenWidth,
								Color = ColorList.colorCellBoader
							};
							this.ScrollViewStackLayout.Children.Add(boxView);
						}
						this.BindingContext = staffInformationPageViewModel;
					});
				}
			});


			this.ReserveStart.Clicked += async (sender, e) =>
			{
				if (App.ProcessManager.CanInvoke())
				{
					try
					{
						string action = "reservation_menulist";
						var parameters = new Dictionary<string, string> {
							{ "deviceId", Config.Instance.Data.deviceId },
							{ "staffId", staffId.ToString() },
							{"salonId", storeID.ToString()}
						};
						ReservationContentInfo ReservationContent = new ReservationContentInfo()
						{
							StaffId = staffId,
							StaffName = staffInformationPageViewModel.StaffNameTxt,
							SalonId = storeID,
							StoreName = storeName
						};

						var apiRet = await APIManager.Post(action, parameters);
						if (apiRet != null)
						{
							var response = JsonConvert.DeserializeObject<ResponseReservationMenu>(apiRet);
							if (response != null)
							{
								Device.BeginInvokeOnMainThread(async () =>
								{
									await App.customNavigationPage.PushAsync(new ReservationMenuList(response, ReservationContent));
								});
							}
							else
							{
								Device.BeginInvokeOnMainThread(() =>
								{
									DependencyService.Get<IToast>().Show("通信エラー");
									//await DisplayAlert("エラー", "読み込みに失敗しました。通信環境の良い場所で再度選択してください。", "OK");
								});
							}
						}
						else
						{
							Device.BeginInvokeOnMainThread(() =>
							{
								DependencyService.Get<IToast>().Show("通信エラー");
							});
						}

					}
					catch (Exception ex)
					{
						System.Diagnostics.Debug.WriteLine("ex :" + ex);
					}
					App.ProcessManager.OnComplete();
				}
			};
		}

		async void DispHairCatalog(int count, string sex, List<InlineResponse2002DataHomeSalonInfoWomanHairStyleList> hairStyle)
		{
			App.customNavigationPage.IsRunning = true;
			await Task.Run(() =>
			{

				var grid = new Grid
				{
					Padding = new Thickness(27 * ScaleManager.WidthScale, 11 * ScaleManager.WidthScale, 27 * ScaleManager.WidthScale, 10),//パディング
					RowSpacing = 11, //縦のスペース
					ColumnSpacing = 11 * ScaleManager.Scale, //横のスペース
					ColumnDefinitions = {//横に３カラム
				    new ColumnDefinition { Width = 100 * ScaleManager.WidthScale },
					new ColumnDefinition { Width = 100 * ScaleManager.WidthScale },
					new ColumnDefinition { Width = 100 * ScaleManager.WidthScale }
				}
				};

				double rowNum = Math.Ceiling((double)count / 3);

				// レディース、メンズなどの項目用Row
				grid.RowDefinitions.Add(new RowDefinition { Height = 22 * ScaleManager.WidthScale });
				// 画像とテキスト用のRow
				for (int loop = 1; loop <= rowNum; loop++)
				{
					// 画像
					grid.RowDefinitions.Add(new RowDefinition { Height = 100 * ScaleManager.WidthScale });

					// テキスト
					grid.RowDefinitions.Add(new RowDefinition { Height = 13 * ScaleManager.WidthScale });
				}

				// テキスト
				grid.Children.Add(
					new Label()
					{
						Text = sex,
						FontSize = 11,
						TextColor = ColorList.colorFont
					}
				);

				int setRowNum = 1;
				int setColumnNum = 0;

				foreach (var val in hairStyle)
				{
					if (setColumnNum > 2)
					{
						setColumnNum = 0;
						setRowNum += 2;
					}

					var tgr = new TapGestureRecognizer();
					tgr.Tapped += (sender, e) => OnHairCatalogClicked((int)val.HairStyleId, val.Name, sex);
					//tgr.Tapped += (sender, e) => OnHairCatalogClicked(sender, e);
					var cachedImage = new CachedImage()
					{
						Aspect = Aspect.AspectFill,
						VerticalOptions = LayoutOptions.Center,
						HorizontalOptions = LayoutOptions.Center,
						HeightRequest = 100 * ScaleManager.Scale,
						WidthRequest = 100 * ScaleManager.Scale
					};
					if (val.ThumbnailImage != null)
					{
						//cachedImage.Source = val.ThumbnailImage.Path;
						cachedImage.Source = DependencyService.Get<IImageService>().ResizeNetImage(val.ThumbnailImage.Path);
					}
					else
					{
						cachedImage.Source = "loginBgImg.png";
					}
					cachedImage.GestureRecognizers.Add(tgr);
					// 画像
					grid.Children.Add(cachedImage, setColumnNum, setRowNum);

					// テキスト
					grid.Children.Add(
						new Label()
						{
							Text = val.Name,
							FontSize = 11,
							HorizontalOptions = LayoutOptions.Center,
							TextColor = ColorList.colorFont
						},
						setColumnNum,
						setRowNum + 1
					);
					setColumnNum++;
				}

				Device.BeginInvokeOnMainThread(() =>
				{
					this.ScrollViewStackLayout.Children.Add(grid);
					App.customNavigationPage.IsRunning = false;
				});

			});

		}


		async void OnFavoIconClicked(object sender, EventArgs e)
		{
			if (App.ProcessManager.CanInvoke())
			{
				FileImageSource souce = (FileImageSource)((Microsoft.Maui.Controls.Image)sender).Source;
				var parameters = new Dictionary<string, string> { { "deviceId", Config.Instance.Data.deviceId } };
				StaticMethod.StaffFavoriteChange(sender, souce, parameters, storeID, staffId);
				//App.ProcessManager.OnComplete();
			}
		}

		//async void OnHairCatalogClicked(object sender, EventArgs e)
		void OnHairCatalogClicked(int id, string name, string sex)
		{
			if (App.ProcessManager.CanInvoke())
			{
				if (sex == "メンズ")
				{
					Navigation.PushAsync(new HairCatalogListPage(2, id, name, 2, storeID, storeName, staffId));
				}
				else
				{
					Navigation.PushAsync(new HairCatalogListPage(2, id, name, 1, storeID, storeName, staffId));
				}
				App.ProcessManager.OnComplete();
			}
		}
	}
}
