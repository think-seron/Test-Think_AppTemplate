using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
    public class MediaPluginManager
    {
        public MediaPluginManager()
        {
            CrossMedia.Current.Initialize();
        }

        public async Task<bool> CheckCameraAvailable()
        {
            var permissionResult = await CheckPermission(Permission.Photos);

            if (!CrossMedia.Current.IsCameraAvailable || !permissionResult)
            {
                await App.Current.MainPage.DisplayAlert("カメラが利用できません", "設定情報などをご確認ください。", "OK");
                return false;
            }
            return true;
        }

        public async Task<Stream> TakePhotoAsync()
        {
            try
            {
                var photo = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    DefaultCamera = CameraDevice.Rear,
                });
                if (photo == null) photo = null;
                return photo.GetStream();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("TakePhotoAsync error :  " + ex);
                return null;
            }
        }

        public async Task<bool> PickPhotoAvailable()
        {
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await App.Current.MainPage.DisplayAlert("ライブラリから画像を取得できません", "この端末ではライブラリから画像を取得できません。", "OK");
                return false;
            }

            // TODO Xamarin.Forms.Device.RuntimePlatform is no longer supported. Use Microsoft.Maui.Devices.DeviceInfo.Platform instead. For more details see https://learn.microsoft.com/en-us/dotnet/maui/migration/forms-projects#device-changes
            if (Device.RuntimePlatform == Device.iOS)
            {
                // アクセス許可がライブラリへのアクセス後に表示されるのを防止する。
                var permissionsResult = await RequestPermissionsAsync(Plugin.Permissions.Abstractions.Permission.Photos);
                if (permissionsResult != Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                {
                    var needsOpenSettings = await App.Current.MainPage.DisplayAlert(null, "フォトライブラリへのアクセスが出来ません", "設定で変更する", "キャンセル");
                    if (needsOpenSettings)
                        CrossPermissions.Current.OpenAppSettings();
                    return false;
                }
            }
            return true;
        }
        public async Task<bool> CheckPermission(Permission requestPermission)
        {
            //if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.M)
            //{
            // iOS or Android6.0以上のみパーミッションチェック
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(requestPermission);
            if (status != PermissionStatus.Granted)
            {
                var results = await CrossPermissions.Current.RequestPermissionsAsync(requestPermission);
                if (results.ContainsKey(requestPermission) &&
                    results[requestPermission] != PermissionStatus.Granted)
                {
                    return false;
                }
            }
            return true;
        }

        public Task<PermissionStatus> RequestPermissionsAsync(Permission permission)
        {
            var taskCompletionSource = new TaskCompletionSource<PermissionStatus>();
            Task.Run(async () =>
            {
                try
                {
                    var status = await CrossPermissions.Current.CheckPermissionStatusAsync(permission);
                    if (status != PermissionStatus.Granted)
                    {
                        var results = await CrossPermissions.Current.RequestPermissionsAsync(permission);
                        if (results.ContainsKey(permission))
                            status = results[permission];
                    }
                    taskCompletionSource.SetResult(status);
                }
                catch (Exception ex)
                {
                    taskCompletionSource.SetResult(PermissionStatus.Unknown);
                    System.Diagnostics.Debug.WriteLine("PermissionsManager.RequestPermissionsAsync : " + ex);
                }
            });
            return taskCompletionSource.Task;
        }
    }
}
