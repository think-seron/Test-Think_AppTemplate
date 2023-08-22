using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
    public partial class MaintenanceView : ContentView
    {
        public MaintenanceView(string message)
        {
            InitializeComponent();
            Message.Text = message;
            HeightRequest = ScaleManager.ScreenHeight;
            WidthRequest = ScaleManager.ScreenWidth;
        }
    }
}
