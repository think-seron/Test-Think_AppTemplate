using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Graphics.Drawables;
using Think_App;
using Think_App.Droid;

[assembly: ExportRenderer(typeof(CustomEditor), typeof(CustomEditorRenderer))]
namespace Think_App.Droid
{
	public class CustomEditorRenderer : EditorRenderer
	{
		CustomEditor _CustomEditor;

		protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
		{
			base.OnElementChanged(e);

			if (Control != null && e.NewElement != null)
			{
				_CustomEditor = e.NewElement as CustomEditor;

				var gd = new GradientDrawable();
				gd.SetColor(Android.Graphics.Color.Transparent);
				Control.SetBackground(gd);

				// ----------Placeholder-----------
				UpdatePlaceholder();
				UpdatePlaceholderColor();
				// --------------------------------
			}
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == CustomEditor.PlaceholderProperty.PropertyName)
			{
				UpdatePlaceholder();
			}
			else if (e.PropertyName == CustomEditor.PlaceholderColorProperty.PropertyName)
			{
				UpdatePlaceholderColor();
			}
		}

		void UpdatePlaceholder()
		{
			Control.Hint = _CustomEditor.Placeholder;
		}

		void UpdatePlaceholderColor()
		{
			Control.SetHintTextColor(_CustomEditor.PlaceholderColor.ToAndroid());
		}
	}
}
