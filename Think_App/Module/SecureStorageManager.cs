using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Think_App
{
    /// <summary>
    /// 任意の情報をセキュリティで保護されたストレージに保存する
    /// </summary>
    public static class SecureStorageManager
    {
        /// <summary>
        /// SecureStorageにJson形式で保存する
        /// </summary>
        /// <param name="key">キー</param>
        /// <param name="value">値</param>
        /// <returns>true:成功　false:失敗</returns>
        public static async Task<bool> SetAsync(string key, object value)
        {
            try
            {
                var json = JsonConvert.SerializeObject(value);
                await SecureStorage.SetAsync(key, json);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// SecureStorageにからJson形式で保存したデータを取得する
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">キー</param>
        /// <returns>保存データ</returns>
        public static async Task<T> GetAsync<T>(string key)
        {
            try
            {
                var data = await SecureStorage.GetAsync(key);
                return JsonConvert.DeserializeObject<T>(data);
            }
            catch
            {
                return default(T);
            }
        }

        /// <summary>
        /// キー
        /// </summary>
        public class Keys
        {
            public static string AppleSignInId(string id) =>  "appleId:" + id;
        }
    }
}
