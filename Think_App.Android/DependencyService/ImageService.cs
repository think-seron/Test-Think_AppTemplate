using System;
using System.IO;
using System.Threading.Tasks;
using Android.Graphics;
using Android.Media;
using Think_App.Droid;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

[assembly: Dependency(typeof(ImageService))]
namespace Think_App.Droid
{
	public class ImageService : IImageService
	{
		public async Task<ImageSource> ResizeAsync(ImageSource source, double width, double height)
		{
			// ImageSource -> Bitmap
			var bitmap = await source.ToBitmapAsync();
			if (bitmap == null)
			{
				return null;
			}

			// リサイズ。
			var screenScale = Forms.Context.Resources.DisplayMetrics.Density;
			var resizedBitmap = Bitmap.CreateScaledBitmap(bitmap, (int)(Math.Round(width * screenScale)), (int)(Math.Round(height * screenScale)), true);

			return resizedBitmap.ToImageSource();
		}

		public async Task<byte[]> ConvertImageSourceToBytesAsync(ImageSource source, bool resize = false, double minLength = 0)
		{
			// ImageSource -> Bitmap
			var bitmap = await source.ToBitmapAsync();
			if (bitmap == null)
			{
				return null;
			}

			if (resize)
			{
				var imageW = bitmap.Width;
				var imageH = bitmap.Height;
				// スケールを算出する。
				var screenScale = Forms.Context.Resources.DisplayMetrics.Density;
				var scale = minLength * screenScale / (double)(Math.Min(imageW, imageH));
				var newW = (int)(Math.Round(imageW * scale));
				var newH = (int)(Math.Round(imageH * scale));
				// リサイズする。
				bitmap = Bitmap.CreateScaledBitmap(bitmap, newW, newH, true);
			}

			// Bitmap -> byte[]
			var bytes = bitmap.ToBytes();
			bitmap.Dispose();

			return bytes;
		}

