using System;
using Foundation;
using CoreGraphics;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Think_App;
using Think_App.iOS;

[assembly: ExportRenderer(typeof(MessageMainPage), typeof(MessageMainPageRenderer))]
namespace Think_App.iOS
{
	public class MessageMainPageRenderer : PageRenderer
	{
		NSObject _keyboardShowObserver;
		NSObject _keyboardHideObserver;
		NSObject _keyboardHidenObserver;

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			_keyboardShowObserver = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillShowNotification, (notification) =>
			{
				var keyboardHeight = ((NSValue)notification.UserInfo[UIKeyboard.FrameEndUserInfoKey]).CGRectValue.Height;
				// 秒単位
				var duration = ((NSNumber)notification.UserInfo[UIKeyboard.AnimationDurationUserInfoKey]).DoubleValue;

				// 速度調整が必要
				duration *= 1.5;
				if (((MessageMainPage)Element).IsSwitchingLines != true)
				{
					((MessageMainPage)Element).SlideContent(false, keyboardHeight, (uint)(duration * 1000));
				}

				// キーボード表示フラグを立てる
				((MessageMainPage)Element).IsKeyboardShown = true;
			});

			_keyboardHideObserver = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, (notification) =>
			{
				// 秒単位
				var duration = ((NSNumber)notification.UserInfo[UIKeyboard.AnimationDurationUserInfoKey]).DoubleValue;

				if (((MessageMainPage)Element).IsSwitchingLines != true)
				{
					((MessageMainPage)Element).SlideContent(true, 0, (uint)(duration * 1000));
				}
			});

			_keyboardHidenObserver = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.DidHideNotification, (_) =>
			{
				// キーボード表示フラグを下ろす
				((MessageMainPage)Element).IsKeyboardShown = false;
			});
		}

		public override void ViewDidDisappear(bool animated)
		{
			base.ViewDidDisappear(animated);
			if (_keyboardShowObserver != null)
			{
				NSNotificationCenter.DefaultCenter.RemoveObserver(_keyboardShowObserver);
				_keyboardShowObserver.Dispose();
				_keyboardShowObserver = null;
			}
			if (_keyboardHideObserver != null)
			{
				NSNotificationCenter.DefaultCenter.RemoveObserver(_keyboardHideObserver);
				_keyboardHideObserver.Dispose();
				_keyboardHideObserver = null;
			}
			if (_keyboardHidenObserver != null)
			{
				NSNotificationCenter.DefaultCenter.RemoveObserver(_keyboardHidenObserver);
				_keyboardHidenObserver.Dispose();
				_keyboardHidenObserver = null;
			}
		}
	}
}
