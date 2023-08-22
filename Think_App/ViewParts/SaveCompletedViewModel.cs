using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
namespace Think_App
{
	public class SaveCompletedViewModel : ViewModelBase
	{
		double _ButtonWidth;
		public double ButtonWidth
		{
			get
			{
				return _ButtonWidth;
			}
			set
			{
				SetProperty(ref _ButtonWidth, value);
			}
		}

		double _ButtonHeight;
		public double ButtonHeight
		{
			get
			{
				return _ButtonHeight;
			}
			set
			{
				SetProperty(ref _ButtonHeight, value);
			}
		}

		double _ButtonFontSize;
		public double ButtonFontSize
		{
			get
			{
				return _ButtonFontSize;
			}
			set
			{
				SetProperty(ref _ButtonFontSize, value);
			}
		}

		double _InfoLblFontSize;
		public double InfoLblFontSize
		{
			get
			{
				return _InfoLblFontSize;
			}
			set
			{
				SetProperty(ref _InfoLblFontSize, value);
			}
		}

		Command _SendMessageCommand;
		public Command SendMessageCommand
		{
			get
			{
				return _SendMessageCommand;
			}
			set
			{
				SetProperty(ref _SendMessageCommand, value);
			}
		}

		Command _EndCommand;
		public Command EndCommand
		{
			get
			{
				return _EndCommand;
			}
			set
			{
				SetProperty(ref _EndCommand, value);
			}
		}
	}
}
