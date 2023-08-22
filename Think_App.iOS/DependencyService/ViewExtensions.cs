using System;
using System.Threading.Tasks;
using CoreGraphics;
using UIKit;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
    /// <summary>
    /// DialogService使用時にXamarinFormsで作成したUIをネイティブUIに移管する処理
    /// </summary>
    public static class ViewExtensions
    {
        public static UIView GetNativeView(this Microsoft.Maui.Controls.View view, CGRect size)
        {
            var renderer = Platform.CreateRenderer(view);
            renderer.NativeView.Frame = size;
            renderer.NativeView.AutoresizingMask = UIViewAutoresizing.All;
            renderer.NativeView.ContentMode = UIViewContentMode.ScaleToFill;
            renderer.Element.Layout(size.ToRectangle());

            var nativeView = renderer.NativeView;
            nativeView.SetNeedsLayout();
            return nativeView;
        }
    }
}
