using System;
using UIKit;
using Think_App.iOS;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
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
