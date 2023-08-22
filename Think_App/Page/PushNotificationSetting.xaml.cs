using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using IO.Swagger.Model;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
	public partial class PushNotificationSetting : ContentPage
	{
		PushNotificationSettingViewModel pushNotificationSettingViewModel;
		Dictionary<string, string> param;
		public PushNotificationSetting(ResponseHome responseHome, string json)
		{
			InitializeComponent();
			NavigationPage.SetBackButtonTitle(this, "");

			pushNotificationSettingViewModel = new PushNotificationSettingViewModel(responseHome);

			var resp = JsonConvert.DeserializeObject<ResponseSettingNotification>(json);
			param = new Dictionary<string, string> { { "deviceId", Config.Instance.Data.deviceId } };
			if (resp.Data.ReserveBeforeDay == true)
			{
				//param.Add("reserveBeforeDay", "true");
				param["reserveBeforeDay"] = "1";
				pushNotificationSettingViewModel.BdgReserveBefore1day.SwitchIsToggled = true;
			}
			else
			{
				param["reserveBeforeDay"] = "0";
				//param.Add("reserveBeforeDay", "false");
				pushNotificationSettingViewModel.BdgReserveBefore1day.SwitchIsToggled = false;
			}
			if (resp.Data.ReserveBeforeHour == true)
			{
				param["reserveBeforeHour"] = "1";
				pushNotificationSettingViewModel.BdgReserveBeforeHour.SwitchIsToggled = true;
			}
			else
			{
				param["reserveBeforeHour"] = "0";
				pushNotificationSettingViewModel.BdgReserveBeforeHour.SwitchIsToggled = false;
			}
			if (resp.Data.Notice == true)
			{
				param["notice"] = "1";
				pushNotificationSettingViewModel.BdgNotice.SwitchIsToggled = true;
			}
			else
			{
				param["notice"] = "0";
				pushNotificationSettingViewModel.BdgNotice.SwitchIsToggled = false;
			}
			if (resp.Data.Coupon == true)
			{
				param["coupon"] = "1";
				pushNotificationSettingViewModel.BdgTicket.SwitchIsToggled = true;
			}
			else
			{
				param["coupon"] = "0";
				pushNotificationSettingViewModel.BdgTicket.SwitchIsToggled = false;
			}
			if (resp.Data.Message == true)
			{
				param["message"] = "1";
				pushNotificationSettingViewModel.BdgMessage.SwitchIsToggled = true;
			}
			else
			{
				param["message"] = "0";
				pushNotificationSettingViewModel.BdgMessage.SwitchIsToggled = false;
			}

			this.BindingContext = pushNotificationSettingViewModel;
			App.customNavigationPage.IsRunning = false;

			bool osSetting = DependencyService.Get<INotificationService>().GetNotificationSetting();
			if (osSetting == false)
			{
				var str = "端末側のプッシュ通知設定を" + Environment.NewLine + "有効にしてください";
				Device.BeginInvokeOnMainThread(() =>
				{
					DisplayAlert(null, str, "OK");
				});
			}

			this.ReserveBefore1day.nSwitch.Toggled += async (sender, e) =>
			{
				if (App.ProcessManager.CanInvoke())
				{
					if (e.Value)
					{
						param["reserveBeforeDay"] = "1";
					}
					else
					{
						param["reserveBeforeDay"] = "0";
					}

					var respJson = await APIManager.Post("setting_notification_regist", param);
					if (respJson == null)
					{
						// 通信失敗時
						if (e.Value)
						{
							pushNotificationSettingViewModel.BdgReserveBefore1day.SwitchIsToggled = false;
						}
						else
						{
							pushNotificationSettingViewModel.BdgReserveBefore1day.SwitchIsToggled = true;
						}
						DependencyService.Get<IToast>().Show("通信エラー");
					}
					App.ProcessManager.OnComplete();
				}
			};

			this.ReserveBeforeHour.nSwitch.Toggled += async (sender, e) =>
			{
				if (App.ProcessManager.CanInvoke())
				{
					if (e.Value)
					{
						param["reserveBeforeHour"] = "1";
					}
					else
					{
						param["reserveBeforeHour"] = "0";
					}

					var respJson = await APIManager.Post("setting_notification_regist", param);
					if (respJson == null)
					{
						// 通信失敗時
						if (e.Value)
						{
							pushNotificationSettingViewModel.BdgReserveBeforeHour.SwitchIsToggled = false;
						}
						else
						{
							pushNotificationSettingViewModel.BdgReserveBeforeHour.SwitchIsToggled = true;
						}
						DependencyService.Get<IToast>().Show("通信エラー");
					}
					App.ProcessManager.OnComplete();
				}
			};

			this.Notice.nSwitch.Toggled += async (sender, e) =>
			{
				if (App.ProcessManager.CanInvoke())
				{
					if (e.Value)
					{
						param["notice"] = "1";
					}
					else
					{
						param["notice"] = "0";
					}

					var respJson = await APIManager.Post("setting_notification_regist", param);
					if (respJson == null)
					{
						// 通信失敗時
						if (e.Value)
						{
							pushNotificationSettingViewModel.BdgNotice.SwitchIsToggled = false;
						}
						else
						{
							pushNotificationSettingViewModel.BdgNotice.SwitchIsToggled = true;
						}
						DependencyService.Get<IToast>().Show("通信エラー");
					}
					App.ProcessManager.OnComplete();
				}
			};

			this.Ticket.nSwitch.Toggled += async (sender, e) =>
			{
				if (App.ProcessManager.CanInvoke())
				{
					if (e.Value)
					{
						param["coupon"] = "1";
					}
					else
					{
						param["coupon"] = "0";
					}

					var respJson = await APIManager.Post("setting_notification_regist", param);
					if (respJson == null)
					{
						// 通信失敗時
						if (e.Value)
						{
							pushNotificationSettingViewModel.BdgTicket.SwitchIsToggled = false;
						}
						else
						{
							pushNotificationSettingViewModel.BdgTicket.SwitchIsToggled = true;
						}
						DependencyService.Get<IToast>().Show("通信エラー");
					}
					App.ProcessManager.OnComplete();
				}
			};

			this.Message.nSwitch.Toggled += async (sender, e) =>
			{
				if (App.ProcessManager.CanInvoke())
				{
					if (e.Value)
					{
						param["message"] = "1";
					}
					else
					{
						param["message"] = "0";
					}

					var respJson = await APIManager.Post("setting_notification_regist", param);
					if (respJson == null)
					{
						// 通信失敗時
						if (e.Value)
						{
							pushNotificationSettingViewModel.BdgMessage.SwitchIsToggled = false;
						}
						else
						{
							pushNotificationSettingViewModel.BdgMessage.SwitchIsToggled = true;
						}
						DependencyService.Get<IToast>().Show("通信エラー");
					}
					App.ProcessManager.OnComplete();
				}
			};
		}
	}
}
