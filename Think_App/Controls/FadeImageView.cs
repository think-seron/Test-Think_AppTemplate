using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Maui.Layouts;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
	public class FadeImageView : AbsoluteLayout
	{
		public class FadeInfo
		{
			public ImageSource ImageSource { get; private set; }
			public Aspect Aspect { get; private set; }
			public FadeInfo(ImageSource imageSource, Aspect aspect = Aspect.AspectFill)
			{
				ImageSource = imageSource;
				Aspect = aspect;
			}
		}

		#region FadeInfoList BindableProperty
		public static readonly BindableProperty FadeInfoListProperty =
			BindableProperty.Create(nameof(FadeInfoList), typeof(List<FadeInfo>), typeof(FadeImageView), null,
				propertyChanged: (bindable, oldValue, newValue) =>
					((FadeImageView)bindable).FadeInfoList = (List<FadeInfo>)newValue);

		public List<FadeInfo> FadeInfoList
		{
			get { return (List<FadeInfo>)GetValue(FadeInfoListProperty); }
			set { SetValue(FadeInfoListProperty, value); }
		}
		#endregion

		#region FadeTime BindableProperty
		public static readonly BindableProperty FadeTimeProperty =
			BindableProperty.Create(nameof(FadeTime), typeof(int), typeof(FadeImageView), 5000,
				propertyChanged: (bindable, oldValue, newValue) =>
					((FadeImageView)bindable).FadeTime = (int)newValue);

		public int FadeTime
		{
			get { return (int)GetValue(FadeTimeProperty); }
			set { SetValue(FadeTimeProperty, value); }
		}
		#endregion

		private List<View> ImageList { get; set; }

		private int ImageIndex = 0;

		private CancellationTokenSource TokenSource { get; set; }
		private bool IsLoop;

		public FadeImageView()
		{
			ImageList = new List<View>();
		}

		public async Task StartFadeImagesAsync(bool refreshImages = false)
		{
			InitImages(refreshImages);
			TokenSource = new CancellationTokenSource();
			IsLoop = true;
			await StartFadeImagesMainAsync();
		}

		public void InitImages(bool refreshImages)
		{
			if (ImageList.Count > 0 && !refreshImages) { return; }

			System.Diagnostics.Debug.WriteLine("ImageListの初期化を行います。");

			ImageList.Clear();
			this.Children.Clear();

			if (FadeInfoList != null)
			{
				for (int i = 0; i < FadeInfoList.Count; ++i)
				{
					var info = FadeInfoList[i];
					var image = new Image()
					{
						Source = info.ImageSource,
						Aspect = info.Aspect,
						Opacity = (i == 0) ? 1.0 : 0.0
					};
					ImageList.Add(image);
					this.Children.Add(image, new Rect(0, 0, 1, 1), AbsoluteLayoutFlags.All);
				}
			}
		}

		private async Task StartFadeImagesMainAsync()
		{
			int maxCount = ImageList.Count;

			// 2枚以上画像がないと何もしない。
			if (maxCount < 2) { return; }

			// ロード待ち。
			bool isLoading = true;
			while (isLoading)
			{
				isLoading = false;
				foreach (var view in ImageList)
				{
					var image = view as Image;
					if (image.IsLoading)
					{
						isLoading = true;
						System.Diagnostics.Debug.WriteLine("ロード中！");
						break;
					}
				}
				if (isLoading)
				{
					// ウェイト
					await Task.Delay(10);
				}
			}

			// フェード処理。
			System.Diagnostics.Debug.WriteLine("FadeImageView: フェード開始。");
			try
			{
				while (IsLoop)
				{
					var image0 = ImageList[ImageIndex];
					ImageIndex = (ImageIndex < maxCount - 1) ? ImageIndex + 1 : 0;
					var image1 = ImageList[ImageIndex];
					var task0 = FadeView(image0, FadeTime, Fade.Out, TokenSource.Token);
					var task1 = FadeView(image1, FadeTime, Fade.In, TokenSource.Token);
					await Task.WhenAll(new Task[] { task0, task1 });
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine("FadeImageView: タスクがキャンセルされました。: " + ex.Message);
			}
		}

		private enum Fade
		{
			In,
			Out
		}

		private async Task FadeView(View view, int length, Fade fade, CancellationToken token)
		{
			double opacity = (fade == Fade.In) ? 1.0 : 0.0;

			token.ThrowIfCancellationRequested();
			await view.FadeTo(opacity, (uint)length).ConfigureAwait(false);
			token.ThrowIfCancellationRequested();
		}

		public void FinishFadeImages()
		{
			System.Diagnostics.Debug.WriteLine("FadeImageView: フェード終了。");
			if (TokenSource != null)
			{
				TokenSource.Cancel();
			}
			IsLoop = false;
		}
	}
}
