using System;
using System.ComponentModel;
using Think_App;
using Think_App.iOS;
using CoreGraphics;
using UIKit;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

// TODO Xamarin.Forms.ExportRendererAttribute is not longer supported. For more details see https://github.com/dotnet/maui/wiki/Using-Custom-Renderers-in-.NET-MAUI
[assembly: ExportRenderer(typeof(GradationView), typeof(GradationViewRenderer))]
namespace Think_App.iOS
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

		public override void Draw(CGRect rect)
		{
			//base.Draw(rect);

			using (var context = UIGraphics.GetCurrentContext())
			{
				var startColor = _GradationView.StartColor;
				var endColor = _GradationView.EndColor;

				var count = (int)Math.Round(rect.Height);
				if (count > 1)
				{
					var unitHeight = rect.Height / (nfloat)count;
					for (int i = 0; i < count; ++i)
					{
						var r = startColor.Red + (endColor.Red - startColor.Red) * (i / (double)(count - 1));
						var g = startColor.Green + (endColor.Green - startColor.Green) * (i / (double)(count - 1));
						var b = startColor.Blue + (endColor.Blue - startColor.Blue) * (i / (double)(count - 1));

						var color = Color.FromRgb(r, g, b);

						context.SetFillColor(color.ToCGColor());
						context.FillRect(new CGRect(0, unitHeight * i, rect.Width, unitHeight));
					}
				}
			}
		}
	}
}
