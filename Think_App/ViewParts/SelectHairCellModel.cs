using System;
using Xamarin.Forms;

namespace Think_App
{
	public class SelectHairCellModel : ViewModelBase
	{
		ImageSource _SHCBackImgSource;
		public ImageSource SHCBackImgSource
		{
			get
			{
				return _SHCBackImgSource;
			}
			set
			{
				SetProperty(ref _SHCBackImgSource, value);
			}
		}

		Thickness _SHCHairImgMargin;
		public Thickness SHCHairImgMargin
		{
			get
			{
				return _SHCHairImgMargin;
			}
			set
			{
				SetProperty(ref _SHCHairImgMargin, value);
			}
		}

		ImageSource _SHCHairImgSource;
		public ImageSource SHCHairImgSource
		{
			get
			{
				return _SHCHairImgSource;
			}
			set
			{
				SetProperty(ref _SHCHairImgSource, value);
			}
		}

		double _SHCHairImgDSWidth;
		public double SHCHairImgDSWidth
		{
			get
			{
				return _SHCHairImgDSWidth;
			}
			set
			{
				SetProperty(ref _SHCHairImgDSWidth, value);
			}
		}

		double _SHCHairImgDSHeight;
		public double SHCHairImgDSHeight
		{
			get
			{
				return _SHCHairImgDSHeight;
			}
			set
			{
				SetProperty(ref _SHCHairImgDSHeight, value);
			}
		}

		Color _SHCHairImgBGColor;
		public Color SHCHairImgBGColor
		{
			get
			{
				return _SHCHairImgBGColor;
			}
			set
			{
				SetProperty(ref _SHCHairImgBGColor, value);
			}
		}

		Thickness _SHCSelectedMarkMargin;
		public Thickness SHCSelectedMarkMargin
		{
			get
			{
				return _SHCSelectedMarkMargin;
			}
			set
			{
				SetProperty(ref _SHCSelectedMarkMargin, value);
			}
		}

		double _SHCSelectedMarkBorderThickness;
		public double SHCSelectedMarkBorderThickness
		{
			get
			{
				return _SHCSelectedMarkBorderThickness;
			}
			set
			{
				SetProperty(ref _SHCSelectedMarkBorderThickness, value);
			}
		}

		Color _SHCSelectedMarkFillColor;
		public Color SHCSelectedMarkFillColor
		{
			get
			{
				return _SHCSelectedMarkFillColor;
			}
			set
			{
				SetProperty(ref _SHCSelectedMarkFillColor, value);
			}
		}

		Color _SHCSelectedMarkStrokeColor;
		public Color SHCSelectedMarkStrokeColor
		{
			get
			{
				return _SHCSelectedMarkStrokeColor;
			}
			set
			{
				SetProperty(ref _SHCSelectedMarkStrokeColor, value);
			}
		}

		double _SHCSelectedMarkRadiusRate;
		public double SHCSelectedMarkRadiusRate
		{
			get
			{
				return _SHCSelectedMarkRadiusRate;
			}
			set
			{
				SetProperty(ref _SHCSelectedMarkRadiusRate, value);
			}
		}

		bool _SHCSelectedMarkVisible;
		public bool SHCSelectedMarkVisible
		{
			get
			{
				return _SHCSelectedMarkVisible;
			}
			set
			{
				SetProperty(ref _SHCSelectedMarkVisible, value);
			}
		}

		public string Url { get; set; }
		public double Scale { get; set; }
		public double ShiftX { get; set; }
		public double SHiftY { get; set; }
	}
}
