using System;
using Xamarin.Forms;
namespace Think_App
{
	public class MultiLineLabel : Label
	{
		private static int _defaultLineSetting = -1;

		#region Lines BindableProperty
		public static readonly BindableProperty LinesProperty =
			BindableProperty.Create(nameof(Lines), typeof(int), typeof(MultiLineLabel), _defaultLineSetting,
				propertyChanged: (bindable, oldValue, newValue) =>
					((MultiLineLabel)bindable).Lines = (int)newValue);

		public int Lines
		{
			get { return (int)GetValue(LinesProperty); }
			set { SetValue(LinesProperty, value); }
		}
		#endregion
	}
}
