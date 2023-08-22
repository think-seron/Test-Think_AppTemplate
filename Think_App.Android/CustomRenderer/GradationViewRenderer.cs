using System;
using Android.Graphics;
using Think_App;
using Think_App.Droid;
using FormsColor = Xamarin.Forms.Color;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

// TODO Xamarin.Forms.ExportRendererAttribute is not longer supported. For more details see https://github.com/dotnet/maui/wiki/Using-Custom-Renderers-in-.NET-MAUI
[assembly: ExportRenderer(typeof(GradationView), typeof(GradationViewRenderer))]
namespace Think_App.Droid
{
	public class GradationViewRenderer : BoxRenderer
	{
		GradationView _GradationView;

		protected override void OnElementChanged(ElementChangedEventArgs<BoxView> e)
		{
			base.OnElementChanged(e);

			if (e.NewElement != null)
			{
				_GradationView = e.NewElement as GradationView;
			}
		}

		public override void Draw(Canvas canvas)
		{
			//base.Draw(canvas);

			using (var paint = new Paint())
			{
				// アンチエイリアス設定。
				paint.AntiAlias = true;

				var startColor = _GradationView.StartColor;
				var endColor = _GradationView.EndColor;

				var count = (int)Math.Round(Height.ToDpFromPixel());
				if (count > 1)
				{
					var unitHeight = Height / (float)count;
					for (int i = 0; i < count; ++i)
					{
						var r = startColor.Red + (endColor.Red - startColor.Red) * (i / (double)(count - 1));
						var g = startColor.Green + (endColor.Green - startColor.Green) * (i / (double)(count - 1));
						var b = startColor.Blue + (endColor.Blue - startColor.Blue) * (i / (double)(count - 1));

						var color = FormsColor.FromRgb(r, g, b);

						paint.Color = color.ToAndroid();
						canvas.DrawRoundRect(new RectF(0, unitHeight * i, Width, unitHeight * (i + 1)), 0, 0, paint);
					}
				}
			}
		}
	}
}