
using Xamarin.Forms;
using Think_App.Droid;
using Android.Support.V4.App;
[assembly: Dependency(typeof(ScanerPermissionService))]
namespace Think_App.Droid
{
	public class ScanerPermissionService : IScanerPermissionService
	{

		public void Call()
		{
			ActivityCompat.RequestPermissions(MainActivity.activity, new[]
			{
				Android.Manifest.Permission.Camera
			}, 0);
		}
	}
}
