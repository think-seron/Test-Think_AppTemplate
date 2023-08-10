using System;
using Xamarin.Forms;
namespace Think_App
{
	public interface ITextService
	{
		double CalculateTextWidth(string text, double fontSize);
		double CalculateTextHeight(string text, double fontSize);
	}
}
