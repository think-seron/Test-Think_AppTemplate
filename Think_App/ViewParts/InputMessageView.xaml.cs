using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Think_App
{
    public partial class InputMessageView : ContentView
    {
        const string _dummyText = " ";

        public new event EventHandler Focused = delegate { };
        public new event EventHandler Unfocused = delegate { };
        public event EventHandler<MessageEditor.Lines> LinesChanged = delegate { };
        public event EventHandler ImageButtonClicked = delegate { };
        public event EventHandler<string> SendButtonClicked = delegate { };
        public event EventHandler DummyTextInputted = delegate { };
        public bool IsSwitchingLines { get; set; }
        InputMessageViewModel Model { get; set; }
        public InputMessageView()
        {
            InitializeComponent();

            Model = new InputMessageViewModel()
            {
                EditorViewModel = new InputMessageEditorViewModel()
                {
                    MessageEditorText = "",
                    MessageEditorOpacity = 1.0
                },
                ViewHeight = 45.0,
                PlusButtonClickedCommand = new Command(OnPlusButtonClicked),
                SendButtonClickedCommand = new Command(OnSendButtonClicked)
            };
            this.BindingContext = Model;

            this.EditorView.InputMessageEditor.TextLinesChanged += OnTextLinesChanged;
            this.EditorView.Focused += OnFocused;
            this.EditorView.Unfocused += OnUnfocused;
        }

        public void InputDummyText()
        {
            // アンドロイドのみ、この対応を入れないとソフトウェアキーボードに入力エリアが隠されてしまう。
            if (Device.RuntimePlatform == Device.Android && Model != null)
            {
                System.Diagnostics.Debug.WriteLine("!!Android対応。キーボードに入力エリアが隠されないように、ダミーで空白を入れて0.5秒後に消去します。");
                Model.EditorViewModel.MessageEditorText += _dummyText;
                Model.EditorViewModel.MessageEditorOpacity = 0.0;
                if (DummyTextInputted != null)
                {
                    DummyTextInputted(this, EventArgs.Empty);
                }
            }
        }

        public async Task ClearDummyTextAsync()
        {
            if (Model != null)
            {
                await Task.Delay(500);
                var length = Model.EditorViewModel.MessageEditorText.Length - _dummyText.Length;
                Model.EditorViewModel.MessageEditorText = Model.EditorViewModel.MessageEditorText.Substring(0, length);
                Model.EditorViewModel.MessageEditorOpacity = 1.0;
            }
        }

        public void ClearText()
        {
            if (Model != null)
            {
                // 入力テキストを空にする。
                Model.EditorViewModel.MessageEditorText = string.Empty;
                // 高さをリセットする。
                Model.ViewHeight = 45.0;
                // １行入力に設定する。
                this.EditorView.InputMessageEditor.Reset();
            }
        }

        void OnPlusButtonClicked()
        {
            if (ImageButtonClicked != null)
            {
                ImageButtonClicked(this, EventArgs.Empty);
            }
        }

        void OnSendButtonClicked()
        {
            if (SendButtonClicked != null)
            {
                SendButtonClicked(this, Model.EditorViewModel.MessageEditorText);
            }
        }

        void OnTextLinesChanged(object sender, MessageEditor.Lines e)
        {
            // ビュー高さ変更
            Model.ViewHeight = (e == MessageEditor.Lines.Multi) ? 66.0 : 45.0;
            // 行変更イベントを飛ばす
            if (LinesChanged != null)
            {
                LinesChanged(this, e);
            }
        }

        void OnFocused(object sender, FocusEventArgs e)
        {
            if (Focused != null)
            {
                Focused(this, e);
            }
        }

        void OnUnfocused(object sender, FocusEventArgs e)
        {
            if (Unfocused != null)
            {
                Unfocused(this, e);
            }
        }

        public new void Unfocus()
        {
            this.EditorView.Unfocus();
        }

        public new bool IsFocused
        {
            get
            {
                return this.EditorView.IsFocused;
            }
        }
    }
}
