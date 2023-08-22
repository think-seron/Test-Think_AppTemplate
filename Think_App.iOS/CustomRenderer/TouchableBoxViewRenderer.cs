using System;
using System.ComponentModel;
using Think_App;
using Think_App.iOS;
using UIKit;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

// TODO Xamarin.Forms.ExportRendererAttribute is not longer supported. For more details see https://github.com/dotnet/maui/wiki/Using-Custom-Renderers-in-.NET-MAUI
[assembly: ExportRenderer(typeof(TouchableBoxView), typeof(TouchableBoxViewRenderer))]

namespace Think_App.iOS
{
	public class TouchableBoxViewRenderer : BoxRenderer
	{
		TouchableBoxView _TouchableBoxView;

		protected override void OnElementChanged(ElementChangedEventArgs<BoxView> e)
		{
			base.OnElementChanged(e);

			if (e.NewElement != null)
			{
				_TouchableBoxView = e.NewElement as TouchableBoxView;
			}
		}

		public override void TouchesBegan(Foundation.NSSet touches, UIEvent evt)
		{
			base.TouchesBegan(touches, evt);

			var touch = touches.AnyObject as UITouch;
			var location = touch.LocationInView(this);

			var locX = (location.X).Clamp(Bounds.Left, Bounds.Right);
			var locY = (location.Y).Clamp(Bounds.Top, Bounds.Bottom);

			_TouchableBoxView.OnTouched(locX, locY);
		}

		public override void TouchesMoved(Foundation.NSSet touches, UIEvent evt)
		{
			base.TouchesMoved(touches, evt);

			var touch = touches.AnyObject as UITouch;
			var location = touch.LocationInView(this);

			var locX = (location.X).Clamp(Bounds.Left, Bounds.Right);
			var locY = (location.Y).Clamp(Bounds.Top, Bounds.Bottom);

			_TouchableBoxView.OnTouched(locX, locY);
		}

		public override void TouchesEnded(Foundation.NSSet touches, UIEvent evt)
		{
			base.TouchesEnded(touches, evt);

			var touch = touches.AnyObject as UITouch;
			var location = touch.LocationInView(this);

			var locX = (location.X).Clamp(Bounds.Left, Bounds.Right);
			var locY = (location.Y).Clamp(Bounds.Top, Bounds.Bottom);

			_TouchableBoxView.OnTouched(locX, locY);
		}
	}
}
