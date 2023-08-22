using System;
using Think_App.Droid;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

[assembly: Dependency(typeof(GalleryService))]
namespace Think_App.Droid
{
	public class GalleryService : IGalleryService
	{
		public event EventHandler<LoadingImageDataFinishedEventArgs> LoadingImageDataFinished = delegate {};

		public void LoadAllImage()
		{
			throw new NotImplementedException();
		}
	}
}
