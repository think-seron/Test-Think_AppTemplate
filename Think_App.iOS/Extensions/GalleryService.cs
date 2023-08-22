using System;
using Think_App.iOS;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

[assembly: Dependency(typeof(GalleryService))]
namespace Think_App.iOS
{
	public class GalleryService : IGalleryService
	{
		public event EventHandler<LoadingImageDataFinishedEventArgs> LoadingImageDataFinished = delegate {};

		public void LoadAllImage()
		{
			MessagingCenter.Subscribe<AseetsLibraryPayload>(this, "", (p) =>
			{
				// ロード完了イベントを通知する。
				if (LoadingImageDataFinished != null)
				{
                    LoadingImageDataFinished(this, new LoadingImageDataFinishedEventArgs() { IsSucceeded = p.IsSucceeded });
				}
				MessagingCenter.Unsubscribe<AseetsLibraryPayload>(this, "");
			});

			// ロード。
			AssetsLibraryManager.LoadAssetsLibrary();
		}
	}
}
