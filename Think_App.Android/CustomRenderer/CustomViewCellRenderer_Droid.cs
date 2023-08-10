using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

using Think_App;
using Think_App.Droid;

[assembly: ExportRenderer(typeof(CustomViewCell), typeof(CustomViewCellRenderer_Droid))]

namespace Think_App.Droid
{
	public class CustomViewCellRenderer_Droid : ViewCellRenderer
	{
		private Android.Views.View _cellCore;
		private bool _selected;
		private Android.Graphics.Drawables.Drawable _unselectedBackground;

		protected override Android.Views.View GetCellCore(Cell item, Android.Views.View convertView, Android.Views.ViewGroup parent, Android.Content.Context context)
		{
			_cellCore = base.GetCellCore(item, convertView, parent, context);

			this._selected = false;
			this._unselectedBackground = _cellCore.Background;

			return _cellCore;
		}

		protected override void OnCellPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnCellPropertyChanged(sender, e);

			if (e.PropertyName == "IsSelected")
			{
				_selected = !(_selected);
				if (_selected)
				{
					_cellCore.SetBackgroundColor(((CustomViewCell)sender).SelectedBackgroundColor.ToAndroid());
				}
				else
				{
					_cellCore.SetBackground(this._unselectedBackground);
				}
			}
		}
	}
}