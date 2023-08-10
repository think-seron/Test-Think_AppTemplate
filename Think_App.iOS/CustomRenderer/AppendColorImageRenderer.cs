using System;
using System.ComponentModel;
using Xamarin.Forms;
using Think_App;
using Think_App.iOS;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using Foundation;
using CoreGraphics;

[assembly: ExportRenderer(typeof(AppendColorImage), typeof(AppendColorImageRenderer))]
namespace Think_App.iOS
{
	public class AppendColorImageRenderer : ImageRenderer
	{
		private AppendColorImage _AppendColorImage;
		private UITouch _Touch;
		private UIImage _baseImage;

		protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
		{
			base.OnElementChanged(e);

			_AppendColorImage = Element as AppendColorImage;

			if (Control != null && _AppendColorImage != null && e.OldElement == null)
			{
				_AppendColorImage.CurrentImageSourceRequested += _AppendColorImage_CurrentImageSourceRequested;
				_AppendColorImage.ForceUpdateRequested += (__, _) =>
				{
					// 強制アップデート
					UpdateImage();
				};

				// 初回更新
				UpdateImage();
			}
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == "Clear")
			{
				// Clearされたら再描画する。
				UpdateImage();
			}
			else if (e.PropertyName == AppendColorImage.BlendModeProperty.PropertyName)
			{
				// ブレンド更新
				UpdateImage();
			}
		}

		public override void TouchesBegan(NSSet touches, UIEvent evt)
		{
			base.TouchesBegan(touches, evt);

			_Touch = touches.AnyObject as UITouch;

			if (_Touch != null)
			{
				var p = _Touch.LocationInView(this);
				_AppendColorImage.OnBegin((double)p.X, (double)p.Y);
			}
		}

		public override void TouchesMoved(NSSet touches, UIEvent evt)
		{
			base.TouchesMoved(touches, evt);

			if (_Touch != null)
			{
				var p = _Touch.LocationInView(this);
				if (_AppendColorImage.OnMove((double)p.X, (double)p.Y))
				{
					// 追加が成功すれば再描画。
					UpdateImage();
				}
			}
		}

		public override void TouchesEnded(NSSet touches, UIEvent evt)
		{
			base.TouchesEnded(touches, evt);

			if (_Touch != null)
			{
				_AppendColorImage.OnEnd();
			}
		}

		public override void TouchesCancelled(NSSet touches, UIEvent evt)
		{
			base.TouchesCancelled(touches, evt);

			if (_Touch != null)
			{
				_AppendColorImage.OnEnd();
			}
		}

		private void UpdateImage()
		{
			if (_baseImage == null)
			{
				_baseImage = Control.Image;
			}
			if (_baseImage == null)
			{
				return;
			}
			var appendImage = GetAppendImage(_baseImage.Size, Control.Bounds.Size);

			// UIImageの合成。
			Control.Image = GetBlendImage(_baseImage, appendImage, ConvertBlendMode(_AppendColorImage.BlendMode));

			// これやらないとメモリ関連で落ちるっぽい
			appendImage.Dispose();
			appendImage = null;
			GC.Collect();

			// 再描画
			SetNeedsDisplay();
		}

		private CGBlendMode ConvertBlendMode(AppendColorImage.BlendingMode mode)
		{
			CGBlendMode cgMode = CGBlendMode.Normal;
			if (mode == AppendColorImage.BlendingMode.Normal)
			{
				cgMode = CGBlendMode.Normal;
			}
			else if (mode == AppendColorImage.BlendingMode.Screen)
			{
				cgMode = CGBlendMode.Screen;
			}
			else if (mode == AppendColorImage.BlendingMode.Multiply)
			{
				cgMode = CGBlendMode.Multiply;
			}
			return cgMode;
		}

