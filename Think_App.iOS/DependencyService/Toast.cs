using System;
using UIKit;
using Foundation;
using CoreGraphics;
using Think_App;

[assembly: Xamarin.Forms.Dependency(typeof(Think_App.iOS.Toast))]
namespace Think_App.iOS
{
	public class Toast : IToast
	{
		public void Show(string message)
		{
			var toast = new ToastView();
			toast.Show(UIApplication.SharedApplication.KeyWindow.RootViewController.View, message);
		}
	}

	// 独自のトーストビューを
	class ToastView
	{
		// トーストビュー本体
		UIView _view;
		// 文字列を表示するためのラベル
		UILabel _label;
		// トーストのサイズ（iphone7 default）
		double _margin = 30;
		double _height = 43;
		double _width = 250;

		NSTimer _timer = null;

		public ToastView()
		{
			_margin = _margin * ScaleManager.Scale;
			_height = _height * ScaleManager.Scale;
			_width = _width * ScaleManager.Scale;

			// トーストビューの生成
			_view = new UIView(new CGRect(0, 0, 0, 0))
			{
				BackgroundColor = UIColor.Black
			};
			_view.Layer.CornerRadius = (nfloat)20.0;
			//  メッセージ表示用のラベル
			_label = new UILabel(new CGRect(0,0,0,0))
			{
				TextAlignment = UITextAlignment.Center,
				TextColor = UIColor.White,
				MinimumFontSize = (nfloat)10,
				AdjustsFontSizeToFitWidth = true
			};
			_view.AddSubview(_label);

		}

		// 表示開始
		public void Show(UIView parent, string message)
		{
			// 既に表示中の場合は、処理を停止する
			if (_timer != null)
			{
				_timer.Invalidate();
				_view.RemoveFromSuperview(); // 親ビューから削除する
			}

			// 当初、アルファ値0.7で表示を開始する
			_view.Alpha = (nfloat)0.7;

			// 親Viewからトーストのサイズを調整する
			//_view.Frame = new CGRect(
			//	(parent.Bounds.Width - _width) / 2,
			//	parent.Bounds.Height - _height - _margin,
			//	_width,
			//	_height);

			// トーストの表示位置設定
			double viewRectX = 67 * ScaleManager.Scale;
			double viewRectY = 85 * ScaleManager.Scale;
			_view.Frame = new CGRect(viewRectX,viewRectY,_width,_height);
			// トースト内のtextの表示位置設定
			double labelRectX = 26 * ScaleManager.Scale;
			double labelRectY = 14 * ScaleManager.Scale;
			double labelRectW = 200 * ScaleManager.Scale;
			double labelRectH = 13 * ScaleManager.Scale;
			_label.Frame = new CGRect(labelRectX, labelRectY, labelRectW, labelRectH);
			_label.Text = message; // ラベルの表示文字列を設定する

			parent.AddSubview(_view);

			//タイマー開始
			var wait = 10; // 消え始めるまでのウエイト
			_timer = NSTimer.CreateRepeatingScheduledTimer(TimeSpan.FromMilliseconds(100), delegate
			{
			// alpha値が0になったらViewのサイズを0にしてタイマーを停止する
			if (_view.Alpha <= 0)
				{
					_timer.Invalidate();
					_view.RemoveFromSuperview(); // 親ビューから削除する
			}
				else
				{
					if (wait > 0)
					{
						wait--;
					}
					else
					{
						_view.Alpha -= (nfloat)0.05;
					}
				}
			});
		}
	}
}
