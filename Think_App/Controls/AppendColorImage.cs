using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Think_App
{
	public class GotCurrentImageSourceEventArgs : EventArgs
	{
		public ImageSource ImageSource { get; set; }
	}

	public class AppendColorImageSaveData
	{
		public double StrokeWidth { get; set; }
		public double R { get; set; }
		public double G { get; set; }
		public double B { get; set; }
		public double A { get; set; }
		public bool IsErase { get; set; }
		public double WidthRate { get; set; }
		public Rectangle DefaultRect { get; set; }
		public Strokes Strokes { get; set; }
		public bool IsEditted { get; set; }
		public AppendColorImage.BlendingMode BlendMode { get; set; }
	}

	public class AppendColorImage : Image
	{
		public event EventHandler CurrentImageSourceRequested = delegate {};
		public event EventHandler ForceUpdateRequested = delegate {};
		public event EventHandler<GotCurrentImageSourceEventArgs> GotCurrentImageSource = delegate {};

		public enum BlendingMode
		{
			Normal,
			Multiply,
			Screen
		}
		public double StrokeWidth { get; set; }

		public Color StrokeColor
		{
			get
			{
				return Color.FromRgba(R, G, B, A);
			}
			set
			{
				R = value.R;
				G = value.G;
				B = value.B;
				A = value.A;
			}
		}
		public double R { get; set; }
		public double G { get; set; }
		public double B { get; set; }
		public double A { get; set; }

		public bool IsErase { get; set; }

		public Strokes Strokes { get; set; }

		public bool IsEditted { get; set; }

		public double WidthRate { get; set; }

		public Rectangle DefaultRect { get; set; }

		#region BlendMode BindableProperty
		public static readonly BindableProperty BlendModeProperty =
			BindableProperty.Create(nameof(BlendMode), typeof(BlendingMode), typeof(AppendColorImage), BlendingMode.Screen,
				propertyChanged: (bindable, oldValue, newValue) =>
					((AppendColorImage)bindable).BlendMode = (BlendingMode)newValue);

		public BlendingMode BlendMode
		{
			get { return (BlendingMode)GetValue(BlendModeProperty); }
			set { SetValue(BlendModeProperty, value); }
		}
		#endregion

		public AppendColorImage()
		{
			// デフォルト
			StrokeWidth = 20;
			StrokeColor = Color.FromHex("#FF0000");

			Strokes = new Strokes();
		}

		public void Clear()
		{
			Strokes.Clear();
			WidthRate = 1.0;
			OnPropertyChanged("Clear");

			// 編集済みフラグを削除
			IsEditted = false;
		}

		public void RecalclateStrokes(double horizontalRate, double verticalRate)
		{
			// 全てのストロークの座標を再計算する。
			Strokes.Recalculate(horizontalRate, verticalRate);
		}

		public void OnBegin(double x, double y)
		{
			if (!IsEnabled) { return; }
			Strokes.Begin(x, y, StrokeColor, IsErase, StrokeWidth);
			
			// 編集済みフラグが立っていなければ立てる。
			if (!IsEditted)
			{
				IsEditted = true;
			}
		}

		public bool OnMove(double x, double y)
		{
			if (!IsEnabled) { return false; }
			return Strokes.Move(x, y);
		}

		public void OnEnd()
		{
			if (!IsEnabled) { return; }
			Strokes.End();
		}

		public void RequestCurrentImageSource()
		{
			if (CurrentImageSourceRequested != null)
			{
				CurrentImageSourceRequested(this, EventArgs.Empty);
			}
		}

		public void SendCurrentImageSource(ImageSource source)
		{
			if (GotCurrentImageSource != null)
			{
				GotCurrentImageSource(this, new GotCurrentImageSourceEventArgs() { ImageSource = source });
			}
		}

		public void ForceUpdate()
		{
			if (ForceUpdateRequested != null)
			{
				ForceUpdateRequested(this, EventArgs.Empty);
			}
		}

		public AppendColorImageSaveData GetSaveData()
		{
			return new AppendColorImageSaveData()
			{
				StrokeWidth = this.StrokeWidth,
				R = this.R,
				G = this.G,
				B = this.B,
				A = this.A,
				IsErase = this.IsErase,
				WidthRate = this.WidthRate,
				DefaultRect = this.DefaultRect,
				Strokes = this.Strokes,
				IsEditted = this.IsEditted,
				BlendMode = this.BlendMode
			};
		}

		public void SetSaveData(AppendColorImageSaveData data)
		{
			this.StrokeWidth = data.StrokeWidth;
			this.R = data.R;
			this.G = data.G;
			this.B = data.B;
			this.A = data.A;
			this.StrokeColor = Color.FromRgba(data.R, data.G, data.B, data.A);
			this.IsErase = data.IsErase;
			this.Strokes = data.Strokes;
			this.IsEditted = data.IsEditted;
			this.WidthRate = data.WidthRate;
			this.DefaultRect = data.DefaultRect;
			this.BlendMode = data.BlendMode;
		}
	}

	public class Strokes
	{
		// 線の太さに対し、この値を掛けた値を下回る移動は移動と認めない。
		// 小さくするほど滑らかになる。
		const double _limitRate = 0.4;

		public List<Stroke> Data { get; set; }
		// 現在描画中の線
		private Stroke _stroke;
		// 描画中の線の太さ（Moveの時に太さの範囲内のデータを追加しないようにするため記憶しておく）
		private double _strokeWidth;
		//最後に追加したデータ
		public double LastX { get; set; }
		public double LastY { get; set; }

		public Strokes()
		{
			Clear();
		}

		public void Recalculate(double horizontalRate, double verticalRate)
		{
			if (Data.Count > 0)
			{
				var newData = new List<Stroke>();
				// 各ストロークの座標を補正する。
				foreach (var stroke in Data)
				{
					if (stroke.Points != null && stroke.Points.Count > 0)
					{
						var points = new List<Point>();
						foreach (var point in stroke.Points)
						{
							points.Add(new Point(point.X * horizontalRate, point.Y * verticalRate));
						}
						var newStroke = new Stroke(stroke.Color, stroke.IsErase, stroke.Width)
						{
							Points = points
						};
						newData.Add(newStroke);
					}
				}

				// Data更新
				Data = null;
				Data = newData;
			}
		}

		public void Begin(double x, double y, Color color, bool isErase, double strokeWidth)
		{
			_strokeWidth = strokeWidth;
			_stroke = new Stroke(color, isErase, _strokeWidth);
			Data.Add(_stroke); //現在描画中の線は、配列の最後にセットされている
			Move(x, y);
		}

		//データの追加があった場合、trueを返す
		public bool Move(double x, double y)
		{
			if (Equals(LastX, x) && Equals(LastY, y))
			{
				// 同じ場所への描画は追加しない。
				return false;
			}
			if (Math.Abs(LastX - x) < _strokeWidth * _limitRate && Math.Abs(LastY - y) < _strokeWidth * _limitRate)
			{
				// 線の太さの範囲内の移動は追加しない。
				return false;
			}
			_stroke.Add(new Point(x, y));
			LastX = x;
			LastY = y;
			return true;
		}

		public void End()
		{
			LastX = -1;
			LastY = -1;
		}

		public void Clear()
		{
			Data = new List<Stroke>();
		}
	}

	public class Stroke
	{
		public Color Color
		{
			get
			{
				return Color.FromRgba(R, G, B, A);
			}
			set
			{
				R = value.R;
				G = value.G;
				B = value.B;
				A = value.A;
			}
		}
		public double R { get; set; }
		public double G { get; set; }
		public double B { get; set; }
		public double A { get; set; }
		public bool IsErase { get; set; }
		public double Width { get; set; }
		public List<Point> Points { get; set; }
		public Stroke(Color color, bool isErase, double width)
		{
			Color = color;
			IsErase = isErase;
			Width = width;
			Points = new List<Point>();
		}

		public void Add(Point point)
		{
			Points.Add(point);
		}
	}
}
