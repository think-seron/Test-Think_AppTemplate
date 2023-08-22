using System;
using System.ComponentModel;
using UIKit;
using CoreGraphics;
using Think_App;
using Think_App.iOS;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

// TODO Xamarin.Forms.ExportRendererAttribute is not longer supported. For more details see https://github.com/dotnet/maui/wiki/Using-Custom-Renderers-in-.NET-MAUI
[assembly: ExportRenderer(typeof(BackCustomizeContentPage), typeof(BackCustomizeContentPageRenderer))]
namespace Think_App.iOS
{
	public class BackCustomizeContentPageRenderer : PageRenderer
	{
		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			if (((BackCustomizeContentPage)Element).EnableBackButtonOverride)
			{
				SetCustomBackButton();
			}
		}

		void SetCustomBackButton()
		{
			// Load the Back arrow Image
			var backBtnImage = UIImage.FromBundle("icon_navigation_back.png");

			backBtnImage = backBtnImage.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
			backBtnImage = GetResizedImage(backBtnImage, 13, 22);

			var backBtn = new UIButton(UIButtonType.Custom)
			{
				HorizontalAlignment = UIControlContentHorizontalAlignment.Left,
				TitleEdgeInsets = new UIEdgeInsets(11.5f, 15f, 10f, 0f),
				ImageEdgeInsets = new UIEdgeInsets(1f, 8f, 0f, 0f),
			};

			// Set the styling for Title
		    // You could set any Text as you wish here
		    backBtn.SetTitle("", UIControlState.Normal);
			backBtn.TintColor = ColorList.colorFont.ToUIColor();

			backBtn.SetImage(backBtnImage, UIControlState.Normal);

			backBtn.SizeToFit();

			backBtn.TouchDown += (sender, e) =>
			{
				if (((BackCustomizeContentPage)Element)?.CustomBackButtonAction != null)
				{
					((BackCustomizeContentPage)Element)?.CustomBackButtonAction.Invoke();
				}
			};

			backBtn.Frame = new CGRect(0, 0, UIScreen.MainScreen.Bounds.Width / 4, NavigationController.NavigationBar.Frame.Height);

			var btnContainer = new UIView(new CGRect(0, 0, backBtn.Frame.Width, backBtn.Frame.Height));
			btnContainer.AddSubview(backBtn);

			var fixedSpace = new UIBarButtonItem(UIBarButtonSystemItem.FixedSpace)
			{
				Width = -16f
			};

			var backButtonItem = new UIBarButtonItem("", UIBarButtonItemStyle.Plain, null)
			{
				CustomView = backBtn
			};

			NavigationController.TopViewController.NavigationItem.LeftBarButtonItems = new[] { fixedSpace, backButtonItem };
		}

		UIImage GetResizedImage(UIImage srcImage, double width, double height)
		{
			var size = new CGSize(width, height);
			UIGraphics.BeginImageContext(size);
			srcImage.Draw(new CGRect(0, 0, size.Width, size.Height));
			var image = UIGraphics.GetImageFromCurrentImageContext();
			UIGraphics.EndImageContext();
			return image;
		}
	}
}
