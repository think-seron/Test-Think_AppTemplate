using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Think_App
{
	public interface IImageService
	{
		Task<ImageSource> ResizeAsync(ImageSource source, double width, double height);
		Task<byte[]> ConvertImageSourceToBytesAsync(ImageSource source, bool resize = false, double minLength = 0);
		Task<byte[]> ConvertImageSourceToBytesWithCombining(ImageSource srcSource, ImageSource dstSource, Rectangle dstRect, Size viewSize, Aspect aspect, bool resize = false, double minLength = 0);
		ImageSource ConvertBytesToImageSource(byte[] bytes);
		Task<Size> GetImageSizeAsync(ImageSource source);
		Task<Rectangle?> GetFaceRangeFromImageSource(ImageSource source, double viewWidth, double viewHeight, Aspect aspect);
		ImageSource GetOrientationAdjustedImageSource(string filePath, bool resize = false, double minLength = 0);
		Task<ImageSource> CloneImageSourceAsync(ImageSource source);
		Task<ImageSource> GetCroppedImageSourceAsync(ImageSource source, Rectangle croppingRect);

		//androidの画像読み込み時のoutOfMemory対策用  iosはGetOrientationAdjustedImageSourceをそのまま使用中
		ImageSource GetOrientationAdjustedImageSourceReduction(string filePath, bool resize = false, double minLength = 0);
		ImageSource ResizeNetImage(string filePath);
	}
}
