using System;
using System.ComponentModel;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

// TODO Xamarin.Forms.ExportRendererAttribute is not longer supported. For more details see https://github.com/dotnet/maui/wiki/Using-Custom-Renderers-in-.NET-MAUI
[assembly: ExportRenderer(typeof(Think_App.RoundedBoxView), typeof(Think_App.Droid.RoundedBoxViewRenderer))]

namespace Think_App.Droid
{
	public class RoundedBoxViewRenderer : ViewRenderer<RoundedBoxView, Android.Views.View>
	{
		Android.Graphics.Drawables.GradientDrawable controlBackground;

		protected override void OnElementChanged(ElementChangedEventArgs<RoundedBoxView> e)
		{
			if (Control == null)
			{
				var nativeControl = new Android.Views.View(Context);
				controlBackground = new Android.Graphics.Drawables.GradientDrawable();
				nativeControl.Background = controlBackground;
				//nativeControl.Click += OnClick;
				SetNativeControl(nativeControl);
			}

			if (e.NewElement != null)
			{
				UpdateRadius();
				UpdateColor();
				UpdateBorder();
			}

			base.OnElementChanged(e);
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == RoundedBoxView.CornerRadiusProperty.PropertyName)
			{
				UpdateRadius();
			}
			if (e.PropertyName == RoundedBoxView.ColorProperty.PropertyName)
			{
				UpdateColor();
			}
			if (e.PropertyName == RoundedBoxView.BorderThicknessProperty.PropertyName)
			{
				UpdateBorder();
			}
			if (e.PropertyName == RoundedBoxView.BorderColorProperty.PropertyName)
			{
				UpdateBorder();
			}
		}

		private void UpdateRadius()
		{
			var radiusDp = (float)(Element.CornerRadius * Resources.DisplayMetrics.Density);
			controlBackground.SetCornerRadius(radiusDp);
		}

		private void UpdateColor()
		{
			controlBackground.SetColor(Element.Color.ToAndroid());
		}

		private void UpdateBorder()
		{
			var thicknessDp = Element.BorderThickness * (int)Resources.DisplayMetrics.Density;
			controlBackground.SetStroke(thicknessDp, Element.BorderColor.ToAndroid());
		}

		//private void OnClick(object sender, EventArgs e)
		//{
		//	Element?.SendClick();
		//}

		protected override void Dispose(bool disposing)
		{
			//if (Control != null)
			//	Control.Click -= OnClick;

			base.Dispose(disposing);
		}
	}
}