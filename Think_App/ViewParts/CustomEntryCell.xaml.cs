using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
    public partial class CustomEntryCell : ContentView
    {
        Color placeholderColor;
        public CustomEntryCell()
        {
            InitializeComponent();

            this.Entry.Focused += (sender, e) =>
            {
                this.Entry.HorizontalTextAlignment = TextAlignment.Start;
                placeholderColor = this.Entry.PlaceholderColor;
                this.Entry.PlaceholderColor = Color.FromRgba(0, 0, 0, 0);
            };

            this.Entry.Unfocused += (sender, e) =>
            {
                this.Entry.HorizontalTextAlignment = TextAlignment.End;
                this.Entry.PlaceholderColor = placeholderColor;
            };
        }
    }
}
