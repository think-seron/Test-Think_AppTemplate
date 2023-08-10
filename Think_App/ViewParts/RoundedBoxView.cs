using System;
using Xamarin.Forms;

namespace Think_App
{
	public class RoundedBoxView : View
	{
		#region CornerRadius BindableProperty
		public static readonly BindableProperty CornerRadiusProperty =
			BindableProperty.Create(nameof(CornerRadius), typeof(double), typeof(RoundedBoxView), 5.0,
				propertyChanged: (bindable, oldValue, newValue) =>
					((RoundedBoxView)bindable).CornerRadius = (double)newValue);

		public double CornerRadius
		{
			get { return (double)GetValue(CornerRadiusProperty); }
			set { SetValue(CornerRadiusProperty, value); }
		}
		#endregion

		#region Color BindableProperty
		public static readonly BindableProperty ColorProperty =
			BindableProperty.Create(nameof(Color), typeof(Color), typeof(RoundedBoxView), Color.Accent,
				propertyChanged: (bindable, oldValue, newValue) =>
					((RoundedBoxView)bindable).Color = (Color)newValue);

		public Color Color
		{
			get { return (Color)GetValue(ColorProperty); }
			set { SetValue(ColorProperty, value); }
		}
		#endregion

		#region BorderThickness BindableProperty
		public static readonly BindableProperty BorderThicknessProperty =
			BindableProperty.Create(nameof(BorderThickness), typeof(int), typeof(RoundedBoxView), 0,
				propertyChanged: (bindable, oldValue, newValue) =>
					((RoundedBoxView)bindable).BorderThickness = (int)newValue);

		public int BorderThickness
		{
			get { return (int)GetValue(BorderThicknessProperty); }
			set { SetValue(BorderThicknessProperty, value); }
		}
		#endregion

		#region BorderColor BindableProperty
		public static readonly BindableProperty BorderColorProperty =
			BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(RoundedBoxView), Color.Accent,
				propertyChanged: (bindable, oldValue, newValue) =>
					((RoundedBoxView)bindable).BorderColor = (Color)newValue);

		public Color BorderColor
		{
			get { return (Color)GetValue(BorderColorProperty); }
			set { SetValue(BorderColorProperty, value); }
		}
		#endregion

		//public event EventHandler Clicked;

		//public void SendClick()
		//{
		//	Clicked?.Invoke(this, EventArgs.Empty);
		//}
	}
}

