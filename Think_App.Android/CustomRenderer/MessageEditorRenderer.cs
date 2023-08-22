using System;
using System.ComponentModel;
using Think_App;
using Think_App.Droid;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

// TODO Xamarin.Forms.ExportRendererAttribute is not longer supported. For more details see https://github.com/dotnet/maui/wiki/Using-Custom-Renderers-in-.NET-MAUI
[assembly: ExportRenderer(typeof(MessageEditor), typeof(MessageEditorRenderer))]
namespace Think_App.Droid
{
	public class MessageEditorRenderer : CustomEditorRenderer
	{
		MessageEditor _MessageEditor;

		protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
		{
			base.OnElementChanged(e);

			if (Control != null && e.NewElement != null)
			{
				_MessageEditor = e.NewElement as MessageEditor;
				try
				{
					Control.TextChanged += OnTextChanged;
				}
				catch (Exception ex)
				{
					System.Diagnostics.Debug.WriteLine(ex);
				}
			}
		}

		void OnTextChanged(object sender, Android.Text.TextChangedEventArgs e)
		{
			try
			{
				if (_MessageEditor.Opacity > 0.0)
				{
					if (Control.LineCount > 1)
					{
						// 2行以上
						_MessageEditor.UpdateLines(MessageEditor.Lines.Multi);
					}
					else
					{
						// 1行
						_MessageEditor.UpdateLines(MessageEditor.Lines.Single);
					}
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex);
			}
		}
	}
}
