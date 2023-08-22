using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
	public partial class HomeButtonView : ContentView
	{
		public HomeButtonView()
		{
			InitializeComponent();

			this.Batch.TranslationX = 3.5;
			this.Batch.TranslationY = -3.5;
			//this.btn.ImageLayoutPosition = CustomButton.LayoutPosition.Top;
			//this.btn.PropertyChanged += (sender, e) => {
			//	System.Diagnostics.Debug.WriteLine("proname :"+e.PropertyName);
			//};
			//this.SizeChanged += (sender, e) => {
			//	System.Diagnostics.Debug.WriteLine("height  :"+this.Height + "     width   :" + this.Width);
			//};
			//this.btn.SizeChanged += (sender, e) => {
			//	System.Diagnostics.Debug.WriteLine("btn height  :" + this.btn.Height + "     width   :" + this.btn.Width);
			//};
			//shadow.SizeChanged += (sender, e) => {
			//	System.Diagnostics.Debug.WriteLine("shadow size :" + this.shadow.Height);
			//};
		}
	}
}
