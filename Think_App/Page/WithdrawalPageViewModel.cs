using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Think_App
{
    public class WithdrawalPageViewModel : ViewModelBase
    {
        private bool _isWithdrawaled;
        public WithdrawalPageViewModel()
        {
            TopText = GetDefaultTopText();
        }

        public Command BtnCommand => BtnAction();

        private string _topText;
        public string TopText
        {
            get => _topText;
            set => SetProperty(ref _topText, value);
        }

        private string _btnText = "退会する";
        public string BtnText
        {
            get => _btnText;
            set => SetProperty(ref _btnText, value);
        }

        private Color _btnBgColor = ColorList.colorNegative;
        public Color BtnBgColor
        {
            get => _btnBgColor;
            set => SetProperty(ref _btnBgColor, value);
        }

        private Color _btnHighlightColor = ColorList.colorNegativeHightLight;
        public Color BtnHighlightColor
        {
            get => _btnHighlightColor;
            set => SetProperty(ref _btnHighlightColor, value);
        }

        private bool _hasBackButton = true;
        public bool HasBackButton
        {
            get => _hasBackButton;
            set => SetProperty(ref _hasBackButton, value);
        }

        public Command BtnAction()
        {
            return new Command(async () =>
            {
                if (!_isWithdrawaled)
                {
                    if (App.ProcessManager.CanInvoke())
                    {
                        var withDrawakResult = await Withdrawal();
                        if (!withDrawakResult)
                        {
                            App.ProcessManager.OnComplete();
                            return;
                        }

                        UpdateBtn();
                        await DeleteLocalFiles();
                        App.ProcessManager.OnComplete();
                    }
                }
                else
                    await NavigateTopPage();
            });
        }

        private async Task<bool> Withdrawal()
        {
            var dialogResult = await App.Current.MainPage.DisplayAlert("確認", "本当に退会しますか？", "OK", "キャンセル");
            if (!dialogResult) return false;
            var deleteRequest = new Dictionary<string, string>();
            var deleteResultJson = await APIManager.Post("delete_account", deleteRequest);
            System.Diagnostics.Debug.WriteLine(deleteResultJson);
            if (deleteResultJson == null) return false;
            try
            {
                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseDeleteAccount>(deleteResultJson);
                if (result == null || result.Status != 0 || !string.IsNullOrEmpty(result.Message)) return false;
            }
            catch
            {
                return false;
            }

            return true;
        }

        private void UpdateBtn()
        {
            HasBackButton = false;
            BtnBgColor = ColorList.colorNeutral;
            BtnHighlightColor = ColorList.colorNeutralHightlight;
            BtnText = "TOPに戻る";
            TopText = GetChangedTopText();
            _isWithdrawaled = true;
        }

        private string GetDefaultTopText()
        {
            return "※ご予約はキャンセルされません。キャンセルされたい場合は店舗へご連絡ください。" +
                System.Environment.NewLine +
                "※退会処理を行うと会員情報の復元はできません。";
        }

        private string GetChangedTopText()
        {
            return "退会処理が完了しました。" +
                System.Environment.NewLine +
                "ご利用ありがとうございました。";
        }

        private async Task DeleteLocalFiles()
        {
            var result1 = await FileManager.DeleteFileAsync("Account", "accountInfo");
            var result2 = await FileManager.DeleteFileAsync("Account", "deviceInfo");
            System.Diagnostics.Debug.WriteLine(result1);
            System.Diagnostics.Debug.WriteLine(result2);
        }

        private async Task NavigateTopPage()
        {
            await App.customNavigationPage.PushAsync(new AppStart());
        }
    }
}
