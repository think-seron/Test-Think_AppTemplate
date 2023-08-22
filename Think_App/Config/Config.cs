using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
namespace Think_App
{
	public class Config
	{
		public Config()
		{
			Data = new ConfigData();
		}
		static Config _Instance;
		public static Config Instance
		{
			get
			{
				if (_Instance == null)
				{
					_Instance = new Config();
				}
				return _Instance;
			}
			set
			{
				_Instance = value;
			}
		}

		string _deviceCode;
		public ConfigData Data { get; set; }


		//登録時に投げるデバイス識別Id
		public string deviceCode
		{
			get
			{
				if (_deviceCode == null
				   //ここにファイルからの読み込み処理も
				   //isFileExist == false 
				   //みたいな感じ
				   )
				{
					_deviceCode = DependencyService.Get<IDeviceInfoService>().GetDeviceCode();
				}
				//ファイルがあったらこっちで中身を呼び出す
				//else if(isFileExist ==true){
				//var data = FileManager.ReadAsync();
				//_deviceCode=data.なんちゃら
				//}
				return _deviceCode;
			}
		}
	}
}