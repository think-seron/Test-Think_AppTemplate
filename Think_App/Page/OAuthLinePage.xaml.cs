using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using Xamarin.Forms;
using Newtonsoft.Json.Linq;
using IO.Swagger.Model;
using System.Threading.Tasks;

namespace Think_App
{
    public partial class OAuthLinePage : ContentPage
    {

        const string line_ChannelId = "@@@Line_ChannelId";
        const string line_AppSecret = "@@@Line_Channelsecret";
        const string line_AccessToken = "https://api.line.me/v2/oauth/accessToken";
        const string line_Redirect_URL = "https://yoyakuapp.sipss.jp/index.html";
        const string line_Base_URL = "https://access.line.me/dialog/oauth/weblogin?response_type=code";
        const string line_GetAccessToken_URL = "https://api.line.me/v2/oauth/accessToken";
        const string line_GetPrifile = "https://api.line.me/v2/profile";
        //"https://access.line.me/dialog/oauth/weblogin?response_type=code&client_id={Channel ID}&redirect_uri={Callback URL}&state={State}";

        string initSource = string.Format(line_Base_URL +
                        "&client_id=" + line_ChannelId +
                        "&redirect_uri=" + line_Redirect_URL);
        HttpResponseMessage getToken;

        SNS_LoginStates loginState = SNS_LoginStates.Error;

