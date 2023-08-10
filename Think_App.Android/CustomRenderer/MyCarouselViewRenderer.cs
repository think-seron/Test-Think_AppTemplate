//using System;
//using System.ComponentModel;
//using Xamarin.Forms;
//using Xamarin.Forms.Platform;
//using Xamarin.Forms.Platform.Android;
//using Think_App.Droid;

//[assembly: ExportRenderer(typeof(CarouselView), typeof(MyCarouselViewRenderer))]
//namespace Think_App.Droid
//{
//	public class MyCarouselViewRenderer : CarouselViewRenderer
//	{
//		protected override void OnLayout(bool changed, int left, int top, int right, int bottom)
//		{
//			// 0割り例外の救済措置。
//			if (left == right)
//			{
//				return;
//			}

//			base.OnLayout(changed, left, top, right, bottom);
//		}
//	}
//}
