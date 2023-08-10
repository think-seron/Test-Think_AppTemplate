using System;
using Android.Widget;
using Android.Graphics;
using Android.Util;
using Android.Views;
using Think_App.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(TextService))]
namespace Think_App.Droid
{
	public class TextService : ITextService
	{
		public double CalculateTextWidth(string text, double fontSize)
		{
			var bounds = new Rect();
			var textView = new TextView(Xamarin.Forms.Forms.Context);
			textView.SetTextSize(ComplexUnitType.Sp, (float)fontSize);
			textView.Paint.GetTextBounds(text, 0, text.Length, bounds);
			var widthPx = bounds.Width();
			return ((double)widthPx).ToDpFromPixel();
		}

		public double CalculateTextHeight(string text, double fontSize)
		{
			double height = fontSize;
			try
			{
				var textView = new TextView(Xamarin.Forms.Forms.Context);
				textView.Text = text;
				textView.SetTextSize(ComplexUnitType.Sp, (float)fontSize);
				textView.Measure(View.MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified),
								 View.MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified));
				var heightPx = textView.MeasuredHeight;
				height = heightPx.ToDpFromPixel();
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex);
			}

			return height;
		}
	}
}
