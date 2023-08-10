using System;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections.Generic;
using PCLStorage;
using Newtonsoft.Json;
using System.Globalization;

namespace Think_App
{
    public static class StorageManager
    {
        static readonly IFolder rootFolder = FileSystem.Current.LocalStorage;

        public enum ExistStatus
        {
            Exists,
            NotFound,
            Unknown,
        }

        // ユーザーデータ書き出し。
        public static async Task UserDataWriteAsync(string foldername, string filename, object content, bool replace = true)
        {
            try
            {
                // Json文字列にコンバート。
                var doc = JsonConvert.SerializeObject(content);

                var folder = await rootFolder.CreateFolderAsync(foldername, CreationCollisionOption.OpenIfExists);
                var option = replace ? CreationCollisionOption.ReplaceExisting : CreationCollisionOption.GenerateUniqueName;
                var file = await folder.CreateFileAsync(filename, option);

                await file.WriteAllTextAsync(doc);
                Debug.WriteLine("Save:{0}", file.Path);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        // ユーザーデータ読み込み。
        public static async Task<T> UserDataReadAsync<T>(string foldername, string filename)
        {
            //パス指定＆Taskの後ろに.ConfigureAwait(continueOnCapturedContext:false);でエラー回避
            try
            {
                var folder = await rootFolder.CreateFolderAsync(foldername, CreationCollisionOption.OpenIfExists).ConfigureAwait(continueOnCapturedContext: false);
                var isFileExisting = await folder.CheckExistsAsync(filename);
                if (isFileExisting == ExistenceCheckResult.FileExists)
                {
                    var file = await folder.CreateFileAsync(filename, CreationCollisionOption.OpenIfExists).ConfigureAwait(continueOnCapturedContext: false);
                    var str = await file.ReadAllTextAsync().ConfigureAwait(continueOnCapturedContext: false);

                    // Json文字列からT型に戻す。
                    Debug.WriteLine("Read:{0}", file.Path);
                    return JsonConvert.DeserializeObject<T>(str);
                }

                return default(T);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return default(T);
            }
        }

        // ユーザーデータ存在確認。
        public static async Task<ExistStatus> UserDataCheckExistAsync(string foldername, string filename)
        {
            try
            {
                var folder = await rootFolder.CreateFolderAsync(foldername, CreationCollisionOption.OpenIfExists).ConfigureAwait(continueOnCapturedContext: false);
                var isFileExisting = await folder.CheckExistsAsync(filename).ConfigureAwait(continueOnCapturedContext: false);

                if (isFileExisting == ExistenceCheckResult.NotFound)
                {
                    return ExistStatus.NotFound;
                }

                return ExistStatus.Exists;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return ExistStatus.Unknown;
            }
        }

        // 指定フォルダーにあるファイル名をフルパスで全て読み込む。
        public static async Task<List<string>> UserFilePathsReadAsync(string foldername)
        {
            var filepaths = new List<string>();
            try
            {
                var folder = await rootFolder.CreateFolderAsync(foldername, CreationCollisionOption.OpenIfExists).ConfigureAwait(continueOnCapturedContext: false);
                var files = await folder.GetFilesAsync();

                if (files != null && files.Count > 0)
                {
                    foreach (var file in files)
                    {
                        filepaths.Add(file.Path);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return filepaths;
        }

        // ユーザーデータ書き出し。(Bytes)
        public static async Task UserBytesDataWriteAsync(string foldername, string filename, byte[] bytes, bool replace = true)
        {
            try
            {
                var folder = await rootFolder.CreateFolderAsync(foldername, CreationCollisionOption.OpenIfExists);
                var option = replace ? CreationCollisionOption.ReplaceExisting : CreationCollisionOption.GenerateUniqueName;
                var file = await folder.CreateFileAsync(filename, option);
                using (var stream = await file.OpenAsync(PCLStorage.FileAccess.ReadAndWrite))
                {
                    await stream.WriteAsync(bytes, 0, bytes.Length);
                    Debug.WriteLine("Save:{0}", file.Path);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        // ユーザーデータ読み込み。(Bytes)
        public static async Task<byte[]> UserBytesDataReadAsync(string foldername, string filename)
        {
            try
            {
                var folder = await rootFolder.CreateFolderAsync(foldername, CreationCollisionOption.OpenIfExists).ConfigureAwait(continueOnCapturedContext: false);
                var isFileExisting = await folder.CheckExistsAsync(filename).ConfigureAwait(continueOnCapturedContext: false);

                if (isFileExisting == ExistenceCheckResult.FileExists)
                {
                    var file = await folder.CreateFileAsync(filename, CreationCollisionOption.OpenIfExists).ConfigureAwait(continueOnCapturedContext: false);
                    byte[] buffer;
                    using (var stream = await file.OpenAsync(PCLStorage.FileAccess.Read))
                    {
                        long length = stream.Length;
                        buffer = new byte[length];
                        await stream.ReadAsync(buffer, 0, (int)length);
                        Debug.WriteLine("Read:{0}", file.Path);
                    }
                    return buffer;
                }

                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        // ユーザーデータ削除。
        public static async Task UserDataDeleteAsync(string foldername, string filename)
        {
            try
            {
                var folder = await rootFolder.CreateFolderAsync(foldername, CreationCollisionOption.OpenIfExists).ConfigureAwait(continueOnCapturedContext: false);
                var isFileExisting = await folder.CheckExistsAsync(filename);
                if (isFileExisting == ExistenceCheckResult.FileExists)
                {
                    var file = await folder.CreateFileAsync(filename, CreationCollisionOption.OpenIfExists).ConfigureAwait(continueOnCapturedContext: false);
                    await file.DeleteAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        // フルパスの取得
        public static async Task<string> GetFullPath(string foldername, string filename)
        {
            try
            {
                var folder = await rootFolder.CreateFolderAsync(foldername, CreationCollisionOption.OpenIfExists).ConfigureAwait(continueOnCapturedContext: false);
                var file = await folder.GetFileAsync(filename).ConfigureAwait(continueOnCapturedContext: false);

                return file.Path;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        // 現在時刻からのファイル名生成
        public static string CreateFileNameByCurrentTime()
        {
            // 日本時間(世界標準時+9時間)を取得
            var time = DateTimeOffset.UtcNow.ToOffset(TimeSpan.FromHours(9));
            var timeStr = time.ToString("yyyyMMddHHmmssfffffff");
            Debug.WriteLine("ファイル名生成:{0}", timeStr);
            return timeStr;
        }
    }
}
