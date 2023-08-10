using System;
using Think_App.iOS;
using UIKit;
using Xamarin.Forms;
[assembly: Dependency(typeof(WebBrowserService_iOS))]

namespace Think_App.iOS
{
	public class WebBrowserService_iOS : IWebBrowserService
	{
		public WebBrowserService_iOS()
		{
		}
		public void Open(Uri url)
		{
			UIApplication.SharedApplication.OpenUrl(url);
		}
	}
}
