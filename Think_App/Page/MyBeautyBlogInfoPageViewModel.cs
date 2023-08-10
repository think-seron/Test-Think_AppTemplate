using System;
using Xamarin.Forms;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Think_App
{
	public class MyBeautyBlogInfoPageViewModel : ViewModelBase
	{
		public MyBeautyBlogInfoPageViewModel(MyBeautyBlogModel model)
		{
			ScreenWidth = ScaleManager.ScreenWidth;
			ImgSouce = model.ImgSouce;

			MatchCollection matchRet = Regex.Matches(model.DateStr, "[0-9]+:[0-9]+:[0-9]+");
			// 全てを列挙する
			//foreach (Match m in results) // Matchと型を明示（varは不可）
			//{
			//	int index = m.Index; // 発見した文字列の開始位置
			//	string value = m.Value; // 発見した文字列
			//}
			if (matchRet.Count > 0)
			{
				DateTxt = model.DateStr.Replace("-", "/").Replace(matchRet[0].Value, "");
			}
			else
			{
				DateTxt = model.DateStr.Replace("-", "/");
			}
			//DateTxt = model.DateStr.Replace("-","/").Replace("("," (");
			TitleTxt = model.Title;
			DescriptionTxt = model.Description;
			if (model.CategoryValue == 1)
			{
				CtgTxt = "ヘア";
				Source = "icon_message_simulation.png";
				ImageWidth = 38.17;
				ImageHeight = 38;
				ImagePadding = 2;
				CtgIsVisible = true;
			}
			else if (model.CategoryValue == 2)
			{
				CtgTxt = "ネイル";
				Source = "Icon_Nail.png";
				ImageWidth = 21;
				ImageHeight = 44;
				ImagePadding = 2;
				CtgIsVisible = true;
			}
			else if (model.CategoryValue == 3)
			{
				CtgTxt = "まつエク";
				Source = "Icon_MatsuEku.png";
				ImageWidth = 50.8;
				ImageHeight = 13.95;
				ImagePadding = 2;
				CtgIsVisible = true;
			}
			else if (model.CategoryValue == 4)
			{
				CtgTxt = "エステ";
				Source = "icon_hand.png";
				ImageWidth = 50;
				ImageHeight = 40;
				ImagePadding = 8;
				CtgIsVisible = true;
			}
			else
			{
				//CtgTxt = "その他";
				CtgIsVisible = false;
			}
		}


		public async Task UpdateVIewModel(MyBeautyBlogModel model)
		{
			Task.Run(() => {

			ScreenWidth = ScaleManager.ScreenWidth;
			ImgSouce = model.ImgSouce;

			MatchCollection matchRet = Regex.Matches(model.DateStr, "[0-9]+:[0-9]+:[0-9]+");
			// 全てを列挙する
			//foreach (Match m in results) // Matchと型を明示（varは不可）
			//{
			//	int index = m.Index; // 発見した文字列の開始位置
			//	string value = m.Value; // 発見した文字列
			//}
			if (matchRet.Count > 0)
			{
				DateTxt = model.DateStr.Replace("-", "/").Replace(matchRet[0].Value, "");
			}
			else
			{
				DateTxt = model.DateStr.Replace("-", "/");
			}
			//DateTxt = model.DateStr.Replace("-","/").Replace("("," (");
			TitleTxt = model.Title;
			DescriptionTxt = model.Description;
			if (model.CategoryValue == 1)
			{
				CtgTxt = "ヘア";
				Source = "icon_message_simulation.png";
				ImageWidth = 38.17;
				ImageHeight = 38;
				ImagePadding = 2;
				CtgIsVisible = true;
			}
			else if (model.CategoryValue == 2)
			{
				CtgTxt = "ネイル";
				Source = "Icon_Nail.png";
				ImageWidth = 21;
				ImageHeight = 44;
				ImagePadding = 2;
				CtgIsVisible = true;
			}
			else if (model.CategoryValue == 3)
			{
				CtgTxt = "まつエク";
				Source = "Icon_MatsuEku.png";
				ImageWidth = 50.8;
				ImageHeight = 13.95;
				ImagePadding = 2;
				CtgIsVisible = true;
			}
			else if (model.CategoryValue == 4)
			{
				CtgTxt = "エステ";
				Source = "icon_hand.png";
				ImageWidth = 50;
				ImageHeight = 40;
				ImagePadding = 8;
				CtgIsVisible = true;
			}
			else
			{
				//CtgTxt = "その他";
				CtgIsVisible = false;
			}

			});
		}






		public double ScreenWidth { get; set; }

		private ImageSource _ImgSouce;
		public ImageSource ImgSouce
		{
			get { return _ImgSouce; }
			set
			{
				if (_ImgSouce != value)
				{
					_ImgSouce = value;
					OnPropertyChanged("ImgSouce");
				}
			}
		}


		private string _DateTxt;
		public string DateTxt
		{
			get { return _DateTxt; }
			set
			{
				if (_DateTxt != value)
				{
					_DateTxt = value;
					OnPropertyChanged("DateTxt");
				}
			}
		}
		private string _TitleTxt;
		public string TitleTxt
		{
			get { return _TitleTxt; }
			set
			{
				if (_TitleTxt != value)
				{
					_TitleTxt = value;
					OnPropertyChanged("TitleTxt");
				}
			}
		}

		private string _DescriptionTxt;
		public string DescriptionTxt
		{
			get { return _DescriptionTxt; }
			set
			{
				if (_DescriptionTxt != value)
				{
					_DescriptionTxt = value;
					OnPropertyChanged("DescriptionTxt");
				}
			}
		}

		private string _CtgTxt;
		public string CtgTxt
		{
			get { return _CtgTxt; }
			set
			{
				if (_CtgTxt != value)
				{
					_CtgTxt = value;
					OnPropertyChanged("CtgTxt");
				}
			}
		}

		private string _Source;
		public string Source
		{
			get { return _Source; }
			set
			{
				if (_Source != value)
				{
					_Source = value;
					OnPropertyChanged("Source");
				}
			}
		}
		private double _ImageWidth;
		public double ImageWidth
		{
			get { return _ImageWidth; }
			set
			{
				if (_ImageWidth != value)
				{
					_ImageWidth = value;
					OnPropertyChanged("ImageWidth");
				}
			}
		}
		private double _ImageHeight;
		public double ImageHeight
		{
			get { return _ImageHeight; }
			set
			{
				if (_ImageHeight != value)
				{
					_ImageHeight = value;
					OnPropertyChanged("ImageHeight");
				}
			}
		}

		public double ImagePadding { get; set; }
		public bool CtgIsVisible { get; set; }

		private bool myBlogPlusListViewIsVisible;
		public bool MyBlogPlusListViewIsVisible
		{
			get
			{
				return myBlogPlusListViewIsVisible;
			}
			set
			{
				if (myBlogPlusListViewIsVisible != value)
				{
					myBlogPlusListViewIsVisible = value;
					OnPropertyChanged("MyBlogPlusListViewIsVisible");
				}
			}
		}

	}
}
