using System;
using Xamarin.Forms;
namespace Think_App
{
	public class CustomEditor : Editor
	{
		#region HideDoneButtonIniPhone BindableProperty
		public static readonly BindableProperty HideDoneButtonIniPhoneProperty =
			BindableProperty.Create(nameof(HideDoneButtonIniPhone), typeof(bool), typeof(CustomEditor), false,
				propertyChanged: (bindable, oldValue, newValue) =>
					((CustomEditor)bindable).HideDoneButtonIniPhone = (bool)newValue);

		public bool HideDoneButtonIniPhone
		{
			get { return (bool)GetValue(HideDoneButtonIniPhoneProperty); }
			set { SetValue(HideDoneButtonIniPhoneProperty, value); }
		}
		#endregion

		#region Placeholder BindableProperty
		public static readonly BindableProperty PlaceholderProperty =
			BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(CustomEditor), String.Empty,
				propertyChanged: (bindable, oldValue, newValue) =>
					((CustomEditor)bindable).Placeholder = (string)newValue);

		public string Placeholder
		{
			get { return (string)GetValue(PlaceholderProperty);}
			set { SetValue(PlaceholderProperty, value);}
		}
		#endregion

		#region PlaceholderColor BindableProperty
		public static readonly BindableProperty PlaceholderColorProperty =
			BindableProperty.Create(nameof(PlaceholderColor), typeof(Color), typeof(CustomEditor), Color.Gray,
				propertyChanged: (bindable, oldValue, newValue) =>
					((CustomEditor)bindable).PlaceholderColor = (Color)newValue);

		public Color PlaceholderColor
		{
			get { return (Color)GetValue(PlaceholderColorProperty); }
			set { SetValue(PlaceholderColorProperty, value); }
		}
		#endregion
	}
}
