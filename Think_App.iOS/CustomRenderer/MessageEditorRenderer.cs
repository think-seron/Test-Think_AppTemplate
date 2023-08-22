using System;
using System.ComponentModel;
using CoreGraphics;
using Think_App;
using Think_App.iOS;
using UIKit;
using Foundation;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

// TODO Xamarin.Forms.ExportRendererAttribute is not longer supported. For more details see https://github.com/dotnet/maui/wiki/Using-Custom-Renderers-in-.NET-MAUI
[assembly: ExportRenderer(typeof(MessageEditor), typeof(MessageEditorRenderer))]
namespace Think_App.iOS
{
	public class MessageEditorRenderer : CustomEditorRenderer
	{
		MessageEditor _MessageEditor;

		bool ForceScrollToTop { get; set; }
		CGRect initRect { get; set; }

		protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
		{
			base.OnElementChanged(e);

			if (Control != null && e.NewElement != null)
			{
				_MessageEditor = e.NewElement as MessageEditor;

				// バウンスは全面的に禁止
				Control.Bounces = false;
				// 初期状態のキャレット位置を取得する。
				var pos = Control.EndOfDocument;
				initRect = Control.GetCaretRectForPosition(pos);

				Control.Scrolled += OnScrolled;
			}
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
			if (e.PropertyName == MessageEditor.TextProperty.PropertyName)
			{
				UpdateLines();
			}
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			if (ForceScrollToTop)
			{
				// 強制的にスクロール位置をトップにする。
				Control.SetContentOffset(new CGPoint(0, 0), false);
				ForceScrollToTop = false;
			}
		}

		void UpdateLines()
		{
			if (Control == null)
			{
				return;
			}

			var pos = Control.EndOfDocument;
			var currentRect = Control.GetCaretRectForPosition(pos);
			if (currentRect.Y > initRect.Y)
			{
				// 2行以上
				_MessageEditor.UpdateLines(MessageEditor.Lines.Multi);
				// 強制的にスクロール位置をトップにする。
				ForceScrollToTop = true;

				SetNeedsLayout();
			}
			else
			{
				// 1行
				_MessageEditor.UpdateLines(MessageEditor.Lines.Single);
			}
		}

		void OnScrolled(object sender, EventArgs e)
		{
			if (_MessageEditor.TextLines == MessageEditor.Lines.Single)
			{
				Control.SetContentOffset(new CGPoint(0, 0), false);
			}
		}
	}
}
