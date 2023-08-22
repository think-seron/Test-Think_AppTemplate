using System;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
namespace Think_App
{
	public class CustomNavigationBarViewModel : ViewModelBase
	{
		
		public enum LeftBtnKinds
		{
			Other,
			None,
			BackBtn,

		}

		public enum RightBtnKinds {
			Other,
			None,
			Text,
			Points,
			Shops,
			ShopsAndBatchs,

		}


		public CustomNavigationBarViewModel(string pageTitle, LeftBtnKinds leftBtnKind, RightBtnKinds rightBtnKind, string rightBtnText = null)
		{
			//とりあえず固定で指定しているが、ビルド時に作成される
			//コンフィグファイル(クラスファイルを参照できるようにすれば良いか
			NavigationBaseColor = ColorList.colorMain;

			PageTitleText = pageTitle;

			//
			PageTitleTextColor = ColorList.colorNavibarTextColor;

			LeftBtnKind = leftBtnKind;

			RightBtnKind = rightBtnKind;
			RightButtonText = rightBtnText;
			// TODO Xamarin.Forms.Device.RuntimePlatform is no longer supported. Use Microsoft.Maui.Devices.DeviceInfo.Platform instead. For more details see https://learn.microsoft.com/en-us/dotnet/maui/migration/forms-projects#device-changes
			if (Device.RuntimePlatform == Device.iOS)
			{
				iOSToppaddingVisible = true;
				NavigationBaseHight = 65;

			}
			else {
				iOSToppaddingVisible = false;
				NavigationBaseHight = 45;
			}
			ProgressRectangle = new Rect(0, NavigationBaseHight, 1, 4);
		}

		public double NavigationBaseHight { get; set; }


		private Rect _ProgressRectangle;
		public Rect ProgressRectangle
		{
			get { return _ProgressRectangle; }
			set
			{
				if (_ProgressRectangle != value)
				{
					_ProgressRectangle = value;
					OnPropertyChanged("ProgressRectangle");
				}
			}
		}

		private bool _iOSToppaddingVisible;
		public bool iOSToppaddingVisible
		{
			get { return _iOSToppaddingVisible; }
			set
			{
				if (_iOSToppaddingVisible != value)
				{
					_iOSToppaddingVisible = value;
					OnPropertyChanged("iOSToppaddingVisible");
				}
			}
		}

		private bool _ProgresVisible;
		public bool ProgresVisible
		{
			get { return _ProgresVisible; }
			set
			{
				if (_ProgresVisible != value)
				{
					_ProgresVisible = value;

					if (value)
					{
						NavigationBaseHight = NavigationBaseHight + 4;
					}
					else {
						NavigationBaseHight = NavigationBaseHight - 4;
					}
					System.Diagnostics.Debug.WriteLine("NavigationBaseHight  :" + NavigationBaseHight);
					      System.Diagnostics.Debug.WriteLine("_ProgresVisible :" + _ProgresVisible);
					OnPropertyChanged("NavigationBaseHight");

					OnPropertyChanged("ProgresVisible");
				}
			}
		}

		private double _ProgressValue;
		public double ProgressValue
		{
			get { return _ProgressValue; }
			set
			{
				if (_ProgressValue != value)
				{
					_ProgressValue = value;
					if (0.0 < value  &&  value<0.95)
					{
						ProgresVisible = true;
					}
					else {
						ProgresVisible = false;
					}
					System.Diagnostics.Debug.WriteLine("_ProgressValue " + _ProgressValue);
					OnPropertyChanged("ProgresVisible");
					OnPropertyChanged("ProgressValue");
				}
			}
		}





		//ナビゲーションバーの色
		private Color _NavigationBaseColor;
		public Color NavigationBaseColor
		{
			get { return _NavigationBaseColor; }
			set
			{
				if (_NavigationBaseColor != value)
				{
					_NavigationBaseColor = value;
					OnPropertyChanged("NavigationBaseColor");
				}
			}
		}
		//左右のボタンやナビゲーション下部のPadding設定
		private Thickness _NavigationParentPadding;
		public Thickness NavigationParentPadding
		{
			get { return _NavigationParentPadding; }
			set
			{
				if (_NavigationParentPadding != value)
				{
					_NavigationParentPadding = value;
					OnPropertyChanged("NavigationParentPadding");
				}
			}
		}
		//左ボタンの設定
		private LeftBtnKinds _LeftBtnKind;
		public LeftBtnKinds LeftBtnKind
		{
			get { return _LeftBtnKind; }
			set
			{
				if (_LeftBtnKind != value)
				{
					_LeftBtnKind = value;


					switch (_LeftBtnKind)
					{

						case LeftBtnKinds.BackBtn:

							LeftImageVisible = true;
							LeftImageSource = "Icon_GoLeft";
							LeftImageHeightRequest = 22;
							LeftImageWidthRequest = 13;
							break;

						case LeftBtnKinds.Other:

							LeftImageVisible = true;
							LeftImageSource = null;
							LeftImageHeightRequest = 22;
							LeftImageWidthRequest = 22;

							break;

						case LeftBtnKinds.None:

							LeftImageVisible = false;
							LeftImageSource = null;
							LeftImageHeightRequest = 0;
							LeftImageWidthRequest = 0;
							LeftBtnClicked = null;
							break;


						default:
							break;
					}

					OnPropertyChanged("LeftImageVisible");
					OnPropertyChanged("LeftImageSource");
					OnPropertyChanged("LeftBtnKind");
				}
			}
		}


