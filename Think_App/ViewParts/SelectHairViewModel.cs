using System;
using System.Collections.Generic;
namespace Think_App
{
	public class SelectHairViewModel : ViewModelBase
	{
		public List<SelectHairUnitViewModelSet> SelectHairUnitViewModelSets { get; set; }

		public class SelectHairUnitViewModelSet
		{
			public SelectHairUnitViewModel Model { get; set; }
			public string HairStyleName { get; set; }
		}
	}
}
