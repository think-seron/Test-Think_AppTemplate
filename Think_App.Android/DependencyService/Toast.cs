using System;
using Android.Widget;

[assembly: Xamarin.Forms.Dependency(typeof(Think_App.Droid.Toast))]
namespace Think_App.Droid
{
	public class Toast : IToast
	{
		public void Show(string message)
		{
			var ts = Android.Widget.Toast.MakeText(Android.App.Application.Context, message, ToastLength.Short);
			ts.SetGravity(Android.Views.GravityFlags.Top, 0, 185);
			ts.Show();
		}
	}
}
