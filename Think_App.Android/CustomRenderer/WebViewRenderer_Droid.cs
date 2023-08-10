using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Think_App;
using Think_App.Droid;
using System.IO;
[assembly: ExportRenderer(typeof(WebView), typeof(WebViewRenderer_Droid))]

namespace Think_App.Droid
{
	public class WebViewRenderer_Droid : WebViewRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<WebView> e)
		{

			base.OnElementChanged(e);

			if (Control == null) return;

			Control.Settings.DomStorageEnabled = true;

			Control.Settings.JavaScriptEnabled = true;

			Control.Settings.AllowFileAccessFromFileURLs = true;

			Control.Settings.AllowUniversalAccessFromFileURLs = true;

			Control.Settings.AllowContentAccess = true;

			Control.Settings.AllowFileAccess = true;

			Control.Settings.UseWideViewPort = true;

			Control.Settings.LoadWithOverviewMode = true;

			Control.Settings.JavaScriptCanOpenWindowsAutomatically = true;
			//Control.Settings.BuiltInZoomControls = true;

			//Control.Settings.DisplayZoomControls = true;

		}
	}
}
