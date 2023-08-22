using System;
using System.Collections.Generic;
using AssetsLibrary;
using Foundation;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App.iOS
{
	/// <summary>
	/// Assets library manager.シングルトンです。
	/// </summary>
	public static class AssetsLibraryManager
	{
		static ALAssetsLibrary AssetsLibrary;
		public static List<ALAsset> Assets { get; private set; }

		public static void LoadAssetsLibrary()
		{
			var status = ALAssetsLibrary.AuthorizationStatus;
			if (status == ALAuthorizationStatus.NotDetermined ||
			   status == ALAuthorizationStatus.Authorized)
			{
				// 読み込みが許可されているか、読み込み許可待ちの状態。
				if (AssetsLibrary == null)
				{
					AssetsLibrary = new ALAssetsLibrary();
				}

				if (Assets == null)
				{
					Assets = new List<ALAsset>();
				}
				else
				{
					Assets.Clear();
				}

				AssetsLibrary.Enumerate(ALAssetsGroupType.SavedPhotos, (ALAssetsGroup group, ref bool _) =>
				{
					// 各グループから全件取得します。
					if (group != null)
					{
						// 画像のみに絞り込み。
						group.SetAssetsFilter(ALAssetsFilter.AllPhotos);
						group.Enumerate((ALAsset result, nint index, ref bool __) =>
						{
							if (result != null)
							{
								// アセットを追加。
								Assets.Add(result);
							}
							else
							{
								// 全件読み込みが終了。
								System.Diagnostics.Debug.WriteLine("読み込みを終了しました。");
								MessagingCenter.Send(new AseetsLibraryPayload { IsSucceeded = true }, "");
							}
						});
					}
				}, (NSError obj) =>
				{
					// 読み込み失敗。
					System.Diagnostics.Debug.WriteLine("読み込みに失敗しました。");
					System.Diagnostics.Debug.WriteLine(obj);
					MessagingCenter.Send(new AseetsLibraryPayload { IsSucceeded = false }, "");
				});
			}
			else
			{
				// アクセス許可なし。失敗。
				System.Diagnostics.Debug.WriteLine("アクセス許可がありません。");
				MessagingCenter.Send(new AseetsLibraryPayload { IsSucceeded = false }, "");
			}
		}
	}
}
