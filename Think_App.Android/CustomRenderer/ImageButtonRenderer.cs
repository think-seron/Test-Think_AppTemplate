using System;
using Android.Graphics;
using DroidColor = Android.Graphics.Color;
using FormColor = Xamarin.Forms.Color;
using Think_App;
using Think_App.Droid;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

// TODO Xamarin.Forms.ExportRendererAttribute is not longer supported. For more details see https://github.com/dotnet/maui/wiki/Using-Custom-Renderers-in-.NET-MAUI
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
