using System;
namespace Think_App
{
	public interface IScreenService
	{
		int GetScreenWidth();
		int GetScreenHeight();
		double GetScreenScale();
		int GetStatusBarHeight();
		bool IsPortrait();
	}
}
