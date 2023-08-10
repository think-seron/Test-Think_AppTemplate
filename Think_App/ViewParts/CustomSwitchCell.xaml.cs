using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Think_App
{
    public partial class CustomSwitchCell : ContentView
    {
        public Switch nSwitch { get { return this.NSwitch; } }
        public CustomSwitchCell()
        {
            InitializeComponent();
        }
    }
}
