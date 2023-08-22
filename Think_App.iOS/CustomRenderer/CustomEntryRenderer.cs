using System;
using Foundation;
using UIKit;
using Think_App.iOS;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

// TODO Xamarin.Forms.ExportRendererAttribute is not longer supported. For more details see https://github.com/dotnet/maui/wiki/Using-Custom-Renderers-in-.NET-MAUI
[assembly: ExportRenderer(typeof(Entry), typeof(CustomEntryRenderer))]
namespace Think_App.iOS
{
	public class CustomEntryRenderer : EntryRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Microsoft.Maui.Controls.Entry> e)
		{
			base.OnElementChanged(e);

			if (this.Control == null) return;

			this.Control.BorderStyle = UITextBorderStyle.None;
		}
	}
}
