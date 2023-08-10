using System;
using System.ComponentModel;
using System.Drawing;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using CoreGraphics;
using UIKit;
using Think_App;
using Think_App.iOS;

[assembly: ExportRenderer(typeof(CustomBoxView), typeof(CustomBoxViewRenderer))]
namespace Think_App.iOS
{
	public class CustomBoxViewRenderer : BoxRenderer
	{
		public override void Draw(CGRect rect)
		{
			//base.Draw(rect);

			var _CustomBoxView = Element as CustomBoxView;
			using (var context = UIGraphics.GetCurrentContext())
			{
				// 塗りつぶし色を指定
				context.SetFillColor(_CustomBoxView.FillColor.ToCGColor());
				// ボーダー色を指定
				context.SetStrokeColor(_CustomBoxView.StrokeColor.ToCGColor());
				// ボーダー幅を指定
				context.SetLineWidth((nfloat)_CustomBoxView.BorderThickness);
				// ボーダーの半分だけ矩形を縮める
				var rectangle = Bounds.Inset((nfloat)_CustomBoxView.BorderThickness / 2, (nfloat)_CustomBoxView.BorderThickness / 2);
				// 短辺の半分を100%として、radiusを求める
				var radius = _CustomBoxView.UseCornerRadiusValue ? (nfloat)_CustomBoxView.CornerRadiusValue : (nfloat)(((Math.Min(rectangle.Width, rectangle.Height)) / 2) * (_CustomBoxView.CornerRadiusRate / 100));
				// 描画
				var path = CGPath.FromRoundedRect(rectangle, radius, radius);
				context.AddPath(path);
				context.DrawPath(CGPathDrawingMode.FillStroke);
			}
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == CustomBoxView.CornerRadiusRateProperty.PropertyName ||
				e.PropertyName == CustomBoxView.BorderThicknessProperty.PropertyName ||
				e.PropertyName == CustomBoxView.FillColorProperty.PropertyName ||
				e.PropertyName == CustomBoxView.StrokeColorProperty.PropertyName)
			{
				SetNeedsDisplay();
			}
		}
	}
}
