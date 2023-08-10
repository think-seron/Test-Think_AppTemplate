using System;
using Xamarin.Forms;

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
