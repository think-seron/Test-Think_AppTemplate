using System;
using AVFoundation;
using CoreFoundation;
using CoreGraphics;
using Foundation;
using CoreMedia;
using UIKit;
using Xamarin.Forms;

namespace Think_App.iOS
{
	public class UICameraView : UIView
	{
		AVCaptureVideoPreviewLayer _previewLayer;
		CameraOptions _cameraOptions;

		public event EventHandler<EventArgs> Tapped;

		public AVCaptureSession CaptureSession { get; private set; }
		public AVCaptureDeviceInput Input { get; set; }
		public AVCaptureVideoDataOutput Output { get; private set; }
		public AVCaptureMetadataOutput MetadataOutput { get; private set; }
		public OutputRecorder Recorder { get; set; }
		public OutputMetadata Metadata { get; set; }
		public DispatchQueue Queue { get; set; }
		public AVCaptureDevice MainDevice { get; private set; }

		private UIPinchGestureRecognizer Pinch;

		private CameraView Camera;
		private float MaxZoom;
		private float MinZoom = 1.0f;

		private bool _IsPreviewing;
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
						CaptureSession.StartRunning();
					}
					else
					{
						CaptureSession.StopRunning();
					}
				}
			}
		}

		private bool _IsFlash;
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
					if (MainDevice != null && MainDevice.HasTorch)
					{
						NSError device_error;
						MainDevice.LockForConfiguration(out device_error);
						if (device_error != null)
						{
							System.Diagnostics.Debug.WriteLine("Error:{0}", device_error.LocalizedDescription);
							MainDevice.UnlockForConfiguration();
						}
						else
						{
							MainDevice.TorchMode = (_IsFlash) ? AVCaptureTorchMode.On : AVCaptureTorchMode.Off;
							MainDevice.UnlockForConfiguration();
						}
					}
					else
					{
						CaptureSession.StopRunning();
					}
				}
			}
		}

		public UICameraView(CameraView camera)
		{
			_cameraOptions = camera.Camera;
			_IsPreviewing = camera.IsPreviewing;
			_IsFlash = camera.IsFlash;
			Camera = camera;

			CaptureSession = new AVCaptureSession();
			_previewLayer = new AVCaptureVideoPreviewLayer(CaptureSession)
			{
				Frame = Bounds,
				VideoGravity = AVLayerVideoGravity.ResizeAspectFill
			};
			Layer.AddSublayer(_previewLayer);

			Initialize();

			MessagingCenter.Subscribe<LifeCyclePayload>(this, "", (p) =>
			{
				if (p.Status == LifeCycle.OnSleep)
				{
					// スリープ状態になるときリソース解放
					Release();
				}
				else if (p.Status == LifeCycle.OnResume)
				{
					// レジューム状態になるとき初期化
					Initialize();
				}
			});
		}

		public override void Draw(CGRect rect)
		{
			base.Draw(rect);
			_previewLayer.Frame = rect;
		}

		public override void TouchesBegan(NSSet touches, UIEvent evt)
		{
			base.TouchesBegan(touches, evt);
			OnTapped();
		}

		protected virtual void OnTapped()
		{
			var eventHandler = Tapped;
			if (eventHandler != null)
			{
				eventHandler(this, new EventArgs());
			}
		}

		public void Release()
		{
			CaptureSession.StopRunning();
			Recorder.Dispose();
			Metadata.Dispose();
			Queue.Dispose();
			CaptureSession.RemoveOutput(Output);
			CaptureSession.RemoveInput(Input);
			CaptureSession.RemoveOutput(MetadataOutput);
			Output.Dispose();
			Input.Dispose();
			MetadataOutput.Dispose();
			MainDevice.Dispose();
			this.RemoveGestureRecognizer(Pinch);
		}

		public void Initialize(CameraOptions? newCameraPosition = null)
		{
			if (newCameraPosition != null)
			{
				// カメラポジション変更
				_cameraOptions = newCameraPosition.Value;
			}

			// pinchジェスチャ登録
			SetPinchGesture();

			// デバイス設定
			var cameraPosition = (_cameraOptions == CameraOptions.Front) ? AVCaptureDevicePosition.Front : AVCaptureDevicePosition.Back;
			if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
			{
				// iOS10.0以降
				MainDevice = AVCaptureDevice.GetDefaultDevice(AVCaptureDeviceType.BuiltInWideAngleCamera, AVMediaType.Video, cameraPosition);
			}
			else
			{
				var videoDevices = AVCaptureDevice.DevicesWithMediaType(AVMediaType.Video);
				for (int i = 0; i < videoDevices.Length; ++i)
				{
					if (videoDevices[i].Position == cameraPosition)
					{
						MainDevice = videoDevices[i];
						break;
					}
				}
			}

			if (MainDevice == null)
			{
				return;
			}

			NSError device_error;
			MainDevice.LockForConfiguration(out device_error);
			if (device_error != null)
			{
				System.Diagnostics.Debug.WriteLine("Error:{0}", device_error.LocalizedDescription);
				MainDevice.UnlockForConfiguration();
				return;
			}

			// フラッシュ設定
			if (MainDevice.HasTorch)
			{
				MainDevice.TorchMode = (_IsFlash) ? AVCaptureTorchMode.On : AVCaptureTorchMode.Off;
			}

			// フレームレート設定
			MainDevice.ActiveVideoMinFrameDuration = new CMTime(1, 24);
			MainDevice.UnlockForConfiguration();

			// 最大ズーム
			MaxZoom = (float)Math.Min(MainDevice.ActiveFormat.VideoMaxZoomFactor, 6);

			// 入力設定
			NSError error;
			Input = new AVCaptureDeviceInput(MainDevice, out error);
			CaptureSession.AddInput(Input);

			// 出力設定
			Output = new AVCaptureVideoDataOutput();

			// フレーム処理
			Queue = new DispatchQueue("myQueue");
			Output.AlwaysDiscardsLateVideoFrames = true;
			Recorder = new OutputRecorder() { Camera = Camera };
			Output.SetSampleBufferDelegate(Recorder, Queue);
			var vSettings = new AVVideoSettingsUncompressed();
			vSettings.PixelFormatType = CoreVideo.CVPixelFormatType.CV32BGRA;
			Output.WeakVideoSettings = vSettings.Dictionary;

			CaptureSession.AddOutput(Output);

			// メタデータ出力
			MetadataOutput = new AVCaptureMetadataOutput();
			CaptureSession.AddOutput(MetadataOutput);
			Metadata = new OutputMetadata() { Camera = Camera, PreviewLayer = _previewLayer };
			MetadataOutput.SetDelegate(Metadata, Queue);
			if ((AVMetadataObjectType.Face & MetadataOutput.AvailableMetadataObjectTypes) == 0)
			{
				System.Diagnostics.Debug.WriteLine("顔認証に対応していません");
			}
			else
			{
				MetadataOutput.MetadataObjectTypes = AVMetadataObjectType.Face;
			}

			if (IsPreviewing)
			{
				CaptureSession.StartRunning();
			}
		}

		private void SetPinchGesture()
		{
			nfloat lastscale = 1.0f;
			Pinch = new UIPinchGestureRecognizer((UIPinchGestureRecognizer pin) =>
			{
				if (pin.State == UIGestureRecognizerState.Changed)
				{
					NSError device_error;
					MainDevice.LockForConfiguration(out device_error);
					if (device_error != null)
					{
						System.Diagnostics.Debug.WriteLine("Error:{0}", device_error.LocalizedDescription);
						MainDevice.UnlockForConfiguration();
						return;
					}
					var scale = pin.Scale + (1 - lastscale);
					var zoom = MainDevice.VideoZoomFactor * scale;
					zoom = zoom.Clamp(MinZoom, MaxZoom);
					MainDevice.VideoZoomFactor = zoom;
					MainDevice.UnlockForConfiguration();
					lastscale = pin.Scale;
				}
				else if (pin.State == UIGestureRecognizerState.Ended)
				{
					lastscale = 1.0f;
				}
			});
			this.AddGestureRecognizer(Pinch);
		}
	}
}
