﻿using System;
using Xamarin.Forms;
using UIKit;
using System.Threading.Tasks;
using CoreGraphics;
using System.Collections.Generic;
using System.Reflection;
using Xamarin.Forms.Platform.iOS;
using Foundation;

[assembly: Dependency(typeof(Think_App.iOS.ModalPageService))]
namespace Think_App.iOS
{
	public class ModalPageService : IModalPageService
	{

		/// <summary>
		/// Gets the size of the screen.
		/// </summary>
		/// <returns>The screen size.</returns>
		public Size ModalGetScreenSize()
		{
			return new Size(UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height);
		}


		public bool ModalControlAnimatesItself { get { return true; } }

	}
}
