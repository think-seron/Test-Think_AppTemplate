using System;
using Think_App.Droid;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

// TODO Xamarin.Forms.ExportRendererAttribute is not longer supported. For more details see https://github.com/dotnet/maui/wiki/Using-Custom-Renderers-in-.NET-MAUI
[assembly: ExportRenderer(typeof(Button), typeof(OriginalButtonRenderer))]
namespace Think_App.Droid
{
public class OriginalButtonRenderer : ButtonRenderer
	{
		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (Control != null)
			{
				Control.SetPadding(0, 0, 0, 0);
				if (!Control.Enabled)
				{
					Control.SetTextColor(Android.Graphics.Color.White);
				}
			}
		}
		public OriginalButtonRenderer()
		{
		}	
	}
}
