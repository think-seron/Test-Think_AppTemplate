using System;
using System.Drawing;
using UIKit;
using Foundation;
using Think_App.iOS;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

[assembly: Dependency(typeof(TextService))]
namespace Think_App.iOS
{
	public class TextService : ITextService
	{
		public double CalculateTextWidth(string text, double fontSize)
		{
			var uiLabel = new UILabel();
			uiLabel.Font = UIFont.SystemFontOfSize((nfloat)fontSize);
			uiLabel.Text = text;
			var size = uiLabel.IntrinsicContentSize;
			return size.Width;
		}

		public double CalculateTextHeight(string text, double fontSize)
		{
			double height = 0;
			string[] delimiter = { Environment.NewLine };
			var lines = text.Split(delimiter, StringSplitOptions.None);
			foreach (var line in lines)
			{
				var uiLabel = new UILabel();
				uiLabel.Font = UIFont.SystemFontOfSize((nfloat)fontSize);
				uiLabel.Text = text;
				var size = uiLabel.IntrinsicContentSize;
				height += size.Height;
			}

			return height;
		}	
	}
}
