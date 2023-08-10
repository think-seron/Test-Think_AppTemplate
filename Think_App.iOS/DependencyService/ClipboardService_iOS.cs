using System;
using UIKit;
using Xamarin.Forms;
using Think_App.iOS;
[assembly: Dependency(typeof(ClipboardService_iOS))]
namespace Think_App.iOS
{
	public class ClipboardService_iOS : IClipboardService
	{
		public void CopyToClipboard(String text)
		{
			UIPasteboard clipboard = UIPasteboard.General;
			clipboard.String = text;
		}
	}
}
