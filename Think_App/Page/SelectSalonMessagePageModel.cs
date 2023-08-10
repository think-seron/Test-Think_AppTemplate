using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
namespace Think_App
{
	public class SelectSalonMessagePageModel : ViewModelBase
	{
		ObservableCollection<SelectSalonMessageListCellModel> _SalonListSource;
		public ObservableCollection<SelectSalonMessageListCellModel> SalonListSource
		{
			get
			{
				return _SalonListSource;
			}
			set
			{
				SetProperty(ref _SalonListSource, value);
			}
		}

		Color _SeparatorColor;
		public Color SeparatorColor
		{
			get
			{
				return _SeparatorColor;
			}
			set
			{
				SetProperty(ref _SeparatorColor, value);
			}
		}
	}
}
