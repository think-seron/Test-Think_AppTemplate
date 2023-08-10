using System;
namespace Think_App
{
	public class LifeCyclePayload
	{
		public LifeCycle Status { get; set; }
	}

	public enum LifeCycle
	{
		OnStart,
		OnSleep,
		OnResume	
	}
}
