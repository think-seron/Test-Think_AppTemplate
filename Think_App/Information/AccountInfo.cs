using System;
namespace Think_App
{
	public class AccountInfo
	{
		public AccountInfo()
		{
		}

		public string name { get; set;}
		public string kana { get; set;}
		public string tel { get; set; }
		public string email { get; set;}
		public int gender { get; set;} // 0:指定なし 男:1 女:2
	}
}
