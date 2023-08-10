using System;
using Xamarin.Forms;
using Think_App.Droid;

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
