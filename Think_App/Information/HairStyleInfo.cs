using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
	public class HairStyleInfo
	{
		public HairStyleInfo()
		{
		}

		public int StoreId { get; set; }
		public string StoreName { get; set; }
		//public string Souce { get; set; }
		public ImageSource Souce { get; set; }
		public string Description { get; set; }
		public string StaffName { get; set; }

		public bool BtnIsVisible { get; set; } 
	}
}
