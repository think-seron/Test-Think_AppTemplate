using System;

namespace Think_App
{
	public class VersionUpdate : Response
	{
		public Contents Data { get; set; }
		public class Contents
		{
			public int Force { get; set; }
			public string Stores { get; set; }
		}
	}
}
