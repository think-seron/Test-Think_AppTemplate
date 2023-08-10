using System;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android.AppCompat;
using Android.Views;
using System.Threading.Tasks;

namespace Think_App.Droid
{
	public class ModalPageRenderer : PageRenderer
	{
		public ModalPageRenderer()
		{
		}
		/// <Docs>This is called when the view is attached to a window.</Docs>
		/// <summary>
		/// Raises the attached to window event.
		/// </summary>
		protected override void OnAttachedToWindow()
		{
			base.OnAttachedToWindow();

			Background = null;
			SetBackgroundColor(Android.Graphics.Color.Transparent);

			var parent = (Parent as Android.Views.ViewGroup);
			if (parent != null)
			{
				for (var i = 0; i < parent.ChildCount; i++)
					parent.GetChildAt(i).SetBackgroundColor(Android.Graphics.Color.Transparent);
			}
		}	
	}
}
