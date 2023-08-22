using System;
using System.Collections.Generic;
using IO.Swagger.Model;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
    public partial class ReservationMenuList : ContentPage
    {

        ReservationMenuListViewModel vm;
        public ReservationMenuList(ResponseReservationMenu response, ReservationContentInfo content)
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "");
            vm = new ReservationMenuListViewModel(response, content);

            this.BindingContext = vm;

            GC.Collect();
        }
    }
}
