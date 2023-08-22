using System;
using System.ComponentModel;
using Android.Graphics;
using Android.Views;
using Think_App;
using Think_App.Droid;
using Android.Graphics.Drawables;
using Size = System.Drawing.Size;
using SizeF = System.Drawing.SizeF;
using Point = Xamarin.Forms.Point;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

// TODO Xamarin.Forms.ExportRendererAttribute is not longer supported. For more details see https://github.com/dotnet/maui/wiki/Using-Custom-Renderers-in-.NET-MAUI
[assembly: ExportRenderer(typeof(AppendColorImage), typeof(AppendColorImageRenderer))]
namespace Think_App.Droid
{
	public class AppendColorImageRenderer : ImageRenderer
	{
		private AppendColorImage _AppendColorImage;
		private Bitmap _OriginalImage;
		private Bitmap _appendImage;

		protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
		{
			base.OnElementChanged(e);

			_AppendColorImage = Element as AppendColorImage;
			if (_AppendColorImage != null)
			{
				_AppendColorImage.CurrentImageSourceRequested += _AppendColorImage_CurrentImageSourceRequested;
				_AppendColorImage.ForceUpdateRequested += (__, _) =>
				{
					// Androidでは何も行わない。
					// UpdateImage();
				};
			}
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == "Clear")
			{
				UpdateImage();
			}
			else if (e.PropertyName == AppendColorImage.BlendModeProperty.PropertyName)
			{
				// ブレンド更新
				UpdateImage();
			}
		}

		protected override void OnLayout(bool changed, int l, int t, int r, int b)
		{
			if (Control.Drawable is BitmapDrawable && Control.Width > 0 && Control.Height > 0)
			{
				// 初回更新
				UpdateImage();
			}

			base.OnLayout(changed, l, t, r, b);
		}

		public override bool OnTouchEvent(MotionEvent e)
		{
			//return base.OnTouchEvent(e);

			// 位置情報取得
			var x = (double)e.GetX();
			var y = (double)e.GetY();

			if (e.Action == MotionEventActions.Down)
			{
				_AppendColorImage.OnBegin(x, y);
			}
			else if (e.Action == MotionEventActions.Move)
			{
				if (_AppendColorImage.OnMove(x, y))
				{
					// 追加があった場合は描画更新
					UpdateImage();
				}
			}
			else if (e.Action == MotionEventActions.Up)
			{
				_AppendColorImage.OnEnd();
			}
			else
			{
				return false;
			}

			return true;
		}

		private void UpdateImage(int width = 0, int height = 0)
		{
			if (_OriginalImage == null)
			{
				var bd = Control.Drawable as BitmapDrawable;
				if (bd != null)
				{
					_OriginalImage = bd.Bitmap;
				}
			}
			if (_OriginalImage == null)
			{
				return;
			}

			Size canvasSize;
			if (width > 0 && height > 0)
			{
				canvasSize = new Size(width, height);
			}
			else
			{
				canvasSize = new Size(Control.Width, Control.Height);
			}
			_appendImage = GetAppendImage(new Size(_OriginalImage.Width, _OriginalImage.Height), canvasSize);
			if (_appendImage == null)
			{
				return;
			}
			Control.SetImageBitmap(GetBlendImage(_OriginalImage, _appendImage, ConvertBlendMode(_AppendColorImage.BlendMode)));

			// !この処理を入れないとメモリ不足で落ちる。
			_appendImage.Dispose();
			_appendImage = null;
			// !この処理を入れないとメモリ不足で落ちる。
			GC.Collect();

			// 再描画
			Invalidate();
		}

		private PorterDuff.Mode ConvertBlendMode(AppendColorImage.BlendingMode mode)
		{
			PorterDuff.Mode pdMode = PorterDuff.Mode.DstOver;
			if (mode == AppendColorImage.BlendingMode.Normal)
			{
				pdMode = PorterDuff.Mode.DstOver;
			}
			else if (mode == AppendColorImage.BlendingMode.Screen)
			{
				pdMode = PorterDuff.Mode.Screen;
			}
			else if (mode == AppendColorImage.BlendingMode.Multiply)
			{
				pdMode = PorterDuff.Mode.Darken;
			}
			return pdMode;
		}

