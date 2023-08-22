using System;
using IO.Swagger.Model;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
namespace Think_App
{
	public class ListViewCouponViewModel : ViewModelBase
	{
		public ListViewCouponViewModel()
		{
            baseTitleFontSize = ScaleManager.SizeSet(14.0);
            DetailFontSize = ScaleManager.SizeSet(12.0);

			CouponThumbnailSize = ScaleManager.SizeSet(135.0);

			CouponTitleFontSize = baseTitleFontSize;
			//13 * ScaleManager.Scale;
			ShopNameFontSize = baseTitleFontSize;
			//13 * ScaleManager.Scale;
			OperationContentFontSize = DetailFontSize;
			//11 * ScaleManager.Scale;
			DiscountContentFontSize = DetailFontSize;
			//11 * ScaleManager.Scale;
			TermsOfUseFontSize = DetailFontSize;
			//11 * ScaleManager.Scale;
			SpatialConditionFontSize = DetailFontSize;
			//11 * ScaleManager.Scale;
		}
		double scale;
        double baseTitleFontSize { get; set; }
        double DetailFontSize { get; set; }



		public int? ID { get; set; }
		//public string CouponImageSouce { get; set; }
		public ImageSource CouponImageSouce { get; set; }
		public string CouponTitle { get; set; }
		public string Type { get; set; }
		public string ShopName { get; set; }
		public string OperationContent { get; set; }
		public string DiscountContent { get; set; }
		public string TermsOfUse { get; set; }
		public string SpatialCondition { get; set; }
		public string Description { get; set; }
		public double CouponTitleFontSize { get; set; }
		public double ShopNameFontSize { get; set; }
		public double OperationContentFontSize { get; set; }
		public double DiscountContentFontSize { get; set; }
		public double TermsOfUseFontSize { get; set; }
		public double SpatialConditionFontSize { get; set; }
		public bool IsAssociated { get; set; }
		public double CouponThumbnailSize { get; set; }
		public int? ShopID { get; set; }
	}
}