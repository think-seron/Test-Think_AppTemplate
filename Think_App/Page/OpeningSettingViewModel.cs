using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
	public class OpeningSettingViewModel : ViewModelBase
	{
		public OpeningSettingViewModel()
		{
			CustomSwitchCellBlog = new CustomSwitchCellViewModel();
			CustomSwitchCellBlog.LabelText = "My 美logを公開にする";
            CustomSwitchCellBlog.ViewVisible = true;
		}
		public CustomSwitchCellViewModel CustomSwitchCellBlog { get; set; }
	}
}
