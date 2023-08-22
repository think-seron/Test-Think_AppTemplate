using System;
using System.Collections.Generic;
using IO.Swagger.Model;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
	public partial class ReservationRegist : ContentPage
	{
		ReservationRegistViewModel vm;
		public ReservationRegist(ReservationContentInfo content, ResponseReservationPoint response)
		{
			InitializeComponent();
			NavigationPage.SetBackButtonTitle(this, "");
			vm = new ReservationRegistViewModel(content, response);
			App.ReservationDetailId = this.Id;
			this.BindingContext = vm;
		}
	}
}
