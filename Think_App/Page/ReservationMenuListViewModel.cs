using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using IO.Swagger.Model;

using Xamarin.Forms;
namespace Think_App
{
	public class ReservationMenuListViewModel : ViewModelBase
	{
		public ReservationMenuListViewModel(ResponseReservationMenu response, ReservationContentInfo content)
		{
			ResponseData = response;
			SetCouponList(content);
			SetMenuList(content);
			SetListHeight();
		}
		void SetCouponList(ReservationContentInfo content)
		{
			if (ResponseData.Data == null || ResponseData.Data.CouponList == null || ResponseData.Data.CouponList.Count == 0)
				return;

			MenuList = new ObservableCollection<MenuItem>();
			foreach (var n in ResponseData.Data.CouponList)
			{
				//content.CouponId = n.CouponId;
				//content.MenuId = 0;
				MenuList.Add(new MenuItem(n, content));
			}

		}



		void SetMenuList(ReservationContentInfo content)
		{
			if (ResponseData.Data == null || ResponseData.Data.SalonMenuList == null || ResponseData.Data.SalonMenuList.Count == 0)
				return;
			foreach (var n in ResponseData.Data.SalonMenuList)
			{
				//content.MenuId = n.SalonMenuId;
				//content.CouponId = 0;
				MenuList.Add(new MenuItem(n, content));
			}
		}

		void SetListHeight()
		{
			//foreach (var n in MenuList)
			//{
			//	MenuListHeight = MenuListHeight + n.ItemHeight;
			//	System.Diagnostics.Debug.WriteLine("list height " + MenuListHeight);
			//}
		}


		public ResponseReservationMenu ResponseData { get; set; }


		private double _MenuListHeight;
		public double MenuListHeight
		{
			get { return _MenuListHeight; }
			set
			{
				if (_MenuListHeight != value)
				{
					_MenuListHeight = value;
					OnPropertyChanged("MenuListHeight");
				}
			}
		}

		private ObservableCollection<MenuItem> _MenuList;
		public ObservableCollection<MenuItem> MenuList
		{
			get
			{
				if (_MenuList == null)
				{
					_MenuList = new ObservableCollection<MenuItem>();
				}
				return _MenuList;
			}
			set
			{
				if (_MenuList == null)
				{
					_MenuList = new ObservableCollection<MenuItem>();
				}
				if (_MenuList != value)
				{
					_MenuList = value;
					OnPropertyChanged("MenuList");
				}
			}
		}

		public class MenuItem : ViewModelBase
		{
			double scale;
			public MenuItem()
			{

			}

			//coupon
			public MenuItem(InlineResponse20016DataList list, ReservationContentInfo content)
			{
				Content = content;
				CouponId = (int)list.CouponId;
				MenuId = 0;
				SetFontSize();
				SetCouponTexts(list);

				SetCommand();
			}

			//menu
			double baseTitleFontSize = 13.0;
			double DetailFontSize = 11.0;
			public MenuItem(InlineResponse20018DataSalonMenuList list, ReservationContentInfo content)
			{
				Content = content;
				CouponId = 0;
				MenuId = (int)list.SalonMenuId;

				SetFontSize();

				SetMenu(list);

				SetCommand();
			}

			void SetFontSize()
			{
				baseTitleFontSize *= ScaleManager.Scale;
				DetailFontSize *= ScaleManager.Scale;

				if (ScaleManager.Scale > 1.0)
				{
					scale = 1.0;
					if (baseTitleFontSize > 18.0)
						baseTitleFontSize = 18.0;
					if (DetailFontSize > 15.0)
						DetailFontSize = 15.0;
				}
				else
				{
					scale = ScaleManager.Scale;
				}



				CouponTitleFontSize = baseTitleFontSize;
				//13 * scale;
				ShopNameFontSize = baseTitleFontSize;
				//13 * scale;
				OperationContentFontSize = DetailFontSize;
				//11 * scale;
				DiscountContentFontSize = DetailFontSize;
				//11 * scale;
				TermsOfUseFontSize = DetailFontSize;
				//11 * scale;
				SpatialConditionFontSize = DetailFontSize;
				//11 * scale;
			}
			public double CouponTitleFontSize { get; set; }
			public double ShopNameFontSize { get; set; }
			public double OperationContentFontSize { get; set; }
			public double DiscountContentFontSize { get; set; }
			public double TermsOfUseFontSize { get; set; }
			public double SpatialConditionFontSize { get; set; }



			void SetCouponTexts(InlineResponse20016DataList list)
			{
				CouponVisible = true;
				MenuVisible = false;
				//ItemHeight = 223.0 * scale;
				CouponThumbnailSize = 135.0 * scale;

				CouponTitle = list.Title;
				CouponImageSouce = list.ThumbnailImage.Path;
				ShopName = list.SalonName;
				OperationContent = list.Treatment;
				DiscountContent = list.Discount;
				TermsOfUse = list.Condition;
				SpatialCondition = list.Timing;
			}


			void SetMenu(InlineResponse20018DataSalonMenuList list)
			{
				//ItemHeight = 84.0;
				CouponVisible = false;
				MenuVisible = true;

				MenuTitle = list.Summary;
				MenuDuration = list.Duration;
				MenuPrice = list.Price;
                MenuOperationContent = list.Treatment;
			}



