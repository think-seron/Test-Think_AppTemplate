using System;
using Android.Content;
using Android.Views;
using Xamarin.Forms;

namespace Think_App.Droid
{
    /// <summary>
    /// DialogService使用時にXamarinFormsで作成したUIをネイティブUIに移管する処理
    /// </summary>
    public static class ViewExtensions
    {
        public static global::Android.Views.View GetNativeView(this Xamarin.Forms.View view, Rectangle size)
        {
            var vRenderer = Xamarin.Forms.Platform.Android.Platform.CreateRendererWithContext(view, MainActivity.activity);
            var nativeView = vRenderer.View;
            vRenderer.Tracker.UpdateLayout();
            var density = MainActivity.activity.Resources.DisplayMetrics.Density;
            nativeView.LayoutParameters = new ViewGroup.LayoutParams((int)(size.Width * density), (int)(size.Height * density));
            view.Layout(Rectangle.FromLTRB(view.X, view.Y, size.Width, size.Height));
            nativeView.Layout((int)nativeView.GetX(), (int)nativeView.GetY(), (int)view.Width, (int)view.Height);
            return nativeView;
        }
    }
}
