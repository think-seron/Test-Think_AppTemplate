using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
    public partial class SavadDataSelectedPage : ContentPage
    {
        const double _buttonBaseWidth = 224;
        const double _buttonBaseHeight = 50;

        string _infoFilename, _hairFilename, _faceFilename, _imageFilename;
        EditHairSaveData _saveData;
        SavadDataSelectedPageModel Model { get; set; }
        public SavadDataSelectedPage(ImageSource source, string infoFileName, string hairFilename, string faceFilename, string imageFilename, EditHairSaveData saveData)
        {
            InitializeComponent();

            // このページに戻るときにタイトルを表示しない。
            NavigationPage.SetBackButtonTitle(this, "");

            var deleteItem = new ToolbarItem()
            {
                Text = "削除",
                Command = new Command(OnDeleteAsync)
            };
            this.ToolbarItems.Add(deleteItem);

            _saveData = saveData;
            _infoFilename = infoFileName;
            _hairFilename = hairFilename;
            _faceFilename = faceFilename;
            _imageFilename = imageFilename;

            this.SelectedImage.Aspect = (_saveData.isGallery) ? Aspect.AspectFit : Aspect.AspectFill;

            this.SelectedImage.Source = source;
            this.EditBtn.Clicked += EditBtn_Clicked;

            // Binding
            Model = new SavadDataSelectedPageModel()
            {
                ScreenSizeScale = ScaleManager.Scale
            };
            this.BindingContext = Model;
        }

        async void OnDeleteAsync()
        {
            if (!App.ProcessManager.CanInvoke())
            {
                return;
            }

            // モーダルページ作成
            var modalView = new ModalView();
            modalView.modalViewViewModel.ImageSource = this.SelectedImage.Source;
            modalView.modalViewViewModel.ImageAspect = Aspect.AspectFill;
            modalView.modalViewViewModel.ImageWidth = 270;
            modalView.modalViewViewModel.ImageHeight = 270;
            modalView.modalViewViewModel.ModalLabelTxt = "この画像を削除しますか？";
            modalView.modalViewViewModel.YesButtonTxt = "はい";
            modalView.modalViewViewModel.NoButtonTxt = "いいえ";
            // TODO Xamarin.Forms.Device.RuntimePlatform is no longer supported. Use Microsoft.Maui.Devices.DeviceInfo.Platform instead. For more details see https://learn.microsoft.com/en-us/dotnet/maui/migration/forms-projects#device-changes
            double posY = (Device.RuntimePlatform == Device.Android) ? 0.2 : 0.31;
            modalView.modalViewViewModel.ImageRect = new Rect(0.5, posY, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize);
            modalView.modalViewViewModel.NomalModalLabelRect = new Rect(0.5, 0.71, 1, AbsoluteLayout.AutoSize);
            modalView.modalViewViewModel.SelectBtnLayoutBounds = new Rect(0.9, 0.77, 1, AbsoluteLayout.AutoSize);


            modalView.yesButton.Clicked += async (sender, e) =>
            {
                // ファイル削除
                await DeleteFilesAsync();
                // モーダルを閉じる
                await DialogManager.Instance.HideView();
                App.ProcessManager.OnComplete();
            };

            modalView.noButton.Clicked += async (sender, e) =>
            {
                // モーダルを閉じる
                await DialogManager.Instance.HideView();
                App.ProcessManager.OnComplete();
            };

            // モーダルを表示
            await DialogManager.Instance.ShowDialogView(modalView);
        }

        async Task DeleteFilesAsync()
        {
            // 関連ファイルを全て消す。
            await StorageManager.UserDataDeleteAsync(ConstantManager.FolderName_EditData, _infoFilename);
            await StorageManager.UserDataDeleteAsync(ConstantManager.FolderName_EditData, _hairFilename);
            await StorageManager.UserDataDeleteAsync(ConstantManager.FolderName_EditData, _faceFilename);
            await StorageManager.UserDataDeleteAsync(ConstantManager.FolderName_EditData, _imageFilename);

            // 一つ前のページ(SavedDataGalleryPage)のIsSetupCompletedフラグをfalseにして、再読み込みがかかるようにする。
            try
            {
                var count = Navigation.NavigationStack.Count;
                var page = Navigation.NavigationStack[count - 2] as SavedDataGalleryPage;
                page.IsSetupCompleted = false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }

        async void EditBtn_Clicked(object sender, EventArgs e)
        {
            if (!App.ProcessManager.CanInvoke())
            {
                return;
            }
            var photoImgSource = await ImageManager.LoadImageFromLocalStorageAsync(ConstantManager.FolderName_EditData, _faceFilename);
            var hairImgSource = await ImageManager.LoadImageFromLocalStorageAsync(ConstantManager.FolderName_EditData, _hairFilename);
            var page = new EditHairPage(_saveData.HairImageRect, photoImgSource, hairImgSource, _saveData.isGallery, _saveData.ImageScale, _saveData.ImageTranslationX, _saveData.ImageTranslationY, _saveData.ViewImageRange);
            page.ApplySavedEditData(_saveData.AppendColorImageSaveData);
            await Navigation.PushAsync(page);
            App.ProcessManager.OnComplete();
        }
    }
}
