using System;
using System.ComponentModel;
using Android.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Think_App;
using Think_App.Droid;

[assembly: ExportRenderer(typeof(CustomDatePicker), typeof(CustomDatePickerRenderer))]
namespace Think_App.Droid
{
	public class CustomDatePickerRenderer : DatePickerRenderer
	{
		Android.Graphics.Drawables.Drawable _defaultBackground;
		int _defaultPaddingLeft, _defaultPaddingTop, _defaultPaddingRight, _defaultPaddingBottom;

		CustomDatePicker _CustomDatePicker;

		protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
		{
			base.OnElementChanged(e);

			if (Control != null && e.NewElement != null)
			{
				_CustomDatePicker = e.NewElement as CustomDatePicker;

				_defaultBackground = Control.Background;
				_defaultPaddingLeft = Control.PaddingLeft;
				_defaultPaddingTop = Control.PaddingTop;
				_defaultPaddingRight = Control.PaddingRight;
				_defaultPaddingBottom = Control.PaddingBottom;

				UpdateBorder();
				UpdateTextAlignment();
				UpdateFont();
			}
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == CustomDatePicker.BorderHiddenProperty.PropertyName)
			{
				UpdateBorder();
				UpdateTextAlignment();
			}
			else if (e.PropertyName == CustomDatePicker.FontSizeProperty.PropertyName)
			{
				UpdateFont();
			}
			else if (e.PropertyName == CustomDatePicker.HorizontalTextAlignmentProperty.PropertyName)
			{
				UpdateTextAlignment();
			}
		}

		void UpdateBorder()
		{
			if (_CustomDatePicker.BorderHidden)
			{
				Control.Background = null;
				Control.SetPadding(_defaultPaddingLeft, 0, _defaultPaddingRight, 0);
			}
			else
			{
				Control.Background = _defaultBackground;
				Control.SetPadding(_defaultPaddingLeft, _defaultPaddingTop, _defaultPaddingRight, _defaultPaddingBottom);
			}
		}

		void UpdateTextAlignment()
		{
			if (_CustomDatePicker.HorizontalTextAlignment == Xamarin.Forms.TextAlignment.Start)
			{
				Control.Gravity = (_CustomDatePicker.BorderHidden) ? GravityFlags.Left | GravityFlags.CenterVertical : GravityFlags.Left;
			}
			else if (_CustomDatePicker.HorizontalTextAlignment == Xamarin.Forms.TextAlignment.Center)
			{
				Control.Gravity = GravityFlags.Center;
			}
			else if (_CustomDatePicker.HorizontalTextAlignment == Xamarin.Forms.TextAlignment.End)
			{
				Control.Gravity = (_CustomDatePicker.BorderHidden) ? GravityFlags.Right | GravityFlags.CenterVertical : GravityFlags.Right;
			}
		}

		void UpdateFont()
		{
			Control.TextSize = (float)_CustomDatePicker.FontSize;
		}
	}
}
