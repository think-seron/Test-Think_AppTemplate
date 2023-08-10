using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Think_App;
using Think_App.Droid;

[assembly: ExportRenderer(typeof(MultiLineLabel), typeof(MultiLineLabelRenderer))]
namespace Think_App.Droid
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
				Control.SetSingleLine(false);
				Control.SetLines(_MultiLineLabel.Lines);
			}
			else
			{
				// 無限
				Control.SetSingleLine(false);
				Control.SetLines(int.MaxValue);
			}
		}
	}
}
