using System;
using IO.Swagger.Model;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
	public class MyBeautyBlogModel
	{
		public string ImgSouce { get; set; }
		public string DateStr { get; set; }
		public string DateStringrShort { get; set; }
		public int MyBeautyBlogId { get; set; }
		public int CategoryValue { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public Rect GridViewImgRect { get; set; }
		public bool BtnIsVisible { get; set; }
		public Color GridViewBgColor { get; set; }
		public Rect GridViewLabelRect { get; set; }
		public double GridViewLabelFontSize { get; set; }
		public double GridViewLabelHeightRequest { get; set; }
		public double GridViewLabelWidthRequest { get; set; }
	}
}
