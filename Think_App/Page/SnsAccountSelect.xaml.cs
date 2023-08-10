using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Think_App
{
	public partial class SnsAccountSelect : ContentPage
	{
		public SnsAccountSelect(int transitionSource)
		{
			InitializeComponent();
			NavigationPage.SetBackButtonTitle(this, "");

			SnsAccountSelectViewModel SnsActSltViewMdl = new SnsAccountSelectViewModel(transitionSource);
			if (transitionSource == 1)
			{
				SnsActSltViewMdl.CustomFormattedText = new FormattedString
				{
					Spans =
															{
																new Span { Text = "アカウントをお持ちの" },
																new Span { Text = System.Environment.NewLine},
																new Span { Text = "SNSを選択してください" }
															}
				};
			}
			else if (transitionSource == 2)
			{
				SnsActSltViewMdl.CustomFormattedText = new FormattedString
				{
					Spans =
															{
																new Span { Text = "以前、登録の際に使用した" },
																new Span { Text = System.Environment.NewLine},
																new Span { Text = "SNSを選択してください" }
															}
				};
			}
			this.BindingContext = SnsActSltViewMdl;
		}
	}
}
