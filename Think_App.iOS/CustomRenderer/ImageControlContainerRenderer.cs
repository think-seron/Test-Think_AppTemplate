using System;
using System.ComponentModel;
using CoreGraphics;
using Think_App;
using Think_App.iOS;
using Foundation;
using UIKit;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

// TODO Xamarin.Forms.ExportRendererAttribute is not longer supported. For more details see https://github.com/dotnet/maui/wiki/Using-Custom-Renderers-in-.NET-MAUI
[assembly: ExportRenderer(typeof(ImageControlContainer), typeof(ImageControlContainerRenderer))]
namespace Think_App.iOS
{
	public class ImageControlContainerRenderer : ViewRenderer
	{
		ImageControlContainer _ImageControlContainer;

		nfloat tempX, tempY;
		nfloat lastX, lastY;
		double lastDistance;
		const double _abnormalDelta = 0.14;

		protected override void OnElementChanged(ElementChangedEventArgs<View> e)
		{
			base.OnElementChanged(e);

			if (e.NewElement != null)
			{
				_ImageControlContainer = e.NewElement as ImageControlContainer;
				_ImageControlContainer.SizeChanged += (__, _) =>
				{
					_ImageControlContainer?.UpdatePositionBasis();
				};
			}
		}

		public override void TouchesBegan(NSSet touches, UIEvent evt)
		{
			base.TouchesBegan(touches, evt);

			if (!_ImageControlContainer.IsControllable)
			{
				return;
			}

			var touchArray = evt.AllTouches.ToArray<UITouch>();
			if (touchArray.Length > 0)
			{
				// この段階での座標を得ておく。
				var point = touchArray[0].LocationInView(this);
				lastX = point.X;
				lastY = point.Y;
			}

			if (touchArray.Length > 1)
			{
				// ピンチ動作になる可能性があるので、2本目の指の座標を得ておく。
				var point = touchArray[1].LocationInView(this);
				tempX = point.X;
				tempY = point.Y;
				// この時点での距離を計算する。
				lastDistance = GetDistance(lastX, lastY, tempX, tempY);
			}
		}

		public override void TouchesMoved(NSSet touches, UIEvent evt)
		{
			base.TouchesMoved(touches, evt);

			if (!_ImageControlContainer.IsControllable)
			{
				return;
			}

			var touchArray = evt.AllTouches.ToArray<UITouch>();
			if (touchArray.Length > 0)
			{
				// この段階での座標を得ておく。
				var point = touchArray[0].LocationInView(this);
				tempX = point.X;
				tempY = point.Y;
			}

			var deltaX = (double)(tempX - lastX);
			var deltaY = (double)(tempY - lastY);

			bool isAbnormalValue = false;
			// 異常な値になっていないかチェックする
			try
			{
				if (Math.Abs(deltaX) / Bounds.Width > _abnormalDelta ||
				    Math.Abs(deltaY) / Bounds.Height > _abnormalDelta)
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
				_ImageControlContainer.OnPanUpdate(new Point(deltaX, deltaY));
			}

			// 座標更新。
			lastX = tempX;
			lastY = tempY;

			if (touchArray.Length > 1)
			{
				// ピンチ動作が行われたので、2本目の指の座標を得ておく。
				var point = touchArray[1].LocationInView(this);
				tempX = point.X;
				tempY = point.Y;
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

		public override void TouchesEnded(NSSet touches, UIEvent evt)
		{
			base.TouchesEnded(touches, evt);

			if (!_ImageControlContainer.IsControllable)
			{
				return;
			}

			// 全指が離されたことを通知する。
			_ImageControlContainer.OnGestureComplete();
		}

		private double GetDistance(nfloat x1, nfloat y1, nfloat x2, nfloat y2)
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
