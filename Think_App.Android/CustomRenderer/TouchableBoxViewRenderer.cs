using System;
using System.ComponentModel;
using Android.Views;
using Think_App;
using Think_App.Droid;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

// TODO Xamarin.Forms.ExportRendererAttribute is not longer supported. For more details see https://github.com/dotnet/maui/wiki/Using-Custom-Renderers-in-.NET-MAUI
[assembly: ExportRenderer(typeof(TouchableBoxView), typeof(TouchableBoxViewRenderer))]

namespace Think_App.Droid
{
	public class TouchableBoxViewRenderer : BoxRenderer
	{
		public override bool OnTouchEvent(MotionEvent e)
		{
			//return base.OnTouchEvent(e);
			var _TouchableBoxView = Element as TouchableBoxView;

			if (e.Action == MotionEventActions.Down ||
			   e.Action == MotionEventActions.Move ||
			   e.Action == MotionEventActions.Up)
			{
				var x = ((double)(e.GetX())).Clamp(0, Width).ToDpFromPixel();
				var y = ((double)(e.GetY())).Clamp(0, Height).ToDpFromPixel();

				_TouchableBoxView.OnTouched(x, y);
			}

			return true;
		}
	}
}