using System;
using Android.Content;
using Think_App.Droid;
using Xamarin.Forms;
[assembly:Dependency(typeof(WebBrowserService_Droid))]
namespace Think_App.Droid
{
	public class WebBrowserService_Droid:IWebBrowserService
	{
		public void Open(Uri url)
		{
			MainActivity.context.StartActivity
			             (
                new Intent(Intent.ActionView,
					global::Android.Net.Uri.Parse(url.AbsoluteUri) ));
		}
	}
}
