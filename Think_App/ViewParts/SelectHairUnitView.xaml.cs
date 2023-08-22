using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
	public partial class SelectHairUnitView : ContentView
	{
		public event EventHandler<HairStyleSelectedEventArgs> HairStyleSelected = delegate { };

		public SelectHairUnitView()
		{
			InitializeComponent();

			this.SelectHairGridView.Command = new Command(ItemSelected);
		}

		void ItemSelected(object obj)
		{
			var model = obj as SelectHairCellModel;
			if (model != null)
			{
				if (HairStyleSelected != null)
				{
					HairStyleSelected(this, new HairStyleSelectedEventArgs()
					{
						Url = model.Url,
						Scale = model.Scale,
						ShiftX = model.ShiftX,
						SHiftY = model.SHiftY
					});
				}
			}
		}

		public async Task UpdateView()
		{
			await this.SelectHairGridView.BuildTiles();
		}
	}
}
