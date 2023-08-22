using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
	public partial class SelectGenderPage : ContentPage
	{
		const double _infoLblBaseFontSize = 20;

		SelectGenderPageModel Model { get; set; }
		bool NeedsRestartFadeImages { get; set; }

		public SelectGenderPage()
		{
			InitializeComponent();

			// このページに戻るときにタイトルを表示しない。
			NavigationPage.SetBackButtonTitle(this, "");

			Model = new SelectGenderPageModel()
			{
				ScreenSizeScale = ScaleManager.Scale,
				FemaleBtnCommand = new Command(FemaleBtn_Clicked),
				MaleBtnCommand = new Command(MaleBtn_Clicked),
				InfoLblFontSize = _infoLblBaseFontSize * ScaleManager.Scale,
				FemaleBtnEnable = true,
				MaleBtnEnable = false,  // このバージョンでは無効
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

		async void FemaleBtn_Clicked()
		{
			if (!App.ProcessManager.CanInvoke())
			{
				return;
			}
			// ストレージに性別を保存する。
			await RegisterGender(Gender.Female);
			await Navigation.PushAsync(new SelectPhotoSourcePage());
			App.ProcessManager.OnComplete();
		}

		async void MaleBtn_Clicked()
		{
			if (!App.ProcessManager.CanInvoke())
			{
				return;
			}
			await RegisterGender(Gender.Male);
			await Navigation.PushAsync(new SelectPhotoSourcePage());
			App.ProcessManager.OnComplete();
		}

		async Task RegisterGender(Gender gender)
		{
			await GenderManager.SaveGenderAsync(gender);
		}
	}
}
