using System;
using System.ComponentModel;
using System.Reflection;

using Android.Widget;
using Android.Graphics;
using Support = Android.Support.V7.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform.Android.AppCompat;
using Think_App;
using Think_App.Droid;

using AProgressBar = Android.Widget.ProgressBar;
using View = Android.Views.View;
using Android.Views;

[assembly: ExportRenderer(typeof(CustomHomePage), typeof(CustomHomePagerenderer_Droid))]

namespace Think_App.Droid
{
	public class CustomHomePagerenderer_Droid : PageRenderer
	{
		public CustomHomePagerenderer_Droid()
		{
		}

		CustomHomePage customContentPage;
		protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Page> e)
		{
			base.OnElementChanged(e);

			System.Diagnostics.Debug.WriteLine(" custom content page  renderer element changed");


			if (e.NewElement != null)
			{
				customContentPage = e.NewElement as CustomHomePage;

			}
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			System.Diagnostics.Debug.WriteLine(" custom content page  renderer element propertychanged changed" + e.PropertyName);

			if (e.PropertyName == CustomHomePage.TitleImageProperty.PropertyName)
			{

				System.Diagnostics.Debug.WriteLine(" e propetyName  :" + e.PropertyName);
				System.Diagnostics.Debug.WriteLine("add :  TitleImage");


				App.customNavigationPage.TitleImage = customContentPage.TitleImage;
			}
			//customContentPage.PropertyChanged += (senderA, eA) => {
			//	if (e.PropertyName == CustomContentPage.TitleImageProperty.PropertyName) {
			//		App.customNavigationPage.TitleImage = customContentPage.TitleImage;
			//	}
			//};
		}
	}
}
