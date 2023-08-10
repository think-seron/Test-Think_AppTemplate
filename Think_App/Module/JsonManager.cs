using System;
using System.Diagnostics;
using Newtonsoft.Json;

namespace Think_App
{
	public static class JsonManager
	{
		public static string Serialize(object obj)
		{
			try
			{
				return JsonConvert.SerializeObject(obj);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				return null;
			}
		}

		public static T Deserialize<T>(string json)
		{
			try
			{
				return JsonConvert.DeserializeObject<T>(json);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				return default(T);
			}
		}
	}
}
