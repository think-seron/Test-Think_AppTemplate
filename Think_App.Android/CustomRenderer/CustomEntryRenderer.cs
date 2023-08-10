using System;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Think_App.Droid;

[assembly: ExportRenderer(typeof(Entry), typeof(CustomEntryRenderer))]
namespace Think_App.Droid
{
	public class CustomEntryRenderer : EntryRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Entry> e)
		{
			base.OnElementChanged(e);

			if (this.Control == null) return;

			this.Control.SetBackgroundColor(Android.Graphics.Color.Argb(0,0,0,0));
		}
	}
}