        public OAuthLinePage()
        {
            InitializeComponent();

            NavigationPage.SetBackButtonTitle(this, "");

            loginState = SnsAccountSelectViewModel.SnsLoginState;

            System.Diagnostics.Debug.WriteLine(" initSource :" + initSource);
            this.OAuthView.Source = initSource;

            this.OAuthView.Navigating += async (sender, e) =>
            {
                System.Diagnostics.Debug.WriteLine("navigating url :" + e.Url);

                if (e.Url == initSource)
                    return;

                if (e.Url.Contains(line_Redirect_URL))
                {
                    e.Cancel = true;

                    if (App.ProcessManager.CanInvoke())
                    {
                        App.customNavigationPage.IsRunning = true;
                        System.Diagnostics.Debug.WriteLine(" line_Redirect_URL : " + true + "   url :" + e.Url);
                        string code = e.Url.Replace(line_Redirect_URL + "?code=", "");

                        var client = new HttpClient();
                        try
                        {
                            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                                "Content-Type", "application/x-www-form-urlencoded");
                            var DictionaryData = new Dictionary<string, string>() {
                            {"grant_type","authorization_code"},
                            {"client_id",line_ChannelId},
                            {"client_secret",line_AppSecret},
                            {"code",code},
                            {"redirect_uri", line_Redirect_URL}
                        };

                            var postcontent = new FormUrlEncodedContent(DictionaryData);

                            //source = line_GetAccessToken_URL;
                            System.Diagnostics.Debug.WriteLine(" post url (source)  :" + line_GetAccessToken_URL);
                            getToken = await client.PostAsync(line_GetAccessToken_URL, postcontent);

                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine(" token ex  :" + ex);
                            await DisplayAlert("エラー", "データ取得に失敗しました。改めてお試しください。", "戻る");
                            await App.customNavigationPage.PopAsync();
                            App.customNavigationPage.IsRunning = false;
                            App.ProcessManager.OnComplete();
                            return;
                        }

                        if (getToken == null)
                        {
                            await DisplayAlert("通信エラー", "ユーザーデータを取得できませんでした。通信環境の良い場所で改めてお試しください。", "戻る");
                            await App.customNavigationPage.PopAsync();
                            App.customNavigationPage.IsRunning = false;
                            App.ProcessManager.OnComplete();
                            return;
                        }
                        DependencyService.Get<IToast>().Show("認証しました。");

                        System.Diagnostics.Debug.WriteLine("getToken :" + getToken);
                        var jsonToken = await getToken.Content.ReadAsStringAsync();

                        System.Diagnostics.Debug.WriteLine("jsonToken :" + jsonToken);
                        try
                        {
                            var responseToken = JsonManager.Deserialize<Response_LineAccesToken>(jsonToken);
                            System.Diagnostics.Debug.WriteLine("responseToken.access_token :" + responseToken.access_token);
                            client = new HttpClient();
                            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                                "Bearer", responseToken.access_token);
                            var getProfile = await client.GetAsync(line_GetPrifile);

                            System.Diagnostics.Debug.WriteLine(" getProfile : " + getProfile);

                            var jsonProfile = await getProfile.Content.ReadAsStringAsync();

                            var responseProfile = JsonManager.Deserialize<Response_LineProfile>(jsonProfile);

                            System.Diagnostics.Debug.WriteLine(" responseProfile : " + responseProfile);

                            var uData = new UserData()
                            {
                                name = responseProfile.displayName,
                                line_id = responseProfile.userId
                            };

                            switch (loginState)
                            {
                                case SNS_LoginStates.Registoration:

                                    await App.customNavigationPage.PushAsync(new AccountRegistration(1, uData));

                                    break;

                                case SNS_LoginStates.TakeOver:

                                    var dic = new Dictionary<string, string>()
                                {
                                    {"lineId",uData.line_id},
                                };
                                    string apiName = "snsId_login";
                                    var resJson = await APIManager.Post(apiName, dic);
                                    if (resJson == null)
                                        break;
                                    var resUserData = JsonManager.Deserialize<ResponseLogin>(resJson);
                                    string ret = await StaticMethod.AccountReg(
                                                            resUserData.Data.Name,
                                                            resUserData.Data.Phonetic,
                                                            resUserData.Data.Tel,
                                                            resUserData.Data.Mail,
                                                            (int)resUserData.Data.Sex
                                                        );
                                    if (ret != "OK")
                                    {
                                        await DisplayAlert("エラー", "データ取得に失敗しました。改めてお試しください。", "戻る");
                                        await App.customNavigationPage.PopAsync();
                                        App.customNavigationPage.IsRunning = false;
                                        App.ProcessManager.OnComplete();
                                        return;
                                    }

                                    ret = await StaticMethod.DeviceIDReg(resUserData.Data.DeviceId, resUserData.Data.TransferId);
                                    if (ret != "OK")
                                    {
                                        await DisplayAlert("エラー", "データ取得に失敗しました。改めてお試しください。", "戻る");
                                        await App.customNavigationPage.PopAsync();
                                        App.customNavigationPage.IsRunning = false;
                                        App.ProcessManager.OnComplete();
                                        return;
                                    }
                                    DeviceTokenManager.PostAndRegistDeviceToken(DeviceTokenInfo.Instance.DeviceToken);
                                    apiName = "home";
                                    var resHomeJson = await APIManager.GET(apiName);

                                    var param = JsonManager.Deserialize<ResponseHome>(resHomeJson);

                                    System.Diagnostics.Debug.WriteLine("Home:" + resHomeJson);

                                    Device.BeginInvokeOnMainThread(async () =>
                                    {
                                        await App.customNavigationPage.PushAsync(new Home(param), false);
                                    });

                                    break;

                                default:
                                    await DisplayAlert("エラー", "データ取得に失敗しました。改めてお試しください。", "戻る");
                                    await App.customNavigationPage.PopAsync();
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine(" profile ex  :" + ex);
                            var profile = JsonConvert.DeserializeObject(jsonToken);
                            System.Diagnostics.Debug.WriteLine(profile);
                            await DisplayAlert("通信エラー", "ユーザーデータを取得できませんでした。通信環境の良い場所で改めてお試しください。", "戻る");
                            await App.customNavigationPage.PopAsync();

                        }
                        App.customNavigationPage.IsRunning = false;
                        App.ProcessManager.OnComplete();
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine(" line_Redirect_URL : " + false + " url : " + e.Url);
                }
            };

            this.OAuthView.Navigated += async (sender, e) =>
            {

                System.Diagnostics.Debug.WriteLine("navigated url :" + e.Url);
                //initSourceの時に最終的に成功していてもfalseに入ることがあるため。
                if (e.Result == WebNavigationResult.Failure && e.Url != initSource)
                {
                    this.OAuthView.Source = null;

                    System.Diagnostics.Debug.WriteLine(" e.result failure");

                    await DisplayAlert("通信エラー", "ユーザーデータを取得できませんでした。通信環境の良い場所で改めてお試しください。", "戻る");

                    await App.customNavigationPage.PopAsync();
                }

            };
        }
        async Task ManageUser(UserData uData)
        {


        }

    }
    public class Response_LineAccesToken
    {
        public string scope { get; set; }
        public string access_token { get; set; }
        public string expires_in { get; set; }
        public string token_type { get; set; }
        public string refresh_token { get; set; }
    }
    public class Response_LineProfile
    {
        public string userId { get; set; }
        public string displayName { get; set; }
        public string pictureUrl { get; set; }
        public string statusMessage { get; set; }
    }
}
