using System;
namespace Think_App
{
	public static class DoubleExtension
	{
		public static double Clamp(this double self, double min, double max)
		{
			return Math.Max(min, Math.Min(max, self));
		}
	}
}
