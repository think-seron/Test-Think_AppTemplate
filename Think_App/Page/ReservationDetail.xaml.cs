using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
	public partial class ReservationDetail : ContentPage
	{
		ReservationDetailViewModel vm;
		public ReservationDetail(IO.Swagger.Model.ReservationDetail data, Guid reservationTopPageId)
		{
			InitializeComponent();
			NavigationPage.SetBackButtonTitle(this, "");
			vm = new ReservationDetailViewModel(data, reservationTopPageId);
			this.BindingContext = vm;
		}
	}
}
