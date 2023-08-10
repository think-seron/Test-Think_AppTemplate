using System;
using Xamarin.Forms;
using Think_App.iOS;
using UIKit;
using System.Threading.Tasks;
using System.IO;
using CoreGraphics;
using CoreImage;
using Foundation;
using System.Net.Http;

[assembly: Dependency(typeof(ImageService))]
namespace Think_App.iOS
{
	public class ImageService : IImageService
	{
		public async Task<ImageSource> ResizeAsync(ImageSource source, double width, double height)
		{
			// ImageSource -> UIImage
			var uiimage = await source.ToUIImageAsync();
			if (uiimage == null)
			{
				return null;
			}

			// リサイズ。
			var resizedImage = uiimage.Resize(width, height);

			// UIImage -> ImageSource
			return resizedImage.ToImageSource();
		}

		public async Task<byte[]> ConvertImageSourceToBytesAsync(ImageSource source, bool resize = false, double minLength = 0)
		{
			// ImageSource -> UIImage
			var uiimage = await source.ToUIImageAsync();
			if (uiimage == null)
			{
				return null;
			}

			if (resize)
			{
				// イメージの縦・横を取得する。
				var imageW = uiimage.Size.Width;
				var imageH = uiimage.Size.Height;

				// スケールを算出する。
				var scale = minLength / Math.Min(imageW, imageH);

				// リサイズする。
				uiimage = uiimage.Resize(imageW * scale, imageH * scale);
			}

			// UIImage -> byte[]
			var bytes = uiimage.ToBytes();
			uiimage.Dispose();

			return bytes;
		}

		public async Task<byte[]> ConvertImageSourceToBytesWithCombining(ImageSource srcSource, ImageSource dstSource, Rectangle dstRect, Size viewSize, Aspect aspect, bool resize = false, double minLength = 0)
		{
			// ImageSource -> UIImage
			var srcImage = await srcSource.ToUIImageAsync();
			var dstImage = await dstSource.ToUIImageAsync();
			if (srcImage == null || dstImage == null)
			{
				return null;
			}

			// 合成のため、座標系の変換
			double scale = 0.0;
			var scaleW = srcImage.Size.Width / viewSize.Width;
			var scaleH = srcImage.Size.Height / viewSize.Height;
			if (aspect == Aspect.AspectFill)
			{
				scale = Math.Min(scaleW, scaleH);
			}
			else if (aspect == Aspect.AspectFit)
			{
				scale = Math.Max(scaleW, scaleH);
			}
			else
			{
				System.Diagnostics.Debug.WriteLine("Fillには対応しません。");
			}

			var scaledWidth = viewSize.Width * scale;
			var scaledHeight = viewSize.Height * scale;

			// AspectFitは左右・上下の隙間、AspectFillは左右・上下の切り捨て分の補正
			var difX = (srcImage.Size.Width - scaledWidth) / 2.0;
			var difY = (srcImage.Size.Height - scaledHeight) / 2.0;

			// 座標系変換。
			var newWidth = dstRect.Width * scale;
			var newHeight = dstRect.Height * scale;
			var newX = dstRect.X * scale + difX;
			var newY = dstRect.Y * scale + difY;
			var newRect = new CGRect(newX, newY, newWidth, newHeight);

			var screenScale = UIScreen.MainScreen.Scale;
			UIGraphics.BeginImageContextWithOptions(srcImage.Size, false, screenScale);
			// 元のサイズのままsrcImageを描画。
			srcImage.Draw(new CGRect(0, 0, srcImage.Size.Width, srcImage.Size.Height));
			// リサイズしてdstImageを合成描画。
			dstImage.Draw(newRect, CGBlendMode.Normal, 1.0f);
			// 現在のイメージを取得。
			var combinedImage = UIGraphics.GetImageFromCurrentImageContext();
			UIGraphics.EndImageContext();

			if (resize)
			{
				// イメージの縦・横を取得する。
				var imageW = combinedImage.Size.Width;
				var imageH = combinedImage.Size.Height;

				// スケールを算出する。
				var s = minLength / Math.Min(imageW, imageH);

				// リサイズ。
				combinedImage = combinedImage.Resize(imageW * s, imageH * s);
			}

			// UIImage -> byte[]
			var bytes = combinedImage.ToBytes();
			combinedImage.Dispose();

			return bytes;
		}

		public ImageSource ConvertBytesToImageSource(byte[] bytes)
		{
			var source = ImageSource.FromStream(() => new MemoryStream(bytes));

			return source;
		}