		private ImageSource _LeftImageSource;
		public ImageSource LeftImageSource
		{
			get { return _LeftImageSource; }
			set
			{
				if (_LeftImageSource != value)
				{
					_LeftImageSource = value;
					OnPropertyChanged("LeftImageSource");
				}
			}
		}

		private bool _LeftImageVisible;
		public bool LeftImageVisible
		{
			get { return _LeftImageVisible; }
			set
			{
				if (_LeftImageVisible != value)
				{
					_LeftImageVisible = value;
					OnPropertyChanged("LeftImageVisible");
				}
			}
		}
		private int _LeftImgRotation;
		public int LeftImgRotation
		{
			get { return _LeftImgRotation; }
			set
			{
				if (_LeftImgRotation != value)
				{
					_LeftImgRotation = value;
					OnPropertyChanged("LeftImgRotation");
				}
			}
		}
		public double LeftImageHeightRequest { get; set; }

		public double LeftImageWidthRequest { get; set; }


		private Command _LeftBtnClicked;
		public Command LeftBtnClicked
		{
			get { return _LeftBtnClicked; }
			set
			{
				if (_LeftBtnClicked != value)
				{
					_LeftBtnClicked = value;
					OnPropertyChanged("LeftBtnClicker");
				}
			}
		}

		//PageTitle
		private string _PageTitleText;
		public string PageTitleText
		{
			get { return _PageTitleText; }
			set
			{
				if (_PageTitleText != value)
				{
					_PageTitleText = value;
					OnPropertyChanged("PageTitleText");
				}
			}
		}

		//タイトルカラー
		private Color _PageTitleTextColor;
		public Color PageTitleTextColor
		{
			get { return _PageTitleTextColor; }
			set
			{
				if (_PageTitleTextColor != value)
				{
					_PageTitleTextColor = value;
					OnPropertyChanged("PageTitleTextColor");
				}
			}
		}


		//右側のボタンなどについて
		private RightBtnKinds _RightBtnkind;
		public RightBtnKinds RightBtnKind
		{
			get { return _RightBtnkind; }
			set
			{
				if (_RightBtnkind != value)
				{
					_RightBtnkind = value;

					switch (_RightBtnkind) {
						case RightBtnKinds.None:

							RightImageGroupVisible = false;
							RightImageBtnClicked = null;
							RightImageSource = null;
							RightImageHeightRequest = 0;
							RightImageWidthRequest = 0;

							//バッチ
							BatchVisible = false;
							RightBatchColor = Colors.Transparent;
							//テキスト
							SetRightTextEmpty();
							NavigationParentPadding = new Thickness(13, 0, 0, 13);
							break;
							//home
						case RightBtnKinds.Points:
							//ボタン画像
							RightImageGroupVisible = true;
							RightImageSource = "Icon_DottedLine";
							RightImageHeightRequest = 24;
							RightImageWidthRequest = 7;
							//バッチ
							BatchVisible = false;
							RightBatchColor = Colors.Transparent;
							//テキスト
							SetRightTextEmpty();

							NavigationParentPadding = new Thickness(13, 0, 0, 13);
							break;
						case RightBtnKinds.Shops:
							//ボタン画像
							RightImageGroupVisible = true;
							RightImageSource = "Icon_Home";
							RightImageHeightRequest = 21.5;
							RightImageWidthRequest = 27;
							//バッチ
							BatchVisible = false;
							RightBatchColor = Colors.Transparent;
							//テキスト
							SetRightTextEmpty();

							NavigationParentPadding = new Thickness(13, 0, 13, 13);

							break;
						case RightBtnKinds.ShopsAndBatchs:
							//ボタン画像
							RightImageGroupVisible = true;
							RightImageSource = "Icon_Home";
							RightImageHeightRequest = 21.5;
							RightImageWidthRequest = 27;
							//バッチ
							RightBatchColor = ColorList.colorbatch;
							//テキスト
							SetRightTextEmpty();
							NavigationParentPadding = new Thickness(13, 0, 8, 13);

							break;
						case RightBtnKinds.Text:
							RightImageBtnClicked = null;
							//ボタン画像
							RightImageGroupVisible = false;
							RightImageSource = null;
							RightImageHeightRequest = 0;
							RightImageWidthRequest = 0;
							//バッチ
							BatchVisible = false;
							RightBatchColor = Colors.Transparent;
							//テキスト
							RightTextVisible = true;
							RightButtonTextColor = ColorList.colorNavibarTextColor;
							NavigationParentPadding = new Thickness(13, 0, 16, 13);

							break;
						case RightBtnKinds.Other:


							break;
						default:
							break;
					}
				}

				OnPropertyChanged("RightButtonTextColor");
				OnPropertyChanged("RightButtonText");
				OnPropertyChanged("RightTextVisible");
				OnPropertyChanged("RightTextVisible");
				OnPropertyChanged("RightTextBtnClicked");
				OnPropertyChanged("RightBatchColor");
				OnPropertyChanged("BatchVisible");
				OnPropertyChanged("RightImageSource");
				OnPropertyChanged("RightImageBtnClicked");
				OnPropertyChanged("RightImageGroupVisible");
				OnPropertyChanged("RightBtnKind");
			}
		}

