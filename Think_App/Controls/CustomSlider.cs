using System;
using Xamarin.Forms;

namespace Think_App
{
	public class CustomSlider : Slider
	{
		#region ThumbColor BindableProperty
		public static readonly BindableProperty ThumbColorProperty =
			BindableProperty.Create(nameof(ThumbColorProperty), typeof(Color), typeof(CustomSlider), Color.White,
				propertyChanged: (bindable, oldValue, newValue) =>
					((CustomSlider)bindable).ThumbColor = (Color)newValue);

		public Color ThumbColor
		{
			get { return (Color)GetValue(ThumbColorProperty); }
			set { SetValue(ThumbColorProperty, value); }
		}
		#endregion

		#region ThumbWidth BindableProperty
		public static readonly BindableProperty ThumbWidthProperty =
			BindableProperty.Create(nameof(ThumbWidthProperty), typeof(double), typeof(CustomSlider), 20.0,
				propertyChanged: (bindable, oldValue, newValue) =>
					((CustomSlider)bindable).ThumbWidth = (double)newValue);

		public double ThumbWidth
		{
			get { return (double)GetValue(ThumbWidthProperty); }
			set { SetValue(ThumbWidthProperty, value); }
		}
		#endregion

		#region ThumbHeight BindableProperty
		public static readonly BindableProperty ThumbHeightProperty =
			BindableProperty.Create(nameof(ThumbHeightProperty), typeof(double), typeof(CustomSlider), 20.0,
				propertyChanged: (bindable, oldValue, newValue) =>
					((CustomSlider)bindable).ThumbHeight = (double)newValue);

		public double ThumbHeight
		{
			get { return (double)GetValue(ThumbHeightProperty); }
			set { SetValue(ThumbHeightProperty, value); }
		}
		#endregion

		#region BarColor BindableProperty
		public static readonly BindableProperty BarColorProperty =
			BindableProperty.Create(nameof(BarColorProperty), typeof(Color), typeof(CustomSlider), Color.Default,
				propertyChanged: (bindable, oldValue, newValue) =>
					((CustomSlider)bindable).BarColor = (Color)newValue);

		public Color BarColor
		{
			get { return (Color)GetValue(BarColorProperty); }
			set { SetValue(BarColorProperty, value); }
		}
		#endregion

		#region BarThickness BindableProperty
		public static readonly BindableProperty BarThicknessProperty =
			BindableProperty.Create(nameof(BarThicknessProperty), typeof(double), typeof(CustomSlider), 1.0,
				propertyChanged: (bindable, oldValue, newValue) =>
					((CustomSlider)bindable).BarThickness = (double)newValue);

		public double BarThickness
		{
			get { return (double)GetValue(BarThicknessProperty); }
			set { SetValue(BarThicknessProperty, value); }
		}
		#endregion

		#region BarHeight BindableProperty
		public static readonly BindableProperty BarHeightProperty =
			BindableProperty.Create(nameof(BarHeightProperty), typeof(double), typeof(CustomSlider), 3.0,
				propertyChanged: (bindable, oldValue, newValue) =>
					((CustomSlider)bindable).BarHeight = (double)newValue);

		public double BarHeight
		{
			get { return (double)GetValue(BarHeightProperty); }
			set { SetValue(BarHeightProperty, value); }
		}
		#endregion

		#region Division BindableProperty
		public static readonly BindableProperty DivisionProperty =
			BindableProperty.Create(nameof(DivisionProperty), typeof(int), typeof(CustomSlider), 5,
				propertyChanged: (bindable, oldValue, newValue) =>
					((CustomSlider)bindable).Division = (int)newValue);

		public int Division
		{
			get { return (int)GetValue(DivisionProperty); }
			set { SetValue(DivisionProperty, value); }
		}
		#endregion
	}
}

