using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using StoreKit;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(Think_App.iOS.DialogService))]
namespace Think_App.iOS
{
    /// <summary>
    /// ダイアログ状の各UIを表示する
    /// </summary>
    public class DialogService : IDialogService
    {
        List<UIView> _nativeViews = new List<UIView>();
        public Task DisplayFormsViewAsync(View formsView)
        {
            var taskCompletionSource = new TaskCompletionSource<Task>();
            Device.BeginInvokeOnMainThread(() =>
            {
                try
                {
                    var nativeView = formsView.GetNativeView(new CoreGraphics.CGRect(0, 0, formsView.WidthRequest, formsView.HeightRequest));
                    if (nativeView.Superview != null) throw new NullReferenceException();
                    nativeView.UserInteractionEnabled = true;

                    _nativeViews.Add(nativeView);
                    UIApplication.SharedApplication.KeyWindow.AddSubview(nativeView);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                }
                finally
                {
                    taskCompletionSource.TrySetResult(taskCompletionSource.Task);
                }

            });
            return taskCompletionSource.Task;
        }

        public Task HideFormsViewAsync()
        {
            if (!_nativeViews.Any()) return Task.FromResult<Task>(null);
            var taskCompletionSource = new TaskCompletionSource<Task>();
            Device.BeginInvokeOnMainThread(() =>
            {
                var nativeView = _nativeViews.FirstOrDefault();
                try
                {
                    nativeView?.RemoveFromSuperview();
                }
                catch { }
                finally
                {
                    _nativeViews.Remove(nativeView);
                    nativeView?.Dispose();
                    nativeView = null;
                    taskCompletionSource.TrySetResult(taskCompletionSource.Task);
                }

            });
            return taskCompletionSource.Task;
        }

        public Task DisplayAlert(string title, string message, DialogItem cancelItem)
        {
            var taskCompletionSource = new TaskCompletionSource<Task>();

            var alert = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);

            // Cancel
            var cancel = UIAlertAction.Create(cancelItem.Text, ToActionStyle(cancelItem.Style), (obj) =>
            {
                taskCompletionSource.SetResult(taskCompletionSource.Task);
            });
            alert.AddAction(cancel);

            ShowAlertController(alert);

            return taskCompletionSource.Task;
        }
        public Task<bool> DisplayAlert(string title, string message, DialogItem acceptItem, DialogItem cancelItem)
        {
            var taskCompletionSource = new TaskCompletionSource<bool>();

            var alert = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);

            // Cancel
            var cancel = UIAlertAction.Create(cancelItem.Text, ToActionStyle(cancelItem.Style), (obj) =>
            {
                taskCompletionSource.SetResult(false);
            });
            alert.AddAction(cancel);

            // Accept
            var accept = UIAlertAction.Create(acceptItem.Text, ToActionStyle(acceptItem.Style), (obj) =>
            {
                taskCompletionSource.SetResult(true);
            });
            alert.AddAction(accept);

            ShowAlertController(alert);

            return taskCompletionSource.Task;
        }

        public Task<string> DisplayActionSheet(string title, string message, string cancel, string destruction, DialogItem[] items)
        {
            var taskCompletionSource = new TaskCompletionSource<string>();

            var alert = UIAlertController.Create(title, message, UIAlertControllerStyle.ActionSheet);

            if (destruction != null)
            {
                alert.AddAction(UIAlertAction.Create(destruction, UIAlertActionStyle.Destructive, (obj) =>
                {
                    taskCompletionSource.SetResult(destruction);
                }));
            }

            foreach (var item in items)
            {
                alert.AddAction(UIAlertAction.Create(item.Text, ToActionStyle(item.Style), (obj) => taskCompletionSource.SetResult(item.Text)));
            }

            if (!string.IsNullOrEmpty(cancel) || UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad)
            {
                alert.AddAction(UIAlertAction.Create(cancel ?? "", UIAlertActionStyle.Cancel, (obj) =>
                {
                    taskCompletionSource.SetResult(null);
                }));
            }

            ShowAlertController(alert);

            return taskCompletionSource.Task;
        }

        void ShowAlertController(UIAlertController alert)
        {
            var viewController = GetVisibleViewController();
            // iPad
            if (alert.PopoverPresentationController != null)
            {
                alert.PopoverPresentationController.SourceView = viewController.View;
                //表示位置指定
                alert.PopoverPresentationController.SourceRect =
                         new CGRect(((nfloat)UIScreen.MainScreen.Bounds.Width) / 2, UIScreen.MainScreen.Bounds.Height, 0, 0);
            }
            viewController.PresentViewController(alert, false, null);
        }

        UIAlertActionStyle ToActionStyle(DialogItem.ItemStyle style)
        {
            switch (style)
            {
                case DialogItem.ItemStyle.Cancel:
                    return UIAlertActionStyle.Cancel;
                case DialogItem.ItemStyle.Default:
                    return UIAlertActionStyle.Default;
                case DialogItem.ItemStyle.Destructive:
                    return UIAlertActionStyle.Destructive;
                default:
                    return UIAlertActionStyle.Default;
            }
        }

        UIViewController GetVisibleViewController(UIViewController controller = null)
        {
            controller = controller ?? UIApplication.SharedApplication.KeyWindow.RootViewController;

            if (controller.PresentedViewController == null) return controller;

            if (controller.PresentedViewController is UINavigationController)
            {
                return ((UINavigationController)controller.PresentedViewController).VisibleViewController;
            }

            if (controller.PresentedViewController is UITabBarController)
            {
                return ((UITabBarController)controller.PresentedViewController).SelectedViewController;
            }

            return GetVisibleViewController(controller.PresentedViewController);
        }
    }
}
