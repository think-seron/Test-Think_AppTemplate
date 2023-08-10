using System;
namespace Think_App
{
	public interface IGoogleSignInService
	{
	void SignIn();
	void SignOut();
	void Disconnect();
	}
}
