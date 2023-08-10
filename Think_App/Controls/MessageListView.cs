using System;
using Xamarin.Forms;
namespace Think_App
{
	public class ExScrollEventArgs : EventArgs
	{
		public double X { get; set; }
		public double Y { get; set; }
		public bool IsBottom { get; set; }
		public bool IsTop { get; set; }
	}

	public class MessageListView : ListView
	{
		public event EventHandler RaiseScrollToBottom = delegate {};
		public event EventHandler<ExScrollEventArgs> Scrolled = delegate {};

		public MessageListView(ListViewCachingStrategy strategy) : base(strategy)
		{
			ItemSelected += OnItemSelected;
		}

		public MessageListView()
		{
			ItemSelected += OnItemSelected;
		}

		void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			SelectedItem = null;
		}

		#region IsScrollable BindableProperty
		public static readonly BindableProperty IsScrollableProperty =
			BindableProperty.Create(nameof(IsScrollable), typeof(bool), typeof(MessageListView), true,
				propertyChanged: (bindable, oldValue, newValue) =>
					((MessageListView)bindable).IsScrollable = (bool)newValue);

		public bool IsScrollable
		{
			get { return (bool)GetValue(IsScrollableProperty); }
			set { SetValue(IsScrollableProperty, value); }
		}
		#endregion

		#region TappedCommand BindableProperty
		public static readonly BindableProperty TappedCommandProperty =
			BindableProperty.Create(nameof(TappedCommand), typeof(Command), typeof(MessageListView), null,
				propertyChanged: (bindable, oldValue, newValue) =>
					((MessageListView)bindable).TappedCommand = (Command)newValue);

		public Command TappedCommand
		{
			get { return (Command)GetValue(TappedCommandProperty); }
			set { SetValue(TappedCommandProperty, value); }
		}
		#endregion

		#region TappedCommandParameter BindableProperty
		public static readonly BindableProperty TappedCommandParameterProperty =
			BindableProperty.Create(nameof(TappedCommandParameter), typeof(object), typeof(MessageListView), default(object),
				propertyChanged: (bindable, oldValue, newValue) =>
					((MessageListView)bindable).TappedCommandParameter = (object)newValue);

		public object TappedCommandParameter
		{
			get { return (object)GetValue(TappedCommandParameterProperty); }
			set { SetValue(TappedCommandParameterProperty, value); }
		}
		#endregion

		public void ExecuteTappedCommand()
		{
			if (TappedCommand != null && TappedCommand.CanExecute(TappedCommandParameter))
			{
				TappedCommand.Execute(TappedCommandParameter);
			}
		}

		public void ScrollToBottom()
		{
			if (RaiseScrollToBottom != null)
			{
				RaiseScrollToBottom(this, EventArgs.Empty);
			}
		}

		public void SendScrollEvent(double x, double y, bool isBottom, bool isTop)
		{
			var args = new ExScrollEventArgs()
			{
				X = x,
				Y = y,
				IsBottom = isBottom,
				IsTop = isTop
			};
			if (Scrolled != null)
			{
				Scrolled(this, args);
			}
		}
	}

	public class MessageViewCell : ViewCell
	{
	}
}
