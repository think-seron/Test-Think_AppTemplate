using System;
using System.ComponentModel;
using Think_App;
using Think_App.iOS;
using UIKit;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

// TODO Xamarin.Forms.ExportRendererAttribute is not longer supported. For more details see https://github.com/dotnet/maui/wiki/Using-Custom-Renderers-in-.NET-MAUI
[assembly: ExportRenderer(typeof(SegmentedControl), typeof(SegmentedControlRenderer))]

namespace Think_App.iOS
{
    public class SegmentedControlRenderer : ViewRenderer<SegmentedControl, UISegmentedControl>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<SegmentedControl> e)
        {
            base.OnElementChanged(e);

            if (Control == null && e.NewElement != null)
            {
                var contents = e.NewElement.Contents;
                if (string.IsNullOrEmpty(contents))
                {
                    return;
                }

                string spl = ",";
                if (!string.IsNullOrEmpty(e.NewElement.SplitString))
                {
                    spl = e.NewElement.SplitString;
                }

                var array = contents.Split(spl.ToCharArray());
                if (array != null)
                {
                    var seg = new UISegmentedControl(array);
                    try
                    {
                        seg.SelectedSegment = (nint)e.NewElement.SelectedIndex;
                    }
                    catch
                    {
                    }
                    // 選択肢が変更された時PCLの値を変更する。
                    seg.AddTarget((__, _) =>
                    {
                        System.Diagnostics.Debug.WriteLine("Checked Change");
                        e.NewElement.SelectedIndex = (int)seg.SelectedSegment;
                    }, UIControlEvent.ValueChanged);
                    SetNativeControl(seg);
                }

                if (Control != null && Element != null)
                {
                    UpdateFontSize(Element);
                    UpdateTextColor(Element);
                    UpdateTintColor(Element);
                }
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (Control == null || Element == null)
            {
                return;
            }

            if (e.PropertyName == SegmentedControl.SelectedIndexProperty.PropertyName)
            {
                UpdateSelect(Element);
                UpdateFontSize(Element);
                UpdateTextColor(Element);
                UpdateTintColor(Element);
            }
        }

        void UpdateSelect(SegmentedControl seg)
        {
            Control.SelectedSegment = (nint)seg.SelectedIndex;
        }

        void UpdateFontSize(SegmentedControl seg)
        {
            var fontSize = (nfloat)seg.FontSize;

            var attr = Control.GetTitleTextAttributes(UIControlState.Normal);
            attr.Font = UIFont.SystemFontOfSize(fontSize);
            Control.SetTitleTextAttributes(attr, UIControlState.Normal);
        }

        void UpdateTextColor(SegmentedControl seg)
        {
            if (seg.UseCustomSettings)
            {
                var attr = Control.GetTitleTextAttributes(UIControlState.Normal);
                attr.TextColor = seg.TextColor.ToUIColor();
                Control.SetTitleTextAttributes(attr, UIControlState.Normal);

                attr = Control.GetTitleTextAttributes(UIControlState.Selected);
                attr.TextColor = seg.SelectedTextColor.ToUIColor();
                Control.SetTitleTextAttributes(attr, UIControlState.Selected);
            }
        }

        void UpdateTintColor(SegmentedControl seg)
        {
            if (seg.UseCustomSettings)
            {
                Control.TintColor = seg.TintColor.ToUIColor();
                if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
                    Control.SelectedSegmentTintColor = seg.TintColor.ToUIColor();
            }
        }

        public override void Draw(CoreGraphics.CGRect rect)
        {
            // CornerRadiusの適用。
            if (Control.Layer != null && Element != null && Element.UseCustomSettings)
            {
                var width = rect.Width;
                var height = rect.Height;

                Control.Layer.CornerRadius = (nfloat)(Math.Min(width, height) * Element.CornerRadiusRate / 100);
            }

            base.Draw(rect);
        }
    }
}