			void SetCommand()
			{

				MenuSelectCommand = new Command(async () =>

				{
					if (App.ProcessManager.CanInvoke())
					{
						App.customNavigationPage.IsRunning = true;
						string apiName = "reservation_schedule";
						var param = new Dictionary<string, string>(){
					{ "deviceId", Config.Instance.Data.deviceId },
					{"salonId", Content.SalonId.ToString()},
					{"staffId", Content.StaffId.ToString()},
					{"date", DependencyService.Get<IDateTimeService>().GetNow().ToString("d",new System.Globalization.CultureInfo("ja-JP"))}
					};
						if (MenuId != 0)
						{
							param.Add("salonMenuId", MenuId.ToString());
							Content.MenuId = MenuId;
							Content.CouponId = 0;
							Content.MenuName = MenuTitle;
							Content.CouponContent = null;
						}
						else
						{
							param.Add("couponId", CouponId.ToString());
							Content.CouponId = CouponId;
							Content.MenuId = 0;
							Content.MenuName = OperationContent;
							Content.CouponContent = CouponTitle;

						}

						try
						{
							var res = await APIManager.GET(apiName, param);
							var scheduleContent = JsonManager.Deserialize<ResponseReservationSchedule>(res);
							System.Diagnostics.Debug.WriteLine("json : " + res);

							if (string.IsNullOrEmpty(res))
							{

								DependencyService.Get<IToast>().Show("通信エラー");
								App.customNavigationPage.IsRunning = false;
								App.ProcessManager.OnComplete();

								return;
							}
							else if (scheduleContent == null)
							{

								DependencyService.Get<IToast>().Show("通信エラー");
								App.customNavigationPage.IsRunning = false;
								App.ProcessManager.OnComplete();

								return;
							}

							await App.customNavigationPage.Navigation.PushAsync(new ReservationSchedule(scheduleContent, Content));
						}
						catch
						{

						}
						App.customNavigationPage.IsRunning = false;
						App.ProcessManager.OnComplete();
					}
				});


			}
			private double _CouponThumbnailSize;
			public double CouponThumbnailSize
			{
				get { return _CouponThumbnailSize; }
				set
				{
					if (_CouponThumbnailSize != value)
					{
						_CouponThumbnailSize = value;
						OnPropertyChanged("CouponThumbnailSize");
					}
				}
			}

			public ReservationContentInfo Content { get; set; }
			public int CouponId { get; set; }
			public int MenuId { get; set; }


			private string _CouponTitle;
			public string CouponTitle
			{
				get { return _CouponTitle; }
				set
				{
					if (_CouponTitle != value)
					{
						_CouponTitle = value;
						OnPropertyChanged("CouponTitle");
					}
				}
			}


			private ImageSource _CouponImageSouce;
			public ImageSource CouponImageSouce
			{
				get { return _CouponImageSouce; }
				set
				{
					if (_CouponImageSouce != value)
					{
						_CouponImageSouce = value;
						OnPropertyChanged("CouponImageSouce");
					}
				}
			}


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


			private string _OperationContent;
			public string OperationContent
			{
				get { return _OperationContent; }
				set
				{
					if (_OperationContent != value)
					{
						_OperationContent = value;
						OnPropertyChanged("OperationContent");
					}
				}
			}


			private string _DiscountContent;
			public string DiscountContent
			{
				get { return _DiscountContent; }
				set
				{
					if (_DiscountContent != value)
					{
						_DiscountContent = value;
						OnPropertyChanged("DiscountContent");
					}
				}
			}


			private string _TermsOfUse;
			public string TermsOfUse
			{
				get { return _TermsOfUse; }
				set
				{
					if (_TermsOfUse != value)
					{
						_TermsOfUse = value;
						OnPropertyChanged("TermsOfUse");
					}
				}
			}

			private string _SpatialCondition;
			public string SpatialCondition
			{
				get { return _SpatialCondition; }
				set
				{
					if (_SpatialCondition != value)
					{
						_SpatialCondition = value;
						OnPropertyChanged("SpatialCondition");
					}
				}
			}

			private double _ItemHeight;
			public double ItemHeight
			{
				get { return _ItemHeight; }
				set
				{
					if (_ItemHeight != value)
					{
						_ItemHeight = value;
						OnPropertyChanged("ItemHeight");
					}
				}
			}

			private bool _CouponVisible;
			public bool CouponVisible
			{
				get { return _CouponVisible; }
				set
				{
					if (_CouponVisible != value)
					{
						_CouponVisible = value;
						OnPropertyChanged("CouponVisible");
					}
				}
			}


			//menu



			private bool _MenuVisible;
			public bool MenuVisible
			{
				get { return _MenuVisible; }
				set
				{
					if (_MenuVisible != value)
					{
						_MenuVisible = value;
						OnPropertyChanged("MenuVisible");
					}
				}
			}

			private string _MenuTitle;
			public string MenuTitle
			{
				get { return _MenuTitle; }
				set
				{
					if (_MenuTitle != value)
					{
						_MenuTitle = value;
						OnPropertyChanged("MenuTitle");
					}
				}
			}

			private string _MenuDuration;
			public string MenuDuration
			{
				get { return _MenuDuration; }
				set
				{
					if (_MenuDuration != value)
					{
						_MenuDuration = value;
						OnPropertyChanged("MenuDuration");
					}
				}
			}

			private string _MenuPrice;
			public string MenuPrice
			{
				get { return _MenuPrice; }
				set
				{
					if (_MenuPrice != value)
					{
						_MenuPrice = value;
						OnPropertyChanged("MenuPrice");
					}
				}
			}

            private string _MenuOperationContent;
            public string MenuOperationContent
            {
                get => _MenuOperationContent;
                set
                {
                    if (_MenuOperationContent != value)
                    {
                        _MenuOperationContent = value;
                        OnPropertyChanged(nameof(MenuOperationContent));
                    }
                }
            }

			private Command _MenuSelectCommand;
			public Command MenuSelectCommand
			{
                get { return _MenuSelectCommand; }
				set
                {
					if (_MenuSelectCommand != value)
					{
						_MenuSelectCommand = value;
						OnPropertyChanged("MenuSelectCommand");
					}
				}
			}
		}
	}
}
