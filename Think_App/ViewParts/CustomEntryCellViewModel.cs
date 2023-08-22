using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
	public class CustomEntryCellViewModel : ViewModelBase
	{
		public CustomEntryCellViewModel()
		{

		}

		public Keyboard EntryKeyboard { get; set; }
		public string LabelText { get; set; }
		public string Placeholder { get; set;}
		private string entryText;
		public string EntryText
		{
			get
			{
				return entryText;
			}
			set
			{
				if (entryText != value)
				{
					entryText = value;
					OnPropertyChanged("EntryText");
				}
			}
		}

		private bool entryIsEnabled;
		public bool EntryIsEnabled
		{
			get
			{
				return entryIsEnabled;
			}
			set
			{
				if (entryIsEnabled != value)
				{
					entryIsEnabled = value;
					OnPropertyChanged("EntryIsEnabled");
				}
			}
		}

	}
}
