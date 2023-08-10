using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Think_App;
using Think_App.iOS;

[assembly: ExportRenderer(typeof(CameraView), typeof(CameraViewRenderer))]
namespace Think_App.iOS
{
	public class CameraViewRenderer : ViewRenderer<CameraView, UICameraView>
	{
		protected override void OnElementChanged(ElementChangedEventArgs<CameraView> e)
		{
			base.OnElementChanged(e);

			if (Control == null || e.NewElement != null)
			{
				var uiCameraView = new UICameraView(e.NewElement);
				SetNativeControl(uiCameraView);
			}
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (Element == null || Control == null)
			{
				return;
			}

			if (e.PropertyName == CameraView.IsPreviewingProperty.PropertyName)
			{
				Control.IsPreviewing = Element.IsPreviewing;
			}
			else if (e.PropertyName == CameraView.CameraProperty.PropertyName)
			{
				Control.Release();
				Control.Initialize(Element.Camera);
			}
			else if (e.PropertyName == CameraView.IsFlashProperty.PropertyName)
			{
				Control.IsFlash = Element.IsFlash;
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				Control.Release();
				Control.CaptureSession.Dispose();
				MessagingCenter.Unsubscribe<LifeCyclePayload>(Control, "");
				Control.Dispose();
			}

			base.Dispose(disposing);
		}
	}
}
