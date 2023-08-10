using System;
namespace Think_App
{
	public static class UuidManager
	{
		public static string GetUuidAsString(bool hasHyphen = false)
		{
			var guid = Guid.NewGuid();
			string format = hasHyphen ? "D" : "N";
			var guidStr = guid.ToString(format);

			System.Diagnostics.Debug.WriteLine("UUIDを生成しました: {0}", guidStr);

			return guidStr;
		}
	}
}
