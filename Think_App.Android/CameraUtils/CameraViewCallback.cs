using System;
using System.IO;
using Android.Graphics;
using Xamarin.Forms;
using static Android.Hardware.Camera;

namespace Think_App.Droid
{
    public class CameraViewCallback : Java.Lang.Object, IPreviewCallback
    {
        //private long FrameCount = 0;
        public CameraView CameraView { get; set; }
        public byte[] Buff { get; set; }

        public void OnPreviewFrame(byte[] data, Android.Hardware.Camera camera)
        {
            if (CameraView.IsTakenPhoto)
            {
                // 処理に時間がかかるので、プレビューを止める。
                CameraView.IsPreviewing = false;
                // いったんjpegDataに変換する。
                byte[] jpegData = ConvertYuvToJpeg(data, camera);
                // Bitmap作成
                var bitmap = BitmapFactory.DecodeByteArray(jpegData, 0, jpegData.Length);
                // Bitmapを90度回転
                var rotatedBitmap = RotateBitmap90(bitmap);
                if (CameraView.Camera == CameraOptions.Front)
                {
                    // 自撮りの場合は反転
                    rotatedBitmap = FlipBitmap(rotatedBitmap);
                }
                // Bitmapをbyte列に変換
                byte[] bitmapData;
                using (var stream = new MemoryStream())
                {
                    rotatedBitmap.Compress(Bitmap.CompressFormat.Png, 0, stream);
                    bitmapData = stream.ToArray();
                }
                // byte列からImageSourceを得る
                var imageSource = ImageSource.FromStream(() => new MemoryStream(bitmapData));
                CameraView.CompleteTakenPhotoAndSendImageSource(imageSource);
            }

            // 次のバッファをセット
            camera.AddCallbackBuffer(Buff);
        }

        private byte[] ConvertYuvToJpeg(byte[] yuvData, Android.Hardware.Camera camera)
        {
            var cameraParameters = camera.GetParameters();
            var width = cameraParameters.PreviewSize.Width;
            var height = cameraParameters.PreviewSize.Height;
            var yuv = new YuvImage(yuvData, cameraParameters.PreviewFormat, width, height, null);
            var ms = new MemoryStream();
            var quality = 100;   // adjust this as needed
            yuv.CompressToJpeg(new Android.Graphics.Rect(0, 0, width, height), quality, ms);
            var jpegData = ms.ToArray();

            return jpegData;
        }

        private Bitmap RotateBitmap90(Bitmap srcImage)
        {
            // 回転マトリックス作成
            var mt = new Matrix();
            mt.PostRotate(90.0f);
            // 回転したBitmap作成
            var bm = Bitmap.CreateBitmap(srcImage, 0, 0, srcImage.Width, srcImage.Height, mt, true);
            return bm;
        }

        private Bitmap FlipBitmap(Bitmap srcImage)
        {
            // 反転マトリックス作成
            var mt = new Matrix();
            mt.PostScale(1.0f, -1.0f);
            // 反転したBitmap作成
            var bm = Bitmap.CreateBitmap(srcImage, 0, 0, srcImage.Width, srcImage.Height, mt, true);
            return bm;
        }
    }
}
