using System;
namespace Think_App
{
	public class InputMessageEditorViewModel : ViewModelBase
	{
		string _MessageEditorText;
		public string MessageEditorText
		{
			get
			{
				return _MessageEditorText;
			}
			set
			{
				SetProperty(ref _MessageEditorText, value);
			}
		}

		double _MessageEditorOpacity;
		public double MessageEditorOpacity
		{
			get
			{
				return _MessageEditorOpacity;
			}
			set
			{
				SetProperty(ref _MessageEditorOpacity, value);
			}
		}
	}
}
