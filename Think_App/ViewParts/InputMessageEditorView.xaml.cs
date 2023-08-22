using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
    public partial class InputMessageEditorView : ContentView
    {
        public event EventHandler<TextChangedEventArgs> TextChanged = delegate { };
        public new event EventHandler<FocusEventArgs> Focused = delegate { };
        public new event EventHandler<FocusEventArgs> Unfocused = delegate { };
        public MessageEditor InputMessageEditor { get { return this.MessageEditor; } }

        InputMessageEditorViewModel Model { get; set; }
        public InputMessageEditorView()
        {
            InitializeComponent();

            // 外からBindingする。
            this.BindingContextChanged += (sender, e) =>
            {
                Model = this.BindingContext as InputMessageEditorViewModel;
            };

            this.MessageEditor.TextChanged += MessageEditor_TextChanged;
            this.MessageEditor.Focused += MessageEditor_Focused;
            this.MessageEditor.Unfocused += MessageEditor_Unfocused;
        }

        public void SetEnable(bool enable)
        {
            this.MessageEditor.IsEnabled = enable;
        }

        void MessageEditor_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextChanged != null)
            {
                TextChanged(this, e);
            }
        }

        void MessageEditor_Focused(object sender, FocusEventArgs e)
        {
            if (Focused != null)
            {
                Focused(this, e);
            }
        }

        void MessageEditor_Unfocused(object sender, FocusEventArgs e)
        {
            if (Unfocused != null)
            {
                Unfocused(this, e);
            }
        }

        public new void Focus()
        {
            this.MessageEditor.Focus();
        }

        public new void Unfocus()
        {
            this.MessageEditor.Unfocus();
        }

        public new bool IsFocused
        {
            get
            {
                return this.MessageEditor.IsFocused;
            }
        }
    }
}
