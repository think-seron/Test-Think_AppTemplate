using System;
using Xamarin.Forms;

namespace Think_App
{
	public class ColorList
	{
		// ビルドサーバで書き換えするcolor
        public static readonly Color colorMain = Color.FromHex("@@@colorMain");
        public static readonly Color colorFont = Color.FromHex("@@@colorFont");
        public static readonly Color colorbatch = Color.FromHex("@@@colorbatch");
        public static readonly Color colorBackground = Color.FromHex("@@@colorBackground");
        public static readonly Color colorHomeMenuBtn = Color.FromHex("@@@colorHomeMenuBtn");



		// ビルドサーバで書き換えしないcolor
		public static readonly Color colorNegative = Color.FromHex("#EFB2CF");
		public static readonly Color colorNegativeHightLight = Color.FromHex("#ffcce5");
		public static readonly Color colorNagativeDisable = Color.FromHex("#b5b5b5");
		public static readonly Color colorPositive = Color.FromHex("#ABDAD1");
		public static readonly Color colorPositiveHightlight = Color.FromHex("#bdf2e8");
		public static readonly Color colorPositiveDisable = Color.FromHex("#b5b5b5");
		public static readonly Color colorNeutral = Color.FromHex("#FCE3B3");
		public static readonly Color colorNeutralHightlight = Color.FromHex("#fcf4e6");
		public static readonly Color colorNeutralDisable = Color.FromHex("#b5b5b5");
		public static readonly Color colorSubfont = Color.FromHex("#FBF7EB");
		public static readonly Color colorWhite = Color.FromHex("#FFFFFF");
		public static readonly Color colorBlack = Color.FromHex("#000000");
		public static readonly Color colorScheduleDisableBtnColor = Color.FromHex("#B5B5B5");
		public static readonly Color colorScheduleSaturday = Color.FromHex("#0A7F81");
		public static readonly Color colorScheduleSunday = Color.FromHex("#DE144F");
		public static readonly Color colorCellBoader = Color.FromHex("#cccccc");
		public static readonly Color colorWhiteBtnBorderColor = Color.FromHex("#979797");
		public static readonly Color colorNavibarTextColor = Color.FromHex("#FFFFFF");
		public static readonly Color colorReservationFontColor = Color.FromHex("#4A4A4A");

		public ColorList()
		{

		}
	}
}
