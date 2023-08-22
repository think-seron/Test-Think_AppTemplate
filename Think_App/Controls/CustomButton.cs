using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
	public class CustomButton : Button
	{
		#region UseCustomColor BindableProperty
		public static readonly BindableProperty UseCustomColorProperty =
			BindableProperty.Create(nameof(UseCustomColor), typeof(bool), typeof(CustomButton), false,
				propertyChanged: (bindable, oldValue, newValue) =>
					((CustomButton)bindable).UseCustomColor = (bool)newValue);

		public bool UseCustomColor
		{
			get { return (bool)GetValue(UseCustomColorProperty); }
			set { SetValue(UseCustomColorProperty, value); }
		}
		#endregion

		#region HighlightColor BindableProperty
		public static readonly BindableProperty HighlightColorProperty =
			BindableProperty.Create(nameof(HighlightColor), typeof(Color), typeof(CustomButton), null,
				propertyChanged: (bindable, oldValue, newValue) =>
					((CustomButton)bindable).HighlightColor = (Color)newValue);

		public Color HighlightColor
		{
			get { return (Color)GetValue(HighlightColorProperty); }
			set { SetValue(HighlightColorProperty, value); }
		}
		#endregion

		#region DisableColor BindableProperty
		public static readonly BindableProperty DisableColorProperty =
			BindableProperty.Create(nameof(DisableColor), typeof(Color), typeof(CustomButton), null,
				propertyChanged: (bindable, oldValue, newValue) =>
					((CustomButton)bindable).DisableColor = (Color)newValue);

		public Color DisableColor
		{
			get { return (Color)GetValue(DisableColorProperty); }
			set { SetValue(DisableColorProperty, value); }
		}
		#endregion

		#region Source BindableProperty
		public static readonly BindableProperty SourceProperty =
			BindableProperty.Create(nameof(Source), typeof(ImageSource), typeof(CustomButton), null,
				propertyChanged: (bindable, oldValue, newValue) =>
					((CustomButton)bindable).Source = (ImageSource)newValue);

		public ImageSource Source
		{
			get { return (ImageSource)GetValue(SourceProperty); }
			set { SetValue(SourceProperty, value); }
		}
		#endregion

		#region ImageHeight BindableProperty
		public static readonly BindableProperty ImageHeightProperty =
			BindableProperty.Create(nameof(ImageHeight), typeof(double), typeof(CustomButton), 0.0,
				propertyChanged: (bindable, oldValue, newValue) =>
					((CustomButton)bindable).ImageHeight = (double)newValue);

		public double ImageHeight
		{
			get { return (double)GetValue(ImageHeightProperty); }
			set { SetValue(ImageHeightProperty, value); }
		}
		#endregion

		#region ImageWidth BindableProperty
		public static readonly BindableProperty ImageWidthProperty =
			BindableProperty.Create(nameof(ImageWidth), typeof(double), typeof(CustomButton), 0.0,
				propertyChanged: (bindable, oldValue, newValue) =>
					((CustomButton)bindable).ImageWidth = (double)newValue);

		public double ImageWidth
		{
			get { return (double)GetValue(ImageWidthProperty); }
			set { SetValue(ImageWidthProperty, value); }
		}
		#endregion

		#region ImageOffset BindableProperty
		public static readonly BindableProperty ImageOffsetProperty =
			BindableProperty.Create(nameof(ImageOffset), typeof(Point), typeof(CustomButton), Point.Zero,
				propertyChanged: (bindable, oldValue, newValue) =>
					((CustomButton)bindable).ImageOffset = (Point)newValue);

		public Point ImageOffset
		{
			get { return (Point)GetValue(ImageOffsetProperty); }
			set { SetValue(ImageOffsetProperty, value); }
		}
		#endregion

		#region ImagePadding BindableProperty
		public static readonly BindableProperty ImagePaddingProperty =
			BindableProperty.Create(nameof(ImagePadding), typeof(double), typeof(CustomButton), 0.0,
				propertyChanged: (bindable, oldValue, newValue) =>
					((CustomButton)bindable).ImagePadding = (double)newValue);

		public double ImagePadding
		{
			get { return (double)GetValue(ImagePaddingProperty); }
			set { SetValue(ImagePaddingProperty, value); }
		}
		#endregion

		#region ImageLayoutPosition BindableProperty
		public static readonly BindableProperty ImageLayoutPositionProperty =
			BindableProperty.Create(nameof(ImageLayoutPosition), typeof(LayoutPosition), typeof(CustomButton), LayoutPosition.Left,
				propertyChanged: (bindable, oldValue, newValue) =>
					((CustomButton)bindable).ImageLayoutPosition = (LayoutPosition)newValue);

		public LayoutPosition ImageLayoutPosition
		{
			get { return (LayoutPosition)GetValue(ImageLayoutPositionProperty); }
			set { SetValue(ImageLayoutPositionProperty, value); }
		}
		#endregion

		#region UpdateInChangingSize BindableProperty
		public static readonly BindableProperty UpdateInChangingSizeProperty =
			BindableProperty.Create(nameof(UpdateInChangingSize), typeof(bool), typeof(CustomButton), false,
				propertyChanged: (bindable, oldValue, newValue) =>
					((CustomButton)bindable).UpdateInChangingSize = (bool)newValue);

		public bool UpdateInChangingSize
		{
			get { return (bool)GetValue(UpdateInChangingSizeProperty); }
			set { SetValue(UpdateInChangingSizeProperty, value); }
		}
		#endregion

		//#region ShadowThickness BindableProperty
		//public static readonly BindableProperty ShadowThicknessProperty =
		//	BindableProperty.Create(nameof(ShadowThickness), typeof(double), typeof(CustomButton), 0.0,
		//		propertyChanged: (bindable, oldValue, newValue) =>
		//			((CustomButton)bindable).ShadowThickness = (double)newValue);

		//public double ShadowThickness
		//{
		//	get { return (double)GetValue(ShadowThicknessProperty); }
		//	set { SetValue(ShadowThicknessProperty, value); }
		//}
		//#endregion

		//#region ShadowOffset BindableProperty
		//public static readonly BindableProperty ShadowOffsetProperty =
		//	BindableProperty.Create(nameof(ShadowOffset), typeof(Point), typeof(CustomButton), Point.Zero,
		//		propertyChanged: (bindable, oldValue, newValue) =>
		//			((CustomButton)bindable).ShadowOffset = (Point)newValue);

		//public Point ShadowOffset
		//{
		//	get { return (Point)GetValue(ShadowOffsetProperty); }
		//	set { SetValue(ShadowOffsetProperty, value); }
		//}
		//#endregion

		//#region ShadowColor BindableProperty
		//public static readonly BindableProperty ShadowColorProperty =
		//	BindableProperty.Create(nameof(ShadowColor), typeof(Color), typeof(CustomButton), Color.Black,
		//		propertyChanged: (bindable, oldValue, newValue) =>
		//			((CustomButton)bindable).ShadowColor = (Color)newValue);

		//public Color ShadowColor
		//{
		//	get { return (Color)GetValue(ShadowColorProperty); }
		//	set { SetValue(ShadowColorProperty, value); }
		//}
		//#endregion

		public enum LayoutPosition
		{
			Left,
			Top,
			Right,
			Bottom
		}
	}
}
