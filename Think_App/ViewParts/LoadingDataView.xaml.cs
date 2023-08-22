using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
    public partial class LoadingDataView : ContentView
    {
        public int ContentsCount { get; set; }
        private int _LoadedCountesCount = 0;
        private double _rate = 0.0;

        public LoadingDataView()
        {
            InitializeComponent();
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            if (width > 0 && height > 0)
            {
                // 初回更新(横サイズが決まらないと更新できないためここで行う)
                UpdateProgressText();
                UpdateProgressBar();
            }
        }

        public void LoadContents(int count)
        {
            _LoadedCountesCount += count;
            if (_LoadedCountesCount > ContentsCount) { _LoadedCountesCount = ContentsCount; }
            _rate = _LoadedCountesCount / (double)ContentsCount;

            UpdateProgressText();
            UpdateProgressBar();
        }

        void UpdateProgressText()
        {
            // 割合をパーセント変換して文字列にして、表示更新。
          Device.BeginInvokeOnMainThread(()=> { this.ProgressRateLbl.Text = _rate.ToString("P0"); });
        }

        void UpdateProgressBar()
        {
            var baseWidth = this.ProgressBaseBoxView.Width;
            var width = baseWidth * _rate;

            this.ProgressBoxView.WidthRequest = width;
        }
    }
}
