using System;
using System.ComponentModel;
using Think_App;
using Think_App.Droid;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

// TODO Xamarin.Forms.ExportRendererAttribute is not longer supported. For more details see https://github.com/dotnet/maui/wiki/Using-Custom-Renderers-in-.NET-MAUI
[assembly: ExportRenderer(typeof(CameraView), typeof(CameraViewRenderer))]
namespace Think_App.Droid
{
	public class CameraViewRenderer : ViewRenderer<CameraView, DroidCameraView>
	{
		DroidCameraView _cameraView;

		protected override void OnElementChanged(ElementChangedEventArgs<CameraView> e)
		{
			base.OnElementChanged(e);

			if (e.NewElement == null)
			{
				return;
			}

			if (Control == null)
			{
				_cameraView = new DroidCameraView(Context, e.NewElement);
				SetNativeControl(_cameraView);
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
				Control.Initialize();
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
				if (Control != null && Control.Preview != null)
				{
					Control.Release();
				}

				MessagingCenter.Unsubscribe<LifeCyclePayload>(Control, "");
			}

			base.Dispose(disposing);
		}
	}
}
