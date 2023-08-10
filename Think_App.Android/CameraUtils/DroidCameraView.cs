using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Android.Content;
using Android.Hardware;
using Android.Views;

namespace Think_App.Droid
{
    //! ***********************************************************************
    //! API Level21からAndroid.Hardware.Cameraは非推奨で、
    //! Android.Hardware.Camera2への移行が推奨されています。ここではCameraを使用します。
    //! ***********************************************************************

    public sealed class DroidCameraView : ViewGroup, ISurfaceHolderCallback
    {
        SurfaceView _surfaceView;
        ISurfaceHolder _holder;
        Camera.Size _previewSize;
        IList<Camera.Size> _supportedPreviewSizes;
        Camera _camera;
        Context _context;
        byte[] Buff;

        private PinchListener _pinchListener;
        private ScaleGestureDetector _scaleGetureDetector;
        private CustomFaceDetectionListener _faceDetectionListener;
        private bool _surfaceCreated;

        public CameraViewCallback PreviewCallback { get; set; }
        public CameraView FormsCameraView { get; set; }

        private bool _IsFlash;
        private bool _IsPreviewing;

        public bool IsFlash
        {
            get
            {
                return _IsFlash;
            }
            set
            {
                if (_IsFlash != value)
                {
                    _IsFlash = value;
                    if (Preview != null)
                    {
                        try
                        {
                            var p = Preview.GetParameters();
                            p.FlashMode = (_IsFlash) ? Camera.Parameters.FlashModeTorch : Camera.Parameters.FlashModeOff;
                            Preview.SetParameters(p);
                        }
                        catch
                        {
                        }
                    }
                }
            }
        }

        public bool IsPreviewing
        {
            get
            {
                return _IsPreviewing;
            }
            set
            {
                if (_IsPreviewing != value)
                {
                    _IsPreviewing = value;
                    if (_IsPreviewing)
                    {
                        StartPreview();
                    }
                    else
                    {
                        StopPreview();
                    }
                }
            }
        }

        public Camera Preview
        {
            get
            {
                return _camera;
            }
            set
            {
                if (_camera != value)
                {
                    _camera = value;
                    if (Preview != null)
                    {
                        _supportedPreviewSizes = Preview.GetParameters().SupportedPreviewSizes;
                    }
                    RequestLayout();
                }
            }
        }

        public DroidCameraView(Context context, CameraView formsCameraView) : base(context)
        {
            FormsCameraView = formsCameraView;
            _surfaceView = new SurfaceView(context);
            AddView(_surfaceView);

            _IsPreviewing = FormsCameraView.IsPreviewing;
            _IsFlash = FormsCameraView.IsFlash;
            _holder = _surfaceView.Holder;
            _holder.AddCallback(this);

            _context = context;

            MessagingCenter.Subscribe<LifeCyclePayload>(this, "", (p) =>
            {
                if (p.Status == LifeCycle.OnSleep)
                {
                    if (_surfaceCreated)
                    {
                        // スリープ時にSurfaceViewが生成されていればリソースを解放
                        Release();
                    }
                }
                else if (p.Status == LifeCycle.OnResume)
                {
                    if (_surfaceCreated)
                    {
                        // スリープ時にSurfaceViewが生成されていればリソースを初期化
                        Initialize();
                    }
                }
            });
        }

        void StopPreview()
        {
            if (Preview != null)
            {
                Preview.AddCallbackBuffer(null);
                Preview.SetPreviewCallbackWithBuffer(null);
                Preview.StopPreview();
            }
        }

