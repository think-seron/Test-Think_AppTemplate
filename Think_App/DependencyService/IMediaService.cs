using System;
using System.Collections.Generic;

namespace Think_App
{
	public interface IMediaService
	{
		//void GetImagePass();

		List<string> GetImagePass();
		byte[] PathChangeByte(string path);
		//List<byte[]> GetImageByte();
		void GetImageByte();
	}
}
