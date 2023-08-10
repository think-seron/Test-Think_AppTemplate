using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Think_App;
using Think_App.Droid;
using Android.Widget;
using Android.Content;
using Android.Graphics;
using System.Collections.Generic;

[assembly: ExportRenderer(typeof(SegmentedControl), typeof(SegmentedControlRenderer))]

namespace Think_App.Droid
{
	public class SegmentedControlRenderer : ViewRenderer<SegmentedControl, RadioGroup>
	{
		protected override void OnElementChanged(ElementChangedEventArgs<SegmentedControl> e)
		{
			base.OnElementChanged(e);

			if (Control == null && e.NewElement != null)
			{
				var contents = e.NewElement.Contents;
				if (string.IsNullOrEmpty(contents))
				{
					return;
				}

				string spl = ",";
				if (!string.IsNullOrEmpty(e.NewElement.SplitString))
				{
					spl = e.NewElement.SplitString;
				}

				var array = contents.Split(spl.ToCharArray());
				if (array != null)
				{
					var radioGroup = new CustomRadioGroup(array.Length, Context);
					radioGroup.Orientation = Orientation.Horizontal;
					radioGroup.SetGravity(Android.Views.GravityFlags.CenterHorizontal);
					for (int i = 0; i < array.Length; ++i)
					{
						var radioBtn = new CustomRadioButton(Context)
						{
							IsFirst = (i == 0),
							Index = i,
							IsEnd = (i == array.Length - 1),
							FontSize = e.NewElement.FontSize,
							TextColor = e.NewElement.TextColor,
							UseCustomSettings = e.NewElement.UseCustomSettings,
							SelectedTextColor = e.NewElement.SelectedTextColor,
							InnerText = array[i],
							TintColor = e.NewElement.TintColor,
							CornerRadiusRate = e.NewElement.CornerRadiusRate
						};
						radioGroup.AddView(radioBtn);
					}
					radioGroup.SelectedIndex = e.NewElement.SelectedIndex;
					radioGroup.CheckedChange += (s1, e1) =>
					{
						radioGroup.SetSelectedIndexById(e1.CheckedId);
						// PCLに通知する。
						System.Diagnostics.Debug.WriteLine("Checked Change");
						e.NewElement.SelectedIndex = radioGroup.SelectedIndex;
					};
					SetNativeControl(radioGroup);
				}
			}
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (Control == null || Element == null)
			{
				return;
			}

			if (e.PropertyName == SegmentedControl.SelectedIndexProperty.PropertyName)
			{
				UpdateSelect(Element);
			}
		}

		void UpdateSelect(SegmentedControl seg)
		{
			var _CustomRadioGroup = Control as CustomRadioGroup;
			if (_CustomRadioGroup != null)
			{
				_CustomRadioGroup.SelectedIndex = seg.SelectedIndex;
			}
		}
	}

	public class CustomRadioGroup : RadioGroup
	{
		public int[] RadioBtnIds;
		int _SelectedIndex;
		public int SelectedIndex
		{
			get
			{
				return _SelectedIndex;
			}
			set
			{
				if (value != _SelectedIndex)
				{
					_SelectedIndex = value;
					UpdateSelection(_SelectedIndex);
				}
			}
		}

		public CustomRadioGroup(int childNum, Context context) : base(context)
		{
			RadioBtnIds = new int[childNum];
		}
		public CustomRadioGroup(int childNum, Context context, Android.Util.IAttributeSet set) : base(context, set)
		{
			RadioBtnIds = new int[childNum];
		}

		protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
		{
			base.OnMeasure(widthMeasureSpec, heightMeasureSpec);

			var wSize = MeasureSpec.GetSize(widthMeasureSpec);
			var hSize = MeasureSpec.GetSize(heightMeasureSpec);

			for (int i = 0; i < ChildCount; ++i)
			{
				var radio = GetChildAt(i);
				RadioBtnIds[i] = radio.Id;
				radio.Measure(MeasureSpec.MakeMeasureSpec(wSize / ChildCount, Android.Views.MeasureSpecMode.AtMost),
				              MeasureSpec.MakeMeasureSpec(hSize, Android.Views.MeasureSpecMode.AtMost));
				// サイズ再設定
				((Android.Widget.RadioButton)radio).SetWidth(wSize / ChildCount);
			}

			// ここで初期選択状態も定めてしまう
			try
			{
				Check(RadioBtnIds[SelectedIndex]);
			}
			catch(Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex);
			}
		}

		public void SetSelectedIndexById(int id)
		{
			for (int i = 0; i < RadioBtnIds.Length; ++i)
			{
				if (id == RadioBtnIds[i])
				{
					SelectedIndex = i;
					break;
				}
			}
		}

