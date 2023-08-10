using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Think_App
{
    public partial class CustomNavigationBar : ContentView
    {

        public ProgressBar NaviProgressBar
        {
            get { return this.progressBar; }
        }
        public CustomNavigationBar()
        {
            InitializeComponent();
            if (Device.RuntimePlatform == Device.iOS)
            {
                //this.progressBar.Progress = 20;
            }
            this.progressBar.SizeChanged += (sender, e) => {
                System.Diagnostics.Debug.WriteLine("progressbar width :" + this.progressBar.Width);
                System.Diagnostics.Debug.WriteLine("progressbar height :" + this.progressBar.Height);
            };

        }


        //xaml定義
        void LeftImageTapped(object sender, EventArgs e)
        {
            TapEffect(LeftImage);
        }

        //xaml定義
        void RightTextTapped(object sender, EventArgs e)
        {
            TapEffect(RightText);
        }

        //xaml定義
        void RightImageTapped(object sender, EventArgs e)
        {
            TapEffect(RightImageGroup);
        }

        void TapEffect(View view)
        {
            view.Opacity = 0.6;
            view.FadeTo(1);
        }
    }
}
