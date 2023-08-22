using System;
using System.ComponentModel;
using Android.Graphics.Drawables;
using Think_App;
using Think_App.Droid;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

// TODO Xamarin.Forms.ExportRendererAttribute is not longer supported. For more details see https://github.com/dotnet/maui/wiki/Using-Custom-Renderers-in-.NET-MAUI
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
