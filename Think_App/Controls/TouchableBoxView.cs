using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
namespace Think_App
{
	public class TouchableBoxView : BoxView
	{
		public event EventHandler<TouchEventArgs> Touched = delegate { };

		public void OnTouched(double x, double y)
		{
			if (Touched != null)
			{
				Touched(this, new TouchEventArgs() { Point = new Point(x, y) });
			}
		}
	}

	public class TouchEventArgs : EventArgs
	{
		public Point Point { get; set; }	
	}
}
