using Microsoft.Maui.Controls;
using Microsoft.Maui;
namespace Think_App
{
	public class CustomViewCell : ViewCell
	{
		public static readonly BindableProperty SelectedBackgroundColorProperty = BindableProperty.Create(
			"SelectedBackgroundColor", typeof(Color), typeof(CustomViewCell), null
		);

		public Color SelectedBackgroundColor
		{
			get { return (Color)GetValue(SelectedBackgroundColorProperty); }
			set { SetValue(SelectedBackgroundColorProperty, value); }
		}

		public CustomViewCell()
		{
		}
	}
}