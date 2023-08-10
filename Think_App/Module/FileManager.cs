using System;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;
using PCLStorage;
using Newtonsoft.Json;

namespace Think_App
{
    public class FileManager
    {

        static IFolder RootFolder = FileSystem.Current.LocalStorage;
        static IFile file;

        public FileManager()
        {
        }

        // json文字列ファイル書き込み
        public static async Task<bool> WriteJsonFileAsync(string folderName, string fileName, object content)
        {
            try
            {
                // Json文字列にコンバート
                var doc = JsonConvert.SerializeObject(content);

                var folder = await RootFolder.CreateFolderAsync(folderName, CreationCollisionOption.OpenIfExists);
                file = await folder.CreateFileAsync(fileName + ".txt", CreationCollisionOption.ReplaceExisting);

                await file.WriteAllTextAsync(doc);
                Debug.WriteLine("create file [folder:" + folderName + "][file:" + fileName + "] Saved.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
            return true;
        }

        //ファイルデータ読み込み
        public static async Task<T> ReadJsonFileAsync<T>(string folderName, string fileName)
        {
            try
            {
                var folder = await RootFolder.CreateFolderAsync(folderName, CreationCollisionOption.OpenIfExists);

                var isFileExisting = await folder.CheckExistsAsync(fileName + ".txt");

                if (isFileExisting != ExistenceCheckResult.NotFound)
                {
                    file = await folder.CreateFileAsync(fileName + ".txt", CreationCollisionOption.OpenIfExists);
                    var str = await file.ReadAllTextAsync();

                    // Json文字列からT型に戻す。
                    Debug.WriteLine("read file [" + fileName + ".txt]");
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

        public static async Task<bool> CheckFileAsync(string folderName, string fileName)
        {
            try
            {
                var folder = await RootFolder.CreateFolderAsync(folderName, CreationCollisionOption.OpenIfExists);

                var isFileExisting = await folder.CheckExistsAsync(fileName + ".txt");
                if (isFileExisting == ExistenceCheckResult.NotFound)
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(" check ex :" + ex);
                return false;
            }
        }

        public static async Task<bool> DeleteFileAsync(string folderName, string fileName)
        {
            try
            {
                return await Task.Run(async () =>
                {
                    try
                    {
                        fileName = fileName + ".txt";
                        var folder = await RootFolder.CreateFolderAsync(folderName, CreationCollisionOption.OpenIfExists);

                        var isFileExisting = await folder.CheckExistsAsync(fileName);
                        var result = (isFileExisting == ExistenceCheckResult.FileExists) ? true : false;
                        if (!result)
                            return result;

                        Config.Instance.Data.deviceId = null;
                        file = await folder.GetFileAsync(fileName);

                        await file.DeleteAsync();

                        return true;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("ex :" + ex);
                        return false;
                    }
                });
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
