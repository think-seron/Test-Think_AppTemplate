using System.ComponentModel;
using UIKit;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

// TODO Xamarin.Forms.ExportRendererAttribute is not longer supported. For more details see https://github.com/dotnet/maui/wiki/Using-Custom-Renderers-in-.NET-MAUI
[assembly: ExportRenderer(typeof(Think_App.RoundedBoxView), typeof(Think_App.iOS.RoundedBoxViewRenderer))]

namespace Think_App.iOS
{
	public class RoundedBoxViewRenderer : ViewRenderer<RoundedBoxView, UIView>
	{
		//private UITapGestureRecognizer tapGesuture;

		protected override void OnElementChanged(ElementChangedEventArgs<RoundedBoxView> e)
		{
			if (Control == null)
			{
				var nativeControl = new UIView();
				//tapGesuture = new UITapGestureRecognizer(() => Element?.SendClick());
				//nativeControl.AddGestureRecognizer(tapGesuture);
				SetNativeControl(nativeControl);
			}

			if (e.NewElement != null)
			{
				UpdateRadius();
				UpdateColor();
				UpdateBorderThickness();
				UpdateBorderColor();
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
				UpdateBorderThickness();
			}
			if (e.PropertyName == RoundedBoxView.BorderColorProperty.PropertyName)
			{
				UpdateBorderColor();
			}
		}

		private void UpdateRadius()
		{
			Control.Layer.CornerRadius = (float)Element.CornerRadius;
		}

		private void UpdateColor()
		{
			Control.BackgroundColor = Element.Color.ToUIColor();
		}

		private void UpdateBorderThickness()
		{
			Control.Layer.BorderWidth = (float)Element.BorderThickness;
		}

		private void UpdateBorderColor()
		{
			Control.Layer.BorderColor = Element.BorderColor.ToCGColor();
		}

		protected override void Dispose(bool disposing)
		{
			//if (Control != null)
			//	Control.RemoveGestureRecognizer(tapGesuture);

			base.Dispose(disposing);
		}
	}
}