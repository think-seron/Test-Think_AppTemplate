using System;
using System.ComponentModel;
using Think_App;
using Think_App.iOS;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

// TODO Xamarin.Forms.ExportRendererAttribute is not longer supported. For more details see https://github.com/dotnet/maui/wiki/Using-Custom-Renderers-in-.NET-MAUI
[assembly: ExportRenderer(typeof(MultiLineLabel), typeof(MultiLineLabelRenderer))]
namespace Think_App.iOS
{
	public class MultiLineLabelRenderer : LabelRenderer
	{
		MultiLineLabel _MultiLineLabel;

		protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
		{
			base.OnElementChanged(e);

			if (Control != null && e.NewElement != null)
			{
				_MultiLineLabel = e.NewElement as MultiLineLabel;
				UpdateLines();
			}
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == MultiLineLabel.LinesProperty.PropertyName)
			{
				UpdateLines();
			}
		}

		void UpdateLines()
		{
			if (_MultiLineLabel.Lines >= 1)
			{
				// 数値設定
				Control.Lines = _MultiLineLabel.Lines;
			}
			else
			{
				// 無限設定
				Control.Lines = nint.MaxValue;
			}
		}
	}
}