        void StartPreview()
        {
            if (Preview != null)
            {
                Preview.StartPreview();
                Preview.SetPreviewCallbackWithBuffer(PreviewCallback);
                Preview.AddCallbackBuffer(Buff);
            }
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            //base.OnMeasure(widthMeasureSpec, heightMeasureSpec);

            int width = ResolveSize(SuggestedMinimumWidth, widthMeasureSpec);
            int height = ResolveSize(SuggestedMinimumHeight, heightMeasureSpec);
            SetMeasuredDimension(width, height);
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            var msw = MeasureSpec.MakeMeasureSpec(r - l, MeasureSpecMode.Exactly);
            var msh = MeasureSpec.MakeMeasureSpec(b - t, MeasureSpecMode.Exactly);

            _surfaceView.Measure(msw, msh);
            _surfaceView.Layout(0, 0, r - l, b - t);
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            // ピンチジェスチャー追加
            return _scaleGetureDetector.OnTouchEvent(e);
        }

        public void SurfaceChanged(ISurfaceHolder holder, Android.Graphics.Format format, int width, int height)
        {
            if (Preview == null)
            {
                Initialize();
            }
        }

        public void SurfaceCreated(ISurfaceHolder holder)
        {
            _surfaceCreated = true;
        }

        public void SurfaceDestroyed(ISurfaceHolder holder)
        {
            if (Preview != null)
            {
                Release();
            }
            _surfaceCreated = false;
        }

        public void Release()
        {
            Preview.StopPreview();
            PreviewCallback.Dispose();
            Preview.AddCallbackBuffer(null);
            Preview.SetPreviewCallbackWithBuffer(null);
            _pinchListener.Dispose();
            _scaleGetureDetector.Dispose();

            try
            {
                Preview.StopFaceDetection();
                _faceDetectionListener.Dispose();
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("顔認識に対応していません。");
            }

            Preview.Release();
            Preview = null;
        }

        public void Initialize()
        {
            int camId = 0;
            var camNumber = Camera.NumberOfCameras;
            for (int i = 0; i < camNumber; ++i)
            {
                Camera.CameraInfo info = new Camera.CameraInfo();
                Camera.GetCameraInfo(i, info);
                if (info.Facing == CameraFacing.Back && FormsCameraView.Camera == CameraOptions.Rear)
                {
                    camId = i;
                    break;
                }
                else if (info.Facing == CameraFacing.Front && FormsCameraView.Camera == CameraOptions.Front)
                {
                    camId = i;
                    break;
                }
            }


            Preview = Camera.Open(camId);

            if (Preview == null)
            {
                return;
            }

            // Portrait固定
            Preview.SetDisplayOrientation(90);

            // 顔認識リスナーをセットする。
            _faceDetectionListener = new CustomFaceDetectionListener()
            {
                CameraView = FormsCameraView,
                SurfaceView = _surfaceView
            };
            Preview.SetFaceDetectionListener(_faceDetectionListener);

            // 顔認識スタート
            try
            {
                Preview.StartFaceDetection();
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("すでに顔認識がスタートしているか、APIが対応していません。");
            }

            var parameters0 = Preview.GetParameters();

            // プレビューサイズ設定
            if (_supportedPreviewSizes != null)
            {
                _previewSize = GetOptimalPreviewSize(_supportedPreviewSizes, _surfaceView.Width, _surfaceView.Height);
            }
            parameters0.SetPreviewSize(_previewSize.Width, _previewSize.Height);

            try
            {
                Preview.SetParameters(parameters0);
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("Can't Set Parameter Preview Size W:{0} H:{1}", _previewSize.Width, _previewSize.Height);
            }

            var parameters1 = Preview.GetParameters();

            // フレームレート設定
            int minFrame = 10000;
            int maxFrame = 24000;
            parameters1.SetPreviewFpsRange(minFrame, maxFrame);

            try
            {
                Preview.SetParameters(parameters1);
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("Can't Set Parameter Preview Fps Range min:{0} max:{1}", minFrame, maxFrame);
            }

            var parameters2 = Preview.GetParameters();

            // フラッシュ設定
            parameters2.FlashMode = (_IsFlash) ? Camera.Parameters.FlashModeTorch : Camera.Parameters.FlashModeOff;

            try
            {
                Preview.SetParameters(parameters2);
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("Can't Set Parameter Preview Flash Mode:{0}", _IsFlash);
            }

            RequestLayout();

            // フレーム処理用バッファの作成
            int size = _previewSize.Width * _previewSize.Height *
                                   Android.Graphics.ImageFormat.GetBitsPerPixel(Android.Graphics.ImageFormatType.Nv21) / 8;
            Buff = new byte[size];

            // フレーム処理用コールバック生成
            PreviewCallback = new CameraViewCallback { CameraView = FormsCameraView, Buff = Buff };

            Preview.SetPreviewCallbackWithBuffer(PreviewCallback);
            Preview.AddCallbackBuffer(Buff);

            // ピンチジェスチャー登録処理
            _pinchListener = new PinchListener { Camera = Preview, PreviewCallback = PreviewCallback, Buff = Buff };
            _scaleGetureDetector = new ScaleGestureDetector(_context, _pinchListener);

            Preview.SetPreviewDisplay(_holder);

            if (IsPreviewing)
            {
                StartPreview();
            }
        }

