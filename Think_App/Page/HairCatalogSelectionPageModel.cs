using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
namespace Think_App
{
	public class HairCatalogSelectionPageModel : ViewModelBase
	{
		bool _WomanHairStyleVisible;
		public bool WomanHairStyleVisible
		{
			get
			{
				return _WomanHairStyleVisible;
			}
			set
			{
				SetProperty(ref _WomanHairStyleVisible, value);
			}
		}

		double _HairStyleViewTileHeight;
		public double HairStyleViewTileHeight
		{
			get
			{
				return _HairStyleViewTileHeight;
			}
			set
			{
				SetProperty(ref _HairStyleViewTileHeight, value);
			}
		}

		int _HairStyleViewMaxColumns;
		public int HairStyleViewMaxColumns
		{
			get
			{
				return _HairStyleViewMaxColumns;
			}
			set
			{
				SetProperty(ref _HairStyleViewMaxColumns, value);
			}
		}

		Thickness _HairStyleViewPadding;
		public Thickness HairStyleViewPadding
		{
			get
			{
				return _HairStyleViewPadding;
			}
			set
			{
				SetProperty(ref _HairStyleViewPadding, value);
			}
		}

		double _HairStyleViewColumnSpacing;
		public double HairStyleViewColumnSpacing
		{
			get
			{
				return _HairStyleViewColumnSpacing;
			}
			set
			{
				SetProperty(ref _HairStyleViewColumnSpacing, value);
			}
		}

		List<HairCatalogSelectionViewCellModel> _WomanHairStyleItemsSource;
		public List<HairCatalogSelectionViewCellModel> WomanHairStyleItemsSource
		{
			get
			{
				return _WomanHairStyleItemsSource;
			}
			set
			{
				SetProperty(ref _WomanHairStyleItemsSource, value);
			}
		}

		Command _WomanHairStyleSelectedCommand;
		public Command WomanHairStyleSelectedCommand
		{
			get
			{
				return _WomanHairStyleSelectedCommand;
			}
			set
			{
				SetProperty(ref _WomanHairStyleSelectedCommand, value);
			}
		}

		bool _SeparatorVisible;
		public bool SeparatorVisible
		{
			get
			{
				return _SeparatorVisible;
			}
			set
			{
				SetProperty(ref _SeparatorVisible, value);
			}
		}

		bool _ManHairStyleVisible;
		public bool ManHairStyleVisible
		{
			get
			{
				return _ManHairStyleVisible;
			}
			set
			{
				SetProperty(ref _ManHairStyleVisible, value);
			}
		}

		List<HairCatalogSelectionViewCellModel> _ManHairStyleItemsSource;
		public List<HairCatalogSelectionViewCellModel> ManHairStyleItemsSource
		{
			get
			{
				return _ManHairStyleItemsSource;
			}
			set
			{
                SetProperty(ref _ManHairStyleItemsSource, value);
			}
		}

		Command _ManHairStyleSelectedCommand;
		public Command ManHairStyleSelectedCommand
		{
			get
			{
				return _ManHairStyleSelectedCommand;
			}
			set
			{
				SetProperty(ref _ManHairStyleSelectedCommand, value);
			}
		}
	}
}
