using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using ZXing.OneD;

namespace Think_App
{
    public delegate void OnError(string message);
    public class APIManager
    {
        public enum Status
        {
            Success
        }
        public static OnError OnErrorEvent;

        public class APIStatus
        {
            public const int Succeeded = 0;
            public const int FatalError = 1;
            public const int Retry = 2;
            public const int ValidationError = 3;
            public const int UnderMaintenance = 4;
            public const int AppVersionError = 5;
            public const int DataVersionError = 6;
            public const int SessionTimeOut = 8;
            public const int TakenOver = 9;
            public const int HomeDeleted = 10;
        }

        public static string BaseDomain()
        {
#if DEBUG
            // return "https://think.entap.xyz/";
            return "https://yoyakuapp.sipss.jp/";
#else
			//return "https://think.entap.xyz/";
            return "https://yoyakuapp.sipss.jp/";
#endif
        }

        public static string BaseURL()
        {
            return BaseDomain() + "api/";
        }

        // App固有：アプリの更新時、必要に応じてAPIVersionをコントロールする。
        const int MinimunAPIVersion = 1;

        // 簡易対応（直近で取得したAPIVersionをローカルストレージへの保存を推奨）
        static string _APIVersion;
        public static string APIVersion
        {
            get
            {
                if (_APIVersion == null)
                    return MinimunAPIVersion.ToString();

                return _APIVersion;
            }
            set
            {
                _APIVersion = value;
            }
        }

        // App固有
        public const int DefaultTimeoutSecond = 5;

        // Entap標準：リトライ設定
        public const int RetryCount = 3;
        public const int RetryInterval = 3000;
        //通常のPOST
        async public static Task<string> Post(string apiName, Dictionary<string, string> DictionaryData = null, Action errorCallback = null, int timeoutSecond = DefaultTimeoutSecond)
        {
            var appId = Config.Instance.Data.appId;
            var nativeVersion = Config.Instance.Data.nativeVersion;
            var platform = Config.Instance.Data.platform;
            var deviceName = Config.Instance.Data.deviceName;
            var appVersion = Config.Instance.Data.appVersion;
            var deviceId = Config.Instance.Data.deviceId;
            DictionaryData["appId"] = appId;
            DictionaryData["nativeVersion"] = nativeVersion;
            DictionaryData["platform"] = platform.ToString();
            DictionaryData["deviceName"] = deviceName;
            DictionaryData["appVersion"] = appVersion;
            DictionaryData["deviceId"] =
                //"0349ba0e5090047d";
                deviceId;
            string paramStr = "?";
            int loopCnt = 1;
            foreach (KeyValuePair<string, string> pair in DictionaryData)
            {
                paramStr += pair.Key + "=" + pair.Value;
                if (DictionaryData.Count <= loopCnt)
                {
                    break;
                }
                paramStr += "&";
                loopCnt++;
            }

            var url = BaseURL() + APIVersion + "/" + apiName + ".php";

            FormUrlEncodedContent postcontent = null;
            if (DictionaryData != null)
                postcontent = new FormUrlEncodedContent(DictionaryData);
            return await PostPrivate(timeoutSecond, url, postcontent);
        }

        private async static Task<string> PostPrivate(int timeoutSeconds, string url, HttpContent content)
        {

            System.Diagnostics.Debug.WriteLine("  api manager post  url   : " + url);
#if DEBUG
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPlicyErrors) => true;
            using (var client = new HttpClient(httpClientHandler))
#else
            using (var client = new HttpClient())
#endif
            {
                client.Timeout = TimeSpan.FromSeconds(timeoutSeconds);
                try
                {
                    for (int i = 0; i < RetryCount; i++)
                    {
                        var posttask = await client.PostAsync(url, content);
                        System.Diagnostics.Debug.WriteLine("posttask.StatusCode  :" + posttask.StatusCode);
                        var responseStr = await posttask.Content.ReadAsStringAsync();
                        System.Diagnostics.Debug.WriteLine("responseStr :" + responseStr);
                        return await GetStatus(responseStr);
                    }
                    Debug.WriteLine("API_PostError : リトライ失敗");
                    return null;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("API_PostError : " + ex.Message);
                    System.Diagnostics.Debug.WriteLine("exception :" + ex);
                    return null;
                }
            }
        }

