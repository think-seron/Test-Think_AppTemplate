using System;
using Android.Gms.Common.Apis;
using Xamarin.Forms;
using Think_App.Droid;
using Android.Gms.Auth.Api;
using Android.Content;
using Android.App;

[assembly: Dependency(typeof(GoogleSignInService_Droid))]
namespace Think_App.Droid
{
    public class GoogleSignInService_Droid : IGoogleSignInService
    {
        public static GoogleApiClient GoogleApiClient;
        //サインイン
        public void SignIn()
        {
            try
            {
#if DEBUG
				System.Diagnostics.Debug.WriteLine("   sign in start ");
				System.Console.WriteLine("   sign in start ");

				//debug用
				//if (iGoogleTestService_Droid.GoogleApiClient != null &&
				//	!iGoogleTestService_Droid.GoogleApiClient.IsConnected)
				//{
				//	System.Diagnostics.Debug.WriteLine("   GoogleApiClient IsConnected ");
				//	System.Console.WriteLine("   GoogleApiClient IsConnected ");
				//	iGoogleTestService_Droid.GoogleApiClient.Connect();
				//}

				//Release同様
				if (GoogleSignInService_Droid.GoogleApiClient != null)
				{
					Intent signInIntent = Auth.GoogleSignInApi.GetSignInIntent(GoogleSignInService_Droid.GoogleApiClient);
					((Activity)Forms.Context).StartActivityForResult(signInIntent, MainActivity.RC_SIGN_IN);
				}
				else
				{
					System.Diagnostics.Debug.WriteLine("  GoogleApiClient Is not Connected ");
				}
#else
                if (GoogleSignInService_Droid.GoogleApiClient != null)
                {
                    Intent signInIntent = Auth.GoogleSignInApi.GetSignInIntent(GoogleSignInService_Droid.GoogleApiClient);
                    ((Activity)Forms.Context).StartActivityForResult(signInIntent, MainActivity.RC_SIGN_IN);
                }
#endif
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message + System.Environment.NewLine + ex.StackTrace);
            }
        }
        //サインアウト
        public void SignOut()
        {
#if DEBUG
			//debug用
			//if (GoogleApiClient != null && GoogleApiClient.IsConnected)
			//{
			//	System.Diagnostics.Debug.WriteLine("   SignOut start ");
			//	PlusClass.AccountApi.ClearDefaultAccount(GoogleApiClient);
			//	PlusClass.AccountApi.RevokeAccessAndDisconnect(GoogleApiClient);
			//}

			//リリース同様
			if (GoogleSignInService_Droid.GoogleApiClient != null &&
GoogleSignInService_Droid.GoogleApiClient.IsConnected)
			{
				Auth.GoogleSignInApi.SignOut(GoogleSignInService_Droid.GoogleApiClient);
			}

#else

            if (GoogleSignInService_Droid.GoogleApiClient != null &&
        GoogleSignInService_Droid.GoogleApiClient.IsConnected)
            {
                Auth.GoogleSignInApi.SignOut(GoogleSignInService_Droid.GoogleApiClient);
            }
#endif
        }
        //切断処理
        public void Disconnect()
        {

#if DEBUG
			//debug用
			//if (GoogleApiClient != null &&
			//	GoogleApiClient.IsConnected)
			//{
			//	PlusClass.AccountApi.ClearDefaultAccount(GoogleApiClient);
			//	PlusClass.AccountApi.RevokeAccessAndDisconnect(GoogleApiClient);
			//	GoogleApiClient.Disconnect();
			//}

			//リリース同様
			if (GoogleSignInService_Droid.GoogleApiClient != null &&
GoogleSignInService_Droid.GoogleApiClient.IsConnected)
			{
				Auth.GoogleSignInApi.RevokeAccess(GoogleSignInService_Droid.GoogleApiClient);
			}

#else

            if (GoogleSignInService_Droid.GoogleApiClient != null &&
        GoogleSignInService_Droid.GoogleApiClient.IsConnected)
            {
                Auth.GoogleSignInApi.RevokeAccess(GoogleSignInService_Droid.GoogleApiClient);
            }
#endif
        }
    }
}
