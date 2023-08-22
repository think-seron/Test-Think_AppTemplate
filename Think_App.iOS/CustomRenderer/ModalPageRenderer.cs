using System;
using System.Threading.Tasks;
using UIKit;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Controls;
using Microsoft.Maui;


namespace Think_App.iOS
{
	public class ModalPageRenderer : PageRenderer
	{
		/// <summary>
		/// The parent.
		/// </summary>
		private UIViewController _parentModalViewController;

		/// <summary>
		/// Dids the move to parent view controller.
		/// </summary>
		/// <param name="parent">Parent.</param>
		public override void DidMoveToParentViewController(UIViewController parent)
		{
			base.DidMoveToParentViewController(parent);

			// Save modal wrapper from Xamarin.Forms
			_parentModalViewController = parent;

			// Set custom to be able to set clear background!
			parent.ModalPresentationStyle = UIModalPresentationStyle.Custom;
		}

		/// <summary>
		/// Views the will appear.
		/// </summary>
		/// <param name="animated">If set to <c>true</c> animated.</param>
		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(false);

			// Clear background on parent modal wrapper!!
			_parentModalViewController.View.BackgroundColor = UIColor.Clear;
			View.BackgroundColor = UIColor.Clear;
		}

		/// <summary>
		/// Views the did appear.
		/// </summary>
		/// <param name="animated">If set to <c>true</c> animated.</param>
		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(false);

			// Clear background on parent modal wrapper!!
			_parentModalViewController.View.BackgroundColor = UIColor.Clear;
			View.BackgroundColor = UIColor.Clear;
		}
	}
}
