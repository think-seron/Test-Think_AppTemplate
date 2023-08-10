using System;
namespace Think_App
{
	public static class IntExtension
	{
		public static int Clamp(this int self, int min, int max)
		{
			return Math.Max(min, Math.Min(max, self));
		}
	}
}
