using System;
using Android.Content;
using Xamarin.Forms;
using Think_App.Droid;
[assembly: Dependency(typeof(ClipboardService_Droid))]
namespace Think_App.Droid
{
	public class ClipboardService_Droid : IClipboardService
	{
		public void CopyToClipboard(String text)
		{
			// Get the Clipboard Manager
			var clipboardManager = (ClipboardManager)Forms.Context.GetSystemService(Context.ClipboardService);

			// Create a new Clip
			ClipData clip = ClipData.NewPlainText("LABEL", text);

			// Copy the text
			clipboardManager.PrimaryClip = clip;
		}
	}
}