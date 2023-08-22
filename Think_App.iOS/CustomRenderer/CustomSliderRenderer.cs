using System;
using System.ComponentModel;
using System.Collections.Generic;
using UIKit;
using CoreGraphics;
using Think_App;
using Think_App.iOS;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

// TODO Xamarin.Forms.ExportRendererAttribute is not longer supported. For more details see https://github.com/dotnet/maui/wiki/Using-Custom-Renderers-in-.NET-MAUI
[assembly: ExportRenderer(typeof(CustomSlider), typeof(CustomSliderRenderer))]
namespace Think_App.iOS
{
	public class CustomSliderRenderer : SliderRenderer
	{
		CustomSlider _CustomSlider;

		bool _barUpdated;

		protected override void OnElementChanged(ElementChangedEventArgs<Slider> e)
		{
			base.OnElementChanged(e);

			if (Control != null && e.NewElement != null)
			{
				_CustomSlider = e.NewElement as CustomSlider;

				// 本来のバーの描画を消去してしまう。(Drawスレッドで行う。)
				Control.MinimumTrackTintColor = UIColor.Clear;
				Control.MaximumTrackTintColor = UIColor.Clear;

				// スライダーに使うThumbImageをElementの設定通り作成。
				var thumb = ConstructOval(_CustomSlider.ThumbWidth, _CustomSlider.ThumbHeight, _CustomSlider.ThumbColor);
				Control.SetThumbImage(thumb, UIControlState.Normal);
			}
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == CustomSlider.WidthProperty.PropertyName)
			{
				SetNeedsDisplay();
			}
			else if (e.PropertyName == CustomSlider.HeightProperty.PropertyName)
			{
				SetNeedsDisplay();
			}
			else if (e.PropertyName == CustomSlider.BarColorProperty.PropertyName ||
					e.PropertyName == CustomSlider.BarHeightProperty.PropertyName ||
					e.PropertyName == CustomSlider.BarThicknessProperty.PropertyName ||
					e.PropertyName == CustomSlider.DivisionProperty.PropertyName)
			{
				_barUpdated = false;
				SetNeedsDisplay();
			}
		}

		public override void Draw(CGRect rect)
		{
			//base.Draw(rect);

			if (_barUpdated)
			{
				return;
			}

			using (var context = UIGraphics.GetCurrentContext())
			{
				var barColor = _CustomSlider.BarColor.ToCGColor();
				var barThickness = (nfloat)_CustomSlider.BarThickness;
				var barHeight = (nfloat)_CustomSlider.BarHeight;

				// 中央に基準となるバーを描画する。
				var centerY = rect.Height / 2;
				var path = CGPath.FromRect(new CGRect(0, centerY - barThickness / 2.0f, rect.Width, barThickness));

				// Division + 1 だけ区切り線を作成。
				var division = _CustomSlider.Division;
				if (division > 0)
				{
					var divisionSpace = (rect.Width - (division + 1) * barThickness) / division;
					nfloat x = 0f;
					for (int i = 0; i < _CustomSlider.Division + 1; ++i)
					{
						if (i == _CustomSlider.Division) { x = rect.Width - barThickness; }
						var divisionPath = CGPath.FromRect(new CGRect(x, centerY - barHeight / 2.0f, barThickness, barHeight));
						path.AddPath(divisionPath);
						x += barThickness + divisionSpace;
					}
				}

				context.SetFillColor(barColor);
				context.AddPath(path);
				context.FillPath();
			}

			_barUpdated = true;
		}

		UIImage ConstructOval(double width, double height, Color color)
		{
			var w = (nfloat)width;
			var h = (nfloat)height;

			UIImage image;

			UIGraphics.BeginImageContext(new CGSize(w, h));

			var context = UIGraphics.GetCurrentContext();
			var path = CGPath.FromRoundedRect(new CGRect(0, 0, w, h), w / 2f, h / 2f);
			context.SetFillColor(color.ToCGColor());
			context.AddPath(path);
			context.FillPath();

			image = UIGraphics.GetImageFromCurrentImageContext();
			UIGraphics.EndImageContext();

			return image;
		}
	}
}
