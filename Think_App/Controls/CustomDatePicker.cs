using System;
using Xamarin.Forms;
namespace Think_App
{
	public class CustomDatePicker : DatePicker
	{
		#region BorderHidden BindableProperty
		public static readonly BindableProperty BorderHiddenProperty =
			BindableProperty.Create(nameof(BorderHidden), typeof(bool), typeof(CustomDatePicker), false,
				propertyChanged: (bindable, oldValue, newValue) =>
					((CustomDatePicker)bindable).BorderHidden = (bool)newValue);

		public bool BorderHidden
		{
			get { return (bool)GetValue(BorderHiddenProperty); }
			set { SetValue(BorderHiddenProperty, value); }
		}
		#endregion

		#region FontSize BindableProperty
		public static readonly BindableProperty FontSizeProperty =
			BindableProperty.Create(nameof(FontSize), typeof(double), typeof(CustomDatePicker), 18.0,
				propertyChanged: (bindable, oldValue, newValue) =>
					((CustomDatePicker)bindable).FontSize = (double)newValue);

		public double FontSize
		{
			get { return (double)GetValue(FontSizeProperty); }
			set { SetValue(TextColorProperty, value); }
		}
		#endregion

		#region HorizontalTextAlignment BindableProperty
		public static readonly BindableProperty HorizontalTextAlignmentProperty =
			BindableProperty.Create(nameof(HorizontalTextAlignment), typeof(TextAlignment), typeof(CustomDatePicker), default(TextAlignment),
				propertyChanged: (bindable, oldValue, newValue) =>
					((CustomDatePicker)bindable).HorizontalTextAlignment = (TextAlignment)newValue);

		public TextAlignment HorizontalTextAlignment
		{
			get { return (TextAlignment)GetValue(HorizontalTextAlignmentProperty); }
			set { SetValue(TextColorProperty, value); }
		}
		#endregion		
	}
}
