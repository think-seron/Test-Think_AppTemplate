using System;
using System.Diagnostics;
using Android.Hardware;
using Android.Views;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App.Droid
{
    public class CustomFaceDetectionListener : Java.Lang.Object, Camera.IFaceDetectionListener
    {
        public CameraView CameraView { get; set; }
        public SurfaceView SurfaceView { get; set; }

        public CustomFaceDetectionListener(IntPtr ptr, Android.Runtime.JniHandleOwnership ship) : base(ptr, ship)
        {

        }

        public CustomFaceDetectionListener() : base()
        {
        }

        void Camera.IFaceDetectionListener.OnFaceDetection(Camera.Face[] faces, Camera camera)
        {
            if (faces == null || faces.Length == 0)
            {
                CameraView?.SendEventFaceDetectionFailed();
                return;
            }

            foreach (var face in faces)
            {
                if (face.Id == -1)
                {
                    Debug.WriteLine("サポート外です！");
                }

                int faceScore;
                Rect faceRange;
                Point? mouthPosition = null, leftEyePostion = null, rightEyePosition = null;

                Debug.WriteLine("Face Id:{0}", face.Id);

                // 顔認識の信頼度(1-100、数値が高いほど信頼できる。)
                faceScore = face.Score;
                Debug.WriteLine("Face Score:{0}", faceScore);

                // 顔範囲
                var faceRect = GetPixelRectFromFace(face);
                faceRange = new Rect(faceRect.Left.ToDpFromPixel(), faceRect.Top.ToDpFromPixel(), faceRect.Width().ToDpFromPixel(), faceRect.Height().ToDpFromPixel());
                Debug.WriteLine("Face Range L:{0} T:{1} R:{2} B{3}", faceRange.Left, faceRange.Top, faceRange.Right, faceRange.Bottom);

                // 顔パーツ認識
                if (face.Mouth != null)
                {
                    mouthPosition = new Point(face.Mouth.X.ToDpFromPixel(), face.Mouth.Y.ToDpFromPixel());
                    leftEyePostion = new Point(face.LeftEye.X.ToDpFromPixel(), face.LeftEye.Y.ToDpFromPixel());
                    rightEyePosition = new Point(face.RightEye.X.ToDpFromPixel(), face.RightEye.Y.ToDpFromPixel());

                    Debug.WriteLine("Face Mouth X:{0} Y:{1}", mouthPosition.Value.X, mouthPosition.Value.Y);
                    Debug.WriteLine("Face LeftEye X:{0} Y:{1}", leftEyePostion.Value.X, leftEyePostion.Value.Y);
                    Debug.WriteLine("Face RightEye X:{0} Y:{1}", rightEyePosition.Value.X, rightEyePosition.Value.Y);
                }

                // イベントをPCLに通知
                if (CameraView != null)
                {
                    CameraView.SendEventFaceDetection(faceScore,
                                                      faceRange,
                                                      mouthPosition,
                                                      leftEyePostion,
                                                      rightEyePosition);
                }
            }
        }

        private Android.Graphics.RectF GetPixelRectFromFace(Camera.Face face)
        {
            int w = SurfaceView.Width;
            int h = SurfaceView.Height;
            var rect = new Android.Graphics.RectF();
            rect.Left = (float)face.Rect.Left;
            rect.Top = (float)face.Rect.Top;
            rect.Right = (float)face.Rect.Right;
            rect.Bottom = (float)face.Rect.Bottom;

            // (-1000,-1000)が左上、(0,0)が中央の座標なので、座標軸変換を行う。
            // ただし、Portraitで90度回転しているのに留意する。
            var mt = new Android.Graphics.Matrix();
            bool mirror = (CameraView.Camera == CameraOptions.Front);
            mt.SetScale(mirror ? -1 : 1, 1);
            mt.PostRotate(90);
            mt.PostScale((float)w / 2000f, (float)h / 2000f);
            mt.PostTranslate((float)w / 2f, (float)h / 2f);

            mt.MapRect(rect);

            return rect;
        }
    }
}
