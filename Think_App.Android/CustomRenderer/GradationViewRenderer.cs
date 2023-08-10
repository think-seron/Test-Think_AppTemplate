using System;
using Xamarin.Forms;
using Android.Graphics;
using Think_App;
using Think_App.Droid;
using Xamarin.Forms.Platform.Android;
using FormsColor = Xamarin.Forms.Color;

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
						var r = startColor.R + (endColor.R - startColor.R) * (i / (double)(count - 1));
						var g = startColor.G + (endColor.G - startColor.G) * (i / (double)(count - 1));
						var b = startColor.B + (endColor.B - startColor.B) * (i / (double)(count - 1));

						var color = FormsColor.FromRgb(r, g, b);

						paint.Color = color.ToAndroid();
						canvas.DrawRoundRect(new RectF(0, unitHeight * i, Width, unitHeight * (i + 1)), 0, 0, paint);
					}
				}
			}
		}
	}
}