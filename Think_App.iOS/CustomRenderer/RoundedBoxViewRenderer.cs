using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;

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