using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Think_App
{
    public partial class ListViewStaffStore : ViewCell
    {
        public ListViewStaffStore()
        {
            InitializeComponent();
        }

        async void OnStaffFavoIconClicked(object sender, EventArgs e)
        {
            if (App.ProcessManager.CanInvoke())
            {
                var content = ((StoreInformationPage)this.Parent.Parent).Content;
                System.Collections.IEnumerable itemsSource = ((ListView)content).ItemsSource;

                FileImageSource souce = (FileImageSource)((Image)sender).Source;
                int staffID = (int)((ListViewStaffStoreViewModel)((Xamarin.Forms.Image)sender).BindingContext).BdContext.StaffID;
                int salonID = (int)((ListViewStaffStoreViewModel)((Xamarin.Forms.Image)sender).BindingContext).BdContext.salonID;
                var parameters = new Dictionary<string, string> { { "deviceId", Config.Instance.Data.deviceId } };
                StaticMethod.StaffFavoriteChange(sender, souce, parameters, salonID, staffID, itemsSource);
                //if (ret == false)
                //{
                //	//var page = ((Xamarin.Forms.Image)sender).Parent.Parent.Parent.Parent.Parent;
                //	//await((StoreInformationPage)page).DisplayAlert("エラー", "通信エラー", "OK");
                //	DependencyService.Get<IToast>().Show("通信エラー");
                //}
            }
        }
    }
}
