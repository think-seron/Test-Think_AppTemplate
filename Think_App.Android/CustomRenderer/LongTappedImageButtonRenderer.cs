using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Think_App;
using Think_App.Droid;

[assembly: ExportRenderer(typeof(LongTappedImageButton), typeof(LongTappedImageButtonRenderer))]
namespace Think_App.Droid
{
	public class LongTappedImageButtonRenderer : CustomImageButtonRenderer
	{
		const float _deltaX = 5;
		const float _deltaY = 5;

		LongTappedImageButton _LongTappedImageButton;

		protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
		{
			base.OnElementChanged(e);

			if (Control != null && e.NewElement != null)
			{
				_LongTappedImageButton = e.NewElement as LongTappedImageButton;
			}
		}

		public override bool OnInterceptTouchEvent(Android.Views.MotionEvent ev)
		{
			// ここで処理を行う。
			if (ev.Action == Android.Views.MotionEventActions.Down)
			{
				System.Diagnostics.Debug.WriteLine("Down");
				// イベント送信をスタートする。
				_LongTappedImageButton.StartTappingTimer();
			}
			else if (ev.Action == Android.Views.MotionEventActions.Move)
			{
				System.Diagnostics.Debug.WriteLine("Move");
				var x = ev.GetX();
				var y = ev.GetY();
				System.Diagnostics.Debug.WriteLine("X:{0} Y:{1}", x, y);

				// ボタン領域内から抜けたらキャンセル扱い。これ以降、ボタンイベントは発生しない。
				// Androidではいったん領域から抜けたら再度領域に入ってもクリック扱いにしない。
				if (x < -_deltaX || Width + _deltaX < x || y < -_deltaY || Height + _deltaY < y)
				{
					System.Diagnostics.Debug.WriteLine("Cancel");
					// イベント送信を中止する。
					_LongTappedImageButton.StopTappingTimer();
					return true;
				}
			}
			else if (ev.Action == Android.Views.MotionEventActions.Up)
			{
				System.Diagnostics.Debug.WriteLine("Up");
				// イベント送信を中止する。
				_LongTappedImageButton.StopTappingTimer();
			}

			return false;
		}
	}
}
