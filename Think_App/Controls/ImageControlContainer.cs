using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
	public class MyPanEventArgs : EventArgs
	{
		public Point Delta { get; set; }
	}

	public class MyPinchEventArgs : EventArgs
	{
		public double DeltaScale { get; set; }
	}

	public class ImageControlContainer : ContentView
	{
		public event EventHandler<MyPanEventArgs> PanUpdated;
		public event EventHandler<MyPinchEventArgs> PinchUpdated;
		public event EventHandler GestureCompleted;

		public int MinPixel { get; set; }

		public double MaxScale { get; set; }
		public double MinScale { get; set; }

		private Basis PositionBasis { get; set; }
		private Aspect ContentAspect { get; set; }

		enum Basis
		{
			Undefined,
			Horizontal,
			Vertical
		}

		// ボーダーに表示されている画像の範囲。各辺を10000として正規化する。
		public Rect ImageRange { get; private set; }

		public void OnPanUpdate(Point delta)
		{
			if (PanUpdated != null)
			{
				PanUpdated(this, new MyPanEventArgs() { Delta = delta });
			}
		}

		public void OnPinchUpdate(double deltaScale)
		{
			if (PinchUpdated != null)
			{
				PinchUpdated(this, new MyPinchEventArgs() { DeltaScale = deltaScale });
			}
		}

		public void OnGestureComplete()
		{
			if (GestureCompleted != null)
			{
				GestureCompleted(this, new EventArgs());
			}
		}

		public ImageControlContainer()
		{
			this.PanUpdated += OnPanUpdated;
			this.PinchUpdated += OnPinchUpdated;
			this.GestureCompleted += OnGestureCompleted;

			// 何もない場合はこの範囲
			MinScale = 1.0;
			MaxScale = 5.0;

			// デフォルト
			this.PositionBasis = Basis.Undefined;

			this.SizeChanged += async (sender, e) =>
			{
				if (Content != null)
				{
					await SetupContentAspectAsync();
					Content.AnchorX = Content.AnchorY = 0.5;
					FinishImage();
				}
			};
		}

		async Task SetupContentAspectAsync()
		{
			// 初期設定としてはAspectFitを選択しておく。
			ContentAspect = Aspect.AspectFit;

			Image image = null;
			if (Content is Image)
			{
				image = Content as Image;
			}
			else if (Content is AbsoluteLayout)
			{
				foreach (var view in ((AbsoluteLayout)Content).Children)
				{
					if (view.IsVisible && view is Image)
					{
						image = view as Image;
					}
				}
			}
			if (image != null)
			{
				ContentAspect = image.Aspect;

				if (image.Aspect == Aspect.AspectFill)
				{
					// 画面外にあるはずの画像がなくなってしまっていることがある。(AbsoluteLayoutの影響？)
					// よって、画面外にはみ出した部分を初めからクロッピングしてしまう。
					var service = DependencyService.Get<IImageService>();
					var size = await service.GetImageSizeAsync(image.Source);
					var imageWidth = size.Width;
					var imageHeight = size.Height;

					if (imageWidth > 0 && imageHeight > 0)
					{
						var scaleControl = Width / Height;
						var scaleImage = imageWidth / imageHeight;
						if (scaleImage > scaleControl)
						{
							// イメージの横幅が大きいので、イメージの幅を切り取る。
							var viewingImageWidth = imageHeight * Width / Height;
							var croppingImageWidth = (imageWidth - viewingImageWidth) / 2.0;
							if (croppingImageWidth > 0)
							{
								var croppingRect = new Rect(croppingImageWidth, 0, viewingImageWidth, imageHeight);
								var source = await service.GetCroppedImageSourceAsync(image.Source, croppingRect);
								image.Source = source;
								// プロパティも変更。
								ImageWidth = ImageHeight * Width / Height;
							}
						}
						else
						{
							// イメージの高さが大きいので、イメージの高さを切り取る。
							var viewingImageHeight = imageWidth * Height / Width;
							var croppingImageHeight = (imageHeight - viewingImageHeight) / 2.0;
							if (croppingImageHeight > 0)
							{
								var croppingRect = new Rect(0, croppingImageHeight, imageWidth, viewingImageHeight);
								var source = await service.GetCroppedImageSourceAsync(image.Source, croppingRect);
								image.Source = source;
								// プロパティも変更。
								ImageHeight = ImageWidth * Height / Width;
							}
						}
					}
				}
			}
		}

		public void UpdatePositionBasis()
		{
			// コントローラーの縦横比と、イメージサイズの縦横比を元にして、
			// 画像がコントローラーの水平基準で収まっているか、鉛直基準で収まっているか、
			// 算出する。
			try
			{
				// この処置は、ImageWidth, ImageHeightが共に値を持つ時のみ行う
				if (ImageWidth > 0 && ImageHeight > 0)
				{
					var scaleControl = Width / Height;
					var scaleImage = ImageWidth / ImageHeight;
					if (scaleImage > scaleControl)
					{
						// 水平基準
						this.PositionBasis = Basis.Horizontal;
					}
					else
					{
						// 鉛直基準
						this.PositionBasis = Basis.Vertical;
					}
				}
			}
			catch
			{
			}
		}

		void OnPanUpdated(object sender, MyPanEventArgs e)
		{
			Debug.WriteLine("Delta X:{0} Y:{1}", e.Delta.X, e.Delta.Y);
			Content.TranslationX += e.Delta.X;
			Content.TranslationY += e.Delta.Y;
		}

		void OnPinchUpdated(object sender, MyPinchEventArgs e)
		{
			Debug.WriteLine("DeltaScale:{0}", e.DeltaScale);
			var newScale = Content.Scale + e.DeltaScale;
			Content.Scale = newScale.Clamp(MinScale, MaxScale);
		}

		void OnGestureCompleted(object sender, EventArgs e)
		{
			// 画像の位置調整を行う。
			if (Content != null)
			{
				FinishImage();
			}
		}

		void FinishImage()
		{
			if (!(ImageWidth > 0) || !(ImageHeight > 0))
			{
				return;
			}

			Task.Run(() =>
			{
				// 画像の境界の設定。
				var borderRect = new Rect(X, Y, Width, Height);

				Debug.WriteLine("Border Left:{0} Top:{1} Right:{2} Bottom:{3}", borderRect.Left, borderRect.Top, borderRect.Right, borderRect.Bottom);

				// 初期状態のときの画像のサイズを求める。
				double originImageWidth, originImageHeight;
				if (this.PositionBasis == Basis.Horizontal)
				{
					if (this.ContentAspect == Aspect.AspectFit)
					{
						// 幅がコントローラーの幅に合うようにサイズを調整している
						originImageWidth = Width;
						var scale = originImageWidth / ImageWidth;
						originImageHeight = ImageHeight * scale;
					}
					else if (this.ContentAspect == Aspect.AspectFill)
					{
						// 高さがコントローラーの高さに合うようにサイズを調整している
						originImageHeight = Height;
						var scale = originImageHeight / ImageHeight;
						originImageWidth = ImageWidth * scale;
					}
					else
					{
						originImageWidth = Width;
						originImageHeight = Height;						
					}
				}
				else
				{
					if (this.ContentAspect == Aspect.AspectFit)
					{
						// 高さがコントローラーの高さに合うようにサイズを調整している
						originImageHeight = Height;
						var scale = originImageHeight / ImageHeight;
						originImageWidth = ImageWidth * scale;
					}
					else if (this.ContentAspect == Aspect.AspectFill)
					{
						// 幅がコントローラーの幅に合うようにサイズを調整している
						originImageWidth = Width;
						var scale = originImageWidth / ImageWidth;
						originImageHeight = ImageHeight * scale;
					}
					else
					{
						originImageWidth = Width;
						originImageHeight = Height;
					}
				}
				Debug.WriteLine("Original Image W:{0} H:{1}", originImageWidth, originImageHeight);

				// イメージの上下左右の座標を求めておく。
				double imageLeft, imageTop, imageRight, imageBottom;

				imageLeft = (Width - originImageWidth * Content.Scale) / 2 + X + Content.TranslationX;
				imageTop = (Height - originImageHeight * Content.Scale) / 2 + Y + Content.TranslationY;
				imageRight = imageLeft + originImageWidth * Content.Scale;
				imageBottom = imageTop + originImageHeight * Content.Scale;

				Debug.WriteLine("Image Left:{0} Top:{1} Right:{2} Bottom:{3}", imageLeft, imageTop, imageRight, imageBottom);

				// 境界に対して、隙間があるかどうかのフラグ。
				bool isSpaceLeft = (borderRect.Left < imageLeft);
				bool isSpaceTop = (borderRect.Top < imageTop);
				bool isSpaceRight = (imageRight < borderRect.Right);
				bool isSpaceBottom = (imageBottom < borderRect.Bottom);

				Debug.WriteLine("Image Space:{0} Top:{1} Right:{2} Bottom:{3}", isSpaceLeft, isSpaceTop, isSpaceRight, isSpaceBottom);

				// 境界に寄せた時に隙間が開いてはならないので、その場合はセンタリングするかのフラグ。
				bool isHorizontalCentering = (originImageWidth * Content.Scale < borderRect.Width);
				bool isVerticalCentering = (originImageHeight * Content.Scale < borderRect.Height);

				// 移動させるべき距離。
				double moveX = 0.0;
				double moveY = 0.0;

				// 各移動距離
				double centeringX = (borderRect.Left + borderRect.Right) / 2 - (imageLeft + imageRight) / 2;
				double centeringY = (borderRect.Top + borderRect.Bottom) / 2 - (imageTop + imageBottom) / 2;
				double attachLeft = borderRect.Left - imageLeft;
				double attachTop = borderRect.Top - imageTop;
				double attachRight = borderRect.Right - imageRight;
				double attachBottom = borderRect.Bottom - imageBottom;

				if (!isSpaceLeft && !isSpaceTop && !isSpaceRight && !isSpaceBottom)
				{
					// 空き：なし
					SetImageRange(imageLeft, imageTop, imageRight, imageBottom, borderRect);
					return;
				}
				else if (isSpaceLeft && !isSpaceTop && !isSpaceRight && !isSpaceBottom)
				{
					// 空き：左
					moveX = (isHorizontalCentering) ? centeringX : attachLeft;
				}
				else if (!isSpaceLeft && isSpaceTop && !isSpaceRight && !isSpaceBottom)
				{
					// 空き：上
					moveY = (isVerticalCentering) ? centeringY : attachTop;
				}
				else if (isSpaceLeft && isSpaceTop && !isSpaceRight && !isSpaceBottom)
				{
					// 空き：左上
					moveX = (isHorizontalCentering) ? centeringX : attachLeft;
					moveY = (isVerticalCentering) ? centeringY : attachTop;
				}
				else if (!isSpaceLeft && !isSpaceTop && isSpaceRight && !isSpaceBottom)
				{
					// 空き：右
					moveX = (isHorizontalCentering) ? centeringX : attachRight;
				}
				else if (isSpaceLeft && !isSpaceTop && isSpaceRight && !isSpaceBottom)
				{
					// 空き：左右
					moveX = centeringX;
				}
				else if (!isSpaceLeft && isSpaceTop && isSpaceRight && !isSpaceBottom)
				{
					// 空き：上右
					moveX = (isHorizontalCentering) ? centeringX : attachRight;
					moveY = (isVerticalCentering) ? centeringY : attachTop;
				}
				else if (isSpaceLeft && isSpaceTop && isSpaceRight && !isSpaceBottom)
				{
					// 空き：左上右
					moveX = centeringX;
					moveY = (isVerticalCentering) ? centeringY : attachTop;
				}
				else if (!isSpaceLeft && !isSpaceTop && !isSpaceRight && isSpaceBottom)
				{
					// 空き：下
					moveY = (isVerticalCentering) ? centeringY : attachBottom;
				}
				else if (isSpaceLeft && !isSpaceTop && !isSpaceRight && isSpaceBottom)
				{
					// 空き：左下
					moveX = (isHorizontalCentering) ? centeringX : attachLeft;
					moveY = (isVerticalCentering) ? centeringY : attachBottom;
				}
				else if (!isSpaceLeft && isSpaceTop && !isSpaceRight && isSpaceBottom)
				{
					// 空き：上下
					moveY = centeringY;
				}
				else if (isSpaceLeft && isSpaceTop && !isSpaceRight && isSpaceBottom)
				{
					// 空き：左上下
					moveX = (isHorizontalCentering) ? centeringX : attachLeft;
					moveY = centeringY;
				}
				else if (!isSpaceLeft && !isSpaceTop && isSpaceRight && isSpaceBottom)
				{
					// 空き：右下
					moveX = (isHorizontalCentering) ? centeringX : attachRight;
					moveY = (isVerticalCentering) ? centeringY : attachBottom;
				}
				else if (isSpaceLeft && !isSpaceTop && isSpaceRight && isSpaceBottom)
				{
					// 空き：左右下
					moveX = centeringX;
					moveY = (isVerticalCentering) ? centeringY : attachBottom;
				}
				else if (!isSpaceLeft && isSpaceTop && isSpaceRight && isSpaceBottom)
				{
					// 空き：上右下
					moveX = (isHorizontalCentering) ? centeringX : attachRight;
					moveY = centeringY;
				}
				else if (isSpaceLeft && isSpaceTop && isSpaceRight && isSpaceBottom)
				{
					// 空き：左上右下
					moveX = centeringX;
					moveY = centeringY;
				}

				// 念のためチェック
				if (moveX.Equals(0) && moveY.Equals(0))
				{
					SetImageRange(imageLeft, imageTop, imageRight, imageBottom, borderRect);
					return;
				}

				double newTransX = Content.TranslationX + moveX;
				double newTransY = Content.TranslationY + moveY;

				Device.BeginInvokeOnMainThread(async () =>
				{
					Debug.WriteLine("位置調整を行います。");
					await Content.TranslateTo(newTransX, newTransY, 250, Easing.Linear);

					// 改めて座標を求める。
					imageLeft = (Width - originImageWidth * Content.Scale) / 2 + X + Content.TranslationX;
					imageTop = (Height - originImageHeight * Content.Scale) / 2 + Y + Content.TranslationY;
					imageRight = imageLeft + originImageWidth * Content.Scale;
					imageBottom = imageTop + originImageHeight * Content.Scale;

					Debug.WriteLine("New Image Left:{0} Top:{1} Right:{2} Bottom:{3}", imageLeft, imageTop, imageRight, imageBottom);

					SetImageRange(imageLeft, imageTop, imageRight, imageBottom, borderRect);
				});
			});
		}

		private void SetImageRange(double left, double top, double right, double bottom, Rect borderRect)
		{
			var rect = new Rect();

			try
			{
				var wScale = ImageWidth / (right - left);
				var hScale = ImageHeight / (bottom - top);

				// 左上の座標、幅、高さを元の画像のスケールで求める。
				var x = (borderRect.Left - left) * wScale;
				var y = (borderRect.Top - top) * hScale;
				var width = borderRect.Width * wScale;
				var height = borderRect.Height * hScale;

				rect.X = x;
				rect.Y = y;
				rect.Width = width;
				rect.Height = height;
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
			}

			System.Diagnostics.Debug.WriteLine("ImageRange x:{0} y:{1} w:{2} h:{3}", rect.X, rect.Y, rect.Width, rect.Height);

			this.ImageRange = rect;
		}

		#region IsControllable BindableProperty
		public static readonly BindableProperty IsControllableProperty =
			BindableProperty.Create(nameof(IsControllable), typeof(bool), typeof(ImageControlContainer), false,
				propertyChanged: (bindable, oldValue, newValue) =>
					((ImageControlContainer)bindable).IsControllable = (bool)newValue);

		public bool IsControllable
		{
			get { return (bool)GetValue(IsControllableProperty); }
			set { SetValue(IsControllableProperty, value); }
		}
		#endregion

		#region ImageWidth BindableProperty
		public static readonly BindableProperty ImageWidthProperty =
			BindableProperty.Create(nameof(ImageWidth), typeof(double), typeof(ImageControlContainer), 0.0,
				propertyChanged: (bindable, oldValue, newValue) =>
					((ImageControlContainer)bindable).ImageWidth = (double)newValue);

		public double ImageWidth
		{
			get { return (double)GetValue(ImageWidthProperty); }
			set { SetValue(ImageWidthProperty, value); }
		}
		#endregion

		#region ImageHeight BindableProperty
		public static readonly BindableProperty ImageHeightProperty =
			BindableProperty.Create(nameof(ImageHeight), typeof(double), typeof(ImageControlContainer), 0.0,
				propertyChanged: (bindable, oldValue, newValue) =>
					((ImageControlContainer)bindable).ImageHeight = (double)newValue);

		public double ImageHeight
		{
			get { return (double)GetValue(ImageHeightProperty); }
			set { SetValue(ImageHeightProperty, value); }
		}
		#endregion
	}
}
