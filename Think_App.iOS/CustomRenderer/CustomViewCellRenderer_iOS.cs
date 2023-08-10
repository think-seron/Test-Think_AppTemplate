using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

using UIKit;

using Think_App;
using Think_App.iOS;

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