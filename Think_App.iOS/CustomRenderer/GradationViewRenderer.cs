using System;
using System.ComponentModel;
using Xamarin.Forms;
using Think_App;
using Think_App.iOS;
using Xamarin.Forms.Platform.iOS;
using CoreGraphics;
using UIKit;

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
						var r = startColor.R + (endColor.R - startColor.R) * (i / (double)(count - 1));
						var g = startColor.G + (endColor.G - startColor.G) * (i / (double)(count - 1));
						var b = startColor.B + (endColor.B - startColor.B) * (i / (double)(count - 1));

						var color = Color.FromRgb(r, g, b);

						context.SetFillColor(color.ToCGColor());
						context.FillRect(new CGRect(0, unitHeight * i, rect.Width, unitHeight));
					}
				}
			}
		}
	}
}
