using System;
using System.Net;
using System.Net.Http;
using DLToolkit.Forms.Controls;
using FFImageLoading;
using FFImageLoading.Config;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
    public partial class App : Application
    {
        public static ProcessManager ProcessManager;
        public static CustomNavigationPage customNavigationPage;
        public static Guid HomeId;
        public static Guid ReservationDetailId;
        public static Guid HistoryTopId;
        public App(double width, double height, double androidDensity = 1.0)
        {
            InitializeComponent();
            Plugin.Media.CrossMedia.Current.Initialize();
            FlowListView.Init();
            ProcessManager = new ProcessManager();
            FFimageInit();
            ScaleManager.AndroidDensity = androidDensity;

            ScaleManager.ScreenWidth = width;
            ScaleManager.ScreenHeight = height;
            System.Diagnostics.Debug.WriteLine("W:" + width + "  H:" + height);
            //MainPage = new MainPage();
            customNavigationPage = new CustomNavigationPage(new LaunchingPageManager());

            MainPage = customNavigationPage;
        }

        private void FFimageInit()
        {
            ImageService.Instance.Initialize(new Configuration
            {
                HttpClient = new HttpClient(new HttpClientHandler()
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, sslErrors) => true
                })
            });
        }

        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
