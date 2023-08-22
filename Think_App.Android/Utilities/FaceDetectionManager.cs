using System;
using Android.Graphics;
using Android.Media;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App.Droid
{
	public static class FaceDetectionManager
	{
		// 読み込む最大の顔の数
		const int _maxFaceNum = 3;

		public static Rect? GetFaceRange(Bitmap image, int index)
		{
			if (index >= _maxFaceNum)
			{
				System.Diagnostics.Debug.WriteLine("読み込める最大の顔の数を超えています。indexを小さくしてください。");
				return null;
			}

			// FaceDetector で検出するために、
			// * Bitmapは16bit
			// * 横幅は偶数であること
			// が必要なので、変換をかける。

			// Bitmap -> Bytes
			var bytes = image.ToBytes();

			// Bytes -> 16bit Bitmap
			var options = new BitmapFactory.Options() { InPreferredConfig = Bitmap.Config.Rgb565 };
			var bitmap = BitmapFactory.DecodeByteArray(bytes, 0, bytes.Length, options);

			// もしbitmapの幅が奇数なら+1してリサイズ。この程度なら、顔認証への影響はほぼないでしょう。
			var width = bitmap.Width;
			if (width % 2 == 1)
			{
				bitmap = Bitmap.CreateScaledBitmap(bitmap, width + 1, bitmap.Height, true);
			}

			var faces = new FaceDetector.Face[_maxFaceNum];

			var detector = new FaceDetector(bitmap.Width, bitmap.Height, _maxFaceNum);

			var num = detector.FindFaces(bitmap, faces);
			if (num == 0)
			{
				System.Diagnostics.Debug.WriteLine("顔が検出できませんでした。");
				return null;
			}

			System.Diagnostics.Debug.WriteLine("検出できた顔の数:{0}", num);
			if (num <= index)
			{
				System.Diagnostics.Debug.WriteLine("検出できた顔の数が足りません。indexを小さくしてください。");
				return null;
			}

			var face = faces[index];
			// faceからは
			// * 左右の目の中心点
			// * 左右の目の距離
			// しか取得できません。
			// ですので、以下の情報で、顔のサイズを推定します。

			// * 顔の縦の長さを考えたとき、中央の高さに両目がある。
			// * 顔を横に3分割したとき、頭頂から眉毛の上まで、眉毛から鼻の下まで、鼻の下からあごの先までがほぼ同じ長さである。
			// * 顔の横幅は、目の横幅の4倍から5倍である。
			// * 両目の間隔は、目の横幅に等しい。
			// * 耳の高さは、ほぼ鼻の下から目尻までである。
			// * 鼻の横幅は目の横幅にほぼ等しい。
			// * 口の横幅は二つの瞳の距離に等しい。

			var midPoint = new PointF();
			face.GetMidPoint(midPoint);

			// 顔の幅(推定)
			var faceWidth = face.EyesDistance() * 2;
			// 顔の高さ(推定)
			var faceHeight = faceWidth * 1.5;

			// 顔の左上の座標
			var x = midPoint.X - faceWidth / 2.0f;
			var y = midPoint.Y - faceHeight / 3.0f;

			// これを元にして、顔サイズを返す。
			return new Rect(x, y, faceWidth, faceHeight);
		}
	}
}
