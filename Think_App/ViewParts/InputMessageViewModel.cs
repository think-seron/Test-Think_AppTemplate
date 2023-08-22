using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
namespace Think_App
{
	public class InputMessageViewModel : ViewModelBase
	{
		double _ViewHeight;
		public double ViewHeight
		{
			get
			{
				return _ViewHeight;
			}
			set
			{
				SetProperty(ref _ViewHeight, value);
			}
		}

		InputMessageEditorViewModel _EditorViewModel;
		public InputMessageEditorViewModel EditorViewModel
		{
			get
			{
				return _EditorViewModel;
			}
			set
			{
				SetProperty(ref _EditorViewModel, value);
			}
		}

		Command _PlusButtonClickedCommand;
		public Command PlusButtonClickedCommand
		{
			get
			{
				return _PlusButtonClickedCommand;
			}
			set
			{
				SetProperty(ref _PlusButtonClickedCommand, value);
			}
		}

		Command _SendButtonClickedCommand;
		public Command SendButtonClickedCommand
		{
			get
			{
				return _SendButtonClickedCommand;
			}
			set
			{
				SetProperty(ref _SendButtonClickedCommand, value);
			}
		}
	}
}
