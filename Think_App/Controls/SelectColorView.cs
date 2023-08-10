using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Think_App
{
	public class SelectColorView : ContentView
	{
		public event EventHandler<ColorEventArgs> ColorSelected = delegate { };

		AbsoluteLayout _absoluteLayout;
		View _circleView;
		double _circleWidth;
		double _circleHeight;

		public class GradationInfo
		{
			public Color StartColor { get; set; }
			public Color EndColor { get; set; }
			public List<Color> ColorList { get; private set; }

			public GradationInfo()
			{
				ColorList = new List<Color>();
			}

			public void AddColorToList(Color color)
			{
				ColorList.Add(color);
			}
		}

		private List<GradationInfo> GradationInfoList;

		public SelectColorView()
		{
			GradationInfoList = new List<GradationInfo>();
		}

		public void AddGradation(Color startColor, Color endColor)
		{
			GradationInfoList.Add(new GradationInfo()
			{
				StartColor = startColor,
				EndColor = endColor
			});
		}

		public void UpdateGradationList()
		{
			if (Width > 0 && Height > 0)
			{
				UpdateGradationListMain(Width, Height);
			}
			else
			{
				Debug.WriteLine("まだサイズが決まっていないので更新できません！");
			}
		}

		private void UpdateGradationListMain(double width, double height)
		{
			if (GradationInfoList.Count == 0)
			{
				Debug.WriteLine("グラデーション情報リストが空なので作れません！");
				return;
			}

			Task.Run(async () =>
			{
				// グラデーションバーの横幅を決定する。
				var gradationWidth = width / GradationInfoList.Count;

				// 円の直径を決定する。
				var rate = this.CircleDiameterRate.Clamp(0, 1);
				_circleWidth = gradationWidth * rate;

				_absoluteLayout = new AbsoluteLayout();
				if (CircleImage != null)
				{
					// CircleImageに指定がある場合は、Imageを使用する。
					var service = DependencyService.Get<IImageService>();
					var size = await service.GetImageSizeAsync(CircleImage);
					var scale = size.Height / size.Width;
					_circleHeight = _circleWidth * scale;
					_circleView = new Image()
					{
						WidthRequest = _circleWidth,
						HeightRequest = _circleHeight,
						Source = CircleImage
					};
				}
				else
				{
					_circleHeight = _circleWidth;
					_circleView = new CircleView()
					{
						WidthRequest = _circleWidth,
						HeightRequest = _circleHeight,
						Color = CircleColor
					};
				}

				// グラデーションバーの高さ(整数値)
				var gradationHeight = (int)(Math.Floor(height - _circleHeight));
				if (gradationHeight < 1)
				{
					Debug.WriteLine("グラデーションの高さが足らないので作れません。");
					return;
				}

				var gradationView = new StackLayout()
				{
					Orientation = StackOrientation.Horizontal,
					Spacing = 0
				};

				// グラデーションの生成。
				for (int i = 0; i < GradationInfoList.Count; ++i)
				{
					var gradationBar = CreateGradationBar(gradationWidth,
														  gradationHeight,
														  GradationInfoList[i]);

					gradationView.Children.Add(gradationBar);
				}

				// 出来上がったグラデーションをAbsoluteLayoutに追加する。
				_absoluteLayout.Children.Add(gradationView,
											new Rectangle(0, _circleHeight / 2, 1, gradationHeight),
											AbsoluteLayoutFlags.WidthProportional);

				// 色選択ボックスビューを作成。
				var specifyColorView = new SpecifyColorView(GradationInfoList)
				{
					Color = Color.Transparent,
					BackgroundColor = Color.Transparent
				};
				specifyColorView.ColorSelected += SpecifyColorView_ColorSelected;

				// 出来上がった色選択ボックスビューをAbsoluteLayoutに追加する。
				_absoluteLayout.Children.Add(specifyColorView,
											new Rectangle(0, _circleHeight / 2, 1, gradationHeight),
											AbsoluteLayoutFlags.WidthProportional);

				Device.BeginInvokeOnMainThread(() =>
				{
					this.Content = _absoluteLayout;

					// 初回のサークル位置更新。
					UpdateCircleViewPosition(DefaultColor);
				});
			});
		}

		private View CreateGradationBar(double width, int count, GradationInfo info)
		{
			var startColor = info.StartColor;
			var endColor = info.EndColor;

			var gradationView = new GradationView()
			{
				WidthRequest = width,
				HeightRequest = count,
				StartColor = startColor,
				EndColor = endColor
			};

			for (int i = 0; i < count; ++i)
			{
				var r = startColor.R + (endColor.R - startColor.R) * (i / (double)(count - 1));
				var g = startColor.G + (endColor.G - startColor.G) * (i / (double)(count - 1));
				var b = startColor.B + (endColor.B - startColor.B) * (i / (double)(count - 1));

				var color = Color.FromRgb(r, g, b);
				info.AddColorToList(color);
			}

			return gradationView;

			// この作り方だとiOSで耐えられない遅さ。

			//var container = new StackLayout()
			//{
			//	WidthRequest = width,
			//	Orientation = StackOrientation.Vertical,
			//	Spacing = 0
			//};

			//for (int i = 0; i < count; ++i)
			//{
			//	var unit = new BoxView()
			//	{
			//		WidthRequest = width,
			//		HeightRequest = 1
			//	};

			//	var r = startColor.R + (endColor.R - startColor.R) * (i / (double)(count - 1));
			//	var g = startColor.G + (endColor.G - startColor.G) * (i / (double)(count - 1));
			//	var b = startColor.B + (endColor.B - startColor.B) * (i / (double)(count - 1));

			//	var color = Color.FromRgb(r, g, b);

			//	unit.Color = color;
			//	info.AddColorToList(color);

			//	container.Children.Add(unit);
			//}

			//return container;
		}

		void SpecifyColorView_ColorSelected(object sender, ColorEventArgs e)
		{
			if (ColorSelected != null)
			{
				ColorSelected(this, new ColorEventArgs() { Color = e.Color });
				UpdateCircleViewPosition(e.Color);
			}
		}

		void UpdateCircleViewPosition(Color color)
		{
			Task.Run(() =>
			{
				var unitWidth = Width / GradationInfoList.Count;

				// 色を元にサークルを置くべき場所を探す。
				double x = 0;
				double y = 0;
				for (int i = 0; i < GradationInfoList.Count; ++i)
				{
					var info = GradationInfoList[i];
					for (int j = 0; j < info.ColorList.Count; ++j)
					{
						if (info.ColorList[j] == color)
						{
							// 置くべき場所。
							y = j;
							var shiftX = (unitWidth - _circleWidth) / 2.0;
							x = unitWidth * i + shiftX;
							break;
						}
					}
				}

				Device.BeginInvokeOnMainThread(() =>
				{
					if (_absoluteLayout.Children.Contains(_circleView))
					{
						_absoluteLayout.Children.Remove(_circleView);
					}

					AbsoluteLayout.SetLayoutBounds(_circleView, new Rectangle(x, y, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
					AbsoluteLayout.SetLayoutFlags(_circleView, AbsoluteLayoutFlags.None);
					_absoluteLayout.Children.Insert(1, _circleView);
				});
			});
		}

		/// <summary>
		/// グラデーション幅に対する円のサイズの割合。0~1で設定する。1で幅いっぱいとなる。
		/// </summary>
		#region CircleDiameterRate BindableProperty
		public static readonly BindableProperty CircleDiameterRateProperty =
			BindableProperty.Create(nameof(CircleDiameterRate), typeof(double), typeof(SelectColorView), 0.8,
				propertyChanged: (bindable, oldValue, newValue) =>
					((SelectColorView)bindable).CircleDiameterRate = (double)newValue);

		public double CircleDiameterRate
		{
			get { return (double)GetValue(CircleDiameterRateProperty); }
			set { SetValue(CircleDiameterRateProperty, value); }
		}
		#endregion

		#region CircleImage BindableProperty
		public static readonly BindableProperty CircleImageProperty =
			BindableProperty.Create(nameof(CircleImage), typeof(ImageSource), typeof(SelectColorView), null,
				propertyChanged: (bindable, oldValue, newValue) =>
					((SelectColorView)bindable).CircleImage = (ImageSource)newValue);

		public ImageSource CircleImage
		{
			get { return (ImageSource)GetValue(CircleImageProperty); }
			set { SetValue(CircleImageProperty, value); }
		}
		#endregion

		#region CircleColor BindableProperty
		public static readonly BindableProperty CircleColorProperty =
			BindableProperty.Create(nameof(CircleColor), typeof(Color), typeof(SelectColorView), Color.White,
				propertyChanged: (bindable, oldValue, newValue) =>
					((SelectColorView)bindable).CircleColor = (Color)newValue);

		public Color CircleColor
		{
			get { return (Color)GetValue(CircleColorProperty); }
			set { SetValue(CircleColorProperty, value); }
		}
		#endregion

		#region DefaultColor BindableProperty
		public static readonly BindableProperty DefaultColorProperty =
			BindableProperty.Create(nameof(DefaultColor), typeof(Color), typeof(SelectColorView), Color.Default,
				propertyChanged: (bindable, oldValue, newValue) =>
					((SelectColorView)bindable).DefaultColor = (Color)newValue);

		public Color DefaultColor
		{
			get { return (Color)GetValue(DefaultColorProperty); }
			set { SetValue(DefaultColorProperty, value); }
		}
		#endregion

		public class ColorEventArgs : EventArgs
		{
			public Color Color { get; set; }
		}

		public class SpecifyColorView : TouchableBoxView
		{
			public event EventHandler<ColorEventArgs> ColorSelected = delegate { };

			List<GradationInfo> InfoList;

			public SpecifyColorView(List<GradationInfo> infoList)
			{
				InfoList = infoList;
				this.Touched += TouchableBoxView_Touched;
			}

			void TouchableBoxView_Touched(object sender, TouchEventArgs e)
			{
				Debug.WriteLine("X:{0} Y:{1}", e.Point.X, e.Point.Y);

				// 触った場所を元に選ばれた色を判定する。
				var count = InfoList.Count;
				var unitWidth = Width / (double)count;

				var verticalIndex = -1;
				if (e.Point.X.Equals(Width))
				{
					// 右端だけfor文での判定に引っかからないため、先に判定する。
					verticalIndex = count - 1;
				}
				else
				{
					for (int i = 0; i < count; ++i)
					{
						if (unitWidth * i <= e.Point.X && e.Point.X < unitWidth * (i + 1))
						{
							verticalIndex = i;
							break;
						}
					}
				}

				if (verticalIndex < 0)
				{
					// 見つからなかった。あり得ないが・・・。
					return;
				}

				var info = InfoList[verticalIndex];
				Color? color = null;
				if (e.Point.Y.Equals(Height))
				{
					// 下端だけfor文での判定に引っかからないため、先に判定する。
					color = info.ColorList[info.ColorList.Count - 1];
				}
				else
				{
					for (int i = 0; i < info.ColorList.Count; ++i)
					{
						if (i <= e.Point.Y && e.Point.Y < i + 1)
						{
							color = info.ColorList[i];
						}
					}
				}

				if (color == null)
				{
					// 見つからなかった。あり得ないが・・・。
					return;
				}

				Debug.WriteLine("Selected Color R:{0} G:{1} B:{2}", color.Value.R, color.Value.G, color.Value.B);

				if (ColorSelected != null)
				{
					// イベントを発行。
					ColorSelected(this, new ColorEventArgs() { Color = color.Value });
				}
			}
		}
	}
}

