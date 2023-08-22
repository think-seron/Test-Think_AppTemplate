using System;
using System.ComponentModel;
using Android.Views;
using Android.Graphics;
using Color = Xamarin.Forms.Color;
using DroidColor = Android.Graphics.Color;
using Think_App;
using Think_App.Droid;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

// TODO Xamarin.Forms.ExportRendererAttribute is not longer supported. For more details see https://github.com/dotnet/maui/wiki/Using-Custom-Renderers-in-.NET-MAUI
[assembly: ExportRenderer(typeof(ImageControlContainer), typeof(ImageControlContainerRenderer))]
namespace Think_App.Droid
{
	public class ImageControlContainerRenderer : ViewRenderer
	{
		float tempX, tempY;
		float lastX, lastY;
		double lastDistance;
		const double _abnormalDelta = 0.14;

		ImageControlContainer _ImageControlContainer;

		protected override void OnElementChanged(ElementChangedEventArgs<Microsoft.Maui.Controls.View> e)
		{
			base.OnElementChanged(e);

			if (Element != null)
			{
				_ImageControlContainer = Element as ImageControlContainer;
			}
		}

		protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
		{
			base.OnSizeChanged(w, h, oldw, oldh);

			_ImageControlContainer.UpdatePositionBasis();
		}

		public override bool OnTouchEvent(MotionEvent e)
		{
			if (_ImageControlContainer.IsControllable != true)
			{
				// コントロール可能ではない場合、基底クラスの処理を行い終わらせる
				System.Diagnostics.Debug.WriteLine("IsControllable:{0}", _ImageControlContainer.IsControllable);
				return base.OnTouchEvent(e);
			}

			if (e.Action == MotionEventActions.Down ||
				e.Action == MotionEventActions.PointerDown ||
				e.Action == MotionEventActions.Pointer1Down ||
				e.Action == MotionEventActions.Pointer2Down ||
				e.Action == MotionEventActions.Pointer3Down)
			{
				// この段階での座標を得ておく
				lastX = e.GetX(0);
				lastY = e.GetY(0);

				if (e.PointerCount > 1)
				{
					// ピンチ動作になる可能性があるので、2本目の指の座標を得ておく。
					tempX = e.GetX(1);
					tempY = e.GetY(1);
					// この時点での距離を計算する。
					lastDistance = GetDistance(lastX, lastY, tempX, tempY);
				}
			}
			else if (e.Action == MotionEventActions.Move)
			{
				// この段階での座標を得ておく
				tempX = e.GetX(0);
				tempY = e.GetY(0);

				var deltaX = (double)(tempX - lastX);
				var deltaY = (double)(tempY - lastY);

				bool isAbnormalValue = false;
				// 異常な値になっていないかチェックする
				try
				{
					if (Math.Abs(deltaX) / (double)Width > _abnormalDelta ||
						Math.Abs(deltaY) / (double)Height > _abnormalDelta)
					{
						isAbnormalValue = true;
					}
				}
				catch
				{
				}

				if (!isAbnormalValue)
				{
					// 移動差分をDpで与える。
					_ImageControlContainer.OnPanUpdate(new Xamarin.Forms.Point(deltaX.ToDpFromPixel(), deltaY.ToDpFromPixel()));
				}

				// 座標更新。
				lastX = tempX;
				lastY = tempY;

				if (e.PointerCount > 1)
				{
					// ピンチ動作が行われたので、2本目の指の座標を得ておく。
					tempX = e.GetX(1);
					tempY = e.GetY(1);
					// この時点での距離を計算する。
					var distance = GetDistance(lastX, lastY, tempX, tempY);

					// 前回の取得した距離との割り算により、スケール差分を与える。
					if (lastDistance > 0)
					{
						var dScale = distance / lastDistance;
						_ImageControlContainer.OnPinchUpdate(dScale - 1.0);
					}

					// 距離更新。
					lastDistance = distance;
				}
			}
			else if (e.Action == MotionEventActions.Up)
			{
				// 全指が離されたことを通知する。
				_ImageControlContainer.OnGestureComplete();
			}
			else
			{
				return base.OnTouchEvent(e);
			}

			return true;
		}

		private double GetDistance(float x1, float y1, float x2, float y2)
		{
			return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == ImageControlContainer.ImageWidthProperty.PropertyName ||
				e.PropertyName == ImageControlContainer.ImageHeightProperty.PropertyName)
			{
				_ImageControlContainer.UpdatePositionBasis();
			}
		}
	}
}
