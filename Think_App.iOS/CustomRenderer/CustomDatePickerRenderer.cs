using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using Think_App;
using Think_App.iOS;

[assembly: ExportRenderer(typeof(CustomDatePicker), typeof(CustomDatePickerRenderer))]
namespace Think_App.iOS
{
	public class CustomDatePickerRenderer : DatePickerRenderer
	{
		UITextBorderStyle _defaultTextBorderStyle;

		CustomDatePicker _CustomDatePicker;

		protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
		{
			base.OnElementChanged(e);

			if (Control != null && e.NewElement != null)
			{
				_CustomDatePicker = e.NewElement as CustomDatePicker;

				_defaultTextBorderStyle = Control.BorderStyle;
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
			}
			else if (e.PropertyName == CustomDatePicker.HorizontalTextAlignmentProperty.PropertyName)
			{
				UpdateTextAlignment();
			}
			else if (e.PropertyName == CustomDatePicker.FontSizeProperty.PropertyName)
			{
				UpdateFont();
			}
		}

		void UpdateBorder()
		{
			try
			{
				Control.BorderStyle = (_CustomDatePicker.BorderHidden) ? UITextBorderStyle.None : _defaultTextBorderStyle;
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex);
			}
		}

		void UpdateTextAlignment()
		{
			if (_CustomDatePicker.HorizontalTextAlignment == TextAlignment.Start)
			{
				Control.TextAlignment = UITextAlignment.Left;
			}
			else if (_CustomDatePicker.HorizontalTextAlignment == TextAlignment.Center)
			{
				Control.TextAlignment = UITextAlignment.Center;
			}
			else if (_CustomDatePicker.HorizontalTextAlignment == TextAlignment.End)
			{
				Control.TextAlignment = UITextAlignment.Right;
			}
		}

		void UpdateFont()
		{
			try
			{
				Control.Font = UIFont.SystemFontOfSize((nfloat)_CustomDatePicker.FontSize);
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex);
			}
		}
	}
}
