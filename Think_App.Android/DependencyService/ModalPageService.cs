using System;
using System.Reflection;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Content;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform;

[assembly: Dependency(typeof(Think_App.Droid.ModalPageService))]
namespace Think_App.Droid
{
	public class ModalPageService : IModalPageService
	{

		/// <summary>
		/// Gets the size of the screen.
		/// </summary>
		/// <returns>The screen size.</returns>
		public Size ModalGetScreenSize()
		{
			var metrics = Forms.Context.Resources.DisplayMetrics;
			var widthInDp = ModalConvertPixelsToDp(metrics.WidthPixels);
			var heightInDp = ModalConvertPixelsToDp(metrics.HeightPixels);

			return new Size(widthInDp, heightInDp);
		}


		public bool ModalControlAnimatesItself
		{
			get
			{
				return true;
			}
		}



		/// <summary>
		/// Converts the pixels to dp.
		/// </summary>
		/// <returns>The pixels to dp.</returns>
		/// <param name="pixelValue">Pixel value.</param>
		private int ModalConvertPixelsToDp(float pixelValue)
		{
			var dp = (int)((pixelValue) / Forms.Context.Resources.DisplayMetrics.Density);
			return dp;
		}

	}

	/// <summary>
	/// Droid card page navigation page.
	/// </summary>
	public class DroidModalPageNavigationPage : NavigationPage
	{
		public DroidModalPageNavigationPage()
		{

		}

		public DroidModalPageNavigationPage(Microsoft.Maui.Controls.Page rootPage) : base(rootPage)
		{

		}
	}
}
