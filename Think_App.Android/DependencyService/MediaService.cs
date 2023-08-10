using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Android.OS;
using Android.App;
using Android.Content;
using Android.Provider;
using Java.IO;
using Android.Media;
using Android.Net;

using Android.Graphics;


[assembly: Dependency(typeof(Think_App.Droid.MediaService))]
namespace Think_App.Droid
{
	public class MediaService: MainActivity, IMediaService
	{
		public MediaService()
		{
		}

		public List<string> GetImagePass()
		{
			//ContentResolver cr = ContentResolver;

			String[] projection =
			{
				MediaStore.Images.Media.InterfaceConsts.Data,
				MediaStore.Images.Media.InterfaceConsts.DisplayName
			};

			//var cursor = cr.Query(MediaStore.Images.Media.ExternalContentUri, projection, null, null, null);
			var cursor = Forms.Context.ContentResolver.Query(MediaStore.Images.Media.ExternalContentUri, projection, null, null, null);

			if (cursor == null)
			{
				return null;
			}

			int pathIndex = cursor.GetColumnIndex(MediaStore.Images.Media.InterfaceConsts.Data);
			int nameIndex = cursor.GetColumnIndex(MediaStore.Images.Media.InterfaceConsts.DisplayName);

			List<string> path = new List<string>();

			cursor.MoveToFirst();
			while (!cursor.IsAfterLast)
			{
				System.Diagnostics.Debug.WriteLine("@@@@@@@:" + cursor.GetString(pathIndex));
				//System.Diagnostics.Debug.WriteLine("@@@@@@@:" + cursor.GetString(nameIndex));
				path.Add(cursor.GetString(pathIndex));
				cursor.MoveToNext();
			}

			return path;
		}


		public byte[] PathChangeByte(string path)
		{
			ExifInterface exifInterface = null;
			try {
		        exifInterface = new ExifInterface(path);
			} catch (System.IO.IOException e) {
		        return null;
		    }

			// ----------------画像の向きを調整---------------
			var matrix = new Matrix();
		    // 画像の向きを取得
			int orientation = exifInterface.GetAttributeInt(ExifInterface.TagOrientation,(int)Android.Media.Orientation.Undefined);

		    // 画像を回転させる処理をマトリクスに追加
		    switch (orientation) {
		        case (int)Android.Media.Orientation.Undefined:
		            break;
				case (int)Android.Media.Orientation.Normal:
		            break;
				case (int)Android.Media.Orientation.FlipHorizontal:
		            // 水平方向にリフレクト
					matrix.PostScale(-1f, 1f);
		            break;
				case (int)Android.Media.Orientation.Rotate180:
		            // 180度回転
					matrix.PostRotate(180f);
		            break;
				case (int)Android.Media.Orientation.FlipVertical:
		            // 垂直方向にリフレクト
					matrix.PostScale(1f, -1f);
		            break;
				case (int)Android.Media.Orientation.Rotate90:
		            // 反時計回り90度回転
					matrix.PostRotate(90f);
		            break;
				case (int)Android.Media.Orientation.Transverse:
		            // 時計回り90度回転し、垂直方向にリフレクト
					matrix.PostRotate(-90f);
					matrix.PostScale(1f, -1f);
		            break;
				case (int)Android.Media.Orientation.Transpose:
		            // 反時計回り90度回転し、垂直方向にリフレクト
					matrix.PostRotate(90f);
					matrix.PostScale(1f, -1f);
		            break;
				case (int)Android.Media.Orientation.Rotate270:
		            // 反時計回りに270度回転（時計回りに90度回転）
					matrix.PostRotate(-90f);
		            break;
		    }
			//----------------------------------------------
		    
			// ----------------bitmapに変換してそれをbyteに変換--------------
			Bitmap bm = BitmapFactory.DecodeFile(path);
			//int width = bm.Width / 2;
			//int height = bm.Height / 2;
			//var resizedBitmap = Bitmap.CreateScaledBitmap(bm, width, height, false);

			int width = bm.Width;
			int height = bm.Height;
			matrix.PostScale(0.5f, 0.5f);  // 元画像の半分の大きさに
			var resizedBitmap = Bitmap.CreateBitmap(bm, 0, 0, width, height, matrix, true); 

			var stream = new MemoryStream();

			if (path.IndexOf(".jpeg") != -1)
			{
				resizedBitmap.Compress(Bitmap.CompressFormat.Jpeg, 100, stream);
			}
			else
			{
				resizedBitmap.Compress(Bitmap.CompressFormat.Png, 100, stream);
			}

			// メモリ解放
			bm.Recycle();
			bm = null;
			resizedBitmap.Recycle();
			resizedBitmap = null;
			// -----------------------------------------------------------

			return stream.ToArray();
		}

