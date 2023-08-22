using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
	public partial class SaveCompletedView : ContentView
	{
		public event EventHandler SendMessageButtonClicled = delegate { };
		public event EventHandler EndButtonClicked = delegate { };

		public SaveCompletedView()
		{
			InitializeComponent();

			this.SendMessageBtn.Clicked += (sender, e) =>
			{
				if (SendMessageButtonClicled != null)
				{
					SendMessageButtonClicled(this, EventArgs.Empty);
				}
			};

			this.EndBtn.Clicked += (sender, e) =>
			{
				if (EndButtonClicked != null)
				{
					EndButtonClicked(this, EventArgs.Empty);
				}
			};
		}
	}
}
