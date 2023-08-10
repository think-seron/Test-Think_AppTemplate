using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Think_App;
using Think_App.iOS;

[assembly: ExportRenderer(typeof(LongTappedImageButton), typeof(LongTappedImageButtonRenderer))]
namespace Think_App.iOS
{
	public class LongTappedImageButtonRenderer : ButtonRenderer
	{
		LongTappedImageButton _LongTappedImageButton;

		protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
		{
			base.OnElementChanged(e);

			if (Control != null && e.NewElement != null)
			{
				_LongTappedImageButton = e.NewElement as LongTappedImageButton;

				Control.TouchDown += Control_TouchDown;
				Control.TouchUpInside += Control_TouchUpInside;
				Control.TouchUpOutside += Control_TouchUpOutside;
				Control.TouchDragExit += Control_TouchDragExit;
				Control.TouchDragEnter += Control_TouchDragEnter;
				Control.TouchCancel += Control_TouchCancel;
			}
		}

		void Control_TouchDown(object sender, EventArgs e)
		{
			System.Diagnostics.Debug.WriteLine("TouchDown");
			// イベント送信をスタートする。
			_LongTappedImageButton.StartTappingTimer();
		}

		void Control_TouchUpInside(object sender, EventArgs e)
		{
			System.Diagnostics.Debug.WriteLine("TouchUpInside");
			// イベント送信を中止する。
			_LongTappedImageButton.StopTappingTimer();
		}

		void Control_TouchUpOutside(object sender, EventArgs e)
		{
			System.Diagnostics.Debug.WriteLine("TouchUpOutside");
			// イベント送信を中止する。
			_LongTappedImageButton.StopTappingTimer();
		}

		void Control_TouchDragExit(object sender, EventArgs e)
		{
			System.Diagnostics.Debug.WriteLine("TouchDragExit");
			// イベント送信を中止する。
			_LongTappedImageButton.StopTappingTimer();
		}

		void Control_TouchDragEnter(object sender, EventArgs e)
		{
			System.Diagnostics.Debug.WriteLine("TouchDragEnter");
			// イベント送信をスタートする。
			_LongTappedImageButton.StartTappingTimer();
		}

		void Control_TouchCancel(object sender, EventArgs e)
		{
			System.Diagnostics.Debug.WriteLine("TouchCancel");
			// イベント送信を中止する。
			_LongTappedImageButton.StopTappingTimer();
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			// イベント送信を中止する。
			_LongTappedImageButton.StopTappingTimer();
		}
	}
}
