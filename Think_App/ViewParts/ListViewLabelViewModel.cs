using System;
using IO.Swagger.Model;

namespace Think_App
{
	public class ListViewLabelViewModel : ViewModelBase
	{
		public string LabelText { get; set; }
		public int? regionId { get; set; }
		public int? salonId { get; set; }
	}
}
