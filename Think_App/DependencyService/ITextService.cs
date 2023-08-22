using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
namespace Think_App
{
	public interface ITextService
	{
		double CalculateTextWidth(string text, double fontSize);
		double CalculateTextHeight(string text, double fontSize);
	}
}
