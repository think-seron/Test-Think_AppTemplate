using System;
using System.Linq;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Views;
using Think_App;
using Think_App.Droid;
using Android.Content;
//using DroidColor = Android.Graphics.Color;
//using FormColor = Xamarin.Forms.Color;

[assembly: ExportRenderer(typeof(CustomButton), typeof(CustomButtonRenderer))]
namespace Think_App.Droid
{
    public class CustomButtonRenderer : ButtonRenderer
    {
        CustomButton _CustomButton;
        Bitmap _Image;

        public CustomButtonRenderer(Context context) : base(context)
        {
        }

        protected override async void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);

            if (Control != null && e.NewElement != null)
            {
                _CustomButton = e.NewElement as CustomButton;

                Control.SetBackgroundColor(Android.Graphics.Color.Transparent);

                if (_CustomButton.Source != null)
                {
                    _Image = await _CustomButton.Source.ToBitmapAsync();
                }
                UpdateDrawable();
            }
        }

        protected override async void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (Control == null)
            {
                return;
            }

            if (e.PropertyName == CustomButton.BackgroundColorProperty.PropertyName)
            {
                UpdateDrawable();
            }
            else if (e.PropertyName == CustomButton.UseCustomColorProperty.PropertyName)
            {
                UpdateDrawable();
            }
            else if (e.PropertyName == CustomButton.HighlightColorProperty.PropertyName)
            {
                UpdateDrawable();
            }
            else if (e.PropertyName == CustomButton.DisableColorProperty.PropertyName)
            {
                UpdateDrawable();
                UpdateTextColor();
            }
            else if (e.PropertyName == CustomButton.TextColorProperty.PropertyName)
            {
                UpdateTextColor();
            }
            else if (e.PropertyName == CustomButton.SourceProperty.PropertyName)
            {
                if (_CustomButton.Source != null)
                {
                    _Image = await _CustomButton.Source.ToBitmapAsync();
                }
                else
                {
                    _Image = null;
                }
                UpdateDrawable();
            }
            else if (e.PropertyName == CustomButton.ImageWidthProperty.PropertyName ||
                     e.PropertyName == CustomButton.ImageHeightProperty.PropertyName ||
                     e.PropertyName == CustomButton.ImageOffsetProperty.PropertyName ||
                     e.PropertyName == CustomButton.ImagePaddingProperty.PropertyName ||
                     e.PropertyName == CustomButton.ImageLayoutPositionProperty.PropertyName ||
                     e.PropertyName == CustomButton.TextProperty.PropertyName ||
                     e.PropertyName == CustomButton.FontProperty.PropertyName ||
                     e.PropertyName == CustomButton.FontSizeProperty.PropertyName ||
                     e.PropertyName == CustomButton.FontFamilyProperty.PropertyName ||
                     e.PropertyName == CustomButton.FontAttributesProperty.PropertyName ||
                     e.PropertyName == CustomButton.ContentLayoutProperty.PropertyName)
            {
                //UpdatePadding();
                UpdateDrawable();
            }
            else if (e.PropertyName == CustomButton.IsEnabledProperty.PropertyName)
            {
                UpdateDrawable();
            }
        }

        protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
        {
            base.OnSizeChanged(w, h, oldw, oldh);

            if (oldw != w && oldh != h && w > 0 && h > 0)
            {
                Update();
            }
        }

        void Update()
        {
            //UpdatePadding();
            UpdateTextColor();
        }

        //void UpdatePadding()
        //{
        //	int left = 0;
        //	int top = 0;
        //	int right = 0;
        //	int bottom = 0;

        //	var textPaint = Control.Paint;
        //	if (textPaint != null && !string.IsNullOrEmpty(_CustomButton.Text))
        //	{
        //		// テキストサイズを取得する。
        //		var textWidth = textPaint.MeasureText(_CustomButton.Text);
        //		var m = textPaint.GetFontMetrics();
        //		var textHeight = m.Descent - m.Ascent;
        //		// イメージサイズを取得する。
        //		var imageWidth = (float)(_CustomButton.ImageWidth.ToPixelFromDp());
        //		var imageHeight = (float)(_CustomButton.ImageHeight.ToPixelFromDp());
        //		// パディングを取得する。
        //		var padding = (float)(_CustomButton.ImagePadding.ToPixelFromDp());

        //		if (_CustomButton.ImageLayoutPosition == CustomButton.LayoutPosition.Left)
        //		{
        //			Control.Gravity = GravityFlags.CenterVertical | GravityFlags.Left;
        //			left = (int)((Width + imageWidth + padding - textWidth) / 2.0f);
        //		}
        //		else if (_CustomButton.ImageLayoutPosition == CustomButton.LayoutPosition.Right)
        //		{
        //			Control.Gravity = GravityFlags.CenterVertical | GravityFlags.Right;
        //			right = (int)((Width - textWidth + padding + imageWidth) / 2.0f);
        //		}
        //		else if (_CustomButton.ImageLayoutPosition == CustomButton.LayoutPosition.Top)
        //		{
        //			Control.Gravity = GravityFlags.CenterHorizontal | GravityFlags.Top;
        //			top = (int)((Height + imageHeight + padding - textHeight) / 2.0f);
        //		}
        //		else if (_CustomButton.ImageLayoutPosition == CustomButton.LayoutPosition.Bottom)
        //		{
        //			Control.Gravity = GravityFlags.CenterHorizontal | GravityFlags.Bottom;
        //			bottom = (int)((Height - textHeight + padding + imageHeight) / 2.0f);
        //		}
        //	}

        //	Control.SetPadding(left, top, right, bottom);
        //}

        void UpdateDrawable()
        {
            var backgroundDrawable = new CustomButtonDrawable();
            backgroundDrawable.Button = _CustomButton;
            backgroundDrawable.Image = _Image;
            backgroundDrawable.NativeButton = Control;
            Control.SetBackground(backgroundDrawable);
            Control.Invalidate();
        }

        void UpdateTextColor()
        {
            if (_CustomButton.UseCustomColor)
            {
                // ここで設定し直すことで、Disable時にも文字色が変わらない。
                Control.SetTextColor(_CustomButton.TextColor.ToAndroid());
            }
        }
    }

    public class CustomButtonDrawable : Drawable
    {
        bool _isDisposed;
        Bitmap _normalBitmap;
        bool _pressed;
        Bitmap _pressedBitmap;

        public CustomButtonDrawable()
        {
            _pressed = false;
        }

        public CustomButton Button { get; set; }
        public Android.Widget.Button NativeButton { get; set; }
        public Bitmap Image { get; set; }

        public override bool IsStateful
        {
            get { return true; }
        }

        public override int Opacity
        {
            get { return 0; }
        }

        public override void Draw(Canvas canvas)
        {
            int width = Bounds.Width();
            int height = Bounds.Height();

            if (width <= 0 || height <= 0)
                return;

            if (_normalBitmap == null)
            {
                Reset();

                _normalBitmap = CreateBitmap(false, width, height);
                _pressedBitmap = CreateBitmap(true, width, height);
            }

            Bitmap bitmap = GetState().Contains(global::Android.Resource.Attribute.StatePressed) ? _pressedBitmap : _normalBitmap;
            canvas.DrawBitmap(bitmap, 0, 0, new Paint());
        }

        public void Reset()
        {
            if (_normalBitmap != null)
            {
                _normalBitmap.Recycle();
                _normalBitmap.Dispose();
                _normalBitmap = null;
            }

            if (_pressedBitmap != null)
            {
                _pressedBitmap.Recycle();
                _pressedBitmap.Dispose();
                _pressedBitmap = null;
            }
        }

        public override void SetAlpha(int alpha)
        {
        }

        public override void SetColorFilter(ColorFilter cf)
        {
        }

        protected override void Dispose(bool disposing)
        {
            if (_isDisposed)
                return;

            _isDisposed = true;

            if (disposing)
                Reset();

            base.Dispose(disposing);
        }

        protected override bool OnStateChange(int[] state)
        {
            bool old = _pressed;
            _pressed = state.Contains(global::Android.Resource.Attribute.StatePressed);
            if (_pressed != old)
            {
                InvalidateSelf();
                return true;
            }
            return false;
        }

        Bitmap CreateBitmap(bool pressed, int width, int height)
        {
            Bitmap bitmap = Bitmap.CreateBitmap(width, height, Bitmap.Config.Argb8888);
            using (var canvas = new Canvas(bitmap))
            {
                DrawBackground(canvas, width, height, pressed);
                DrawImage(canvas, width, height);
                DrawOutline(canvas, width, height);
            }

            return bitmap;
        }

        void DrawBackground(Canvas canvas, int width, int height, bool pressed)
        {
            var paint = new Paint { AntiAlias = true };
            var path = new Path();

            float borderRadius = Forms.Context.ToPixels(Button.BorderRadius);

            path.AddRoundRect(new RectF(0, 0, width, height), borderRadius, borderRadius, Path.Direction.Cw);

            if (Button.IsEnabled)
            {
                Android.Graphics.Color highlightColor;
                try
                {
                    highlightColor = (Button.UseCustomColor) ? Button.HighlightColor.ToAndroid() : Button.BackgroundColor.AddLuminosity(-0.1).ToAndroid();
                }
                catch
                {
                    highlightColor = Button.BackgroundColor.ToAndroid();
                }
                paint.Color = pressed ? highlightColor : Button.BackgroundColor.ToAndroid();
            }
            else
            {
                paint.Color = (Button.UseCustomColor) ? Button.DisableColor.ToAndroid() : Button.BackgroundColor.ToAndroid();
            }
            paint.SetStyle(Paint.Style.Fill);
            canvas.DrawPath(path, paint);
        }

        void DrawImage(Canvas canvas, int width, int height)
        {
            if (Image == null)
            {
                return;
            }
            using (var paint = new Paint())
            {
                paint.AntiAlias = true;
                // Bitmapを滑らかに描画するために必要。
                paint.FilterBitmap = true;

                // 描画範囲を決定する。
                // テキストサイズを取得する。
                float textWidth = 0.0f;
                float textHeight = 0.0f;
                var textPaint = NativeButton.Paint;
                if (textPaint != null && !string.IsNullOrEmpty(Button.Text))
                {
                    textWidth = textPaint.MeasureText(Button.Text);
                    var m = textPaint.GetFontMetrics();
                    textHeight = m.Descent - m.Ascent;
                }
                // 指定の描画範囲
                var imageW = (float)Button.ImageWidth.ToPixelFromDp();
                var imageH = (float)Button.ImageHeight.ToPixelFromDp();
                var padding = string.IsNullOrEmpty(Button.Text) ? 0.0f : (float)Button.ImagePadding.ToPixelFromDp();
                var offsetX = (float)Button.ImageOffset.X.ToPixelFromDp();
                var offsetY = (float)Button.ImageOffset.Y.ToPixelFromDp();

                float left = 0f;
                float top = 0f;
                float right = 0f;
                float bottom = 0f;
                if (Button.ImageLayoutPosition == CustomButton.LayoutPosition.Left)
                {
                    left = (width - imageW - padding - textWidth) / 2.0f + offsetX;
                    top = (height - imageH) / 2.0f + offsetY;
                    right = left + imageW;
                    bottom = top + imageH;
                }
                else if (Button.ImageLayoutPosition == CustomButton.LayoutPosition.Right)
                {
                    left = (width + textWidth + padding - imageW) / 2.0f + offsetX;
                    top = (height - imageH) / 2.0f + offsetY;
                    right = left + imageW;
                    bottom = top + imageH;
                }
                else if (Button.ImageLayoutPosition == CustomButton.LayoutPosition.Top)
                {
                    left = (width - imageW) / 2.0f + offsetX;
                    top = (height - imageH - padding - textHeight) / 2.0f + offsetY;
                    right = left + imageW;
                    bottom = top + imageH;
                }
                else if (Button.ImageLayoutPosition == CustomButton.LayoutPosition.Bottom)
                {
                    left = (width - imageW) / 2.0f + offsetX;
                    top = (height + textHeight + padding - imageH) / 2.0f + offsetY;
                    right = left + imageW;
                    bottom = top + imageH;
                }

                var rect = new RectF(left, top, right, bottom);

                try
                {
                    DrawBitmapImage(canvas, paint, Image, rect, Aspect.Fill);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("描画できませんでした:{0}", ex);
                }
            }
        }

        void DrawBitmapImage(Canvas canvas, Paint paint, Bitmap bitmap, RectF dstRect, Aspect aspect)
        {
            // 描画元の矩形
            Android.Graphics.Rect srcRect = null;
            // 描画先の矩形
            RectF scaledDstRect = dstRect;
            if (aspect == Aspect.AspectFit)
            {
                // 元画像を描画範囲のスケールに変換。
                var scaleW = dstRect.Width() / (float)bitmap.Width;
                var scaleH = dstRect.Height() / (float)bitmap.Height;
                var scale = Math.Min(scaleW, scaleH);

                var imageW = bitmap.Width * scale;
                var imageH = bitmap.Height * scale;
                scaledDstRect.Left = (dstRect.Left + dstRect.Right - imageW) / 2.0f;
                scaledDstRect.Top = (dstRect.Top + dstRect.Bottom - imageH) / 2.0f;
                scaledDstRect.Right = scaledDstRect.Left + imageW;
                scaledDstRect.Bottom = scaledDstRect.Top + imageH;
            }
            else if (aspect == Aspect.AspectFill)
            {
                // 描画範囲を元画像のスケールに変換。
                var scaleW = bitmap.Width / dstRect.Width();
                var scaleH = bitmap.Height / dstRect.Height();

                int left, top, right, bottom;
                if (scaleW > scaleH)
                {
                    // bitmapの横方向の両端を切る。
                    var imageW = (int)(Math.Round(dstRect.Width() * scaleH));
                    imageW = imageW.Clamp(0, bitmap.Width);
                    left = (bitmap.Width - imageW) / 2;
                    top = 0;
                    right = left + imageW;
                    bottom = bitmap.Height;

                    srcRect = new Android.Graphics.Rect(left, top, right, bottom);
                }
                else if (scaleW < scaleH)
                {
                    // bitmapの縦方向の両端を切る。
                    var imageH = (int)(Math.Round(dstRect.Height() * scaleW));
                    imageH = imageH.Clamp(0, bitmap.Height);
                    left = 0;
                    top = (bitmap.Height - imageH) / 2;
                    right = bitmap.Width;
                    bottom = top + imageH;

                    srcRect = new Android.Graphics.Rect(left, top, right, bottom);
                }
            }

            // 描画。
            canvas.DrawBitmap(bitmap, srcRect, scaledDstRect, paint);
        }

        RectF ResizeDstRect(RectF rect, int imageWidth, int imageHeight, Aspect aspect)
        {
            // 描画先のRectを決定します。
            if (aspect == Aspect.Fill || aspect == Aspect.AspectFill)
            {
                // Fill, AspectFillはそのまま返す。
                return rect;
            }

            var scaleW = imageWidth / rect.Width();
            var scaleH = imageHeight / rect.Height();

            // AspectFitの場合は小さい倍率を返す。
            var scale = Math.Min(scaleW, scaleH);

            var imageW = rect.Width() * scale;
            var imageH = rect.Height() * scale;
            var left = (rect.Width() - imageW) / 2.0f;
            var top = (rect.Height() - imageH) / 2.0f;
            var right = left + imageW;
            var bottom = top + imageH;

            return new RectF(left, top, right, bottom);
        }

        void DrawOutline(Canvas canvas, int width, int height)
        {
            if (Button.BorderWidth <= 0)
                return;

            using (var paint = new Paint { AntiAlias = true })
            using (var path = new Path())
            {
                float borderWidth = Forms.Context.ToPixels(Button.BorderWidth);
                float inset = borderWidth / 2;

                // adjust border radius so outer edge of stroke is same radius as border radius of background
                float borderRadius = Forms.Context.ToPixels(Button.BorderRadius) - inset;

                path.AddRoundRect(new RectF(inset, inset, width - inset, height - inset), borderRadius, borderRadius, Path.Direction.Cw);
                paint.StrokeWidth = borderWidth;
                paint.SetStyle(Paint.Style.Stroke);
                paint.Color = Button.BorderColor.ToAndroid();

                canvas.DrawPath(path, paint);
            }
        }
    }
}
