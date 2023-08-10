using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Think_App
{
    public partial class GalleryViewCell : ViewCell
    {
        public GalleryViewCell()
        {
            InitializeComponent();

            // キャッシュしない。
            this.ThumbnailImg.CacheType = null;
        }
    }
}
