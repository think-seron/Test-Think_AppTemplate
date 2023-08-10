using System;
using System.Collections.Generic;
using IO.Swagger.Model;
using Xamarin.Forms;

namespace Think_App
{
	public partial class ReservationSelectStaff : ContentPage
	{

		ReservationSelectStaffViewModel vm;

		//ReservationType
		//1:通常の遷移
		//2クーポンからの遷移
		//3:スケジュールからの遷移
		public ReservationSelectStaff(ResponseStaffList response, ReservationContentInfo content, int ReservationType)
		{
			InitializeComponent();
			NavigationPage.SetBackButtonTitle(this, "");

			vm = new ReservationSelectStaffViewModel(response, content, ReservationType);
			this.BindingContext = vm;

			//this.StaffList.ItemTapped += (sender, e) => {
			//	var item = e.Item as ReservationSelectStaffViewModel.StaffListItem;
			//	System.Diagnostics.Debug.WriteLine("name" + item.StaffName);
			//};
		}
	}
}
