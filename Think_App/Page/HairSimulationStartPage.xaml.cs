using System;
using System.Collections.Generic;
using Xamarin.Forms;
using IO.Swagger.Model;
using System.Threading.Tasks;

namespace Think_App
{
	public partial class HairSimulationStartPage : ContentPage
	{
		static bool IsLoaded;
		const double _infoLblBaseFontSize = 20;
		const double _loadingDataViewBaseFontSize = 20;
		const double _attentionLblBaseFontSize = 12;

		HairSimulationStartPageModel Model { get; set; }
		bool NeedsRestartFadeImages { get; set; }

		public HairSimulationStartPage()
		{
			InitializeComponent();

			// このページに戻るときにタイトルを表示しない。
			NavigationPage.SetBackButtonTitle(this, "");

			this.AttentionLbl.Text = "ヘアシミュレーションのヘアスタイル画像は、ヘアスタイルショップ（http://hair-style.photo/）より提供されています。";
			this.AttentionLbl.Text += Environment.NewLine;
			this.AttentionLbl.Text += "これらの画像は無断転載禁止です。";

			// Binding
			Model = new HairSimulationStartPageModel()
			{
				ScreenSizeScale = ScaleManager.Scale,
				BeginBtnCommand = new Command(OnBeginBtnClicked),
				LookSavedImagesBtnCommand = new Command(OnLookSavedImagesBtnClicked),
				InfoLblFontSize = _infoLblBaseFontSize * ScaleManager.Scale,
				AttentionLblFontSize = _attentionLblBaseFontSize * ScaleManager.Scale,
				BGFadeImageViewInfoList = GetFadeInfoList()
			};
			this.BindingContext = Model;

			Task.Run(() =>
			{
				Device.BeginInvokeOnMainThread(async () =>
				{
					await this.BGFadeImageView.StartFadeImagesAsync();
				});
			});

			if (!IsLoaded)
			{
				LoadInitData();
			}
		}

		List<FadeImageView.FadeInfo> GetFadeInfoList()
		{
			var list = new List<FadeImageView.FadeInfo>();
			list.Add(new FadeImageView.FadeInfo("bg_hairsimulation_01.png"));
			list.Add(new FadeImageView.FadeInfo("bg_hairsimulation_02.png"));
			list.Add(new FadeImageView.FadeInfo("bg_hairsimulation_03.png"));
			return list;
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();

			if (NeedsRestartFadeImages)
			{
				await this.BGFadeImageView.StartFadeImagesAsync();
			}
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();

			this.BGFadeImageView.FinishFadeImages();
			NeedsRestartFadeImages = true;
		}

		async void OnBeginBtnClicked()
		{
			if (!IsLoaded)
			{
				return;
			}

			if (App.ProcessManager.CanInvoke())
			{
				await this.Navigation.PushAsync(new SelectGenderPage());
				App.ProcessManager.OnComplete();
			}
		}

		async void OnLookSavedImagesBtnClicked()
		{
			if (!IsLoaded)
			{
				return;
			}

			if (App.ProcessManager.CanInvoke())
			{
				await this.Navigation.PushAsync(new SavedDataGalleryPage());
				App.ProcessManager.OnComplete();
			}
		}

