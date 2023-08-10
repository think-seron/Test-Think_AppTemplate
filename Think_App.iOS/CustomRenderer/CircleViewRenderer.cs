using System;
using Xamarin.Forms;
using Think_App;
using Think_App.iOS;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using CoreGraphics;

[assembly: ExportRenderer(typeof(CircleView), typeof(CircleViewRenderer))]
namespace Think_App.iOS
{
	public class CircleViewRenderer : BoxRenderer
	{
		public override void Draw(CGRect rect)
		{
			//base.Draw(rect);

			var _CircleView = Element as CircleView;

			using (var context = UIGraphics.GetCurrentContext())
			{
				// 塗りつぶしの色
				context.SetFillColor(_CircleView.Color.ToCGColor());
				// 描画
				var path = CGPath.FromRoundedRect(Bounds, Bounds.Width / 2, Bounds.Height / 2);
				context.AddPath(path);
				context.DrawPath(CGPathDrawingMode.Fill);
			}
		}
	}
}
