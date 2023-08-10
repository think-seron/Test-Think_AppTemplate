using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Think_App;
using Think_App.iOS;
using UIKit;

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
			if (Device.Idiom == TargetIdiom.Phone)
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
