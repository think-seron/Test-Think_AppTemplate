using System;
using System.ComponentModel;
using Xamarin.Forms;
using Android.Views;
using Think_App;
using Think_App.Droid;
using Xamarin.Forms.Platform.Android;

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