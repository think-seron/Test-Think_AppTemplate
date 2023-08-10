using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Think_App
{
    public partial class ConfigList : ContentView
    {
        ConfigListVIewModel viewModel;
        public ConfigList()
        {
            InitializeComponent();

            ConfigListView.Footer = null;
        }
    }
}
