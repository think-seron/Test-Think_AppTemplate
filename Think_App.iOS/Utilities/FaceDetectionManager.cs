using System;
using UIKit;
using CoreImage;
using CoreGraphics;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App.iOS
{
	public static class FaceDetectionManager
	{
		public static Rect? GetFaceRange(UIImage image, int index)
		{
			var context = new CIContext(new CIContextOptions());
			var detector = CIDetector.CreateFaceDetector(context, true);
			var ciImage = CIImage.FromCGImage(image.CGImage);
			var features = detector.FeaturesInImage(ciImage);

			if (features == null || features.Length == 0)
			{
				System.Diagnostics.Debug.WriteLine("顔が検出できませんでした。");
				return null;
			}

			System.Diagnostics.Debug.WriteLine("検出できた顔の数:{0}", features.Length);
			if (features.Length <= index)
			{
				System.Diagnostics.Debug.WriteLine("検出できた顔の数が足りません。indexを小さくしてください。");
				return null;
			}

			var feature = features[index] as CIFaceFeature;
			if (feature == null)
			{
				return null;
			}

			// CoreImageは左下の座標が(0,0)になるので、UIKit座標系に変換する。
			var transform = CGAffineTransform.MakeScale(1, -1);
			transform = CGAffineTransform.Translate(transform, 0, -image.Size.Height);
			var faceRect = CGAffineTransform.CGRectApplyAffineTransform(feature.Bounds, transform);

			return new Rect(faceRect.X, faceRect.Y, faceRect.Width, faceRect.Height);
		}
	}
}