		private UIImage GetAppendImage(CGSize size, CGSize canvasSize)
		{
			UIImage image = null;

			// ビットマップ形式のグラフィックコンテキストの生成。
			UIGraphics.BeginImageContext(size);

			var context = UIGraphics.GetCurrentContext();

			// 描画形状、接続部の形状を丸に変更
			context.SetLineCap(CGLineCap.Round);
			context.SetLineJoin(CGLineJoin.Round);

			foreach (var d in _AppendColorImage.Strokes.Data)
			{
				try
				{
					context.SetLineWidth((nfloat)d.Width);
					context.SetStrokeColor(d.Color.ToCGColor());
					for (var i = 0; i < d.Points.Count; i++)
					{
						// 座標系の変換
						var point = ConvertCanvasPoint(d.Points[i], canvasSize, size);

						var x = (nfloat)point.X;
						var y = (nfloat)point.Y;

						if (i == 0)
						{
							// 始点
							context.MoveTo(x, y);
						}
						else
						{
							// 追加点
							context.AddLineToPoint(x, y);
						}
					}
					// 描画
					context.SetBlendMode((d.IsErase) ? CGBlendMode.Clear : CGBlendMode.Normal);
					context.StrokePath();
				}
				catch
				{
					System.Diagnostics.Debug.WriteLine("このストロークは描画できませんでした");
				}
			}

			// 現在のグラフィックコンテキストの画像を取得する。
			image = UIGraphics.GetImageFromCurrentImageContext();

			// 現在のグラフィックコンテキストへの編集を終了。
			UIGraphics.EndImageContext();

			return image;
		}

		private UIImage GetBlendImage(UIImage baseImage, UIImage blendImage, CGBlendMode mode)
		{
			UIImage image = null;

			// 大きい方をとって、ImageContextを作成する。
			var width = (nfloat)Math.Max(baseImage.Size.Width, blendImage.Size.Width);
			var height = (nfloat)Math.Max(baseImage.Size.Height, blendImage.Size.Height);
			var size = new CGSize(width, height);
			UIGraphics.BeginImageContext(size);

			var context = UIGraphics.GetCurrentContext();

			var rect = new CGRect(0, 0, size.Width, size.Height);

			if (mode == CGBlendMode.Screen)
			{
				// 下地を白で塗る。
				context.SetFillColor(Color.FromHex("#FFFFFF").ToCGColor());
				context.FillRect(rect);
			}

			baseImage.Draw(rect);
			blendImage.Draw(rect, mode, 1);

			var tempImage = UIGraphics.GetImageFromCurrentImageContext();

			// いったんコンテキストをクリア
			context.ClearRect(rect);

			// このままでは塗りつぶしが残るので、元画像でクリップした画像を取得する
			// Translate,Scaleをかけないと画像が反転してしまう！
			context.TranslateCTM(0.0f, size.Height);
			context.ScaleCTM(1.0f, -1.0f);

			context.ClipToMask(rect, baseImage.CGImage);
			context.DrawImage(rect, tempImage.CGImage);

			image = UIGraphics.GetImageFromCurrentImageContext();

			UIGraphics.EndImageContext();

			tempImage.Dispose();

			return image;
		}

		private Point ConvertCanvasPoint(Point originalPoint, CGSize srcSize, CGSize dstSize)
		{
			// srcSize ... Control Size
			// dstSize ... Image Size

			var point = new Point();

			if (Control.ContentMode == UIViewContentMode.ScaleAspectFit)
			{
				var scaleW = srcSize.Width / dstSize.Width;
				var scaleH = srcSize.Height / dstSize.Height;

				if (scaleW > scaleH)
				{
					var dx = (srcSize.Width - dstSize.Width * scaleH) / 2;
					point.X = (originalPoint.X - dx) / scaleH;
					point.Y = originalPoint.Y / scaleH;
				}
				else
				{
					var dy = (srcSize.Height - dstSize.Height * scaleW) / 2;
					point.X = originalPoint.X / scaleW;
					point.Y = (originalPoint.Y - dy) / scaleW;
				}
			}
			else if (Control.ContentMode == UIViewContentMode.ScaleAspectFill)
			{
				var scaleW = srcSize.Width / dstSize.Width;
				var scaleH = srcSize.Height / dstSize.Height;

				if (scaleW < scaleH)
				{
					var dx = (srcSize.Width - dstSize.Width * scaleH) / 2;
					point.X = (originalPoint.X - dx) / scaleH;
					point.Y = originalPoint.Y / scaleH;
				}
				else
				{
					var dy = (srcSize.Height - dstSize.Height * scaleW) / 2;
					point.X = originalPoint.X / scaleW;
					point.Y = (originalPoint.Y - dy) / scaleW;
				}
			}
			else
			{
				point.X = originalPoint.X * dstSize.Width / srcSize.Width;
				point.Y = originalPoint.Y * dstSize.Height / srcSize.Height;
			}

			return point;
		}

		void _AppendColorImage_CurrentImageSourceRequested(object sender, EventArgs e)
		{
			if (Control.Image != null)
			{
				var source = Control.Image.ToImageSource();
				_AppendColorImage.SendCurrentImageSource(source);
			}
			else
			{
				_AppendColorImage.SendCurrentImageSource(null);
			}
		}
	}
}
