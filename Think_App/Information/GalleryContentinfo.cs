using System;
using Xamarin.Forms;
namespace Think_App
{
	public class GalleryContentinfo
	{
		public GalleryContentinfo(ImageSource source)
		{
			Source = source;
		}
		public ImageSource Source { get; set; }
	}
}
