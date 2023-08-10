using System;
namespace Think_App
{
	public class ScaleManager
	{
		public ScaleManager()
		{
		}

		public const double BaseWidth = 375.0;
		public const double BaseHeight = 667.0;

		public static double AndroidDensity { get; set; }
		public static double ScreenWidth { get; set; }
		public static double ScreenHeight { get; set; }

		public static double NavigationHeight { get; set; }


		public static double Scale
		{
			get
			{
				System.Diagnostics.Debug.WriteLine("SW:" + WidthScale + "  SH:" + HeightScale);
				//return Math.Min(WidthScale, HeightScale);
				return Math.Max(WidthScale, HeightScale);
			}
		}
		public static double WidthScale
		{
			get
			{
				return ScreenWidth / BaseWidth;
			}
		}
		public static double HeightScale
		{
			get
			{
				return ScreenHeight / BaseHeight;
			}
		}

		public static double SizeSet(double baseSize)
		{
			if (ScaleManager.Scale > 1.0)
			{
				return baseSize;
			}
			return baseSize * ScaleManager.Scale;
		}
	}
}
