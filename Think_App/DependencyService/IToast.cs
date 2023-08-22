using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
	public interface IToast
	{
		void Show(string message);
	}
}
