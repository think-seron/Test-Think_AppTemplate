using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Think_App;
using Think_App.Droid;
using DroidListView = Android.Widget.ListView;
using DroidAbsListView = Android.Widget.AbsListView;
using DroidScrollState = Android.Widget.ScrollState;
using Android.Views;
using Android.Runtime;
using Android.Widget;

[assembly: ExportRenderer(typeof(MessageListView), typeof(MessageListViewRenderer))]
[assembly: ExportRenderer(typeof(MessageViewCell), typeof(MessageViewCellRenderer))]
namespace Think_App.Droid
{
	public class MessageListViewRenderer : ListViewRenderer
	{
		MessageListView _MessageListView;
		bool IsRunning { get; set; }

		protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.ListView> e)
		{
			base.OnElementChanged(e);

			if (Control != null && e.NewElement != null)
			{
				_MessageListView = e.NewElement as MessageListView;
				_MessageListView.RaiseScrollToBottom += OnScrollToBottom;

				Control.SetSelector(Android.Resource.Color.Transparent);
				Control.CacheColorHint = Android.Graphics.Color.Transparent;
				UpdateScroll();

				var listener = new CustomOnTouchListener()
				{
					Element = _MessageListView
				};
				Control.SetOnTouchListener(listener);
				Control.SetOnScrollListener(new CustomViewScrollDetector(this));
			}
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == MessageListView.IsScrollableProperty.PropertyName)
			{
				UpdateScroll();
			}
		}

		void UpdateScroll()
		{
			try
			{
				// ScrollViewの中に入っているときのみ有効なプロパティだが、実用的にはこれで十分。
				Control.NestedScrollingEnabled = _MessageListView.IsScrollable;
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex);
			}
		}

		private class CustomOnTouchListener : Java.Lang.Object, IOnTouchListener
		{
			public MessageListView Element { get; set; }
			private bool IsScroll { get; set; }
			private float _oldX, _oldY;
			const double delta = 80.0;
			public bool OnTouch(Android.Views.View v, MotionEvent e)
			{
				if (e.Action == MotionEventActions.Down)
				{
					IsScroll = false;
					_oldX = e.GetX();
					_oldY = e.GetY();
				}
				if (e.Action == MotionEventActions.Up)
				{
					// スクロール動作ではない時のみ、タップコマンドの実行。
					if (!IsScroll)
					{
						Element?.ExecuteTappedCommand();
					}
				}
				else if (e.Action == MotionEventActions.Move)
				{
					var newX = e.GetX();
					var newY = e.GetY();

					if (delta < GetDistanceSquare(_oldX, _oldY, newX, newY))
					{
						// スクロール動作フラグを立てる。
						IsScroll = true;
					}
					_oldX = newX;
					_oldY = newY;
				}
				return false;
			}

			double GetDistanceSquare(float x1, float y1, float x2, float y2)
			{
				var val = (x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1);
				return val;
			}
		}

		async void OnScrollToBottom(object sender, EventArgs e)
		{
			if (IsRunning)
			{
				return;
			}
			IsRunning = true;
			if (Control != null && Control.Adapter != null)
			{
				// このDelayを入れないと、スクロールされないことがある。
				await System.Threading.Tasks.Task.Delay(20);
				Control.SmoothScrollToPosition(Control.Adapter.Count - 1);
			}
			IsRunning = false;
		}

		private class CustomViewScrollDetector : Java.Lang.Object, DroidAbsListView.IOnScrollListener
		{
			Xamarin.Forms.ListView _element;
			float _density;
			int _contentOffset = 0;
			TrackElement[] _trackElements =
			{
				new TrackElement(0),
				new TrackElement(1),
				new TrackElement(2),
				new TrackElement(3)
			};

			double _maxOffset = -1;
			int _totalItemCount = -1;

			public CustomViewScrollDetector(MessageListViewRenderer renderer)
			{
				_element = renderer.Element;
				_density = renderer.Context.Resources.DisplayMetrics.Density;
			}

			public void OnScroll(DroidAbsListView view, int firstVisibleItem, int visibleItemCount, int totalItemCount)
			{
				if (_totalItemCount != totalItemCount)
				{
					// アイテム数が変更されたら、最大オフセットを変更する。
					_totalItemCount = totalItemCount;
					_maxOffset = -1;
				}
				bool isTop = (firstVisibleItem == 0);
				bool isBottom = (firstVisibleItem + visibleItemCount >= totalItemCount);
				bool isTracked = false;
				foreach (var t in _trackElements)
				{
					if (!isTracked)
					{
						if (t.IsSafeToTrack(view))
						{
							isTracked = true;
							_contentOffset += t.GetDeltaY();
							SendScrollEvent(_contentOffset, isTop, isBottom);
							t.SyncState(view);
						}
						else
						{
							t.Reset();
						}
					}
					else
					{
						t.SyncState(view);
					}
				}
			}

			public void OnScrollStateChanged(DroidAbsListView view, [GeneratedEnum] DroidScrollState scrollState)
			{
				if (scrollState == DroidScrollState.TouchScroll || scrollState == DroidScrollState.Fling)
				{
					foreach (var t in _trackElements)
					{
						t.SyncState(view);
					}
				}
			}

			void SendScrollEvent(double y, bool isTop, bool isBottom)
			{
				var offset = Math.Abs(y) / _density;
				if (offset.Equals(_maxOffset))
				{
					// 最大オフセットに達していれば、一番下までスクロールしたとみなす。
					isBottom = true;
				}
				var messageListView = _element as MessageListView;
				if (messageListView != null)
				{
					messageListView.SendScrollEvent(0, offset, isBottom, isTop);
				}
				if (isBottom && _maxOffset < 0)
				{
					// 最大オフセットを更新。
					_maxOffset = offset;
				}
			}

			private class TrackElement
			{
				readonly int _position;
				Android.Views.View _trackedView;
				int _trackedViewPrevPosition;
				int _trackedViewPrevTop;

				public TrackElement(int postion)
				{
					_position = postion;
				}

				public void SyncState(DroidAbsListView view)
				{
					if (view.ChildCount > 0)
					{
						_trackedView = GetChild(view);
						_trackedViewPrevTop = GetY();
						_trackedViewPrevPosition = view.GetPositionForView(_trackedView);
					}
				}

				public void Reset()
				{
					_trackedView = null;
				}

				public bool IsSafeToTrack(DroidAbsListView view)
				{
					return _trackedView != null && _trackedView.Parent == view && view.GetPositionForView(_trackedView) == _trackedViewPrevPosition;
				}

				public int GetDeltaY()
				{
					return GetY() - _trackedViewPrevTop;
				}

				Android.Views.View GetChild(DroidAbsListView view)
				{
					if (_position == 0)
					{
						return view.GetChildAt(0);
					}
					else if (_position == 1 || _position == 2)
					{
						return view.GetChildAt(view.ChildCount / 2);
					}
					else if (_position == 3)
					{
						return view.GetChildAt(view.ChildCount - 1);
					}

					return null;
				}

				int GetY()
				{
					return _position <= 1 ? _trackedView.Bottom : _trackedView.Top;
				}
			}
		}
	}

	public class MessageViewCellRenderer : ViewCellRenderer
	{
		protected override Android.Views.View GetCellCore(Cell item, Android.Views.View convertView, Android.Views.ViewGroup parent, Android.Content.Context context)
		{
			var cell = base.GetCellCore(item, convertView, parent, context);

			var listView = parent as DroidListView;

			if (listView != null)
			{
				listView.SetSelector(Android.Resource.Color.Transparent);
				listView.CacheColorHint = Android.Graphics.Color.Transparent;
			}

			return cell;
		}
	}
}