		void UpdateSelection(int index)
		{
			for (int i = 0; i < this.ChildCount; ++i)
			{
				var button = this.GetChildAt(i) as CustomRadioButton;
				if (button != null)
				{
					button.Selected = (index == i);
				}
			}
		}
	}

	public class CustomRadioButton : Android.Widget.RadioButton
    {
		const string _blueHex = "#157EFB";
		const string _whiteHex = "#FFFFFF";

		public bool IsFirst { get; set; }
		public int Index { get; set; }
		public bool IsEnd { get; set; }
		public double FontSize { get; set; }
		public bool UseCustomSettings { get; set; }
		public Xamarin.Forms.Color TextColor { get; set; }
		public Xamarin.Forms.Color SelectedTextColor { get; set; }
		public Xamarin.Forms.Color TintColor { get; set; }
		public double CornerRadiusRate { get; set; }
		public string InnerText { get; set; }

		public CustomRadioButton(Context context) : base(context)
		{
		}

		public CustomRadioButton(Context context, Android.Util.IAttributeSet set) : base(context, set)
		{
		}

		public CustomRadioButton(Context context, Android.Util.IAttributeSet set, int attr) : base(context, set, attr)
		{
		}

		public CustomRadioButton(Context context, Android.Util.IAttributeSet set, int attr, int res) : base(context, set, attr, res)
		{
		}

		public override void Draw(Canvas canvas)
		{
			//base.Draw(canvas);
			var blue = Xamarin.Forms.Color.FromHex(_blueHex).ToAndroid();
			var white = Xamarin.Forms.Color.FromHex(_whiteHex).ToAndroid();
			var radius = (float)(Math.Min(Width, Height) / 100.0 * CornerRadiusRate);

			using (var paint = new Paint())
			{
				paint.AntiAlias = true;

				var lineWidth = (float)(2.0).DpToPx(Context);

				// まず青い矩形を作成。
				paint.SetStyle(Android.Graphics.Paint.Style.Fill);
				paint.Color = (UseCustomSettings) ? TintColor.ToAndroid() : blue;

				var rectangle = new RectF(0, 0, Width, Height);

				if (IsFirst)
				{
					// 角丸矩形描画。
					canvas.DrawRoundRect(rectangle, radius, radius, paint);
					// 上から通常矩形描画。
					rectangle = new RectF(radius, 0, Width, Height);
					canvas.DrawRoundRect(rectangle, 0, 0, paint);
				}
				else if (IsEnd)
				{
					// 角丸矩形描画。
					canvas.DrawRoundRect(rectangle, radius, radius, paint);
					// 上から通常矩形描画。
					rectangle = new RectF(0, 0, Width - radius, Height);
					canvas.DrawRoundRect(rectangle, 0, 0, paint);
				}
				else
				{
					// 通常矩形描画。
					canvas.DrawRoundRect(rectangle, 0, 0, paint);
				}

				if(!Checked)
				{
					// 非選択状態は一回り小さな矩形で塗りつぶし。
					paint.SetStyle(Android.Graphics.Paint.Style.Fill);
					paint.Color = white;

					if (IsFirst)
					{
						rectangle = new RectF(lineWidth, lineWidth, Width - lineWidth / 2, Height - lineWidth);
						canvas.DrawRoundRect(rectangle, radius, radius, paint);
					}
					else if (IsEnd)
					{
						rectangle = new RectF(lineWidth / 2, lineWidth, Width - lineWidth, Height - lineWidth);
						canvas.DrawRoundRect(rectangle, radius, radius, paint);
					}
					else
					{
						rectangle = new RectF(lineWidth / 2, lineWidth, Width - lineWidth / 2, Height - lineWidth);
						canvas.DrawRoundRect(rectangle, 0, 0, paint);
					}

					if (IsFirst)
					{
						// 上から通常矩形描画。
						rectangle = new RectF(lineWidth + radius, lineWidth, Width - lineWidth / 2, Height - lineWidth);
						canvas.DrawRoundRect(rectangle, 0, 0, paint);
					}
					else if (IsEnd)
					{
						// 上から通常矩形描画。
						rectangle = new RectF(lineWidth / 2, lineWidth, Width - lineWidth - radius, Height - lineWidth);
						canvas.DrawRoundRect(rectangle, 0, 0, paint);
					}
				}

				// 文字描画。
				if (UseCustomSettings)
				{
					paint.Color = (Checked) ? SelectedTextColor.ToAndroid() : TextColor.ToAndroid();
				}
				else
				{
					paint.Color = (Checked) ? white : blue;
				}
				paint.TextSize = (float)FontSize.DpToPx(Context);
				var tWidth = paint.MeasureText(InnerText);
				var tHeight = paint.TextSize;
				canvas.DrawText(InnerText, (Width - tWidth) / 2, Height - (Height - tHeight) / 2, paint);
			}
		}
	}

	public static class DoubleExtensions
	{
		public static double DpToPx(this double dp, Context context)
		{
			var scale = context.Resources.DisplayMetrics.Density;
			return dp * scale;
		}
	}
}
