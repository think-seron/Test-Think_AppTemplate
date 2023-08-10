using System;
using Xamarin.Forms;
namespace Think_App
{
	public class GradationView : BoxView
	{
		#region StartColor BindableProperty
		public static readonly BindableProperty StartColorProperty =
			BindableProperty.Create(nameof(StartColor), typeof(Color), typeof(GradationView), Color.Default,
				propertyChanged: (bindable, oldValue, newValue) =>
					((GradationView)bindable).StartColor = (Color)newValue);

		public Color StartColor
		{
			get { return (Color)GetValue(StartColorProperty); }
			set { SetValue(StartColorProperty, value); }
		}
		#endregion

		#region EndColor BindableProperty
		public static readonly BindableProperty EndColorProperty =
			BindableProperty.Create(nameof(EndColor), typeof(Color), typeof(GradationView), Color.Default,
				propertyChanged: (bindable, oldValue, newValue) =>
					((GradationView)bindable).EndColor = (Color)newValue);

		public Color EndColor
		{
			get { return (Color)GetValue(EndColorProperty); }
			set { SetValue(EndColorProperty, value); }
		}
		#endregion
	}
}
