using System;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;
using System.Linq;


using Foundation;
using Xamarin.Forms;
using Xamarin.Forms.Platform;
using Xamarin.Forms.Platform.iOS;

using Think_App;
using Think_App.iOS;
using UIKit;
using CoreAnimation;
using CoreGraphics;
[assembly: ExportRenderer(typeof(CustomHomePage), typeof(CustomHomePageRenderer_iOS))]

namespace Think_App.iOS
{
	public class CustomHomePageRenderer_iOS : PageRenderer
	{

		public CustomHomePageRenderer_iOS()
		{
		}

		protected override void OnElementChanged(VisualElementChangedEventArgs e)
		{
			base.OnElementChanged(e);
			if (e.NewElement != null)
			{


			}
		}
		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);

			//if (customContentPage.TitleImage != null)
			//{
			//	System.Diagnostics.Debug.WriteLine("title ホーム");

			//	App.customNavigationPage.TitleImage = customContentPage.TitleImage;

			//}
		}
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			System.Diagnostics.Debug.WriteLine("  CustomHomePageRenderer_iOS  ViewWillAppear");
			if (App.customNavigationPage.IsBadgeVisble)
			{
				System.Diagnostics.Debug.WriteLine("CustomHomePageRenderer_iOS ViewWillAppear IsBadgeVisble true");
				App.customNavigationPage.IsBadgeVisble = false;
			}
			else
			{
				System.Diagnostics.Debug.WriteLine("CustomHomePageRenderer_iOS ViewWillAppear IsBadgeVisble false");
			}
		}
	}
}
