using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using IO.Swagger.Model;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
	public partial class NoticePage : ContentPage
	{
		NoticePageViewModel noticePageViewModel;
		public NoticePage(string title, string description, int? noticeId, bool isread)
		{
			InitializeComponent();
			NavigationPage.SetBackButtonTitle(this, "");

			noticePageViewModel = new NoticePageViewModel();

			// ----------------------------------------------
			// 選択したお知らせの情報を取得し表示
			noticePageViewModel.NoticeTitle = title;
			noticePageViewModel.NoticeContents = description;
			noticePageViewModel.NoticeTitleFontSize = ScaleManager.SizeSet(13);
			noticePageViewModel.NoticeContentsFontSize = ScaleManager.SizeSet(17);
			noticePageViewModel.ScrollViewRect = new Rect(0, 81, 1, ScaleManager.ScreenHeight - 81);
			//if (Device.RuntimePlatform == Device.iOS)
			//{
			//	noticePageViewModel.ScrollViewRect = new Rectangle(0, 146, 1, ScaleManager.ScreenHeight - 146);
			//}
			//else if (Device.RuntimePlatform == Device.Android)
			//{
			//	noticePageViewModel.ScrollViewRect = new Rectangle(0, 126, 1, ScaleManager.ScreenHeight- 126);
			//}
			// ----------------------------------------------
			this.BindingContext = noticePageViewModel;

			if (noticeId != null && isread == false)
			{
				// 既読登録
				Task.Run(async () =>
				{
					string action = "notice_read_regist";
					var parameters = new Dictionary<string, string> {
						{ "deviceId", Config.Instance.Data.deviceId },
						{ "noticeId", noticeId.ToString() }
					};
					var apiRet = await APIManager.Post(action, parameters);
				});
			}
		}
	}
}
