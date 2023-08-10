using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Graphics;
using Think_App;
using Think_App.Droid;

[assembly: ExportRenderer(typeof(CustomBoxView), typeof(CustomBoxViewRenderer))]
namespace Think_App.Droid
{
	public class CustomBoxViewRenderer : BoxRenderer
	{
		public override void Draw(Canvas canvas)
		{
			//base.Draw(canvas);

			var _CustomBoxView = Element as CustomBoxView;
			using (var paint = new Paint())
			{
				// アンチエイリアス有効
				paint.AntiAlias = true;

				// ボーダーの分だけ縮める。
				var borderThickness = (float)(_CustomBoxView.BorderThickness.ToPixelFromDp());
				var rectangle = new RectF(borderThickness, borderThickness, Width - borderThickness, Height - borderThickness);
				// 短辺のサイズの半分を100%として、radiusを求める
				var radius = _CustomBoxView.UseCornerRadiusValue ? (float)_CustomBoxView.CornerRadiusValue.ToPixelFromDp() : (float)((Math.Min(Width, Height) / 2) * (_CustomBoxView.CornerRadiusRate / 100));

				// 塗りつぶし色を指定
				paint.Color = _CustomBoxView.FillColor.ToAndroid();
				// 塗りつぶしのBoxを描画
				canvas.DrawRoundRect(rectangle, radius, radius, paint);

				// 塗りつぶしない
				paint.SetStyle(Paint.Style.Stroke);
				// ボーダーの幅を指定
				paint.StrokeWidth = borderThickness;
				// ボーダーの色を指定
				paint.Color = _CustomBoxView.StrokeColor.ToAndroid();
				// ボーダー描画
				canvas.DrawRoundRect(rectangle, radius, radius, paint);
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
				Invalidate();
			}
		}
	}
}
