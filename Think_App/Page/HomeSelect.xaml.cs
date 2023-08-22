using System;
using System.Collections.Generic;
using IO.Swagger.Model;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
    public partial class HomeSelect : ContentPage
    {

        HomeSelectViewModel vm;
        public HomeSelect(ResponseFavoriteSalonList response, int homeSalonId)
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "");
            vm = new HomeSelectViewModel(response, homeSalonId);
            this.BindingContext = vm;

        }
    }
}
