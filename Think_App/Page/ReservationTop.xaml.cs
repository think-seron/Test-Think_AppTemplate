using System;
using System.Collections.Generic;
using IO.Swagger.Model;
using Xamarin.Forms;

namespace Think_App
{
    public partial class ReservationTop : ContentPage
    {

        ReservationTopViewModel ThisVM;
        public ReservationTop(ResponseHome data)
        //(List<InlineResponse2002DataReservationList> reservationList)
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "");
            ThisVM = new ReservationTopViewModel(data, this.Id);
            this.BindingContext = ThisVM;
        }
    }
}
