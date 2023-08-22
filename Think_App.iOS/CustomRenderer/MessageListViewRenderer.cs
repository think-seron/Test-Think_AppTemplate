using System;
using System.ComponentModel;
using UIKit;
using Think_App;
using Think_App.iOS;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

// TODO Xamarin.Forms.ExportRendererAttribute is not longer supported. For more details see https://github.com/dotnet/maui/wiki/Using-Custom-Renderers-in-.NET-MAUI
[assembly: ExportRenderer(typeof(MessageListView), typeof(MessageListViewRenderer))]
namespace Think_App.iOS
{
	public class MessageListViewRenderer : ListViewRenderer
	{
		MessageListView _MessageListView;

		bool ForceScrollToBottom { get; set; }

		protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
		{
			base.OnElementChanged(e);

			if (Control != null && e.NewElement != null)
			{
				_MessageListView = e.NewElement as MessageListView;
				_MessageListView.RaiseScrollToBottom += (__, _) =>
				{
					ForceScrollToBottom = true;
					SetNeedsLayout();
				};

				//選択しない
				Control.AllowsSelection = false;
				UpdateScroll();

				// タップイベントを追加
				Control.AddGestureRecognizer(new UITapGestureRecognizer(OnTapped));
				// スクロールイベント取得
				Control.Delegate = new CustomTableViewDelegate(this);
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
			Control.ScrollEnabled = _MessageListView.IsScrollable;
		}

		void OnTapped()
		{
			_MessageListView.ExecuteTappedCommand();
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			if (ForceScrollToBottom)
			{
				ScrollToBottom();
				ForceScrollToBottom = false;
			}
		}

		void ScrollToBottom()
		{
			// Footerまではスクロールしないので注意する。
			var section = Control.NumberOfSections() - 1;
			var row = Control.NumberOfRowsInSection(section) - 1;
			if (row >= 0 && section >= 0)
			{
				var indexPath = Foundation.NSIndexPath.FromRowSection(row, section);
				Control.ScrollToRow(indexPath, UITableViewScrollPosition.Bottom, false);
			}
		}

		private class CustomTableViewDelegate : UITableViewDelegate
		{
			ListView _element;
			UITableViewSource _source;

			public CustomTableViewDelegate(MessageListViewRenderer renderer)
			{
				_element = renderer.Element;
				_source = renderer.Control.Source;
			}

			public override void DraggingEnded(UIScrollView scrollView, bool willDecelerate)
			{
				_source.DraggingEnded(scrollView, willDecelerate);
			}

			public override void DraggingStarted(UIScrollView scrollView)
			{
				_source.DraggingStarted(scrollView);
			}

			public override nfloat GetHeightForHeader(UITableView tableView, nint section)
			{
				return _source.GetHeightForHeader(tableView, section);
			}

			public override UIView GetViewForHeader(UITableView tableView, nint section)
			{
				return _source.GetViewForHeader(tableView, section);
			}

			public override void RowSelected(UITableView tableView, Foundation.NSIndexPath indexPath)
			{
				_source.RowSelected(tableView, indexPath);
			}

			public override void RowDeselected(UITableView tableView, Foundation.NSIndexPath indexPath)
			{
				_source.RowDeselected(tableView, indexPath);
			}

			public override void Scrolled(UIScrollView scrollView)
			{
				_source.Scrolled(scrollView);
				SendScrollEvent(scrollView);
			}

			void SendScrollEvent(UIScrollView scrollView)
			{
				var messageListView = _element as MessageListView;
				if (messageListView != null)
				{
					double x = scrollView.ContentOffset.X;
					double y = scrollView.ContentOffset.Y;
					var dy = scrollView.ContentSize.Height - scrollView.Frame.Size.Height;
					// y は0.5刻みだが、dy はもっと細かい刻みになるので、実際には一番下にスクロールしていても、
					// スクロールしていない判定になることがある。よって、dyを0.5刻みに修正する。
					dy = (nfloat)ConvertHalfIncrements(dy);
					bool isBottom = y.Equals(dy) || y > dy;
					bool isTop = y.Equals(0) || y < 0;
					messageListView.SendScrollEvent(x, y, isBottom, isTop);
				}
			}

			double ConvertHalfIncrements(double val)
			{
				// 数値を切り捨て0.5刻みにして返す。
				var floorVal = Math.Floor(val);
				var roundVal = Math.Round(val);
				if (floorVal < roundVal)
				{
					// valは切り捨ての結果 < 四捨五入の結果なので、小数点以下が.5以上。
					// 切り捨ての結果に0.5を足して返す。
					return floorVal + 0.5;
				}
				else
				{
					// valは切り捨ての結果 = 四捨五入の結果なので、小数点以下が.5に満たない。
					// 切り捨ての結果をそのまま返す。
					return floorVal;
				}
			}
		}
	}
}
