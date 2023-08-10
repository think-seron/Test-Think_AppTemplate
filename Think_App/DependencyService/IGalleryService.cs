using System;
namespace Think_App
{
	public interface IGalleryService
	{
		event EventHandler<LoadingImageDataFinishedEventArgs> LoadingImageDataFinished;

		void LoadAllImage();
	}

	public class LoadingImageDataFinishedEventArgs : EventArgs
	{
		public bool IsSucceeded { get; set; }
	}
}
