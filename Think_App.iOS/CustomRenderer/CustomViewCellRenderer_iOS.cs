
using UIKit;

using Think_App;
using Think_App.iOS;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

// TODO Xamarin.Forms.ExportRendererAttribute is not longer supported. For more details see https://github.com/dotnet/maui/wiki/Using-Custom-Renderers-in-.NET-MAUI
[assembly: ExportRenderer(typeof(CustomViewCell), typeof(CustomViewCellRenderer_iOS))]

namespace Think_App.iOS
{
	public class CustomViewCellRenderer_iOS : ViewCellRenderer
	{
		public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
		{
			var cell = base.GetCell(item, reusableCell, tv);

			cell.SelectedBackgroundView = new UIView
			{
				BackgroundColor = ((CustomViewCell)item).SelectedBackgroundColor.ToUIColor()
			};

			return cell;
		}
	}
}