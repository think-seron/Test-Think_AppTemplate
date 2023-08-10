using System;
using Xamarin.Forms;
namespace Think_App
{
	public class BalloonView : BoxView
	{
		public enum Direction
		{
			Left,
			Right
		}

		#region TailDirection BindableProperty
		public static readonly BindableProperty TailDirectionProperty =
			BindableProperty.Create(nameof(TailDirection), typeof(Direction), typeof(BalloonView), Direction.Left,
				propertyChanged: (bindable, oldValue, newValue) =>
					((BalloonView)bindable).TailDirection = (Direction)newValue);

		public Direction TailDirection
		{
			get { return (Direction)GetValue(TailDirectionProperty); }
			set { SetValue(TailDirectionProperty, value); }
		}
		#endregion

		#region TailThickness BindableProperty
		public static readonly BindableProperty TailThicknessProperty =
			BindableProperty.Create(nameof(TailThickness), typeof(double), typeof(BalloonView), 12.78,
				propertyChanged: (bindable, oldValue, newValue) =>
					((BalloonView)bindable).TailThickness = (double)newValue);

		public double TailThickness
		{
			get { return (double)GetValue(TailThicknessProperty); }
			set { SetValue(TailThicknessProperty, value); }
		}
		#endregion

		#region TailWidth BindableProperty
		public static readonly BindableProperty TailWidthProperty =
			BindableProperty.Create(nameof(TailWidth), typeof(double), typeof(BalloonView), 15.88,
				propertyChanged: (bindable, oldValue, newValue) =>
					((BalloonView)bindable).TailWidth = (double)newValue);

		public double TailWidth
		{
			get { return (double)GetValue(TailWidthProperty); }
			set { SetValue(TailWidthProperty, value); }
		}
		#endregion

		#region TailTopPosition BindableProperty
		public static readonly BindableProperty TailTopPositionProperty =
			BindableProperty.Create(nameof(TailTopPosition), typeof(double), typeof(BalloonView), 7.59,
				propertyChanged: (bindable, oldValue, newValue) =>
					((BalloonView)bindable).TailTopPosition = (double)newValue);

		public double TailTopPosition
		{
			get { return (double)GetValue(TailTopPositionProperty); }
			set { SetValue(TailTopPositionProperty, value); }
		}
		#endregion

		#region CornerRadius BindableProperty
		public static readonly BindableProperty CornerRadiusProperty =
			BindableProperty.Create(nameof(CornerRadius), typeof(double), typeof(BalloonView), 0.0,
				propertyChanged: (bindable, oldValue, newValue) =>
					((BalloonView)bindable).CornerRadius = (double)newValue);

		public double CornerRadius
		{
			get { return (double)GetValue(CornerRadiusProperty); }
			set { SetValue(CornerRadiusProperty, value); }
		}
		#endregion
	}
}