		private Bitmap GetAppendImage(Size size, Size canvasSize)
		{
			Bitmap image = null;

			int width = Math.Max(size.Width, canvasSize.Width);
			int height = Math.Max(size.Height, canvasSize.Height);
			try
			{
				image = Bitmap.CreateBitmap(width, height, Bitmap.Config.Argb8888);
			}
			catch
			{
				System.Diagnostics.Debug.WriteLine("Bitmapが作成できませんでした。");
				return null;
			}

			var scale = 1;//Context.Resources.DisplayMetrics.Density;

			using (var canvas = new Canvas(image))
			{
				using (var paint = new Paint())
				{
					paint.AntiAlias = true;

					paint.SetStyle(Paint.Style.Stroke);

					// 描画形状、接続部の形状を丸に変更
					paint.StrokeCap = Paint.Cap.Round;
					paint.StrokeJoin = Paint.Join.Round;

					//Lineデータにしたがって線を描画する
					foreach (var d in _AppendColorImage.Strokes.Data)
					{
						try
						{
							paint.StrokeWidth = (float)d.Width * scale;
							paint.Color = d.Color.ToAndroid();
							var path = new Path();
							for (var i = 0; i < d.Points.Count; i++)
							{
								// 座標系の変換
								var point = ConvertCanvasPoint(d.Points[i], canvasSize, size);

								var x = (float)point.X;
								var y = (float)point.Y;

								if (i == 0)
								{
									path.MoveTo(x, y); //始点
								}
								else
								{
									path.LineTo(x, y); //追加点
								}
							}
							paint.SetXfermode((d.IsErase) ? new PorterDuffXfermode(PorterDuff.Mode.Clear) : null);
							canvas.DrawPath(path, paint); //描画
						}
						catch
						{
							System.Diagnostics.Debug.WriteLine("このストロークは描画できませんでした");
						}
					}
				}
			}

			return image;
		}

		private Bitmap GetBlendImage(Bitmap baseImage, Bitmap blendImage, PorterDuff.Mode mode)
		{
			Bitmap image = null;

			int width = baseImage.Width;
			int height = baseImage.Height;
			try
			{
				image = Bitmap.CreateBitmap(width, height, Bitmap.Config.Argb8888);
			}
			catch
			{
				System.Diagnostics.Debug.WriteLine("Bitmapが作成できませんでした。");
				return null;
			}

			using (var canvas = new Canvas(image))
			{
				using (var paint = new Paint())
				{
					if (mode == PorterDuff.Mode.Screen)
					{
						// 背景を白でぬりつぶす
						var rect = new RectF(0, 0, baseImage.Width, baseImage.Height);
						paint.Color = Xamarin.Forms.Color.FromArgb("#FFFFFF").ToAndroid();
						canvas.DrawRoundRect(rect, 0, 0, paint);
					}

					canvas.DrawBitmap(baseImage, 0, 0, paint);
					paint.SetXfermode(new PorterDuffXfermode(mode));
					canvas.DrawBitmap(blendImage, 0, 0, paint);

					// 背景の塗りつぶしをクリップする
					paint.SetXfermode(new PorterDuffXfermode(PorterDuff.Mode.DstIn));
					canvas.DrawBitmap(baseImage, 0, 0, paint);
				}
			}

			return image;
		}

		private Point ConvertCanvasPoint(Point originalPointPx, Size srcSizePx, Size dstSizePx)
		{
			// srcSize ... Control Size
			// dstSize ... Image Size

			// Px -> Dp
			var scale = Context.Resources.DisplayMetrics.Density;
			var originalPoint = new Point(originalPointPx.X / scale, originalPointPx.Y / scale);
			var srcSize = new SizeF(srcSizePx.Width / scale, srcSizePx.Height / scale);
			var dstSize = new SizeF(dstSizePx.Width / scale, dstSizePx.Height / scale);
			double x, y;

			var point = new Point();

			if (_AppendColorImage.Aspect == Aspect.AspectFit)
			{
				var scaleW = srcSize.Width / dstSize.Width;
				var scaleH = srcSize.Height / dstSize.Height;

				if (scaleW > scaleH)
				{
					var dx = (srcSize.Width - dstSize.Width * scaleH) / 2;
					x = (originalPoint.X - dx) / scaleH;
					y = originalPoint.Y / scaleH;
				}
				else
				{
					var dy = (srcSize.Height - dstSize.Height * scaleW) / 2;
					x = originalPoint.X / scaleW;
					y = (originalPoint.Y - dy) / scaleW;
				}
			}
			else if (_AppendColorImage.Aspect == Aspect.AspectFill)
			{
				var scaleW = srcSize.Width / dstSize.Width;
				var scaleH = srcSize.Height / dstSize.Height;

				if (scaleW < scaleH)
				{
					var dx = (srcSize.Width - dstSize.Width * scaleH) / 2;
					x = (originalPoint.X - dx) / scaleH;
					y = originalPoint.Y / scaleH;
				}
				else
				{
					var dy = (srcSize.Height - dstSize.Height * scaleW) / 2;
					x = originalPoint.X / scaleW;
					y = (originalPoint.Y - dy) / scaleW;
				}
			}
			else
			{
				x = originalPoint.X * dstSize.Width / srcSize.Width;
				y = originalPoint.Y * dstSize.Height / srcSize.Height;
			}

			// Dp -> Px
			point.X = x * scale;
			point.Y = y * scale;

			return point;
		}

		void _AppendColorImage_CurrentImageSourceRequested(object sender, EventArgs e)
		{
			ImageSource source = null;
			if (Control != null)
			{
				var bd = Control.Drawable as BitmapDrawable;
				if (bd != null)
				{
					var bitmap = bd.Bitmap;
					if (bitmap != null)
					{
						// Bitmap -> ImageSource
						source = bitmap.ToImageSource();
					}
				}
			}
			_AppendColorImage.SendCurrentImageSource(source);
		}
	}
}
