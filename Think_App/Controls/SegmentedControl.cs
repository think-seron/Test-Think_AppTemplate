using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
	public class SegmentedControl : View
	{

		public string SplitString { get; set; }

		#region Contents BindableProperty
		public static readonly BindableProperty ContentsProperty =
					BindableProperty.Create(nameof(ContentsProperty),
					typeof(string),
					typeof(SegmentedControl),
					null,
					BindingMode.OneWay,
											null,
					propertyChanged: (bindable, oldValue, newValue) =>
						((SegmentedControl)bindable).Contents = (string)newValue);

		public string Contents
		{
			get { return (string)GetValue(ContentsProperty); }
			set { SetValue(ContentsProperty, value); }
		}
		#endregion

		#region FontSize BindableProperty
		public static readonly BindableProperty FontSizeProperty =
			BindableProperty.Create(nameof(FontSizeProperty), typeof(double), typeof(SegmentedControl), 14.0,
				propertyChanged: (bindable, oldValue, newValue) =>
					((SegmentedControl)bindable).FontSize = (double)newValue);

		public double FontSize
		{
			get { return (double)GetValue(FontSizeProperty); }
			set { SetValue(FontSizeProperty, value); }
		}
		#endregion

		#region UseCustomSettings BindableProperty
		public static readonly BindableProperty UseCustomSettingsProperty =
			BindableProperty.Create(nameof(UseCustomSettingsProperty), typeof(bool), typeof(SegmentedControl), false,
				propertyChanged: (bindable, oldValue, newValue) =>
					((SegmentedControl)bindable).UseCustomSettings = (bool)newValue);

		public bool UseCustomSettings
		{
			get { return (bool)GetValue(UseCustomSettingsProperty); }
			set { SetValue(UseCustomSettingsProperty, value); }
		}
		#endregion

		#region TextColor BindableProperty
		public static readonly BindableProperty TextColorProperty =
			BindableProperty.Create(nameof(TextColorProperty), typeof(Color), typeof(SegmentedControl), default(Color),
				propertyChanged: (bindable, oldValue, newValue) =>
					((SegmentedControl)bindable).TextColor = (Color)newValue);

		public Color TextColor
		{
			get { return (Color)GetValue(TextColorProperty); }
			set { SetValue(TextColorProperty, value); }
		}
		#endregion

		#region SelectedTextColor BindableProperty
		public static readonly BindableProperty SelectedTextColorProperty =
			BindableProperty.Create(nameof(SelectedTextColorProperty), typeof(Color), typeof(SegmentedControl), default(Color),
				propertyChanged: (bindable, oldValue, newValue) =>
					((SegmentedControl)bindable).SelectedTextColor = (Color)newValue);

		public Color SelectedTextColor
		{
			get { return (Color)GetValue(SelectedTextColorProperty); }
			set { SetValue(SelectedTextColorProperty, value); }
		}
		#endregion

		#region TintColor BindableProperty
		public static readonly BindableProperty TintColorProperty =
			BindableProperty.Create(nameof(TintColorProperty), typeof(Color), typeof(SegmentedControl), default(Color),
				propertyChanged: (bindable, oldValue, newValue) =>
					((SegmentedControl)bindable).TintColor = (Color)newValue);

		public Color TintColor
		{
			get { return (Color)GetValue(TintColorProperty); }
			set { SetValue(TintColorProperty, value); }
		}
		#endregion

		#region CornerRadiusRate BindableProperty
		public static readonly BindableProperty CornerRadiusRateProperty =
			BindableProperty.Create(nameof(CornerRadiusRateProperty), typeof(double), typeof(SegmentedControl), 25.0,
				propertyChanged: (bindable, oldValue, newValue) =>
					((SegmentedControl)bindable).CornerRadiusRate = (double)newValue);

		public double CornerRadiusRate
		{
			get { return (double)GetValue(CornerRadiusRateProperty); }
			set { SetValue(CornerRadiusRateProperty, value); }
		}
		#endregion

		#region SelectedIndex BindableProperty
		public static readonly BindableProperty SelectedIndexProperty =
			BindableProperty.Create(nameof(SelectedIndexProperty), typeof(int), typeof(SegmentedControl), 0,
				propertyChanged: (bindable, oldValue, newValue) =>
					((SegmentedControl)bindable).SelectedIndex = (int)newValue);

		public int SelectedIndex
		{
			get { return (int)GetValue(SelectedIndexProperty); }
			set { SetValue(SelectedIndexProperty, value); }
		}
		#endregion

		public SegmentedControl()
		{
		}
	}
}