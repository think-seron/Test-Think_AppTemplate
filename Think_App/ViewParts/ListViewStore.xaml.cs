using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Newtonsoft.Json;
using IO.Swagger.Model;

namespace Think_App
{
    public partial class ListViewStore : ViewCell
    {
        public ListViewStore()
        {
            InitializeComponent();
        }

        async void OnFavoIconClicked(object sender, EventArgs e)
        {
            if (App.ProcessManager.CanInvoke())
            {
                int id = (int)((ListViewStoreViewModel)((Xamarin.Forms.Image)sender).BindingContext).BdContext.SalonID;
                StaticMethod.SalonFavoriteChange(sender, id);
                //if (ret == false)
                //{
                //	//var page = ((Xamarin.Forms.Image)sender).Parent.Parent.Parent.Parent.Parent;
                //	//await ((StoreListPage)page).DisplayAlert("エラー", "通信エラー", "OK");
                //	DependencyService.Get<IToast>().Show("通信エラー");
                //}
            }
        }
    }
}
