using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
	public partial class HistoryTop : ContentPage
	{
		HistoryTopViewModel vm;
		public HistoryTop(bool bilogAvailable)
		{
			InitializeComponent();
			NavigationPage.SetBackButtonTitle(this, "");
			vm = new HistoryTopViewModel(bilogAvailable);
			this.BindingContext = vm;
			App.HistoryTopId = this.Id;
		}
	}
}