        private Camera.Size GetOptimalPreviewSize(IList<Camera.Size> sizes, int w, int h)
        {
            double AspectTolerance = 0.1;
            double targetRatio = (double)w / h;

            if (sizes == null)
            {
                return null;
            }

            Camera.Size optimalSize = null;
            double minDiff = double.MaxValue;

            int targetHeight = h;

            foreach (Camera.Size size in sizes)
            {
                double ratio = (double)size.Height / size.Width;    //Portraitは縦横逆


                if (Math.Abs(ratio - targetRatio) > AspectTolerance)
                    continue;
                if (Math.Abs(size.Width - targetHeight) < minDiff)
                {
                    optimalSize = size;
                    minDiff = Math.Abs(size.Width - targetHeight);
                }

            }

            if (optimalSize == null)
            {
                minDiff = double.MaxValue;
                foreach (Camera.Size size in sizes)
                {
                    if (Math.Abs(size.Width - targetHeight) < minDiff)
                    {
                        optimalSize = size;
                        minDiff = Math.Abs(size.Width - targetHeight);
                    }
                }
            }

            return optimalSize;
        }

        class PinchListener : ScaleGestureDetector.SimpleOnScaleGestureListener
        {
            public Camera Camera { get; set; }
            public byte[] Buff { get; set; }
            public CameraViewCallback PreviewCallback { get; set; }

            public override bool OnScale(ScaleGestureDetector detector)
            {

                var param = Camera.GetParameters();

                if (Math.Abs(detector.ScaleFactor - 1.0f) < 0.01f)
                {
                    return base.OnScale(detector);
                }
                if (detector.ScaleFactor > 1.0)
                {
                    param.Zoom += (int)Math.Round(2.0 * detector.ScaleFactor, 0);

                    if (param.Zoom == 0)
                    {
                        param.Zoom = 2;
                    }
                    if (param.Zoom > param.MaxZoom)
                    {
                        param.Zoom = param.MaxZoom;
                    }
                }
                else
                {
                    //param.Zoom -= 3;
                    param.Zoom -= (int)Math.Round(4.0 * detector.ScaleFactor, 0);
                    if (param.Zoom < 0)
                    {
                        param.Zoom = 0;
                    }
                }

                Camera.SetParameters(param);

                return base.OnScale(detector);
            }
            public override bool OnScaleBegin(ScaleGestureDetector detector)
            {
                Camera.AddCallbackBuffer(null);
                Camera.SetPreviewCallbackWithBuffer(null);
                return base.OnScaleBegin(detector);
            }
            public override void OnScaleEnd(ScaleGestureDetector detector)
            {
                Camera.SetPreviewCallbackWithBuffer(PreviewCallback);
                Camera.AddCallbackBuffer(Buff);
                base.OnScaleEnd(detector);
            }
        }
    }
}
