using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Think_App;
using IO.Swagger.Model;
using System.Threading.Tasks;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
	public class ReservationRegistViewModel : ViewModelBase
	{
		public ReservationRegistViewModel(ReservationContentInfo content, ResponseReservationPoint response)
		{

			ScreenSizeScale = ScaleManager.Scale;
			System.Diagnostics.Debug.WriteLine(" scale :" + ScreenSizeScale);

			ReservationContent = content;

			ReservationDate = ReservationContent.Date.ToString("yy/MM/dd") + "（" + ReservationContent.Date.ToString("ddd") + "）" + ReservationContent.Date.ToString("HH") + ":" + ReservationContent.Date.ToString("mm");
			System.Diagnostics.Debug.WriteLine("ReservationDate:" + ReservationDate);
			ReservationStore = ReservationContent.StoreName;
			ReservationStyList = ReservationContent.StaffName;
			ReservationMenu = ReservationContent.MenuName;

			if (ReservationContent.MenuId != 0)
			{
				ReservationUsingCoupon = "無";
				ReservationCouponContent = null;
				ReservationCouponContentVisible = false;
			}
			else
			{
				ReservationUsingCoupon = "有";
				ReservationCouponContent = ReservationContent.CouponContent;
				ReservationCouponContentVisible = true;
			}

			ReservationUserName = new FormattedString
			{
				Spans = {
				new Span {
						Text="お名前："+ReservationContent.Account.name
				},
				new Span{
					Text="（"+ReservationContent.Account.kana+"）"
				}
			},
			};


			ReservationUserTelNumber = new FormattedString
			{
				Spans = { new Span {
					Text="TEL       ："+ReservationContent.Account.tel
				}
			},
			};
			//var resultAwaiter = FileManager.ReadJsonFileAsync<AccountInfo>("Account", "accountInfo").GetAwaiter();
			//resultAwaiter.OnCompleted(() =>
			//{
			//	System.Diagnostics.Debug.WriteLine("str :" + resultAwaiter.GetResult().name);
			//	System.Diagnostics.Debug.WriteLine("str :" + resultAwaiter.GetResult().kana);
			//	System.Diagnostics.Debug.WriteLine("str :" + resultAwaiter.GetResult().tel);
			//	if (resultAwaiter.GetResult() != null)
			//	{
			//		ReservationUserName = new FormattedString
			//		{
			//			Spans = {
			//				new Span {
			//					Text="お名前："+resultAwaiter.GetResult().name
			//				},
			//				new Span{
			//					Text="（"+resultAwaiter.GetResult().kana+"）"
			//				}
			//			},
			//		};


			//		ReservationUserTelNumber = new FormattedString
			//		{
			//			Spans = { new Span {
			//					Text="TEL       ："+resultAwaiter.GetResult().tel
			//				}
			//			},
			//		};
			//	}
			//});

			string point = null;

			if (response.Data.Points != null)
			{
				foreach (var n in response.Data.Points)
				{
					if (!string.IsNullOrEmpty(point))
					{
						point = point + " , ";
					}
					if (!string.IsNullOrEmpty(n.Name))
						point = point + n.Name + " :" + n.Value + n.Unit;
				}

				ReservationUserPoint = new FormattedString
				{
					Spans = {
					new Span{Text=point},
				}
				};
			}
			SetBtns();
		}

		public void UpdateUserSettings(string name, string kana, string telNum)
		{
			ReservationUserName = new FormattedString
			{
				Spans = {
							new Span {
								Text="お名前："+name
							},
							new Span{
								Text="（"+kana+"）"
							}
						},
			};

			ReservationUserTelNumber = telNum;

		}


		bool IsResisted = false;

		void SetBtns()
		{
			System.Diagnostics.Debug.WriteLine("set btns  :");
			EditAccountInfoCommand = new Command(async () =>
						{
							System.Diagnostics.Debug.WriteLine("account tap");
							if (App.ProcessManager.CanInvoke())
							{
								await App.customNavigationPage.PushAsync(new AccountRegistration(4));
								App.ProcessManager.OnComplete();
							}


						});
			//EditAccountBtnBC = new WhiteButtonViewModel("アカウント情報を変更する", accountCommand);

			StartReservationCommand = new Command(async () =>
						{
							App.customNavigationPage.IsRunning = true;
							System.Diagnostics.Debug.WriteLine("regCommand  tap");
							if (App.ProcessManager.CanInvoke())
							{
								var dic = new Dictionary<string, string>();
								dic.Add("salonId", ReservationContent.SalonId.ToString());
								dic.Add("staffId", ReservationContent.StaffId.ToString());
								if (ReservationContent.CouponId == null || ReservationContent.CouponId == 0)
								{
									dic.Add("salonMenuId", ReservationContent.MenuId.ToString());
								}
								else
								{
									dic.Add("couponId", ReservationContent.CouponId.ToString());
								}
								dic.Add("date", ReservationContent.Date.ToString("g", new System.Globalization.CultureInfo("ja-JP")));
								dic.Add("memo", EditorText);

								try
								{
									var jsonRegist = await APIManager.Post("reservation__regist", dic);
									var responseRegist = JsonManager.Deserialize<ResponseBase>(jsonRegist);


									System.Diagnostics.Debug.WriteLine("  response json : " + jsonRegist);

									System.Diagnostics.Debug.WriteLine("response status :" + responseRegist.Status + " response message  " + responseRegist.Message);

									//登録エラー

									if (responseRegist.Status == 0)
									{

										var confirmationodalView = new ModalView();
										confirmationodalView.modalViewViewModel.ModalLabelTxt = "予約されました";
										confirmationodalView.modalViewViewModel.NomalModalLabelRect = new Rect(0.5, 0.4, 1, AbsoluteLayout.AutoSize);
										confirmationodalView.modalViewViewModel.OKBtnLayoutBounds = new Rect(0.5, 0.6, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize);
								

										var currentApp = Microsoft.Maui.Controls.Application.Current;
										confirmationodalView.okButton.Clicked += async (okSender, okE) =>
																	{
																		if (!IsResisted)
																		{
																			IsResisted = true;
																			if (currentApp != Microsoft.Maui.Controls.Application.Current)
																			{
																				IsResisted = false;
																				App.ProcessManager.OnComplete();
																				throw new InvalidOperationException("Application.Current changed");

																				return;
																			}

																			string json = null;
																			ResponseHome param = null;

																			while (string.IsNullOrEmpty(json))
																			{
																				json = await APIManager.GET("home");

																				param = JsonManager.Deserialize<ResponseHome>(json);
																				System.Diagnostics.Debug.WriteLine("Home:" + json);
																				if (string.IsNullOrEmpty(json) || param == null)
																				{
																					await App.Current.MainPage.DisplayAlert("通信エラー", "通信環境の良い場所で、時間をおいて改めてログインを行ってください", "OK");
																					await DialogManager.Instance.HideView();
																				}
																			}

																			try
																			{
																				Device.BeginInvokeOnMainThread(async () =>
																				{
																					await App.customNavigationPage.PushAsync(new Home(param), false);
                                                                                    await DialogManager.Instance.HideView();
                                                                                });
																			}
																			catch (Exception ex)
																			{
																				System.Diagnostics.Debug.WriteLine(" ex : " + ex);
																				bool jsonDataIsNormal = false;
																				while (jsonDataIsNormal)
																				{
																					json = await APIManager.GET("home");

																					param = JsonManager.Deserialize<ResponseHome>(json);
																					System.Diagnostics.Debug.WriteLine("Home:" + json);
																					if (string.IsNullOrEmpty(json))
																					{
																						await App.Current.MainPage.DisplayAlert("通信データエラー", "通信環境の良い場所で、時間をおいて改めてログインを行ってください", "OK");
																					}
																					else
																					{
																						jsonDataIsNormal = true;
																					}
																				}
                                                                                await DialogManager.Instance.HideView();
                                                                            }

																			IsResisted = false;
																			App.ProcessManager.OnComplete();
																		}
																	};
										App.customNavigationPage.IsRunning = false;
										await DialogManager.Instance.ShowDialogView(confirmationodalView);

									}
									else
									{
										DependencyService.Get<IToast>().Show("登録エラー" + System.Environment.NewLine + responseRegist.Message);
										App.ProcessManager.OnComplete();
										App.customNavigationPage.IsRunning = false;
										return;
									}
								}
								catch (Exception ex)
							{
								System.Diagnostics.Debug.WriteLine(" ex :" + ex);
								await App.customNavigationPage.Navigation.NavigationStack.Last().DisplayAlert("登録エラー", "通信環境の良い場所で、初めから登録をやり直してください。", "OK");

							}
							App.ProcessManager.OnComplete();
							App.customNavigationPage.IsRunning = false;
						}
						});
			//RegistReservationBtnBC = new StandardButtonViewModel("この内容で登録する");
		}

	async void PushHome()
	{
		//string json = null;
		//ResponseHome param = null;
		//while (string.IsNullOrEmpty(json))
		//{
		//	json = await APIManager.GET("home");

		//	param = JsonManager.Deserialize<ResponseHome>(json);
		//	System.Diagnostics.Debug.WriteLine("Home:" + json);
		//	if (string.IsNullOrEmpty(json) || param == null)
		//	{
		//		await App.Current.MainPage.DisplayAlert("通信エラー", "通信環境の良い場所で、時間をおいて改めてログインを行ってください", "OK");
		//	}
		//}
		//try
		//{
		//	Device.BeginInvokeOnMainThread(async () =>
		//	{
		//		await App.customNavigationPage.PushAsync(new Home(param), false);
		//	});
		//}
		//catch (Exception ex)
		//{
		//	System.Diagnostics.Debug.WriteLine(" ex : " + ex);
		//	bool jsonDataIsNormal = false;
		//	while (jsonDataIsNormal)
		//	{
		//		json = await APIManager.GET("home");

		//		param = JsonManager.Deserialize<ResponseHome>(json);
		//		System.Diagnostics.Debug.WriteLine("Home:" + json);
		//		if (string.IsNullOrEmpty(json))
		//		{
		//			await App.Current.MainPage.DisplayAlert("通信データエラー", "通信環境の良い場所で、時間をおいて改めてログインを行ってください", "OK");
		//		}
		//		else
		//		{
		//			jsonDataIsNormal = true;
		//		}
		//	}
		//}
	}


	ReservationContentInfo ReservationContent { get; set; }

	public double ScreenSizeScale { get; set; }
	public string ReservationDate { get; set; }
	public string ReservationStore { get; set; }
	public string ReservationStyList { get; set; }
	public string ReservationMenu { get; set; }
	public string ReservationUsingCoupon { get; set; }
	public string ReservationCouponContent { get; set; }
	public bool ReservationCouponContentVisible { get; set; }

	//public string ReservationUserName { get; set; }
	//public string ReservationUserfurigana { get; set; }

	//public string ReservationUserTelNumber { get; set; }

	private FormattedString _ReservationUserName;
	public FormattedString ReservationUserName
	{
		get { return _ReservationUserName; }
		set
		{
			if (_ReservationUserName != value)
			{
				_ReservationUserName = value;
				OnPropertyChanged("ReservationUserName");
			}
		}
	}

	private FormattedString _ReservationUserTelNumber;
	public FormattedString ReservationUserTelNumber
	{
		get { return _ReservationUserTelNumber; }
		set
		{
			if (_ReservationUserTelNumber != value)
			{
				_ReservationUserTelNumber = value;
				OnPropertyChanged("ReservationUserTelNumber");
			}
		}
	}

	private FormattedString _ReservationUserPoint;
	public FormattedString ReservationUserPoint
	{
		get { return _ReservationUserPoint; }
		set
		{
			if (_ReservationUserPoint != value)
			{
				_ReservationUserPoint = value;
				OnPropertyChanged("ReservationUserPoint");
			}
		}
	}

	//private WhiteButtonViewModel _EditAccountBtnBC;
	//public WhiteButtonViewModel EditAccountBtnBC
	//{
	//	get { return _EditAccountBtnBC; }
	//	set
	//	{
	//		if (_EditAccountBtnBC != value)
	//		{
	//			_EditAccountBtnBC = value;
	//			OnPropertyChanged("EditAccountBtnBC");
	//		}
	//	}
	//}
	//private StandardButtonViewModel _RegistReservationBtnBC;
	//public StandardButtonViewModel RegistReservationBtnBC
	//{
	//	get { return _RegistReservationBtnBC; }
	//	set
	//	{
	//		if (_RegistReservationBtnBC != value)
	//		{
	//			_RegistReservationBtnBC = value;
	//			OnPropertyChanged("RegistReservationBtnBC");
	//		}
	//	}
	//}
	private string _EditorText;
	public string EditorText
	{
		get { return _EditorText; }
		set
		{
			if (_EditorText != value)
			{
				_EditorText = value;
				OnPropertyChanged("EditorText");
			}
		}
	}

	private Command _EditAccountInfoCommand;
	public Command EditAccountInfoCommand
	{
		get { return _EditAccountInfoCommand; }
		set
		{
			if (_EditAccountInfoCommand != value)
			{
				_EditAccountInfoCommand = value;
				OnPropertyChanged("EditAccountInfoCommand");
			}
		}
	}
	private Command _StartReservationCommand;
	public Command StartReservationCommand
	{
		get { return _StartReservationCommand; }
		set
		{
			if (_StartReservationCommand != value)
			{
				_StartReservationCommand = value;
				OnPropertyChanged("StartReservationCommand");
			}
		}
	}
}
}