		public void GetImageByte()
		{

		}



		/*
		private Action<int, Result, Intent> ResultCallback;
		private TaskCompletionSource<List<string>> TaskCmpSrc;

		public async Task<List<string>> SelectPhoto(bool multiSelect)
		{
			TaskCmpSrc = new TaskCompletionSource<List<string>>();

			var intent = new Intent(Intent.ExtraAllowMultiple, MediaStore.Images.Media.ExternalContentUri);
			intent.SetType("image/*");
			intent.PutExtra(Intent.ExtraAllowMultiple, multiSelect);
			intent.SetAction(Intent.ActionPick);

			//var appInfo = GetPhotoInfo(intent);

			//if (appInfo == null)
			//	return null;

			//intent.SetComponent(new ComponentName(appInfo.ActivityInfo.PackageName, appInfo.ActivityInfo.Name));
			//((MainActivity)(Forms.Context)).StartActivityForResult(Intent.CreateChooser(intent, "画像を選択"), (int)IntentCode.MediaPicker);
			((MainActivity)(Forms.Context)).StartActivity(intent, OnActivityResult);
			return await TaskCmpSrc.Task;
		}

		private void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult(requestCode, resultCode, data);
			var result = new List<string>();

			if (resultCode == Result.Ok)
			{
				switch (requestCode)
				{
					case (int)IntentCode.MediaPicker:
						if (data.Data != null)
						{
							var url = GetFileUri(data.Data);
							if (!string.IsNullOrEmpty(url))
							{
								result.Add(url);
							}
						}
						else if (data.ClipData != null)
						{
							for (int i = 0; i < data.ClipData.ItemCount; i++)
							{
								string fileName = null;
								var item = data.ClipData.GetItemAt(i);

								var url = GetFileUri(item.Uri);
								if (!string.IsNullOrEmpty(url))
								{
								result.Add(url);
								}
							}
						}
						break;
				}
			}
			else if (resultCode == Result.Canceled)
			{

			}
			else
			{

			}
			TaskCmpSrc.SetResult(result);
		}

		private string GetFileUri(Android.Net.Uri intentUri)
		{
			// ファイルパス取得
			Android.Net.Uri uri;
			string selection;
			string[] selectionArgs;
			if (Build.VERSION.SdkInt >= BuildVersionCodes.Kitkat)
			{
				uri = MediaStore.Images.Media.ExternalContentUri;
				selection = "_id=?";
				try
				{
					var docID = DocumentsContract.GetDocumentId(intentUri);
					var docSplit = docID.Split(':');

					selectionArgs = new string[] { docSplit[docSplit.Length - 1] };
				}
				catch
				{
					selectionArgs = new string[] { intentUri.LastPathSegment };
				}

			}
			else
			{
				uri = intentUri;
				selection = null;
				selectionArgs = null;

			}
			var projection = new string[] { MediaStore.Images.ImageColumns.Data };

			var cursor = ((MainActivity)(Forms.Context)).ContentResolver.Query(uri, projection, selection, selectionArgs, null);
			if (cursor != null)
			{
				if (cursor.MoveToFirst())
				{
					var idx = cursor.GetColumnIndex(MediaStore.Images.ImageColumns.Data);
					return cursor.GetString(idx);
				}
			}
			return null;
		}

		public bool HasphotosConmponent()
		{
			var intent = new Intent(Intent.ExtraAllowMultiple, MediaStore.Images.Media.ExternalContentUri);
			intent.SetType("image/*");
			intent.PutExtra(Intent.ExtraAllowMultiple, true);
			intent.SetAction(Intent.ActionGetContent);

			using (var info = GetPhotoInfo(intent))
			{
				if (info == null)
				{
					return false;
				}
				else
				{
					return true;
				}
			}
		}

		private Android.Content.PM.ResolveInfo GetPhotoInfo(Intent intent)
		{
			return (Android.Content.PM.ResolveInfo)Forms.Context.PackageManager.QueryIntentActivities(intent, 0).Where((arg) => arg.ActivityInfo.TargetActivity == "com.google.android.apps.photos.phone.GetContentActivity").FirstOrDefault();
		}
		*/
	}
}