		public async Task<byte[]> ConvertImageSourceToBytesWithCombining(ImageSource srcSource, ImageSource dstSource, Rect dstRect, Size viewSize, Aspect aspect, bool resize = false, double minLength = 0)
		{
			// ImageSource -> Bitmap
			var srcBitmap = await srcSource.ToBitmapAsync();
			var dstBitmap = await dstSource.ToBitmapAsync();
			if (srcBitmap == null || dstBitmap == null)
			{
				return null;
			}

			// 合成のため、座標系の変換
			double scale = 0.0;
			var scaleW = srcBitmap.Width / viewSize.Width;
			var scaleH = srcBitmap.Height / viewSize.Height;
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
			var difX = (srcBitmap.Width - scaledWidth) / 2.0;
			var difY = (srcBitmap.Height - scaledHeight) / 2.0;

			// 座標系変換。
			var newWidth = (float)(dstRect.Width * scale);
			var newHeight = (float)(dstRect.Height * scale);
			var newX = (float)(dstRect.X * scale + difX);
			var newY = (float)(dstRect.Y * scale + difY);
			var newRect = new RectF(newX, newY, newX + newWidth, newY + newHeight);

			var bitmap = Bitmap.CreateBitmap(srcBitmap.Width, srcBitmap.Height, Bitmap.Config.Argb8888);

			if (resize)
			{
				var imageW = bitmap.Width;
				var imageH = bitmap.Height;
				// スケールを算出する。
				var screenScale = Forms.Context.Resources.DisplayMetrics.Density;
				var s = minLength * screenScale / (double)(Math.Min(imageW, imageH));
				var newW = (int)(Math.Round(imageW * s));
				var newH = (int)(Math.Round(imageH * s));
				// リサイズする。
				bitmap = Bitmap.CreateScaledBitmap(bitmap, newW, newH, true);
			}

			byte[] bytes = null;
			using (var canvas = new Canvas(bitmap))
			{
				using (var paint = new Paint())
				{
					paint.AntiAlias = true;
					paint.FilterBitmap = true;

					canvas.DrawBitmap(srcBitmap, 0, 0, paint);
					canvas.DrawBitmap(dstBitmap, null, newRect, paint);

					// Bitmap -> byte[]
					bytes = bitmap.ToBytes();
				}
			}

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
			var screenScale = Forms.Context.Resources.DisplayMetrics.Density;

			try
			{
				// ImageSource -> Bitmap
				using (var bitmap = await source.ToBitmapAsync())
				{
					// Pixel -> Dp
					size.Width = bitmap.Width / screenScale;
					size.Height = bitmap.Height / screenScale;
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine("サイズ取得失敗:{0}", ex);
			}

			return size;
		}

		public async Task<Rect?> GetFaceRangeFromImageSource(ImageSource source, double viewWidth, double viewHeight, Aspect aspect)
		{
			Rect? rect = null;
			using (var bitmap = await source.ToBitmapAsync())
			{
				// Bitmapを元に、顔の範囲を取得する。(index == 0)
				var faceRect = FaceDetectionManager.GetFaceRange(bitmap, 0);
				if (faceRect != null)
				{
					// 顔の範囲が取れたら、viewのサイズに合わせて座標変換をかける。
					var scaleW = viewWidth / (double)bitmap.Width;
					var scaleH = viewHeight / (double)bitmap.Height;
					double shiftX = 0.0;
					double shiftY = 0.0;

					if (aspect == Aspect.AspectFill)
					{
						scaleW = Math.Max(scaleW, scaleH);
						scaleH = scaleW;

						shiftX = (viewWidth - bitmap.Width * scaleW) / 2.0;
						shiftY = (viewHeight - bitmap.Height * scaleH) / 2.0;
					}
					else if (aspect == Aspect.AspectFit)
					{
						scaleW = Math.Min(scaleW, scaleH);
						scaleH = scaleW;

						shiftX = (viewWidth - bitmap.Width * scaleW) / 2.0;
						shiftY = (viewHeight - bitmap.Height * scaleH) / 2.0;
					}

					var x = faceRect.Value.X * scaleW + shiftX;
					var y = faceRect.Value.Y * scaleH + shiftY;
					var w = faceRect.Value.Width * scaleW;
					var h = faceRect.Value.Height * scaleH;

					rect = new Rect(x, y, w, h);
				}
			}

			return rect;
		}

		public ImageSource GetOrientationAdjustedImageSource(string filePath, bool resize = false, double minLength = 0)
		{
			// 元画像の取得
			var orgBitmap = BitmapFactory.DecodeFile(filePath);
			var width = orgBitmap.Width;
			var height = orgBitmap.Height;

			// マトリックスを回転情報から決定する。
			var matrix = new Matrix();
			try
			{
				var exifInterface = new ExifInterface(filePath);
				var o = (Orientation)exifInterface.GetAttributeInt(ExifInterface.TagOrientation, (int)Orientation.Undefined);

				if (o == Orientation.Normal)
				{
					// なにもしない
				}
				else if (o == Orientation.FlipHorizontal)
				{
					// 水平方向にリフレクト
					matrix.PostScale(-1f, 1f);
				}
				else if (o == Orientation.Rotate180)
				{
					// 180度回転
					matrix.PostRotate(180f);
				}
				else if (o == Orientation.FlipVertical)
				{
					// 垂直方向にリフレクト
					matrix.PostScale(1f, -1f);
				}
				else if (o == Orientation.Rotate90)
				{
					// 反時計周り90度回転
					matrix.PostRotate(90f);
				}
				else if (o == Orientation.Transverse)
				{
					// 時計回り90度回転し、垂直方向にリフレクト
					matrix.PostRotate(-90f);
					matrix.PostScale(1f, -1f);
				}
				else if (o == Orientation.Transpose)
				{
					// 反時計回り90度回転し、垂直方向にリフレクト
					matrix.PostRotate(90f);
					matrix.PostScale(1f, -1f);
				}
				else if (o == Orientation.Rotate270)
				{
					// 反時計回りに270度回転＝時計回りに90度回転
					matrix.PostRotate(-90f);
				}
			}
			catch(IOException e)
			{
				System.Diagnostics.Debug.WriteLine(e);
			}

			// マトリックスを反映させたBitmapを生成する。
			var bitmap = Bitmap.CreateBitmap(orgBitmap, 0, 0, width, height, matrix, true);

			// orgBitmapは破棄 -> 破棄するとダメなケースがあったので封印します。
			//orgBitmap.Dispose();
			//orgBitmap = null;

			if (resize)
			{
				var imageW = bitmap.Width;
				var imageH = bitmap.Height;
				// スケールを算出する。
				var screenScale = Forms.Context.Resources.DisplayMetrics.Density;
				var s = minLength * screenScale / (double)(Math.Min(imageW, imageH));
				var newW = (int)(Math.Round(imageW * s));
				var newH = (int)(Math.Round(imageH * s));
				// リサイズする。
				bitmap = Bitmap.CreateScaledBitmap(bitmap, newW, newH, true);
			}

			// Bitmap -> ImageSource
			var source = bitmap.ToImageSource();

			return source;
		}

		public async Task<ImageSource> CloneImageSourceAsync(ImageSource source)
		{
			var bitmap = await source.ToBitmapAsync();
			return bitmap.ToImageSource();
		}

		public async Task<ImageSource> GetCroppedImageSourceAsync(ImageSource source, Rect croppingRect)
		{
			// ImageSource -> Bitmap
			var bitmap = await source.ToBitmapAsync();

			// Bitmapのトリミング範囲
			var left = ((int)croppingRect.Left.ToPixelFromDp()).Clamp(0, bitmap.Width);
			var top = ((int)croppingRect.Top.ToPixelFromDp()).Clamp(0, bitmap.Height);
			var right = ((int)croppingRect.Right.ToPixelFromDp()).Clamp(0, bitmap.Width);
			var bottom = ((int)croppingRect.Bottom.ToPixelFromDp()).Clamp(0, bitmap.Height);
			var width = right - left;
			var height = bottom - top;

			// トリミングしたBitmapの作成
			var croppedBitmap = Bitmap.CreateBitmap(bitmap, left, top, width, height, null, true);

			// Bitmap -> ImageSource
			var croppedSource = croppedBitmap.ToImageSource();

			return croppedSource;
		}


		public ImageSource GetOrientationAdjustedImageSourceReduction(string filePath, bool resize = false, double minLength = 0)
		{
			BitmapFactory.Options bmfOptions = new BitmapFactory.Options();
			bmfOptions.InJustDecodeBounds = true ;
			int sampleSize;
			// サイズ取得し縮小するscaleを求める
			using (BitmapFactory.DecodeFile(filePath, bmfOptions))
			{
				if((bmfOptions.OutWidth * bmfOptions.OutHeight) > 1048576){
					//１Mピクセル超えてる
					double outArea = (double)(bmfOptions.OutWidth * bmfOptions.OutHeight) / 1048576.0;
					sampleSize = (int) (Math.Sqrt(outArea) + 1);
				}else{
					//小さいのでそのまま
					sampleSize = 1;
				}
			}

			// 画像を実際に読み込む指定
			bmfOptions.InJustDecodeBounds = false ;

			//BitmapFactory.Options bmfOptions = new BitmapFactory.Options();
			// 画像を1/InSampleSize サイズに縮小（メモリ対策）
			bmfOptions.InSampleSize = sampleSize;
			// システムメモリ上に再利用性の無いオブジェクトがある場合に勝手に解放（メモリ対策）
			bmfOptions.InPurgeable = true;
			//bmfOptions.InPreferredConfig = Bitmap.Config.Argb4444;

			// 元画像の取得
			//var orgBitmap = BitmapFactory.DecodeFile(filePath, bmfOptions);
			////var orgBitmap = BitmapFactory.DecodeFile(filePath);
			//var width = orgBitmap.Width;
			//var height = orgBitmap.Height;

			// メモリオーバー対策でusing使用
			using (Bitmap orgBitmap = BitmapFactory.DecodeFile(filePath, bmfOptions))
			{
				// Bitmap -> ImageSource
				var width = orgBitmap.Width;
				var height = orgBitmap.Height;

				// マトリックスを回転情報から決定する。
				var matrix = new Matrix();
				try
				{
					var exifInterface = new ExifInterface(filePath);
					var o = (Orientation)exifInterface.GetAttributeInt(ExifInterface.TagOrientation, (int)Orientation.Undefined);

					if (o == Orientation.Normal)
					{
						// なにもしない
					}
					else if (o == Orientation.FlipHorizontal)
					{
						// 水平方向にリフレクト
						matrix.PostScale(-1f, 1f);
					}
					else if (o == Orientation.Rotate180)
					{
						// 180度回転
						matrix.PostRotate(180f);
					}
					else if (o == Orientation.FlipVertical)
					{
						// 垂直方向にリフレクト
						matrix.PostScale(1f, -1f);
					}
					else if (o == Orientation.Rotate90)
					{
						// 反時計周り90度回転
						matrix.PostRotate(90f);
					}
					else if (o == Orientation.Transverse)
					{
						// 時計回り90度回転し、垂直方向にリフレクト
						matrix.PostRotate(-90f);
						matrix.PostScale(1f, -1f);
					}
					else if (o == Orientation.Transpose)
					{
						// 反時計回り90度回転し、垂直方向にリフレクト
						matrix.PostRotate(90f);
						matrix.PostScale(1f, -1f);
					}
					else if (o == Orientation.Rotate270)
					{
						// 反時計回りに270度回転＝時計回りに90度回転
						matrix.PostRotate(-90f);
					}
				}
				catch (IOException e)
				{
					System.Diagnostics.Debug.WriteLine(e);
				}

				// マトリックスを反映させたBitmapを生成する。 メモリオーバー対策でusing使用
				using (Bitmap bitmap = Bitmap.CreateBitmap(orgBitmap, 0, 0, width, height, matrix, true))
				{
					if (resize)
					{
						var imageW = bitmap.Width;
						var imageH = bitmap.Height;
						// スケールを算出する。
						var screenScale = Forms.Context.Resources.DisplayMetrics.Density;
						var s = minLength * screenScale / (double)(Math.Min(imageW, imageH));
						var newW = (int)(Math.Round(imageW * s));
						var newH = (int)(Math.Round(imageH * s));
						using (Bitmap resizedBitmap = Bitmap.CreateScaledBitmap(bitmap, newW, newH, true))
						{
							// Bitmap -> ImageSource
							var source = bitmap.ToImageSource();
							return source;
						}
					}
					else
					{
						// Bitmap -> ImageSource
						var source = bitmap.ToImageSource();
						return source;
					}
				}
			}
			//var bitmap = Bitmap.CreateBitmap(orgBitmap, 0, 0, width, height, matrix, true);
			//// Bitmap -> ImageSource
			//var source = bitmap.ToImageSource();

			//return source;
		}


		public ImageSource ResizeNetImage(string filePath)
		{
			BitmapFactory.Options bmfOptions = new BitmapFactory.Options();
			bmfOptions.InJustDecodeBounds = true;
			int sampleSize;


			var task = new Task<ImageSource>(() =>
			{
				var url =  new Java.Net.URL(filePath);
				Java.Net.HttpURLConnection connection = (Java.Net.HttpURLConnection)url.OpenConnection();
				connection.DoInput = true;  
				connection.Connect();  

				System.IO.Stream inputStream;
				inputStream = connection.InputStream;
			
				BitmapFactory.DecodeStream(inputStream, null, bmfOptions);
				inputStream.Close();

				if ((bmfOptions.OutWidth * bmfOptions.OutHeight) > 1048576)
				{
					//１Mピクセル超えてる
					double outArea = (double)(bmfOptions.OutWidth * bmfOptions.OutHeight) / 1048576.0;
					sampleSize = (int)(Math.Sqrt(outArea) + 1);
				}
				else
				{
					//小さいのでそのまま
					sampleSize = 1;
				}

				// 画像を実際に読み込む指定
				bmfOptions.InJustDecodeBounds = false;
				// 画像を1/InSampleSize サイズに縮小（メモリ対策）
				bmfOptions.InSampleSize = sampleSize;
				// システムメモリ上に再利用性の無いオブジェクトがある場合に勝手に解放（メモリ対策）
				bmfOptions.InPurgeable = true;

				connection = (Java.Net.HttpURLConnection)url.OpenConnection();
				connection.DoInput = true;  
				connection.Connect();
				inputStream = connection.InputStream;

				// メモリオーバー対策でusing使用
				using (Bitmap orgBitmap = BitmapFactory.DecodeStream(inputStream,null, bmfOptions))
				{
					// Bitmap -> ImageSource
					var width = orgBitmap.Width;
					var height = orgBitmap.Height;
					inputStream.Close();

					// マトリックスを回転情報から決定する。
					var matrix = new Matrix();
					try
					{
						// ReadJpegが失敗することがあるので最大5回情報取得試みる
						for (int i = 0; i <= 5; i++ )
						{
							connection = (Java.Net.HttpURLConnection)url.OpenConnection();
							connection.DoInput = true;
							connection.Connect();
							inputStream = connection.InputStream;
							var jpegInfo = ExifLib.ExifReader.ReadJpeg(inputStream).Orientation;
							if (jpegInfo != 0)
							{
								System.Diagnostics.Debug.WriteLine("jpegInfo：" + jpegInfo);
								inputStream.Close();
								if (jpegInfo == ExifLib.ExifOrientation.BottomRight)
								{
									// 180度回転
									matrix.PostRotate(180f);
								}
								else if (jpegInfo == ExifLib.ExifOrientation.TopRight)
								{
									// 反時計周り90度回転
									matrix.PostRotate(90f);
								}
								else if (jpegInfo == ExifLib.ExifOrientation.BottomLeft)
								{
									// 反時計回りに270度回転＝時計回りに90度回転
									matrix.PostRotate(-90f);
								}
								inputStream.Close();
								connection.Disconnect();
								System.Diagnostics.Debug.WriteLine("Loop回数：" + i);
								break;
							}
							connection.Disconnect();
							inputStream.Close();
						}
					}
					catch (IOException e)
					{
						System.Diagnostics.Debug.WriteLine("error ::" + e);
					}

					// マトリックスを反映させたBitmapを生成する。 メモリオーバー対策でusing使用
					using (Bitmap bitmap = Bitmap.CreateBitmap(orgBitmap, 0, 0, width, height, matrix, true))
					{
						// Bitmap -> ImageSource
						var source = bitmap.ToImageSource();
						return source;
					}
				}
			});
			task.Start();
   			task.Wait();
			return task.Result;
		}

	}
}
