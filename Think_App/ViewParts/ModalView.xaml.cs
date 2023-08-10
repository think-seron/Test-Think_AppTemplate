using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Think_App
{
    public partial class ModalView : ContentView
    {
        public ModalViewViewModel modalViewViewModel { get; set; }
        public Button okButton { get { return OKButton; } }
        public Button yesButton { get { return YesButton; } }
        public Button noButton { get { return NoButton; } }
        public ModalView()
        {
            InitializeComponent();

            modalViewViewModel = new ModalViewViewModel();
            this.BindingContext = modalViewViewModel;
            HeightRequest = ScaleManager.ScreenHeight;
            WidthRequest = ScaleManager.ScreenWidth;
        }
    }
}
