using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
    public partial class CameraControllerView : ContentView
    {
        public event EventHandler TakePhotoButtonClicked = delegate { };
        public event EventHandler SwitchCameraButtonClicked = delegate { };

        public CameraControllerView()
        {
            InitializeComponent();

            this.TakePhotoBtn.Clicked += (sender, e) =>
            {
                if (TakePhotoButtonClicked != null)
                {
                    TakePhotoButtonClicked(this, EventArgs.Empty);
                }
            };

            this.SwitchCameraBtn.Clicked += (sender, e) =>
            {
                if (SwitchCameraButtonClicked != null)
                {
                    SwitchCameraButtonClicked(this, EventArgs.Empty);
                }
            };
        }
    }
}
