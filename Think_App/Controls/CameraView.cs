using System;
using Xamarin.Forms;

namespace Think_App
{
	public class CameraView : View
	{
		public event EventHandler<PhotoTakenEventArgs> PhotoTaken = delegate {};
		public event EventHandler<FaceDetectedEventArgs> FaceDetected = delegate {};
		public event EventHandler FaceDetectionFailed = delegate {};

		#region Camera BindableProperty
		public static readonly BindableProperty CameraProperty =
			BindableProperty.Create(nameof(Camera), typeof(CameraOptions), typeof(CameraView), CameraOptions.Rear,
				propertyChanged: (bindable, oldValue, newValue) =>
					((CameraView)bindable).Camera = (CameraOptions)newValue);

		public CameraOptions Camera
		{
			get { return (CameraOptions)GetValue(CameraProperty); }
			set { SetValue(CameraProperty, value); }
		}
		#endregion

		#region IsPreviewing BindableProperty
		public static readonly BindableProperty IsPreviewingProperty =
			BindableProperty.Create(nameof(IsPreviewing), typeof(bool), typeof(CameraView), false,
				propertyChanged: (bindable, oldValue, newValue) =>
					((CameraView)bindable).IsPreviewing = (bool)newValue);

		public bool IsPreviewing
		{
			get { return (bool)GetValue(IsPreviewingProperty); }
			set { SetValue(IsPreviewingProperty, value); }
		}
		#endregion

		#region IsFlash BindableProperty
		public static readonly BindableProperty IsFlashProperty =
			BindableProperty.Create(nameof(IsFlash), typeof(bool), typeof(CameraView), false,
				propertyChanged: (bindable, oldValue, newValue) =>
					((CameraView)bindable).IsFlash = (bool)newValue);

		public bool IsFlash
		{
			get { return (bool)GetValue(IsFlashProperty); }
			set { SetValue(IsFlashProperty, value); }
		}
		#endregion

		bool _IsTakenPhoto;
		public bool IsTakenPhoto
		{
			get
			{
				if (_IsTakenPhoto)
				{
					_IsTakenPhoto = false;
					return true;
				}
				else
				{
					return false;
				}
			}
			private set
			{
				if (_IsTakenPhoto != value)
				{
					_IsTakenPhoto = value;
				}
			}
		}

		public void TakePhoto()
		{
			IsTakenPhoto = true;
		}

		public void CompleteTakenPhotoAndSendImageSource(ImageSource source)
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				if (PhotoTaken != null && source != null)
				{
					PhotoTaken(this, new PhotoTakenEventArgs() { ImageSource = source });
				}
			});
		}

		public void SendEventFaceDetection(int faceScore,
										   Rectangle faceRange,
										   Point? mouthPosition,
										   Point? leftEyePosition,
										   Point? rightEyePostion)
		{
			if (FaceDetected != null)
			{
				FaceDetected(this, new FaceDetectedEventArgs()
				{
					FaceScore = faceScore,
					FaceRange = faceRange,
					MouthPosition = mouthPosition,
					LeftEyePosition = leftEyePosition,
					RightEyePostion = rightEyePostion
				});
			}
		}

		public void SendEventFaceDetectionFailed()
		{
			if (FaceDetectionFailed != null)
			{
				FaceDetectionFailed(this, EventArgs.Empty);
			}
		}
	}

	public enum CameraOptions
	{
		Rear,
		Front	
	}

	public class PhotoTakenEventArgs : EventArgs
	{
		public ImageSource ImageSource { get; set; }
	}

	public class FaceDetectedEventArgs : EventArgs
	{
		public int FaceScore { get; set; }
		public Rectangle FaceRange { get; set; }
		public Point? MouthPosition { get; set; }
		public Point? LeftEyePosition { get; set; }
		public Point? RightEyePostion { get; set; }
	}
}