		public async Task<Size> GetImageSizeAsync(ImageSource source)
		{
			var size = Size.Zero;
			try
			{
				// ImageSource -> UIImage
				using (var uiimage = await source.ToUIImageAsync())
				{
					size.Width = uiimage.Size.Width;
					size.Height = uiimage.Size.Height;
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine("サイズ取得失敗:{0}", ex);
			}

			return size;
		}

		public async Task<Rectangle?> GetFaceRangeFromImageSource(ImageSource source, double viewWidth, double viewHeight, Aspect aspect)
		{
			Rectangle? rect = null;
			using (var uiImage = await source.ToUIImageAsync())
			{
				// UIImageを元に、顔の範囲を取得する。(index == 0)
				var faceRect = FaceDetectionManager.GetFaceRange(uiImage, 0);
				if (faceRect != null)
				{
					// 顔の範囲が取れたら、viewのサイズに合わせて座標変換をかける。
					var scaleW = viewWidth / uiImage.Size.Width;
					var scaleH = viewHeight / uiImage.Size.Height;
					double shiftX = 0.0;
					double shiftY = 0.0;

					if (aspect == Aspect.AspectFill)
					{
						scaleW = Math.Max(scaleW, scaleH);
						scaleH = scaleW;

						shiftX = (viewWidth - uiImage.Size.Width * scaleW) / 2.0;
						shiftY = (viewHeight - uiImage.Size.Height * scaleH) / 2.0;
					}
					else if (aspect == Aspect.AspectFit)
					{
						scaleW = Math.Min(scaleW, scaleH);
						scaleH = scaleW;

						shiftX = (viewWidth - uiImage.Size.Width * scaleW) / 2.0;
						shiftY = (viewHeight - uiImage.Size.Height * scaleH) / 2.0;
					}

					var x = faceRect.Value.X * scaleW + shiftX;
					var y = faceRect.Value.Y * scaleH + shiftY;
					var w = faceRect.Value.Width * scaleW;
					var h = faceRect.Value.Height * scaleH;

					rect = new Rectangle(x, y, w, h);
				}
			}
			       
			return rect;
		}

		public ImageSource GetOrientationAdjustedImageSource(string filePath, bool resize = false, double minLength = 0)
		{
			// 画像を作成。
			var uiImage = UIImage.FromFile(filePath);

			var cgImage = uiImage.CGImage;
			var width = cgImage.Width;
			var height = cgImage.Height;

			var bounds = new CGRect(0, 0, width, height);
			var imageSize = new CGSize(width, height);

			// アフィン変換用の実装。
			CGAffineTransform transform;
			var o = uiImage.Orientation;
			if (o == UIImageOrientation.Up)
			{
				transform = CGAffineTransform.MakeIdentity();
			}
			else if (o == UIImageOrientation.UpMirrored)
			{
				transform = CGAffineTransform.MakeTranslation(imageSize.Width, 0.0f);
				transform = CGAffineTransform.Scale(transform, -1.0f, 1.0f);
			}
			else if (o == UIImageOrientation.Down)
			{
				transform = CGAffineTransform.MakeTranslation(imageSize.Width, imageSize.Height);
				transform = CGAffineTransform.Rotate(transform, (nfloat)Math.PI);
			}
			else if (o == UIImageOrientation.DownMirrored)
			{
				transform = CGAffineTransform.MakeTranslation(0.0f, imageSize.Height);
				transform = CGAffineTransform.Scale(transform, 1.0f, -1.0f);
			}
			else if (o == UIImageOrientation.LeftMirrored)
			{
				bounds.Size = new CGSize(bounds.Size.Height, bounds.Size.Width);
				transform = CGAffineTransform.MakeTranslation(imageSize.Height, imageSize.Width);
				transform = CGAffineTransform.Scale(transform, -1.0f, 1.0f);
				transform = CGAffineTransform.Rotate(transform, (nfloat)(3.0 * Math.PI / 2.0));
			}
			else if (o == UIImageOrientation.Left)
			{
				bounds.Size = new CGSize(bounds.Size.Height, bounds.Size.Width);
				transform = CGAffineTransform.MakeTranslation(0.0f, imageSize.Width);
				transform = CGAffineTransform.Rotate(transform, (nfloat)(3.0 * Math.PI / 2.0));
			}
			else if (o == UIImageOrientation.RightMirrored)
			{
				bounds.Size = new CGSize(bounds.Size.Height, bounds.Size.Width);
				transform = CGAffineTransform.MakeScale(-1.0f, 1.0f);
				transform = CGAffineTransform.Rotate(transform, (nfloat)(Math.PI / 2.0));
			}
			else if (o == UIImageOrientation.Right)
			{
				bounds.Size = new CGSize(bounds.Size.Height, bounds.Size.Width);
				transform = CGAffineTransform.MakeTranslation(imageSize.Height, 0.0f);
				transform = CGAffineTransform.Rotate(transform, (nfloat)(Math.PI / 2.0));
			}
			else
			{
				System.Diagnostics.Debug.WriteLine("Orientation Undefined.");
				transform = CGAffineTransform.MakeIdentity();
			}

			UIGraphics.BeginImageContext(bounds.Size);
			var context = UIGraphics.GetCurrentContext();

			if (o == UIImageOrientation.Right || o == UIImageOrientation.Left)
			{
				context.ScaleCTM(-1.0f, 1.0f);
				context.TranslateCTM(-height, 0.0f);
			}
			else
			{
				context.ScaleCTM(1.0f, -1.0f);
				context.TranslateCTM(0.0f, -height);
			}

			context.ConcatCTM(transform);

			context.DrawImage(new CGRect(0, 0, width, height), cgImage);
			var adjustedImage = UIGraphics.GetImageFromCurrentImageContext();
			UIGraphics.EndImageContext();

			// 元画像は破棄
			uiImage.Dispose();
			uiImage = null;

			if (resize)
			{
				// イメージの縦・横を取得する。
				var imageW = adjustedImage.Size.Width;
				var imageH = adjustedImage.Size.Height;

				// スケールを算出する。
				var s = minLength / Math.Min(imageW, imageH);

				// リサイズ。
				adjustedImage = adjustedImage.Resize(imageW * s, imageH * s);
			}

			// UIImage -> ImageSource
			var source = adjustedImage.ToImageSource();

			return source;
		}

		public async Task<ImageSource> CloneImageSourceAsync(ImageSource source)
		{
			var uiimage = await source.ToUIImageAsync();
			return uiimage.ToImageSource();
		}

		public async Task<ImageSource> GetCroppedImageSourceAsync(ImageSource source, Rectangle croppingRect)
		{
			// ImageSource -> UIImage
			var uiImage = await source.ToUIImageAsync();
			var scale = uiImage.CurrentScale;
			var orientaion = uiImage.Orientation;

			// UIImage -> CGImage
			var cgImage = uiImage.CGImage;

			// CGImageのトリミング範囲
			var left = (croppingRect.Left * scale).Clamp(0, cgImage.Width);
			var top = (croppingRect.Top * scale).Clamp(0, cgImage.Height);
			var right = (croppingRect.Right * scale).Clamp(0, cgImage.Width);
			var bottom = (croppingRect.Bottom * scale).Clamp(0, cgImage.Height);
			var rect = new CGRect(left, top, right - left, bottom - top);

			// トリミング
			var croppedcgImage = cgImage.WithImageInRect(rect);

			// CGImage -> UIImage
			var croppeduiImage = UIImage.FromImage(croppedcgImage, scale, orientaion);

			// UIImage -> ImageSource
			var croppedSource = croppeduiImage.ToImageSource();

			return croppedSource;
		}


		public ImageSource GetOrientationAdjustedImageSourceReduction(string filePath, bool resizeflg = false, double minLength = 0)
		{
			// ios は画像の向き正しく表示されるので画像サイズだけ修正
			var uiImage = UIImage.FromFile(filePath);
			int resize;
			if ((uiImage.Size.Width* uiImage.Size.Height) > 1048576)
			{
				//１Mピクセル超えてる
				double outArea = (double)(uiImage.Size.Width * uiImage.Size.Height) / 1048576.0;
				resize = (int)(Math.Sqrt(outArea) + 1);
			}
			else
			{
				//小さいのでそのまま
				resize = 1;
			}
			var width = (double)(uiImage.Size.Width / resize);
			var height = (double)(uiImage.Size.Height / resize);

			// リサイズ
			UIGraphics.BeginImageContext(new CoreGraphics.CGSize(width, height));
			uiImage.Draw(new CGRect(0, 0, width, height));
			UIImage resizedImage = UIGraphics.GetImageFromCurrentImageContext();
			UIGraphics.EndImageContext();

			// UIImage -> ImageSource
			return resizedImage.ToImageSource();
		}

		public ImageSource ResizeNetImage(string filePath)
		{
			UIImage uiImage = null;
			using (var client = new HttpClient())
		    {
				byte[] content = null;
				var task = Task.Run(async () =>
				{
					// imageUrlからバイト配列を取得します。
					content = await client.GetByteArrayAsync(filePath);
				});

				while (true)
				{
					if (task.IsCompleted)
					{
						break;
					}
				}
		        // バイト配列のデータからUIImageを生成します。
				uiImage = UIImage.LoadFromData(NSData.FromArray(content));
		    }

			if (uiImage == null)
			{
				return null;
			}

			int resize;
			if ((uiImage.Size.Width * uiImage.Size.Height) > 1048576)
			{
				//１Mピクセル超えてる
				double outArea = (double)(uiImage.Size.Width * uiImage.Size.Height) / 1048576.0;
				resize = (int)(Math.Sqrt(outArea) + 1);
			}
			else
			{
				//小さいのでそのまま
				resize = 1;
			}
			var width = (double)(uiImage.Size.Width / resize);
			var height = (double)(uiImage.Size.Height / resize);

			// リサイズ
			UIGraphics.BeginImageContext(new CoreGraphics.CGSize(width, height));
			uiImage.Draw(new CGRect(0, 0, width, height));
			UIImage resizedImage = UIGraphics.GetImageFromCurrentImageContext();
			UIGraphics.EndImageContext();

			// UIImage -> ImageSource
			return resizedImage.ToImageSource();
		}
	}
}
