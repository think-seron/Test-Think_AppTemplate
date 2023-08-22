using System;
using System.ComponentModel;
using Think_App;
using Think_App.iOS;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

// TODO Xamarin.Forms.ExportRendererAttribute is not longer supported. For more details see https://github.com/dotnet/maui/wiki/Using-Custom-Renderers-in-.NET-MAUI
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
