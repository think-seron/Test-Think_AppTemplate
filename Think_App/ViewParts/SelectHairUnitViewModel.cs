using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Think_App
{
	public class SelectHairUnitViewModel : ViewModelBase
	{
		public int GridMaxColumns { get; set; }

		public List<SelectHairCellModel> GridItemsSource { get; set; }

		double _GridColumnSpacing;
		public double GridColumnSpacing
		{
			get
			{
				return _GridColumnSpacing;
			}
			set
			{
				SetProperty(ref _GridColumnSpacing, value);
			}
		}

		double _GridRowSpacing;
		public double GridRowSpacing
		{
			get
			{
				return _GridRowSpacing;
			}
			set
			{
				SetProperty(ref _GridRowSpacing, value);
			}
		}

		Thickness _GridMargin;
		public Thickness GridMargin
		{
			get
			{
				return _GridMargin;
			}
			set
			{
				SetProperty(ref _GridMargin, value);
			}
		}

		public double GridTileHeight { get; set; }
	}
}
