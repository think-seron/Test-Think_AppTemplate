using System;
using System.IO;
using System.Collections.Generic;
using Think_App.iOS;
using UIKit;
using Foundation;
using AssetsLibrary;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

[assembly: Dependency(typeof(Think_App.iOS.MediaService))]
namespace Think_App.iOS
{
	public class MediaService : IMediaService
	{
		List<string> pathList = new List<string>();
		//List<byte[]> imageBytes = new List<byte[]>();


		public List<string> GetImagePass()
		{
			return null;
		}

		public byte[] PathChangeByte(string path)
		{
			return null;
		}


		public void GetImageByte()
		{
			//var qqq = NSFileManager.DefaultManager.GetUrls(NSSearchPathDirectory.PicturesDirectory, NSSearchPathDomain.User)[0].Path;


			//NSBundle bundle = NSBundle.MainBundle;
			//string[] pathListJpeg = NSBundle.GetPathsForResources("jpeg", bundle.BundlePath);
			//string[] pathListPng = NSBundle.GetPathsForResources("png", bundle.BundlePath);
			//List<string> patpathh = new List<string>();
			//foreach (var val in pathListJpeg)
			//{
			//	path.Add(val);
			//}
			//foreach (var val in pathListPng)
			//{
			//	path.Add(val);
			//}
			//return path;

			ALAuthorizationStatus authorizationStatus = new ALAuthorizationStatus();
			if (authorizationStatus == ALAuthorizationStatus.NotDetermined || authorizationStatus == ALAuthorizationStatus.Authorized)
			{
				// 読み込みが許可されているか読み込み許可待ちの状態
				ALAssetsLibrary assetsLibrary = new ALAssetsLibrary();
				NSMutableArray assets = new NSMutableArray();

				assetsLibrary.Enumerate(ALAssetsGroupType.SavedPhotos, GroupEnumerator, Console.WriteLine);
				//assetsLibrary.Enumerate(ALAssetsGroupType.Library, GroupEnumerator, Console.WriteLine);
				//assetsLibrary.Enumerate(ALAssetsGroupType.Album, GroupEnumerator, Console.WriteLine);
				//assetsLibrary.Enumerate(ALAssetsGroupType.All, GroupsEnumerator, (NSError e) => { Console.WriteLine ("Could not enumerate albums: " + e.LocalizedDescription); });
			}

		}

		protected void GroupEnumerator(ALAssetsGroup group, ref bool stop)
		{
			if (group == null)
			{
				stop = true;
			}
			else
			{
				group.SetAssetsFilter(ALAssetsFilter.AllPhotos);
				group.Enumerate(AssetEnumerator);
			}
		}

		private void AssetEnumerator(ALAsset asset, nint index, ref bool shouldStop)
		{
			if (asset == null)
			{
				shouldStop = true;
				//Think_App.GalleryListPage.pathListEndFlg = shouldStop;
				return;
			}
			if (!shouldStop)
			{
				var reseption = asset.DefaultRepresentation;
				//var uiImage = UIImage.FromImage(reseption.GetFullScreenImage(), reseption.Scale, UIImageOrientation.Up);
				var uiImage = UIImage.FromImage(reseption.GetFullScreenImage(), (nfloat)0.3, UIImageOrientation.Up);
				//uiImage.na
				byte[] bytes = UiimageToByteArray(uiImage);
				//imageBytes.Add(bytes);
				//Think_App.GalleryListPage.pathList.Add(bytes);

				pathList.Add(asset.AssetUrl.AbsoluteString);
				System.Diagnostics.Debug.WriteLine(String.Format("Item[{0}] : {1}", index, asset.ToString()));
				shouldStop = false;
			}
		}

		byte[] UiimageToByteArray(UIImage image)
		{
			byte[] bytes = null;
			try
			{
				using (var data = image.AsPNG())
				{
					bytes = data.ToArray();
				}
			}
			catch (Exception)
			{
				bytes = null;
			}

			return bytes;
		}
	}
}
