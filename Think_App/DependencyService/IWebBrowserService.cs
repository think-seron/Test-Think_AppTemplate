using System;
using Xamarin.Forms;
namespace Think_App
{
	public interface IWebBrowserService
	{
		void Open(Uri url);
	}
}
