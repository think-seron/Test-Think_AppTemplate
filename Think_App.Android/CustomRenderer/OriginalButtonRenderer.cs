using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Think_App.Droid;

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
