using System;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using static Android.Views.View;

[assembly: Dependency(typeof(Think_App.Droid.DialogService))]
namespace Think_App.Droid
{
    /// <summary>
    /// ダイアログ状の各UIを表示する
    /// </summary>
    public class DialogService : IDialogService
    {
        Activity _currentActivity => MainActivity.activity;
        global::Android.Views.View _nativeView;
        public Task DisplayFormsViewAsync(Xamarin.Forms.View formsView)
        {
            var density = ScaleManager.AndroidDensity;
            var taskCompletionSource = new TaskCompletionSource<Task>();
            Device.BeginInvokeOnMainThread(() =>
            {
                if (_nativeView != null)
                    ((ViewGroup)_nativeView.Parent)?.RemoveView(_nativeView);

                var decoreView = _currentActivity.Window.DecorView;
                var rectangle = new global::Android.Graphics.Rect();
                decoreView.GetWindowVisibleDisplayFrame(rectangle);
                var decoreWidht = decoreView.Width;
                var height = rectangle.Bottom - rectangle.Top;
                _nativeView = formsView.GetNativeView(
                    new Rectangle(0, 0, decoreWidht / density, height / density));
                _nativeView.SetOnTouchListener(new CustomOnTouchListener(ignoreTouch: true));
                MainActivity.BackButtonDisable = true;
                _currentActivity.Window.AddContentView(_nativeView, new ViewGroup.LayoutParams(decoreWidht, height));
                taskCompletionSource.TrySetResult(taskCompletionSource.Task);
            });
            return taskCompletionSource.Task;
        }

        public Task HideFormsViewAsync()
        {
            if (_nativeView == null) return Task.FromResult<Task>(null);

            var taskCompletionSource = new TaskCompletionSource<Task>();
            Device.BeginInvokeOnMainThread(() =>
            {
                try
                {
                    ((ViewGroup)_nativeView.Parent)?.RemoveView(_nativeView);
                }
                catch { }
                finally
                {
                    _nativeView = null;
                    MainActivity.BackButtonDisable = false;
                    taskCompletionSource.SetResult(taskCompletionSource.Task);
                }
            });
            return taskCompletionSource.Task;
        }

        public Task DisplayAlert(string title, string message, DialogItem cancelItem)
        {
            return null;
            //var dialog = new AlertDialog.Builder(_currentContext);
            //return App.Current.MainPage.DisplayAlertAsync(title, message, cancelItem.Text);
        }

        public Task<bool> DisplayAlert(string title, string message, DialogItem acceptItem, DialogItem cancelItem)
        {

            var taskCompletionSource = new TaskCompletionSource<bool>();
            Device.BeginInvokeOnMainThread(() =>
            {
                var dialogBuilder = new AlertDialog.Builder(_currentActivity);
                if (!string.IsNullOrEmpty(title))
                    dialogBuilder.SetTitle(title);

                if (!string.IsNullOrEmpty(message))
                    dialogBuilder.SetMessage(message);

                dialogBuilder.SetPositiveButton(acceptItem.Text, (sender, e) =>
                {
                    taskCompletionSource.SetResult(true);
                });

                dialogBuilder.SetNegativeButton(cancelItem.Text, (sender, e) =>
                {
                    taskCompletionSource.SetResult(false);
                });

                var dialog = dialogBuilder.Create();
                dialog.SetCanceledOnTouchOutside(true);
                dialog.CancelEvent += (o, e) => taskCompletionSource.TrySetResult(false);
                dialog.Show();
                //if (acceptItem.Style == DialogItem.ItemStyle.Destructive)
                //    dialog.GetButton((int)DialogButtonType.Positive).SetTextColor(Colors.TextCaution.ToAndroid());
                //if (cancelItem.Style == DialogItem.ItemStyle.Cancel)
                //    dialog.GetButton((int)DialogButtonType.Negative).SetTextColor(Colors.TextDefault.ToAndroid());
            });
            return taskCompletionSource.Task;
        }

        public Task<string> DisplayActionSheet(string title, string message, string cancel, string destruction, DialogItem[] items)
        {
            var taskCompletionSource = new TaskCompletionSource<string>();

            var builder = new AlertDialog.Builder(_currentActivity);
            builder.SetTitle(title);
            //builder.SetMessage(message);
            var stringItems = items.Select((arg) => arg.Text).ToArray();

            builder.SetSingleChoiceItems(new CustomArrayAdapter(_currentActivity, items), -1, (o, args) =>
            {
                if (o is AlertDialog alertDialog)
                    alertDialog.Dismiss();

                var index = (message == null) ? args.Which : args.Which - 1;
                taskCompletionSource.TrySetResult(stringItems[index]);
            });

            if (cancel != null)
                builder.SetPositiveButton(cancel, (o, args) => taskCompletionSource.TrySetResult(cancel));

            if (destruction != null)
                builder.SetNegativeButton(destruction, (o, args) => taskCompletionSource.TrySetResult(destruction));

            var dialog = builder.Create();
            builder.Dispose();
            dialog.SetCanceledOnTouchOutside(true);
            dialog.CancelEvent += (o, e) => taskCompletionSource.TrySetResult(null);
            dialog.Show();

            if (message != null)
            {
                var textView = new TextView(_currentActivity)
                {
                    Text = message
                };
                var horizontalPadding = (int)(16 * ScaleManager.AndroidDensity);
                textView.SetPadding(horizontalPadding, 0, horizontalPadding, 0);
                dialog.ListView.AddHeaderView(textView, null, false);
            }

            //if (cancel != null)
            //    dialog.GetButton((int)DialogButtonType.Positive).SetTextColor(Colors.TextDefault.ToAndroid());
            //if (destruction != null)
            //    dialog.GetButton((int)DialogButtonType.Negative).SetTextColor(Colors.TextCaution.ToAndroid());

            return taskCompletionSource.Task;
        }

        private class CustomArrayAdapter : ArrayAdapter<string>
        {
            const int resourceId = global::Android.Resource.Layout.SimpleListItem1;
            DialogItem[] _items;
            public CustomArrayAdapter(Context context, DialogItem[] items) : base(context, resourceId, items.Select((arg) => arg.Text).ToArray())
            {
                _items = items;
            }

            public override global::Android.Views.View GetView(int position, global::Android.Views.View convertView, ViewGroup parent)
            {
                var view = base.GetView(position, convertView, parent);
                if (view is TextView textView)
                {
                    //if (_items[position].Style == DialogItem.ItemStyle.Destructive)
                    //    textView.SetTextColor(ColorResources.RedTextColor.ToAndroid());
                    //else
                    //    textView.SetTextColor(ColorResources.DefaultTextColor.ToAndroid());

                    return textView;
                }
                else
                    return view;
            }
        }
    }
}
