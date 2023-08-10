using System;
namespace Think_App
{
    public class AppleAccount
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Token { get; set; }
        public string RealUserStatus { get; set; }
        public string UserId { get; set; }

        /// <summary>
        /// 認証済みかどうか
        /// </summary>
        public bool HasAuthenticated => string.IsNullOrEmpty(Email) && string.IsNullOrEmpty(Name);

        /// <summary>
        /// SecureStorageの情報と同期する。
        /// AppleSignInの２回目以降は、Email、Nameを取得できないため
        /// 初回認証時に、SecureStorage（KeyChain）に保存し、以降はSecureStorageから補完する。
        /// </summary>
        public async void Sync()
        {
            if (HasAuthenticated)
            {
                var account = await SecureStorageManager.GetAsync<AppleAccount>(SecureStorageManager.Keys.AppleSignInId(UserId));
                if (account == null) return;

                Email = account.Email;
                Name = account.Name;
            }
            else
            {
                await SecureStorageManager.SetAsync(SecureStorageManager.Keys.AppleSignInId(UserId), this);
            }
        }
    }
}