		void LoadInitData()
		{
			// サムネイルとして適切なサイズになるように、ざっくりサイズを計算しておく。
			var service = DependencyService.Get<IScreenService>();
			var screenWidth = service.GetScreenWidth();
			var thumbnailSize = screenWidth / 5.0;

			// ここで写真の読み込みと保存を行う。dd
			var awaiter = APIManager.GET("wig_all").GetAwaiter();
			awaiter.OnCompleted(async () =>
			{
				try
				{
					var json = awaiter.GetResult();
					var response = JsonManager.Deserialize<ResponseWigAll>(json);

					int fileCount = 0;
					var loadedUrlList = new List<string>();
					if (response.Data.WomanWigList != null && response.Data.WomanWigList.Count > 0)
					{
						foreach (var info in response.Data.WomanWigList)
						{
							if (info.WigList != null && info.WigList.Count > 0)
							{
								foreach (var wig in info.WigList)
								{
									// ファイルがローカルストレージに読まれているかチェック。
									var isExist = await ImageManager.IsExistImageInLocalStorageByUrlAsync(wig.Image.Path, true);
									if (isExist)
									{
										// 読まれていたら、読まれていたUrlのリストに追加。
										loadedUrlList.Add(wig.Image.Path);
									}
									else
									{
										// 読まれていなければ、読み取るべきファイル数をカウントアップ。
										fileCount++;
									}
								}
							}
						}
					}

					if (response.Data.ManWigList != null && response.Data.ManWigList.Count > 0)
					{
						foreach (var info in response.Data.ManWigList)
						{
							if (info.WigList != null && info.WigList.Count > 0)
							{
								foreach (var wig in info.WigList)
								{
									// ファイルがローカルストレージに読まれているかチェック。
									var isExist = await ImageManager.IsExistImageInLocalStorageByUrlAsync(wig.Image.Path, true);
									if (isExist)
									{
										// 読まれていたら、読まれていたUrlのリストに追加。
										loadedUrlList.Add(wig.Image.Path);
									}
									else
									{
										// 読まれていなければ、読み取るべきファイル数をカウントアップ。
										fileCount++;
									}
								}
							}
						}
					}

					System.Diagnostics.Debug.WriteLine("未読み込みデータ数:{0}", fileCount);

					if (fileCount == 0)
					{
						System.Diagnostics.Debug.WriteLine("全ファイル読み込み済みです！");
						IsLoaded = true;
						return;
					}
					else
					{
						// 初期データダウンロード中のメッセージの表示開始。
						var nl = Environment.NewLine;
						var model = new LoadingDataViewModel()
						{
							LoadingMessageLblText = "初期データをダウンロード中です" + nl + "しばらくお待ちください",
							CommonFontSize = _loadingDataViewBaseFontSize * ScaleManager.Scale
						};
						var view = new LoadingDataView() { BindingContext = model, ContentsCount = fileCount };
						this.AbsLayout.Children.Add(view, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);
						var navPage = this.Parent as CustomNavigationPage;
						navPage?.UpdateShadow(true);

						if (response.Data.WomanWigList != null && response.Data.WomanWigList.Count > 0)
						{
							foreach (var wigInfo in response.Data.WomanWigList)
							{
								if (wigInfo.WigList != null && wigInfo.WigList.Count > 0)
								{
									foreach (var wig in wigInfo.WigList)
									{
										// ファイルがローカルストレージに読まれているかチェック。
										var isLoaded = loadedUrlList.Contains(wig.Image.Path);
										if (!isLoaded)
										{
											// 画像読み込みとストレージへの保存。
											// 画像が大きすぎるので、サムネイル用に小さな画像を保存する。
											await ImageManager.SaveImageToLocalStorageByUrlAsync(wig.Image.Path, true, false, true, thumbnailSize);
											// ビューに読み込みを通知。
											 view.LoadContents(1);
										}
									}
								}
							}
						}
						if (response.Data.ManWigList != null && response.Data.ManWigList.Count > 0)
						{
							foreach (var wigInfo in response.Data.ManWigList)
							{
								if (wigInfo.WigList != null && wigInfo.WigList.Count > 0)
								{
									foreach (var wig in wigInfo.WigList)
									{
										// ファイルがローカルストレージに読まれているかチェック。
										var isLoaded = loadedUrlList.Contains(wig.Image.Path);
										if (!isLoaded)
										{
											// 画像読み込みとストレージへの保存。
											// 画像が大きすぎるので、サムネイル用に小さな画像を保存する。
											await ImageManager.SaveImageToLocalStorageByUrlAsync(wig.Image.Path, true, false, true, thumbnailSize);
											// ビューに読み込みを通知。
											view.LoadContents(1);
										}
									}
								}
							}
						}

                        // 全データの読み込みが終了したところで、ビューを取り除く。
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            this.AbsLayout.Children.Remove(view);
                            navPage?.UpdateShadow(false);
                            IsLoaded = true;
                        });
					}
				}
				catch (Exception ex)
				{
					System.Diagnostics.Debug.WriteLine(ex);
					//! ここは読み込みをちゃんとやってもらわないと、今後の表示が重くなるのでリトライさせたい。
					var retry = await DisplayAlert("通信エラー", "初期データ読み込みに失敗しました。電波状況などを確認して、通信をやり直しますか？", "再試行", "キャンセル");
					if (retry)
					{
						LoadInitData();
					}
					else
					{
						// ロードを諦める。（諦めても表示が重いだけで今後に問題はない）
						IsLoaded = true;
					}
				}
			});
		}
	}
}
