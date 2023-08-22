using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
namespace Think_App
{
	public class CustomBoxView : BoxView
	{
		#region StrokeColor BindableProperty
		public static readonly BindableProperty StrokeColorProperty =
			BindableProperty.Create(nameof(StrokeColor), typeof(Color), typeof(CustomBoxView), null,
				propertyChanged: (bindable, oldValue, newValue) =>
					((CustomBoxView)bindable).StrokeColor = (Color)newValue);

		public Color StrokeColor
		{
			get { return (Color)GetValue(StrokeColorProperty); }
			set { SetValue(StrokeColorProperty, value); }
		}
		#endregion

		#region FillColor BindableProperty
		public static readonly BindableProperty FillColorProperty =
			BindableProperty.Create(nameof(FillColor), typeof(Color), typeof(CustomBoxView), null,
				propertyChanged: (bindable, oldValue, newValue) =>
					((CustomBoxView)bindable).FillColor = (Color)newValue);

		public Color FillColor
		{
			get { return (Color)GetValue(FillColorProperty); }
			set { SetValue(FillColorProperty, value); }
		}
		#endregion

		#region BorderThickness BindableProperty
		public static readonly BindableProperty BorderThicknessProperty =
			BindableProperty.Create(nameof(BorderThickness), typeof(double), typeof(CustomBoxView), 0.0,
				propertyChanged: (bindable, oldValue, newValue) =>
					((CustomBoxView)bindable).BorderThickness = (double)newValue);

		public double BorderThickness
		{
			get { return (double)GetValue(BorderThicknessProperty); }
			set { SetValue(BorderThicknessProperty, value); }
		}
		#endregion

		#region CornerRadiusRate BindableProperty
		public static readonly BindableProperty CornerRadiusRateProperty =
			BindableProperty.Create(nameof(CornerRadiusRate), typeof(double), typeof(CustomBoxView), 0.0,
				propertyChanged: (bindable, oldValue, newValue) =>
					((CustomBoxView)bindable).CornerRadiusRate = (double)newValue);

		public double CornerRadiusRate
		{
			get { return (double)GetValue(CornerRadiusRateProperty); }
			set { SetValue(CornerRadiusRateProperty, value); }
		}
		#endregion

		#region UseCornerRadiusValue BindableProperty
		public static readonly BindableProperty UseCornerRadiusValueProperty =
			BindableProperty.Create(nameof(UseCornerRadiusValue), typeof(bool), typeof(CustomBoxView), false,
				propertyChanged: (bindable, oldValue, newValue) =>
					((CustomBoxView)bindable).UseCornerRadiusValue = (bool)newValue);

		public bool UseCornerRadiusValue
		{
			get { return (bool)GetValue(UseCornerRadiusValueProperty); }
			set { SetValue(UseCornerRadiusValueProperty, value); }
		}
		#endregion

		#region CornerRadiusValue BindableProperty
		public static readonly BindableProperty CornerRadiusValueProperty =
			BindableProperty.Create(nameof(CornerRadiusValue), typeof(double), typeof(CustomBoxView), 0.0,
				propertyChanged: (bindable, oldValue, newValue) =>
					((CustomBoxView)bindable).CornerRadiusValue = (double)newValue);

		public double CornerRadiusValue
		{
			get { return (double)GetValue(CornerRadiusValueProperty); }
			set { SetValue(CornerRadiusValueProperty, value); }
		}
		#endregion
	}
}