		private bool _RightImageGroupVisible;
		public bool RightImageGroupVisible
		{
			get { return _RightImageGroupVisible; }
			set
			{
				if (_RightImageGroupVisible != value)
				{
					_RightImageGroupVisible = value;
					OnPropertyChanged("RightImageGroupVisible");
				}
			}
		}

		private Command _RightImageBtnClicked;
		public Command RightImageBtnClicked
		{
			get { return _RightImageBtnClicked; }
			set
			{
				if (_RightImageBtnClicked != value)
				{
					_RightImageBtnClicked = value;
					OnPropertyChanged("RightImageBtnClicked");
				}
			}
		}

		private ImageSource _RightImageSource;
		public ImageSource RightImageSource
		{
			get { return _RightImageSource; }
			set
			{
				if (_RightImageSource != value)
				{
					_RightImageSource = value;
					OnPropertyChanged("RightImageSource");
				}
			}
		}

		public double RightImageHeightRequest { get; set; }

		public double RightImageWidthRequest { get; set; }


		private bool _BatchVisible;
		public bool BatchVisible
		{
			get { return _BatchVisible; }
			set
			{
				if (_BatchVisible != value)
				{
					_BatchVisible = value;
					OnPropertyChanged("BatchVisible");
				}
			}
		}

		private Color _RightBatchColor;
		public Color RightBatchColor
		{
			get { return _RightBatchColor; }
			set
			{
				if (_RightBatchColor != value)
				{
					_RightBatchColor = value;
					OnPropertyChanged("RightBatchColor");
				}
			}
		}

		private bool _RightTextVisible;
		public bool RightTextVisible
		{
			get { return _RightTextVisible; }
			set
			{
				if (_RightTextVisible != value)
				{
					_RightTextVisible = value;
					OnPropertyChanged("RightTextVisible");
				}
			}
		}

		private Command _RightTextBtnClicked;
		public Command RightTextBtnClicked
		{
			get { return _RightTextBtnClicked; }
			set
			{
				if (_RightTextBtnClicked != value)
				{
					_RightTextBtnClicked = value;
					OnPropertyChanged("RightTextBtnClicked");
				}
			}
		}

		private string _RightButtonText;
		public string RightButtonText
		{
			get { return _RightButtonText; }
			set
			{
				if (_RightButtonText != value)
				{
					_RightButtonText = value;
					if (string.IsNullOrEmpty(_RightButtonText))
					{
						RightButtonTextColor = Colors.Transparent;
					}
						
					OnPropertyChanged("RightButtonTextColor");
					OnPropertyChanged("RightButtonText");
				}
			}
		}

		private Color _RightButtonTextColor;
		public Color RightButtonTextColor
		{
			get { return _RightButtonTextColor; }
			set
			{
				if (_RightButtonTextColor != value)
				{
					_RightButtonTextColor = value;
					OnPropertyChanged("RightButtonTextColor");
				}
			}
		}


		void SetRightTextEmpty() {
			//テキスト
			RightTextBtnClicked = null;
			RightTextVisible = false;
			RightButtonTextColor = Colors.Transparent;
		}
	}
}
