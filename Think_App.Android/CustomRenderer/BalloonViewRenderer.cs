using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Graphics;
using Think_App;
using Think_App.Droid;

[assembly: ExportRenderer(typeof(BalloonView), typeof(BalloonViewRenderer))]
namespace Think_App.Droid
{
	public class BalloonViewRenderer : BoxRenderer
	{
		public override void Draw(Canvas canvas)
		{
			//base.Draw(canvas);

			var _BalloonView = Element as BalloonView;
			using (var paint = new Paint())
			{
				// アンチエイリアス有効
				paint.AntiAlias = true;

				// 0)各種長さの設定
				// しっぽの長さ調整。基本プロパティの値だが、四角を描く関係上、幅を上回らないように設定する。
				float tailWidth = (float)Math.Min(_BalloonView.TailWidth.ToPixelFromDp(), Width);
				// しっぽの開始位置調整。基本プロパティの値だが、高さを上回らないように設定する。
				float tailTopPosition = (float)Math.Min(_BalloonView.TailTopPosition.ToPixelFromDp(), Height);
				// しっぽの太さ調整。基本プロパティの値だが、しっぽの開始位置 + 太さ が高さを超える場合縮める。
				float tailThickness = (Height < tailTopPosition + _BalloonView.TailThickness.ToPixelFromDp()) ? Height - tailTopPosition : (float)_BalloonView.TailThickness.ToPixelFromDp();

				// 1)矩形部分作成
				var path = new Path();
				float left = (_BalloonView.TailDirection == BalloonView.Direction.Left) ? tailWidth : 0f;
				float top = 0f;
				float right = left + Width - tailWidth;
				float bottom = Height;
				var rectangle = new RectF(left, top, right, bottom);
				var radius = (float)(_BalloonView.CornerRadius.ToPixelFromDp());
				path.AddRoundRect(rectangle, radius, radius, Path.Direction.Ccw);

				// 2)しっぽ部分作成
				if (_BalloonView.TailDirection == BalloonView.Direction.Left)
				{
					path.MoveTo(0f, 0f);
					path.LineTo(tailWidth, tailTopPosition);
					path.LineTo(tailWidth, tailTopPosition + tailThickness);
					path.LineTo(0f, 0f);
				}
				else
				{
					path.MoveTo(Width, 0f);
					path.LineTo(Width - tailWidth, tailTopPosition);
					path.LineTo(Width - tailWidth, tailTopPosition + tailThickness);
					path.LineTo(Width, 0f);
				}

				// 3)描画
				// 塗りつぶし色設定。
				paint.SetStyle(Paint.Style.Fill);
				paint.Color = _BalloonView.Color.ToAndroid();
				// 塗りつぶし。
				canvas.DrawPath(path, paint);
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
				Invalidate();
			}
		}
	}
}
