using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using IO.Swagger.Model;
namespace Think_App
{
	public class HomeSelectViewModel : ViewModelBase
	{
		public HomeSelectViewModel(ResponseFavoriteSalonList response, int homeSalonId)
		{
			HomeSalonId = homeSalonId;
			System.Diagnostics.Debug.WriteLine("  home salon id  :" + homeSalonId);
			FavoriteList = new ObservableCollection<FavoriteListItems>();
			Initialize(response);
		}

		void Initialize(ResponseFavoriteSalonList response)
		{
			foreach (var n in response.Data.List)
			{

				System.Diagnostics.Debug.WriteLine("  list salon id  :" + n.SalonId);

				Task.Run(() =>
				{
					var listItem = new FavoriteListItems(n);
					Device.BeginInvokeOnMainThread(() =>
					{
						FavoriteList.Add(listItem);
					});
				});
			}
		}

		async void SelectHomeSalon(FavoriteListItems item)
		{
			System.Diagnostics.Debug.WriteLine(" check salon id : home " + HomeSalonId + "   selected id : " + item.SalonId);

			if (item.SalonId == HomeSalonId)
			{
				DependencyService.Get<IToast>().Show("既にホーム店舗に登録されています。");
			}
			else
			{
				var modalView = new ModalView();
				modalView.modalViewViewModel.ModalLabelTxt = "ホーム店舗を変更しますか？";
				modalView.modalViewViewModel.NomalModalLabelRect = new Rectangle(0.5, 0.4, 1, AbsoluteLayout.AutoSize);
				modalView.modalViewViewModel.SelectBtnLayoutBounds = new Rectangle(0.9, 0.6, 1, AbsoluteLayout.AutoSize);
				modalView.modalViewViewModel.YesButtonTxt = "はい";
				modalView.modalViewViewModel.NoButtonTxt = "いいえ";

				modalView.noButton.Clicked += async (noSender, noE) =>
						{
							System.Diagnostics.Debug.WriteLine("clicked ");
							if (App.ProcessManager.CanInvoke())
							{
								System.Diagnostics.Debug.WriteLine("clicked  not invoke");
								await DialogManager.Instance.HideView();
								App.ProcessManager.OnComplete();
							}
							System.Diagnostics.Debug.WriteLine("clicked release");
						};


				modalView.yesButton.Clicked += async (yesSender, yesE) =>
						{
							System.Diagnostics.Debug.WriteLine("clicked ");
							if (App.ProcessManager.CanInvoke())
							{
								System.Diagnostics.Debug.WriteLine("clicked  not invoke");
								var parameters = new Dictionary<string, string>();
								parameters["salonId"] = item.SalonId.ToString();
								var resp = await APIManager.Post("salon_home_regist", parameters);
								if (resp == null)
								{
									DependencyService.Get<IToast>().Show("通信エラー");
									return;
								}

								var res = JsonManager.Deserialize<ResponseHome>(resp);
								var home = App.customNavigationPage.Navigation.NavigationStack.Where((arg) => arg.Id == App.HomeId);
								if (home.FirstOrDefault() != null)
								{
									System.Diagnostics.Debug.WriteLine(" could  get home and the id  :");
									var home_page = home.First() as Home;

									var home_bc = home_page.BindingContext as HomeViewModel;
									home_bc.UpdateHomeSalon(res);
								}
								else
								{
									DependencyService.Get<IToast>().Show("レスポンスエラー");
									App.ProcessManager.OnComplete();
									return;
								}
								HomeSalonId = (int)item.SalonId;
                                await DialogManager.Instance.HideView();
                                App.ProcessManager.OnComplete();



								//ここからホーム店舗登録後の処理
								var confirmationodalView = new ModalView();
								confirmationodalView.modalViewViewModel.ModalLabelTxt = "ホーム店舗に選択されました";
								confirmationodalView.modalViewViewModel.NomalModalLabelRect = new Rectangle(0.5, 0.4, 1, AbsoluteLayout.AutoSize);
								confirmationodalView.modalViewViewModel.OKBtnLayoutBounds = new Rectangle(0.5, 0.6, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize);
								var currentApp = Xamarin.Forms.Application.Current;
								confirmationodalView.okButton.Clicked += async (okSender, okE) =>
																					{
																						if (App.ProcessManager.CanInvoke())
																						{
																							await DialogManager.Instance.HideView();
																							if (currentApp != Xamarin.Forms.Application.Current)
																							{
																								throw new InvalidOperationException("Application.Current changed");
																							}
																							await App.customNavigationPage.PopAsync();
																							App.ProcessManager.OnComplete();
																						}
																					};
                                await DialogManager.Instance.ShowDialogView(confirmationodalView);
                            }

							System.Diagnostics.Debug.WriteLine("clicked release");

						};
                await DialogManager.Instance.ShowDialogView(modalView);
            }
		}

