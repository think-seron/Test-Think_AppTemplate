using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Think_App;
using Think_App.Droid;

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
