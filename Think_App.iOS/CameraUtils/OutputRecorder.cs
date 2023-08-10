using System;
using AVFoundation;
using UIKit;
using CoreMedia;
using CoreVideo;
using CoreGraphics;
using System.Drawing;

namespace Think_App.iOS
{
	public class OutputRecorder : AVCaptureVideoDataOutputSampleBufferDelegate
	{
		public CameraView Camera { get; set; }
		//private long FrameCount = 1;

		private UIImage GetImageFromSampleBuffer(CMSampleBuffer sampleBuffer)
		{
			// Sample BufferからPixel Bufferを取得する。
			using (var pixelBuffer = sampleBuffer.GetImageBuffer() as CVPixelBuffer)
			{
				// base addressをロックする。
				pixelBuffer.Lock(CVPixelBufferLock.None);

				// バッファのデコードの準備
				var flags = CGBitmapFlags.PremultipliedFirst | CGBitmapFlags.ByteOrder32Little;

				// バッファをデコードし、新しい色空間を作成する。
				using (var cs = CGColorSpace.CreateDeviceRGB())
				{
					// バッファから新しいcontextを作成する
					using (var context = new CGBitmapContext(
						pixelBuffer.BaseAddress,
						pixelBuffer.Width,
						pixelBuffer.Height,
						8,
						pixelBuffer.BytesPerRow,
						cs,
						(CGImageAlphaInfo)flags))
					{
						// contextからイメージを得る。
						using (var cgImage = context.ToImage())
						{
							// ロックを外し、イメージを返す。
							pixelBuffer.Unlock(CVPixelBufferLock.None);
							return UIImage.FromImage(cgImage);
						}
					}
				}
			}
		}

		public override void DidOutputSampleBuffer(AVCaptureOutput captureOutput, CMSampleBuffer sampleBuffer, AVCaptureConnection connection)
		{
			//base.DidOutputSampleBuffer(captureOutput, sampleBuffer, connection);
			try
			{
				if (Camera.IsTakenPhoto)
				{
					// ここでフレーム画像を取得していろいろできる
					var uiImage = GetImageFromSampleBuffer(sampleBuffer);
					var cgImage = uiImage.CGImage;
					UIImage newImage = null;
					if (Camera.Camera == CameraOptions.Front)
					{
						// 自撮りの場合
						newImage = GetRotateImage(uiImage, UIImageOrientation.Right);
						newImage = GetRotateImage(newImage, UIImageOrientation.UpMirrored);
					}
					else
					{
						// リアの場合
						newImage = GetRotateImage(uiImage, UIImageOrientation.Right);
					}
					if (newImage != null)
					{
						var imageSource = Xamarin.Forms.ImageSource.FromStream(() => newImage.AsPNG().AsStream());
						if (imageSource != null)
						{
							Camera.CompleteTakenPhotoAndSendImageSource(imageSource);
						}
					}
				}

				// これがないとReveived memory warning.で落ちたり、画像の更新止まったりする。
				GC.Collect();
			}
			catch (Exception e)
			{
				System.Diagnostics.Debug.WriteLine("Error sampling buffer: {0}", e.Message);
			}
		}

		UIImage GetRotateImage(UIImage srcImage, UIImageOrientation orientation)
		{
			int kMaxResolution = 2048;

			var imgRef = srcImage.CGImage;
			float width = imgRef.Width;
			float height = imgRef.Height;
			CGAffineTransform transform = CGAffineTransform.MakeIdentity();
			var bounds = new RectangleF(0, 0, width, height);

			if (width > kMaxResolution || height > kMaxResolution)
			{
				float ratio = width / height;

				if (ratio > 1)
				{
					bounds.Width = kMaxResolution;
					bounds.Height = bounds.Width / ratio;
				}
				else
				{
					bounds.Height = kMaxResolution;
					bounds.Width = bounds.Height * ratio;
				}
			}

			float scaleRatio = bounds.Width / width;
			var imageSize = new SizeF(width, height);
			float boundHeight;

			if (orientation == UIImageOrientation.Up)
			{
				transform = CGAffineTransform.MakeIdentity();
			}
			else if (orientation == UIImageOrientation.UpMirrored)
			{
				transform = CGAffineTransform.MakeTranslation(imageSize.Width, 0.0f);
				transform = CGAffineTransform.Scale(transform, -1.0f, 1.0f);
			}
			else if (orientation == UIImageOrientation.Down)
			{
				transform = CGAffineTransform.MakeTranslation(imageSize.Width, imageSize.Height);
				transform = CGAffineTransform.Rotate(transform, (float)Math.PI);
			}
			else if (orientation == UIImageOrientation.DownMirrored)
			{
				transform = CGAffineTransform.MakeTranslation(0f, imageSize.Height);
				transform = CGAffineTransform.MakeScale(1.0f, -1.0f);
			}
			else if (orientation == UIImageOrientation.LeftMirrored)
			{
				boundHeight = bounds.Height;
				bounds.Height = bounds.Width;
				bounds.Width = boundHeight;
				transform = CGAffineTransform.MakeTranslation(imageSize.Height, imageSize.Width);
				transform = CGAffineTransform.MakeScale(-1.0f, 1.0f);
				transform = CGAffineTransform.Rotate(transform, 3.0f * (float)Math.PI / 2.0f);
			}
			else if (orientation == UIImageOrientation.Left)
			{
				boundHeight = bounds.Height;
				bounds.Height = bounds.Width;
				bounds.Width = boundHeight;
				transform = CGAffineTransform.MakeTranslation(0.0f, imageSize.Width);
				transform = CGAffineTransform.Rotate(transform, 3.0f * (float)Math.PI / 2.0f);
			}
			else if (orientation == UIImageOrientation.RightMirrored)
			{
				boundHeight = bounds.Height;
				bounds.Height = bounds.Width;
				bounds.Width = boundHeight;
				transform = CGAffineTransform.MakeScale(-1.0f, 1.0f);
				transform = CGAffineTransform.Rotate(transform, (float)Math.PI / 2.0f);
			}
			else if (orientation == UIImageOrientation.Right)
			{
				boundHeight = bounds.Height;
				bounds.Height = bounds.Width;
				bounds.Width = boundHeight;
				transform = CGAffineTransform.MakeTranslation(imageSize.Height, 0.0f);
				transform = CGAffineTransform.Rotate(transform, (float)Math.PI / 2.0f);
			}
			else
			{
				System.Diagnostics.Debug.WriteLine("ここに来てはいけません！");
				return srcImage;
			}

			UIGraphics.BeginImageContext(bounds.Size);

			var context = UIGraphics.GetCurrentContext();

			if (orientation == UIImageOrientation.Right || orientation == UIImageOrientation.Left)
			{
				context.ScaleCTM(-scaleRatio, scaleRatio);
				context.TranslateCTM(-height, 0);
			}
			else
			{
				context.ScaleCTM(scaleRatio, -scaleRatio);
				context.TranslateCTM(0, -height);
			}

			context.ConcatCTM(transform);
			context.DrawImage(new RectangleF(0, 0, width, height), imgRef);

			var newImage = UIGraphics.GetImageFromCurrentImageContext();
			UIGraphics.EndImageContext();

			return newImage;
		}
	}
}
