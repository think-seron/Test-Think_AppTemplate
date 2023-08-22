using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
	public partial class SelectHairCell : ViewCell
	{
		public SelectHairCell()
		{
			InitializeComponent();

			// キャッシュしない。
			this.HairImg.CacheType = null;
		}
	}
}
