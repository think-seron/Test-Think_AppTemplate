using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
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
