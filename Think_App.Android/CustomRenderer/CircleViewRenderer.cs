using System;
using System.ComponentModel;
using Xamarin.Forms;
using Think_App;
using Think_App.Droid;
using Xamarin.Forms.Platform.Android;
using Android.Graphics;

[assembly: ExportRenderer(typeof(CircleView), typeof(CircleViewRenderer))]
namespace Think_App.Droid
{
	public class CircleViewRenderer : BoxRenderer
	{
		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == CircleView.ColorProperty.PropertyName)
			{
				Invalidate();
			}
		}

		public override void Draw(Canvas canvas)
		{
			//base.Draw(canvas);

			var _CircleView = Element as CircleView;

			using (var paint = new Paint())
			{
				// アンチエイリアス設定
				paint.AntiAlias = true;

				// サイズ指定
				var rectangle = new RectF(0, 0, Width, Height);
				// 塗りつぶしの色を設定
				paint.Color = _CircleView.Color.ToAndroid();
				// 円を描画
				canvas.DrawOval(rectangle, paint);
			}
		}
	}
}