		int HomeSalonId { get; set; }

		private ObservableCollection<FavoriteListItems> _FavoriteList;
		public ObservableCollection<FavoriteListItems> FavoriteList
		{
			get
			{
				if (_FavoriteList == null)
					_FavoriteList = new ObservableCollection<FavoriteListItems>();
				return _FavoriteList;
			}
			set
			{
				if (_FavoriteList == null)
					_FavoriteList = new ObservableCollection<FavoriteListItems>();
				if (_FavoriteList != value)
				{
					_FavoriteList = value;
					OnPropertyChanged("FavoriteList");
				}
			}
		}

		private FavoriteListItems _SelectedItem;
		public FavoriteListItems SelectedItem
		{
			get { return _SelectedItem; }
			set
			{
				if (_SelectedItem != value)
				{
					_SelectedItem = value;
					SelectHomeSalon(value);
					_SelectedItem = null;
					OnPropertyChanged("SelectedItem");
				}
			}
		}


		public class FavoriteListItems : ViewModelBase
		{
			public FavoriteListItems(InlineResponse2007DataList items)
			{

				if (string.IsNullOrEmpty(items.FavoriteStaffName) || items.FavoriteStaffImage == null || string.IsNullOrEmpty(items.FavoriteStaffImage.Path))
				{
					ThumbNailImage = items.ThumbnailImage.Path;
				}
				else
				{
					try
					{
						ThumbNailImage = items.FavoriteStaffImage.Path;
					}
					catch {
						ThumbNailImage = items.ThumbnailImage.Path;
					}
				}
				SalonId = items.SalonId;
				ShopName = items.Name;

				if (string.IsNullOrEmpty(items.FavoriteStaffName))
				{
					StaffVisible = false;
				}
				else
				{
					StaffVisible = true;
					StaffName = items.FavoriteStaffName;
				}


				ShopAddress = items.Address;
				ShopTelNumber = items.Tel.ToString();

				ShopBusinessHours = items.BusinessHours;
				if (ScaleManager.Scale > 1.0)
				{
					ThumbNailSize = 135.0;

				}
				else
				{
					ThumbNailSize = 135.0 * ScaleManager.Scale;
				}
			}


			public int? SalonId { get; set; }
			private ImageSource _ThumbNailImage;
			public ImageSource ThumbNailImage
			{
				get { return _ThumbNailImage; }
				set
				{
					if (_ThumbNailImage != value)
					{
						_ThumbNailImage = value;
						OnPropertyChanged("ThumbNailImage");
					}
				}
			}


			public double ThumbNailSize { get; set; }

			private string _ShopName;
			public string ShopName
			{
				get { return _ShopName; }
				set
				{
					if (_ShopName != value)
					{
						_ShopName = value;
						OnPropertyChanged("ShopName");
					}
				}
			}

			private string _StaffName;
			public string StaffName
			{
				get { return _StaffName; }
				set
				{
					if (_StaffName != value)
					{
						_StaffName = value;
						OnPropertyChanged("StaffName");
					}
				}
			}

			private bool _StaffVisible;
			public bool StaffVisible
			{
				get { return _StaffVisible; }
				set
				{
					if (_StaffVisible != value)
					{
						_StaffVisible = value;
						OnPropertyChanged("StaffVisible");
					}
				}
			}

			private string _ShopAddress;
			public string ShopAddress
			{
				get { return _ShopAddress; }
				set
				{
					if (_ShopAddress != value)
					{
						_ShopAddress = value;
						OnPropertyChanged("ShopAddress");
					}
				}
			}

			private string _ShopTelNumber;
			public string ShopTelNumber
			{
				get { return _ShopTelNumber; }
				set
				{
					if (_ShopTelNumber != value)
					{
						_ShopTelNumber = value;
						OnPropertyChanged("ShopTelNumber");
					}
				}
			}


			private string _ShopBusinessHours;
			public string ShopBusinessHours
			{
				get { return _ShopBusinessHours; }
				set
				{
					if (_ShopBusinessHours != value)
					{
						_ShopBusinessHours = value;
						OnPropertyChanged("ShopBusinessHours");
					}
				}
			}
		}
	}
}