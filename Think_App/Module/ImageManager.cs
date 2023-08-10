using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Think_App
{
	public static class ImageManager
	{
		public static async Task SaveImageToLocalStorageAsync(ImageSource source, string foldername, string filename, bool replace = true)
		{
			// ImageSource -> bytes
			var service = DependencyService.Get<IImageService>();
			var bytes = await service.ConvertImageSourceToBytesAsync(source);

			// ストレージに保存
			await StorageManager.UserBytesDataWriteAsync(foldername, filename, bytes, replace);
			System.Diagnostics.Debug.WriteLine("ファイルを保存:{0}", filename);
		}

		public static async Task SaveCombinedImageToLocalStorageAsync(ImageSource srcSource, ImageSource dstSource, Rectangle dstRect, Size viewSize, Aspect aspect, string foldername, string filename, bool replace = true, bool resize = false, double minLength = 0)
		{
			// 2 ImageSources -> bytes
			var service = DependencyService.Get<IImageService>();
			var bytes = await service.ConvertImageSourceToBytesWithCombining(srcSource, dstSource, dstRect, viewSize, aspect, resize, minLength);

			// ストレージに保存
			await StorageManager.UserBytesDataWriteAsync(foldername, filename, bytes, replace);
			System.Diagnostics.Debug.WriteLine("合成ファイルを保存:{0}", filename);
		}

		public static async Task<bool> IsExistImageInLocalStorageByUrlAsync(string url, bool isThumbnail)
		{
			var uri = new Uri(url);
			var foldername = isThumbnail ? ConstantManager.FolderName_HairThumbnailImages : ConstantManager.FolderName_HairImages;
			var filename = Path.GetFileName(uri.LocalPath);
			var status = await StorageManager.UserDataCheckExistAsync(foldername, filename);
			if (status == StorageManager.ExistStatus.Exists)
			{
				System.Diagnostics.Debug.WriteLine("ファイルが存在:{0}", filename);
				return true;
			}
			return false;
		}

		public static async Task SaveImageToLocalStorageByUrlAsync(string url, bool isThumbnail = false, bool replace = false, bool resize = false, double minLength = 0)
		{
			var uri = new Uri(url);
			var foldername = isThumbnail ? ConstantManager.FolderName_HairThumbnailImages : ConstantManager.FolderName_HairImages;
			var filename = Path.GetFileName(uri.LocalPath);

			if (!replace)
			{
				// 上書きしない場合は、ファイルの存在確認をする。
				var status = await StorageManager.UserDataCheckExistAsync(foldername, filename);
				if (status == StorageManager.ExistStatus.Exists)
				{
					// ファイルが確実に存在しているときは返す
					System.Diagnostics.Debug.WriteLine("ファイルが存在:{0}", filename);
					return;
				}
			}

			var imageSource = ImageSource.FromUri(uri);

			// ImageSource -> bytes
			var service = DependencyService.Get<IImageService>();
			var bytes = await service.ConvertImageSourceToBytesAsync(imageSource, resize, minLength);

			// ストレージに保存
			await StorageManager.UserBytesDataWriteAsync(foldername, filename, bytes);
			System.Diagnostics.Debug.WriteLine("ファイルを保存:{0}", filename);
		}

		public static async Task<ImageSource> LoadImageFromLocalStorageAsync(string foldername, string filename)
		{
			// ストレージから読み込み
			var bytes = await StorageManager.UserBytesDataReadAsync(foldername, filename);

			if (bytes == null || bytes.Length == 0)
			{
				return null;
			}

			// bytes -> ImageSource
			var service = DependencyService.Get<IImageService>();
			var imageSource = service.ConvertBytesToImageSource(bytes);

			return imageSource;
		}

		public static async Task<ImageSource> LoadImageFromLocalStorageByUrlAsync(string url, bool isThumbnail = false)
		{
			var uri = new Uri(url);
			var foldername = isThumbnail ? ConstantManager.FolderName_HairThumbnailImages : ConstantManager.FolderName_HairImages;
			var filename = Path.GetFileName(uri.LocalPath);

			// ストレージから読み込み
			var bytes = await StorageManager.UserBytesDataReadAsync(foldername, filename);

			if (bytes == null || bytes.Length == 0)
			{
				return null;
			}

			// bytes -> ImageSource
			var service = DependencyService.Get<IImageService>();
			var imageSource = service.ConvertBytesToImageSource(bytes);

			return imageSource;
		}

		public static async Task<string> GetLocalStorageFullPathByUrlAsync(string url, bool isThumbnail = false)
		{
			var uri = new Uri(url);
			var foldername = isThumbnail ? ConstantManager.FolderName_HairThumbnailImages : ConstantManager.FolderName_HairImages;
			var filename = Path.GetFileName(uri.LocalPath);

			var path = await StorageManager.GetFullPath(foldername, filename);

			// ファイルが存在していなければ、nullが返ります。
			return path;
		}
	}
}
