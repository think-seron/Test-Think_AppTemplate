using System;
using Xamarin.Forms;
namespace Think_App
{
	public class BackCustomizeContentPage : ContentPage
	{
		public Action CustomBackButtonAction { get; set; }

		#region EnableBackButtonOverride BindableProperty
		public static readonly BindableProperty EnableBackButtonOverrideProperty =
			BindableProperty.Create(nameof(EnableBackButtonOverride), typeof(bool), typeof(BackCustomizeContentPage), false,
				propertyChanged: (bindable, oldValue, newValue) =>
					((BackCustomizeContentPage)bindable).EnableBackButtonOverride = (bool)newValue);

		public bool EnableBackButtonOverride
		{
			get { return (bool)GetValue(EnableBackButtonOverrideProperty); }
			set { SetValue(EnableBackButtonOverrideProperty, value); }
		}
		#endregion
	}
}
