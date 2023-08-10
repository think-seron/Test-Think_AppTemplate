using System;
using Xamarin.Forms;

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
