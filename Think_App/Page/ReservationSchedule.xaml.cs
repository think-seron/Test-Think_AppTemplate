using System;
using System.Collections.Generic;

using Xamarin.Forms;
using IO.Swagger.Model;
//using Xamarin.Forms.Xaml;

namespace Think_App
{
	public partial class ReservationSchedule : ContentPage
	{

		//ReservationScheduleViewModel vm;
		ReservationScheduleViewModel vm;

		public ReservationSchedule(ResponseReservationSchedule response, ReservationContentInfo content)
		{
			InitializeComponent();

			NavigationPage.SetBackButtonTitle(this, "");

			vm = new ReservationScheduleViewModel(response, content);

			this.BindingContext = vm;
		}
	}
}
