using System;
using AVFoundation;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App.iOS
{
	public class OutputMetadata : AVCaptureMetadataOutputObjectsDelegate
	{
		public CameraView Camera { get; set; }
		public AVCaptureVideoPreviewLayer PreviewLayer { get; set; }

		public override void DidOutputMetadataObjects(AVCaptureMetadataOutput captureOutput, AVMetadataObject[] metadataObjects, AVCaptureConnection connection)
		{
			//base.DidOutputMetadataObjects(captureOutput, metadataObjects, connection);

			if (metadataObjects == null || metadataObjects.Length == 0)
			{
				if (Camera != null)
				{
					Device.BeginInvokeOnMainThread(() =>
					{
						Camera.SendEventFaceDetectionFailed();
					});
				}
				return;
			}

			foreach (var obj in metadataObjects)
			{
				if (obj.Type == AVMetadataObjectType.Face && obj is AVMetadataFaceObject)
				{
					var face = obj as AVMetadataFaceObject;
					var transformed = PreviewLayer.GetTransformedMetadataObject(face);
					var bounds = transformed.Bounds;
					System.Diagnostics.Debug.WriteLine("Face Range L:{0} T:{1} R;{2} B:{3}", bounds.Left, bounds.Top, bounds.Right, bounds.Bottom);
					var rectangle = new Rect(bounds.X, bounds.Y, bounds.Width, bounds.Height);

					if (Camera != null)
					{
						Device.BeginInvokeOnMainThread(() =>
						{
							Camera.SendEventFaceDetection(100, rectangle, null, null, null);
						});
					}
				}
			}
		}
	}
}
