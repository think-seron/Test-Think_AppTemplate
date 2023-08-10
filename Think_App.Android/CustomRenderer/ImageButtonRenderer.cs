using System;
using Xamarin.Forms;
using Android.Graphics;
using Xamarin.Forms.Platform.Android;
using DroidColor = Android.Graphics.Color;
using FormColor = Xamarin.Forms.Color;
using Think_App;
using Think_App.Droid;

[assembly: ExportRenderer(typeof(CustomImageButton), typeof(CustomImageButtonRenderer))]
namespace Think_App.Droid
{
	public class CustomImageButtonRenderer : ButtonRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
		{
			base.OnElementChanged(e);

			if (Control != null && e.NewElement != null)
			{
				// これを行わないと、完全に背景が透明色になりません。
				Control.SetBackgroundColor(DroidColor.Transparent);
			}
		}
	}
}
