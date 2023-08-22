using System;
using System.ComponentModel;
using Think_App;
using Think_App.iOS;
using UIKit;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

// TODO Xamarin.Forms.ExportRendererAttribute is not longer supported. For more details see https://github.com/dotnet/maui/wiki/Using-Custom-Renderers-in-.NET-MAUI
[assembly: ExportRenderer(typeof(CustomEditor), typeof(CustomEditorRenderer))]
namespace Think_App.iOS
{
	public class CustomEditorRenderer : EditorRenderer
	{
		CustomEditor _CustomEditor;

		UIView _accessoryView;

		protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
		{
			base.OnElementChanged(e);

			if (e.NewElement != null)
			{
				_CustomEditor = e.NewElement as CustomEditor;

				UpdateDoneButton();

				// Placeholder初期設定
				if (!string.IsNullOrEmpty(_CustomEditor.Placeholder) && string.IsNullOrEmpty(Control.Text))
				{
					Control.Text = _CustomEditor.Placeholder;
					Control.TextColor = _CustomEditor.PlaceholderColor.ToUIColor();
				}
				Control.ShouldBeginEditing += OnShouldBeginEditing;
				Control.ShouldEndEditing += OnShouldEndEditing;
			}
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == CustomEditor.HideDoneButtonIniPhoneProperty.PropertyName)
			{
				UpdateDoneButton();
			}
		}

		void UpdateDoneButton()
		{
			if (Device.Idiom == DeviceIdiom.Phone)
			{
				if (_accessoryView == null)
				{
					_accessoryView = Control.InputAccessoryView;
				}

				if (_CustomEditor.HideDoneButtonIniPhone)
				{
					Control.InputAccessoryView = null;
				}
				else
				{
					if (Control.InputAccessoryView == null)
					{
						Control.InputAccessoryView = _accessoryView;
					}
				}
			}
		}

		bool OnShouldBeginEditing(UITextView textView)
		{
			if (!string.IsNullOrEmpty(_CustomEditor.Placeholder) && textView.Text == _CustomEditor.Placeholder)
			{
				textView.Text = "";
				textView.TextColor = _CustomEditor.TextColor.ToUIColor();
			}
			return true;
		}

		bool OnShouldEndEditing(UITextView textView)
		{
			if (!string.IsNullOrEmpty(_CustomEditor.Placeholder) && textView.Text == "")
			{
				textView.Text = _CustomEditor.Placeholder;
				textView.TextColor = _CustomEditor.PlaceholderColor.ToUIColor();
			}
			return true;
		}
	}
}