        //通常のGET
        async public static Task<string> GET(string action, Dictionary<string, string> DictionaryData = null)
        {
            if (Config.Instance.Data.deviceId != null)
            {
                if (DictionaryData == null)
                {
                    DictionaryData = new Dictionary<string, string>();
                }
                DictionaryData["deviceId"] = Config.Instance.Data.deviceId;
            }
            string paramStr = "";
            if (DictionaryData != null)
            {
                paramStr = "&";
                int loopCnt = 1;
                foreach (KeyValuePair<string, string> pair in DictionaryData)
                {
                    paramStr += pair.Key + "=" + pair.Value;
                    if (DictionaryData.Count <= loopCnt)
                    {
                        break;
                    }
                    paramStr += "&";
                    loopCnt++;
                }
            }
            var appId = Config.Instance.Data.appId;
            var nativeVersion = Config.Instance.Data.nativeVersion;
            var platform = Config.Instance.Data.platform;
            var deviceName = Config.Instance.Data.deviceName;
            var appVersion = Config.Instance.Data.appVersion;
            //string url = "http://192.168.0.117:8080/1/"+ action + "?appId="+ appId + "&nativeVersion=" + nativeVersion + "&platform=" + platform + "&deviceName=" + deviceName + "&appVersion=" + appVersion + "&deviceId=" + deviceId + paramStr;
            string url = BaseURL() + APIVersion + "/" + action + ".php" + "?appId=" + appId + "&nativeVersion=" + nativeVersion + "&platform=" + platform + "&deviceName=" + deviceName + "&appVersion=" + appVersion + paramStr;
            System.Diagnostics.Debug.WriteLine(" get api name : " + url);
#if DEBUG
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPlicyErrors) => true;
            var client = new HttpClient(httpClientHandler);
#else
            var client = new HttpClient();
#endif
            try
            {
                var resp = await client.GetAsync(url);
                System.Diagnostics.Debug.WriteLine("resp  :" + resp);
                System.Diagnostics.Debug.WriteLine("GetAsync.StatusCode  :" + resp.StatusCode);
                if (resp.IsSuccessStatusCode == false)
                {
                    System.Diagnostics.Debug.WriteLine("resp  :" + resp);
                    return null;
                }
                var response = await resp.Content.ReadAsStringAsync();
                var index = response.IndexOf("status\":", StringComparison.Ordinal);
                var statusNum = response.Substring(index + 8, 1);
                // status:0 → 成功
                //if (statusNum == "0")
                //{
                //    System.Diagnostics.Debug.WriteLine("resp  0:" + resp);
                //    return response;
                //}
                //return null;
                return await GetStatus(response);

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("exception :" + ex);
                return null;
            }
        }

        static bool _showingHomeDeletedError;
        static async Task<string> GetStatus(string responseStr)
        {

            var response = JsonConvert.DeserializeObject<Response>(responseStr);
            System.Diagnostics.Debug.WriteLine("response :" + response);
            switch (response.status)
            {
                case APIStatus.Succeeded:
                    // 成功
                    return responseStr;

                case APIStatus.FatalError:
                    // 致命的なエラー：ErrorMessageを表示し、「再試行」・「キャンセル」の選択肢を表示
                    var retry = await App.Current.MainPage.DisplayAlert("エラー", response.message, "再試行", "キャンセル");
                    if (!retry)
                        return responseStr;

                    break;
                case APIStatus.Retry:
                    // 再試行：一定時間経過後にリクエストを再試行。一定回数繰り返す
                    await Task.Delay(RetryInterval);

                    break;
                case APIStatus.ValidationError:
                    // バリデーションエラー：ErrorMessageを表示
                    await App.Current.MainPage.DisplayAlert("エラー", response.message, "確認");
                    System.Diagnostics.Debug.WriteLine("error message : " + response.message);
                    return responseStr;

                case APIStatus.UnderMaintenance:
                    // メンテナンス中：ErrorMessageを表示。確認ボタンタップ後はタイトル画面に遷移
                    await Task.Delay(500);
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        var _dialog = new MaintenanceView(response.message);
                        await DialogManager.Instance.ShowDialogView(_dialog);
                    });
                    //await App.Current.MainPage.DisplayAlert("メンテナンス中", response.message, "確認");
                    //App.Current.MainPage = new MaintenancePage();
                    return responseStr;
                case APIStatus.AppVersionError:
                    // アプリバージョンエラー：「アップデート」ボタンを用意し、ストアのサイトを開く
                    var update = JsonConvert.DeserializeObject<VersionUpdate>(responseStr);

                    // 強制アップデートの場合は、
                    while (true)
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                                                       await App.Current.MainPage.DisplayAlert("新しいアプリがあります。", response.message, "アップデート"));

                        if (!String.IsNullOrEmpty(update.Data.Stores))
                            Device.OpenUri(new Uri(update.Data.Stores));

                        if (update.Data.Force == 0)
                            break;
                    }

                    return responseStr;
                case APIStatus.DataVersionError:
                    // データバージョンエラー：「アップデート」ボタンを用意し、タイトル画面・アップデート画面を開く
                    Device.BeginInvokeOnMainThread(async () =>
                                                   await App.Current.MainPage.DisplayAlert("新しいデータがあります。", response.message, "ダウンロード"));
                    update = JsonConvert.DeserializeObject<VersionUpdate>(responseStr);

                    //await App.Current.MainPage.Navigation.PushModalAsync();

                    return responseStr;
                case APIStatus.SessionTimeOut:
                    // セッションタイムアウト：認証系のAPIリクエストを実行する
                    return responseStr;

                case APIStatus.TakenOver:
                    Device.BeginInvokeOnMainThread(async () =>
                                                   await App.Current.MainPage.DisplayAlert(response.message, "改めてアカウントを作成してください", "OK"));
                    System.Diagnostics.Debug.WriteLine(" TakenOver 引き継ぎ済み　:" + responseStr);


                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await App.customNavigationPage.PushAsync(new AppStart());
                    });

                    await FileManager.DeleteFileAsync("Account", "deviceInfo");

                    return null;
                case APIStatus.HomeDeleted:
                    //お知らせやクーポンなどで連続でapiを走らせている場合、ダイアログなどが連続で表示されてしまうため
                    //このエラー表示中は2回目を表示させないように。
                    if (_showingHomeDeletedError) return responseStr;
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        _showingHomeDeletedError = true;
                        await App.Current.MainPage.DisplayAlert("該当店舗のアプリ掲載は終了しました。", "改めてホーム店舗の選択を行なってください。", "OK");

                        await App.customNavigationPage.PushAsync(new FavoriteStoreReg());
                        _showingHomeDeletedError = false;
                    });
                    return responseStr;
            }
            return responseStr;
        }

        //Debug用にDeviceiDなどを変更してGet処理を行う。
        async public static Task<string> DebugGET(string action, Dictionary<string, string> DictionaryData = null)
        {
            if (Config.Instance.Data.deviceId != null)
            {
                if (DictionaryData == null)
                {
                    DictionaryData = new Dictionary<string, string>();
                }
                DictionaryData["deviceId"] = "0349ba0e5090047d";
            }
            string paramStr = "";
            if (DictionaryData != null)
            {
                paramStr = "&";
                int loopCnt = 1;
                foreach (KeyValuePair<string, string> pair in DictionaryData)
                {
                    paramStr += pair.Key + "=" + pair.Value;
                    if (DictionaryData.Count <= loopCnt)
                    {
                        break;
                    }
                    paramStr += "&";
                    loopCnt++;
                }
            }
            var appId = Config.Instance.Data.appId;
            var nativeVersion = Config.Instance.Data.nativeVersion;
            var platform = Config.Instance.Data.platform;
            var deviceName = Config.Instance.Data.deviceName;
            var appVersion = Config.Instance.Data.appVersion;
            //string url = "http://192.168.0.117:8080/1/"+ action + "?appId="+ appId + "&nativeVersion=" + nativeVersion + "&platform=" + platform + "&deviceName=" + deviceName + "&appVersion=" + appVersion + "&deviceId=" + deviceId + paramStr;
            string url = BaseURL() + APIVersion + "/" + action + ".php" + "?appId=" + appId + "&nativeVersion=" + nativeVersion + "&platform=" + platform + "&deviceName=" + deviceName + "&appVersion=" + appVersion + paramStr;
            System.Diagnostics.Debug.WriteLine("api name : " + url);
            var client = new HttpClient();
            try
            {
                var resp = await client.GetAsync(url);
                if (resp.IsSuccessStatusCode == false)
                {
                    return null;
                }
                var response = await resp.Content.ReadAsStringAsync();
                var index = response.IndexOf("status\":", StringComparison.Ordinal);
                var statusNum = response.Substring(index + 8, 1);
                // status:0 → 成功
                if (statusNum == "0")
                {
                    return response;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        async public static Task<string> PostBytes(string action, string name, byte[] byteArray, Dictionary<string, string> DictionaryData = null)
        {
            var url = BaseURL() + APIVersion + "/" + action + ".php";
            System.Diagnostics.Debug.WriteLine("api name " + url);
            using (var client = new HttpClient())
            using (var multi = new MultipartFormDataContent())
            {
                try
                {
                    if (DictionaryData != null)
                    {
                        var appId = Config.Instance.Data.appId;
                        var nativeVersion = Config.Instance.Data.nativeVersion;
                        var platform = Config.Instance.Data.platform;
                        var deviceName = Config.Instance.Data.deviceName;
                        var appVersion = Config.Instance.Data.appVersion;
                        var deviceId = Config.Instance.Data.deviceId;
                        DictionaryData["appId"] = appId;
                        DictionaryData["nativeVersion"] = nativeVersion;
                        DictionaryData["platform"] = platform.ToString();
                        DictionaryData["deviceName"] = deviceName;
                        DictionaryData["appVersion"] = appVersion;
                        DictionaryData["deviceId"] = deviceId;
                        foreach (KeyValuePair<string, string> kvp in DictionaryData)
                        {
                            multi.Add(new StringContent(kvp.Value), kvp.Key);
                        }
                    }

                    if (byteArray != null)
                    {
                        var postbyte = new ByteArrayContent(byteArray);
                        // pngファイル名を生成。被らないようにGUIDを使う。
                        var filename = System.Guid.NewGuid().ToString("N") + ".png";
                        multi.Add(postbyte, name, filename);
                    }

                    var posttask = await client.PostAsync(url, multi);
                    var response = await posttask.Content.ReadAsStringAsync();
                    return response;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    return null;
                }
            }
        }

    }
}