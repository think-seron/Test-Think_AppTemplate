using System;
using Xamarin.Forms;
namespace Think_App
{
	public class LongTappedImageButton : CustomImageButton
    {
		public event EventHandler LongTapping = delegate { };

		#region SendTappingEventIntervalSeconds BindableProperty
		public static readonly BindableProperty SendTappingEventIntervalSecondsProperty =
			BindableProperty.Create(nameof(SendTappingEventIntervalSeconds), typeof(double), typeof(LongTappedImageButton), 0.1,
				propertyChanged: (bindable, oldValue, newValue) =>
					((LongTappedImageButton)bindable).SendTappingEventIntervalSeconds = (double)newValue);

		public double SendTappingEventIntervalSeconds
		{
			get { return (double)GetValue(SendTappingEventIntervalSecondsProperty); }
			set { SetValue(SendTappingEventIntervalSecondsProperty, value); }
		}
		#endregion

		private bool IsContinue { get; set; }
		private bool IsTimerRunning { get; set; }

		public void StartTappingTimer()
		{
			if (IsTimerRunning)
			{
				return;
			}
			IsContinue = true;
			Device.StartTimer(TimeSpan.FromSeconds(SendTappingEventIntervalSeconds), SendTappingEvent);
			IsTimerRunning = true;
		}

		public void StopTappingTimer()
		{
			if (IsTimerRunning)
			{
				IsContinue = false;
				IsTimerRunning = false;
			}
		}

		bool SendTappingEvent()
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				if (LongTapping != null)
				{
					LongTapping(this, EventArgs.Empty);
				}
			});

			return IsContinue;
		}
	}
}
