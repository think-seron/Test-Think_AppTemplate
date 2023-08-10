using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using IO.Swagger.Model;

namespace Think_App
{
    public partial class SelectSalonMessagePage : ContentPage
    {
        public class SalonInfo
        {
            public int SalonId { get; private set; }
            public string SalonName { get; private set; }
            public SalonInfo(int salonId, string salonName)
            {
                SalonId = salonId;
                SalonName = salonName;
            }
        }

        public event EventHandler<SalonInfo> SalonSelected = delegate { };
        SelectSalonMessagePageModel Model { get; set; }
        public SelectSalonMessagePage()
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "");
            App.customNavigationPage.IsRunning = true;

            // 読み込みスタート
            Task.Run(async () =>
            {
                Model = new SelectSalonMessagePageModel();
                Model.SeparatorColor = ColorList.colorWhiteBtnBorderColor;
                Model.SalonListSource = new ObservableCollection<SelectSalonMessageListCellModel>();

                var json = await APIManager.GET("message_salonlist");
                try
                {
                    var response = JsonManager.Deserialize<ResponseMessageSalonList>(json);

                    foreach (var salon in response.Data.SalonList)
                    {
                        Model.SalonListSource.Add(new SelectSalonMessageListCellModel()
                        {
                            SSMLCBackgroundColor = ColorList.colorWhite,
                            SSMLCSeparatorColor = ColorList.colorWhiteBtnBorderColor,
                            SSMLCSalonName = salon.SalonName,
                            SSMLCNewMessage = salon.NewMessage,
                            SSMLCMessageDate = salon.NewMessageCreated,
                            SSMLCBatchVisible = salon.UnreadExists.Value,
                            SalonId = salon.SalonId.Value
                        });
                    }

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        this.BindingContext = Model;
                        App.customNavigationPage.IsRunning = false;
                    });
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        this.BindingContext = Model;
                        App.customNavigationPage.IsRunning = false;
                        // 通信エラー
                        DependencyService.Get<IToast>().Show("通信エラー");
                    });
                }
            });

            this.SalonList.ItemSelected += SalonList_ItemSelected;
        }

        void SalonList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var model = e.SelectedItem as SelectSalonMessageListCellModel;
            if (model != null)
            {
                if (SalonSelected != null)
                {
                    SalonSelected(this, new SalonInfo(model.SalonId, model.SSMLCSalonName));
                }
            }
        }
    }
}
