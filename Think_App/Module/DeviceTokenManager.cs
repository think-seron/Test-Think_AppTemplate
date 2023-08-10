using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IO.Swagger.Model;
using Think_App;
namespace Think_App
{
    public class DeviceTokenInfo
    {
        public const string DeviceToken_RegistFolderName = "DeviceTokenInfo";
        public const string DeviceToken_RegistFileName = "DeviceToken";

        static DeviceTokenData instanse;
        static DeviceTokenData data;
        public static DeviceTokenData Instance
        {
            get
            {
                if (instanse == null)
                {
                    instanse = new DeviceTokenData();
                    var result = Task.Run(async () =>
                    {
                        try
                        {
                            data = null;
                            data = await FileManager.ReadJsonFileAsync<DeviceTokenData>(DeviceToken_RegistFolderName, DeviceToken_RegistFileName);

                            //一度もdevicetoken登録していない場合(ユーザー登録を行なっていない場合)
                            if (data == null)
                                data = new DeviceTokenData()
                                {
                                    IsRegistServer = null,
                                    DeviceToken = null
                                };
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine("ex :" + ex);
                        }
                        return data;
                    });
                    instanse = result.Result;
                }

                return instanse;
            }
            set
            {
                instanse = value;
            }
        }
    }

    public class DeviceTokenData
    {


        public bool? IsRegistServer { get; set; }

        public string DeviceToken { get; set; }
    }

    public class DeviceTokenManager
    {
        public DeviceTokenManager()
        {
        }

        public static bool RegistLocalDeviceToken(DeviceTokenData data)
        {
            var result = Task.Run(async () =>
            {

                return await FileManager.WriteJsonFileAsync(DeviceTokenInfo.DeviceToken_RegistFolderName,
                                                                      DeviceTokenInfo.DeviceToken_RegistFileName, data);
            });

            return result.Result;
        }
        //DeviceTokenData
        public static void PostAndRegistDeviceToken(string token)
        {
            System.Diagnostics.Debug.WriteLine(" PostAndRegistDeviceToken token:" + token);
            DeviceTokenInfo.Instance.DeviceToken = token;
            Task.Run(async () =>
        {

            if (string.IsNullOrEmpty(Config.Instance.Data.deviceId) || string.IsNullOrEmpty(token))
            {
                return;
            }
            //post処理
            var dic = new Dictionary<string, string>() {
                    {"deviceToken",DeviceTokenInfo.Instance.DeviceToken}
            };

            var res = await APIManager.Post("deviceToken__regist", dic);
            //devicetoken devicetokendata はレスポンスのクラスに
            var json = JsonManager.Deserialize<Response>(res);

            DeviceTokenInfo.Instance.IsRegistServer = json.status == 0 ? true : false;
            //書き込み処理
            RegistLocalDeviceToken(DeviceTokenInfo.Instance);

            return;
        });
        }
    }
}
