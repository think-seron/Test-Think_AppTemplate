using System;
using System.ComponentModel;
using System.Drawing;
using CoreGraphics;
using UIKit;
using Think_App;
using Think_App.iOS;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

// TODO Xamarin.Forms.ExportRendererAttribute is not longer supported. For more details see https://github.com/dotnet/maui/wiki/Using-Custom-Renderers-in-.NET-MAUI
[assembly: ExportRenderer(typeof(BalloonView), typeof(BalloonViewRenderer))]
namespace Think_App.iOS
{
	public class BalloonViewRenderer : BoxRenderer
	{
		public override void Draw(CGRect rect)
		{
			//base.Draw(rect);

			var _BalloonView = Element as BalloonView;
			using (var context = UIGraphics.GetCurrentContext())
			{
				// 0)各種長さの設定
				// しっぽの長さ調整。基本プロパティの値だが、四角を描く関係上、幅を上回らないように設定する。
				nfloat tailWidth = (nfloat)Math.Min(_BalloonView.TailWidth, Bounds.Width);
				// しっぽの開始位置調整。基本プロパティの値だが、高さを上回らないように設定する。
				nfloat tailTopPosition = (nfloat)Math.Min(_BalloonView.TailTopPosition, Bounds.Height);
				// しっぽの太さ調整。基本プロパティの値だが、しっぽの開始位置 + 太さ が高さを超える場合縮める。
				nfloat tailThickness = (Bounds.Height < tailTopPosition + _BalloonView.TailThickness) ? Bounds.Height - tailTopPosition : (nfloat)_BalloonView.TailThickness;

				// 1)矩形部分作成
				nfloat x = (_BalloonView.TailDirection == BalloonView.Direction.Left) ? tailWidth : 0;
				nfloat y = 0;
				nfloat width = Bounds.Width - tailWidth;
				nfloat height = Bounds.Height;
				var rectangle = new CGRect(x, y, width, height);
				nfloat radius = (nfloat)(_BalloonView.CornerRadius);
				// コンテキストにパス追加
				var rectPath = CGPath.FromRoundedRect(rectangle, radius, radius);
				context.AddPath(rectPath);

				// 2)しっぽ部分作成
				var tailPath = new CGPath();
				if (_BalloonView.TailDirection == BalloonView.Direction.Left)
				{
					tailPath.MoveToPoint(0, 0);
					tailPath.AddLineToPoint(tailWidth, tailTopPosition);
					tailPath.AddLineToPoint(tailWidth, tailTopPosition + tailThickness);
					tailPath.CloseSubpath();
				}
				else
				{
					tailPath.MoveToPoint(Bounds.Width, 0);
					tailPath.AddLineToPoint(Bounds.Width - tailWidth, tailTopPosition);
					tailPath.AddLineToPoint(Bounds.Width - tailWidth, tailTopPosition + tailThickness);
					tailPath.CloseSubpath();
				}
				// コンテキストにパス追加。
				context.AddPath(tailPath);

				// 3)描画
				// 塗りつぶし色設定。
				context.SetFillColor(_BalloonView.Color.ToCGColor());
				// 塗りつぶし。
				context.FillPath();
			}
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == BalloonView.TailDirectionProperty.PropertyName ||
			   e.PropertyName == BalloonView.TailThicknessProperty.PropertyName ||
			   e.PropertyName == BalloonView.TailWidthProperty.PropertyName ||
			   e.PropertyName == BalloonView.TailTopPositionProperty.PropertyName ||
			   e.PropertyName == BalloonView.CornerRadiusProperty.PropertyName)
			{
				SetNeedsDisplay();
			}
		}
	}
}
