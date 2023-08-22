using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
namespace Think_App
{
	public class CustomHomePage : ContentPage
	{
		public CustomHomePage()
		{


			this.PropertyChanged += (sender, e) =>
			{
				if (e.PropertyName == TitleImageProperty.PropertyName)
				{
					System.Diagnostics.Debug.WriteLine("  CustomContentPage propety changed  " + e.PropertyName);
					App.customNavigationPage.TitleImage = TitleImage;
				}
			};
		}


		#region
		public static readonly BindableProperty TitleImageProperty =
			BindableProperty.Create(
			nameof(TitleImage),
			typeof(ImageSource),
			typeof(CustomHomePage),
			null,
			//propertyChanged: CustomContentPage.OnValuePropertyChanged,
			//propertyChanging: CustomContentPage.OnValuePropertyChanging
			propertyChanged: (bindable, oldValue, newValue) => ((CustomHomePage)bindable).TitleImage = (ImageSource)newValue,
			propertyChanging: (bindable, oldValue, newValue) => ((CustomHomePage)bindable).TitleImage = (ImageSource)newValue
		);

		public ImageSource TitleImage
		{
			get { return (ImageSource)GetValue(TitleImageProperty); }
			set
			{ SetValue(TitleImageProperty, (ImageSource)value); }
		}
		#endregion

		//public static readonly BindableProperty TitleImageProperty = 
		//	BindableProperty.Create(
		//	propertyName: "TitleImage",
		//	returnType: typeof(ImageSource),
		//	declaringType: typeof(CustomContentPage),
		//	defaultValue: null,
		//	defaultBindingMode: BindingMode.OneWay,
		//	validateValue: null,
		//	propertyChanged: CustomContentPage.OnValuePropertyChanged
		//);



		private static void OnValuePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var view = bindable as CustomHomePage;
			if (view == null || !(newValue is ImageSource))
			{
				return;
			}
			view.TitleImage = (ImageSource)newValue;
		}
		private static void OnValuePropertyChanging(BindableObject bindable, object oldValue, object newValue)
		{
			var view = bindable as CustomHomePage;
			if (view == null || !(newValue is ImageSource))
			{
				return;
			}
			view.TitleImage = (ImageSource)newValue;
		}
		//private void OnValueChanged(object sender, change e)
		//{
		//	if (e.NewValue != eOldValue)
		//	{
		//		this.TitleImage = (ImageSource)e.NewValue;
		//	}
		//}
	}
}