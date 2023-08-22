using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
    public partial class GallerySeletedView : ContentView
    {
        public event EventHandler Canceled = delegate { };
        public event EventHandler<ImageDecidedEventArgs> Decided = delegate { };

        public GallerySeletedView(ImageSource source)
        {
            InitializeComponent();

            var service = DependencyService.Get<IScreenService>();
            var scale = service.GetScreenScale();

            this.Image.Source = source;
            this.Image.DownsampleWidth = this.Image.WidthRequest * scale;
            this.Image.DownsampleHeight = this.Image.HeightRequest * scale;
            this.Image.CacheType = null;

            this.CancelBtn.Clicked += (sender, e) =>
            {
                if (Canceled != null)
                {
                    Canceled(this, EventArgs.Empty);
                }
            };

            this.OKBtn.Clicked += (sender, e) =>
            {
                if (Decided != null)
                {
                    Decided(this, new ImageDecidedEventArgs() { ImageSource = source });
                }
            };
        }
    }

    public class ImageDecidedEventArgs : EventArgs
    {
        public ImageSource ImageSource { get; set; }
    }
}
