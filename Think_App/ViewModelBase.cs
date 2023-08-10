using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace Think_App
{
	public class ViewModelBase : INotifyPropertyChanged
	{
		public ViewModelBase() {
			guid = Guid.NewGuid();
		}

		public Guid guid { get; set; }
		public event PropertyChangedEventHandler PropertyChanged = delegate {};

		protected void SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
		{
			if (object.Equals(storage, value))
			{
				return;
			}
			storage = value;
			OnPropertyChanged(propertyName);
		}

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		public static async void ScreenTransition(Xamarin.Forms.Page page =null)
		{
			if (page == null)
			{
			   if (App.ProcessManager.CanInvoke())
			   {
					await App.customNavigationPage.PopAsync();
				   App.ProcessManager.OnComplete();
			   }
			}
			else
			{
			   if (App.ProcessManager.CanInvoke())
			   {
					await App.customNavigationPage.PushAsync(page);
				   App.ProcessManager.OnComplete();
			   }
			}
		}
	}
}
