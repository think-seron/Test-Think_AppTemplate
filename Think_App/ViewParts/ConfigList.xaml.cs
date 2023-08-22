using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

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
