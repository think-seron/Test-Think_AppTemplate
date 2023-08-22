using System;
using System.ComponentModel;
using Android.Graphics;
using Android.Graphics.Drawables;
using FormColor = Xamarin.Forms.Color;
using Think_App;
using Think_App.Droid;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

// TODO Xamarin.Forms.ExportRendererAttribute is not longer supported. For more details see https://github.com/dotnet/maui/wiki/Using-Custom-Renderers-in-.NET-MAUI
[assembly: ExportRenderer(typeof(CustomSlider), typeof(CustomSliderRenderer))]
namespace Think_App.Droid
{
	public class CustomSliderRenderer : SliderRenderer
	{
		CustomSlider _CustomSlider;
		float _thumbWidth;

		protected override void OnElementChanged(ElementChangedEventArgs<Slider> e)
		{
			base.OnElementChanged(e);

			if (Control != null && e.NewElement != null)
			{
				_CustomSlider = e.NewElement as CustomSlider;

				UpdateThumb();
			}
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == CustomSlider.ThumbWidthProperty.PropertyName ||
			   e.PropertyName == CustomSlider.ThumbHeightProperty.PropertyName ||
			   e.PropertyName == CustomSlider.ThumbColorProperty.PropertyName ||
			   e.PropertyName == CustomSlider.BarColorProperty.PropertyName ||
			   e.PropertyName == CustomSlider.BarHeightProperty.PropertyName ||
			   e.PropertyName == CustomSlider.BarThicknessProperty.PropertyName)
			{
				UpdateThumb();
				UpdateBar();
			}
		}

		protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
		{
			base.OnSizeChanged(w, h, oldw, oldh);

			if ((w != oldw || h != oldh) && w > 0 && h > 0)
			{
				UpdateBar();
			}
		}

		void UpdateThumb()
		{
			var bm = ConstructOval(_CustomSlider.ThumbWidth, _CustomSlider.ThumbHeight, _CustomSlider.ThumbColor);
			_thumbWidth = bm.Width;
			var bd = new BitmapDrawable(bm);
			bd.SetTargetDensity(Forms.Context.Resources.DisplayMetrics);
			Control.SetThumb(bd);
		}

		void UpdateBar()
		{
			// 元々のプログレスバーは消す。
			Control.ProgressDrawable = null;

			var bm = constructBar(Width, Height, _CustomSlider.BarThickness, _CustomSlider.BarHeight, _CustomSlider.Division, _CustomSlider.BarColor);
			var bd = new BitmapDrawable(bm);
			bd.SetTargetDensity(Forms.Context.Resources.DisplayMetrics);
			Control.Background = bd;
		}

		Bitmap ConstructOval(double width, double height, FormColor color)
		{
			int widthPx = (int)width.ToPixelFromDp();
			int heightPx = (int)height.ToPixelFromDp();

			// 保存用Bitmap作成
			Bitmap bitmap = Bitmap.CreateBitmap(widthPx, heightPx, Bitmap.Config.Argb8888);

			using (var canvas = new Canvas(bitmap))
			{
				using (var paint = new Paint())
				{
					paint.Color = color.ToAndroid();
					paint.SetStyle(Paint.Style.Fill);
					canvas.DrawOval(new RectF(0, 0, widthPx, heightPx), paint);
				}
			}

			return bitmap;
		}

		Bitmap constructBar(int widthPx, int heightPx, double barThickness, double barHeight, int division, FormColor color)
		{
			var barThicknessPx = (float)barThickness.ToPixelFromDp();
			var barHeightPx = (float)barHeight.ToPixelFromDp();

			var insetLeft = Control.PaddingLeft - _thumbWidth / 2.0f;
			var insetRight = Control.PaddingRight - _thumbWidth / 2.0f;

			// 保存用Bitmap作成
			Bitmap bitmap = Bitmap.CreateBitmap(widthPx, heightPx, Bitmap.Config.Argb8888);

			using (var canvas = new Canvas(bitmap))
			{
				using (var paint = new Paint())
				{
					paint.Color = color.ToAndroid();
					paint.SetStyle(Paint.Style.Fill);

					// 中央線を作成。
					var centerY = heightPx / 2.0f;
					canvas.DrawRect(new RectF(insetLeft, centerY - barThicknessPx / 2.0f, widthPx - insetRight, centerY + barThicknessPx / 2.0f), paint);

					// 区切り線を作成。
					float left = insetLeft;
					float top = centerY - barHeightPx / 2.0f;
					float divisionSpace = (widthPx - insetLeft - insetRight - (division + 1) * barThicknessPx) / (float)division;
					for (int i = 0; i < division + 1; ++i)
					{
						if (i == division) { left = widthPx - insetRight - barThicknessPx; }
						canvas.DrawRect(new RectF(left, top, left + barThicknessPx, top + barHeightPx), paint);
						left += divisionSpace + barThicknessPx;
					}
				}
			}

			return bitmap;
		}
	}
}
