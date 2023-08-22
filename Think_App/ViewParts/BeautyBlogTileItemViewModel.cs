using System;
using IO.Swagger.Model;
using System.Threading.Tasks;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
namespace Think_App
{
	public class BeautyBlogTileItemViewModel : ViewModelBase
	{
		public BeautyBlogTileItemViewModel(InlineResponse20013DataMyBeautyBlogList listData = null)
		{
			UpdateData(listData);
		}


		public async Task UpdateData(InlineResponse20013DataMyBeautyBlogList listData = null)
		{
			await Task.Run(() =>
			{
				SetSizes();
				if (listData != null)
				{
					ItemIsVisible = true;
					MyBeautyBlogId = (int)listData.MyBeautyBlogId;
					BlogImageSouce = listData.ThumbnailImage.Path;
					ImageDateStringrShort = listData.Date.Replace("-", "/").Substring(0, 10);

					myBeautyViewModel = new MyBeautyBlogModel()
					{
						BtnIsVisible = false,
						MyBeautyBlogId = (int)listData.MyBeautyBlogId,
						ImgSouce = listData.ThumbnailImage.Path,
						DateStr = listData.Date.Replace("-", "/"),
						DateStringrShort = listData.Date.Replace("-", "/").Substring(0, 10),
						CategoryValue = listData.Category.Value,
						Title = listData.Title,
						Description = listData.Description,
						GridViewImgRect = new Rect(0, 0, 1, 1),
						GridViewLabelRect = new Rect(0.5, 1, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize),
						GridViewLabelFontSize = 12 * ScaleManager.Scale,
						GridViewLabelHeightRequest = 14 * ScaleManager.Scale,
						GridViewLabelWidthRequest = 69 * ScaleManager.Scale
					};
				}
				else
				{
					ItemIsVisible = false;
				}
				SetCoomand();
			});
		}


		public async Task ClearData()
		{
			await Task.Run(() =>
						{
							SetSizes();

							
							ItemIsVisible = false;
							SetCoomand();
						});
		}

		public void SetCoomand()
		{
			OnImageClickCommand = new Command(async () =>
			{
				if (App.ProcessManager.CanInvoke())
				{
					if (!ItemIsVisible)
					{
						App.ProcessManager.OnComplete();
						return;
					}
					await App.customNavigationPage.PushAsync(new MyBeautyBlogInfoPage(myBeautyViewModel));
					App.ProcessManager.OnComplete();
				}
			});
		}

		public void SetSizes()
		{
			BlogImageFontSize = ScaleManager.SizeSet(12);
			DateLabelWidth = ScaleManager.SizeSet(69);
			BlogImageSize = ScaleManager.SizeSet(100);
			DateLabelHeight = ScaleManager.SizeSet(14);
		}

		public MyBeautyBlogModel myBeautyViewModel { get; set; }



		public int MyBeautyBlogId { get; set; }
		private double _BlogImageSize;
		public double BlogImageSize
		{
			get { return _BlogImageSize; }
			set
			{
				if (_BlogImageSize != value)
				{
					_BlogImageSize = value;
					OnPropertyChanged("BlogImageSize");
				}
			}
		}

		private string _BlogImageSouce;
		public string BlogImageSouce
		{
			get { return _BlogImageSouce; }
			set
			{
				if (_BlogImageSouce != value)
				{
					_BlogImageSouce = value;
					OnPropertyChanged("BlogImageSouce");
				}
			}
		}


		private string _ImageDateStringrShort;
		public string ImageDateStringrShort
		{
			get { return _ImageDateStringrShort; }
			set
			{
				if (_ImageDateStringrShort != value)
				{
					_ImageDateStringrShort = value;
					OnPropertyChanged("ImageDateStringrShort");
				}
			}
		}

		private bool _ItemIsVisible;
		public bool ItemIsVisible
		{
			get { return _ItemIsVisible; }
			set
			{
				if (_ItemIsVisible != value)
				{
					_ItemIsVisible = value;
					OnPropertyChanged("ItemIsVisible");
				}
			}
		}

		private double _DateLabelWidth;
		public double DateLabelWidth
		{
			get { return _DateLabelWidth; }
			set
			{
				if (_DateLabelWidth != value)
				{
					_DateLabelWidth = value;
					OnPropertyChanged("DateLabelWidth");
				}
			}
		}

		private double _DateLabelHeight;
		public double DateLabelHeight
		{
			get { return _DateLabelHeight; }
			set
			{
				if (_DateLabelHeight != value)
				{
					_DateLabelHeight = value;
					OnPropertyChanged("DateLabelHeight");
				}
			}
		}

		public double BlogImageFontSize { get; set; }

		private Command _OnImageClickCommand;
		public Command OnImageClickCommand
		{
			get { return _OnImageClickCommand; }
			set
			{
				if (_OnImageClickCommand != value)
				{
					_OnImageClickCommand = value;
					OnPropertyChanged("OnImageClickCommand");
				}
			}
		}

	}
}
