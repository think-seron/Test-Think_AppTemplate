using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
namespace Think_App
{
	public class CustomNavigationPage : NavigationPage
	{
		public event EventHandler<bool> UpdateShadowRequested = delegate { };

		public CustomNavigationPage() : base()
		{

		}
		public CustomNavigationPage(Microsoft.Maui.Controls.Page page) : base(page)
		{

		}
		public static readonly BindableProperty IsBadgeVisbleProperty = BindableProperty.Create(
			propertyName: "IsBadgeVisble",
			returnType: typeof(bool),
			declaringType: typeof(CustomNavigationPage),
			defaultValue: false);

		public bool IsBadgeVisble
		{
			get { return (bool)GetValue(IsBadgeVisbleProperty); }
			set
			{ SetValue(IsBadgeVisbleProperty, value); }
		}

		public static readonly BindableProperty IsRunningPoperty = BindableProperty.Create(
			propertyName: "IsRunning",
			returnType: typeof(bool),
			declaringType: typeof(CustomNavigationPage),
			defaultValue: false);

		public bool IsRunning
		{
			get { return (bool)GetValue(IsRunningPoperty); }
			set
			{ SetValue(IsRunningPoperty, value); }
		}

		public static readonly BindableProperty TitleImageProperty = BindableProperty.Create(
			propertyName: "TitleImage",
					returnType: typeof(ImageSource),
			declaringType: typeof(CustomNavigationPage),
			defaultValue: null,
			defaultBindingMode: BindingMode.TwoWay,
			validateValue: null,
			//propertyChanged: CustomNavigationPage.OnValuePropertyChanged,
			//propertyChanging: CustomNavigationPage.OnValuePropertyChanging
			propertyChanged: (bindable, oldValue, newValue) => ((CustomNavigationPage)bindable).TitleImage = (ImageSource)newValue,
			propertyChanging: (bindable, oldValue, newValue) => ((CustomNavigationPage)bindable).TitleImage = (ImageSource)newValue
		);
		public ImageSource TitleImage
		{
			get { return (ImageSource)GetValue(TitleImageProperty); }
			set
			{ SetValue(TitleImageProperty, (ImageSource)value); }
		}

		public void UpdateShadow(bool shadow)
		{
			if (UpdateShadowRequested != null)
			{
				UpdateShadowRequested(this, shadow);
			}
		}

		//private static void OnValuePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		//{
		//	var view = bindable as CustomNavigationPage;
		//	if (view == null || !(newValue is ImageSource))
		//	{
		//		return;
		//	}
		//	view.TitleImage = (ImageSource)newValue;
		//}
		//private static void OnValuePropertyChanging(BindableObject bindable, object oldValue, object newValue)
		//{
		//	var view = bindable as CustomNavigationPage;
		//	if (view == null || !(newValue is ImageSource))
		//	{
		//		return;
		//	}
		//	view.TitleImage = (ImageSource)newValue;
		//}
	}
}