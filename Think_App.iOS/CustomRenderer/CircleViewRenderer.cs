using System;
using Think_App;
using Think_App.iOS;
using UIKit;
using CoreGraphics;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

// TODO Xamarin.Forms.ExportRendererAttribute is not longer supported. For more details see https://github.com/dotnet/maui/wiki/Using-Custom-Renderers-in-.NET-MAUI
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
