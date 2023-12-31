﻿using System;
using Xamarin.Forms;

namespace Think_App
{
    public class CheckBoxView : ContentView
    {
        private readonly Color NotSelectedColor_Droid = Color.FromHex("#767676");
        private readonly Color SelectedColor_Droid = Color.FromHex("#009788");
        private CheckBox _checkBox;
        private Image _image;
        public CheckBoxView()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                Initialize_Droid();
                Update_Droid();
            }
            else
                Initialize_iOS();
        }

        private void Initialize_Droid()
        {
            _checkBox = new CheckBox();
            _checkBox.CheckedChanged += Checkbox_CheckedChanged;
            this.Content = _checkBox;
        }

        private void Checkbox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (Device.RuntimePlatform == Device.Android)
                Update_Droid();
            else
                Update_iOS();
            IsChecked = e.Value;
        }

        public static readonly BindableProperty IsCheckedProperty = BindableProperty.Create(
           propertyName: nameof(IsChecked),
           returnType: typeof(bool),
           declaringType: typeof(CheckBoxView),
           defaultValue: false,
           defaultBindingMode: BindingMode.TwoWay,
           propertyChanged: (bindable, oldValue, newValue) =>
           ((CheckBoxView)bindable).IsChecked = (bool)newValue);

        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }

        private void Update_Droid()
        {
            _checkBox.Color = _checkBox.IsChecked ? SelectedColor_Droid : NotSelectedColor_Droid;
        }

        private void Initialize_iOS()
        {
            _image = new Image
            {
                Source = "checkbox.png",
                HeightRequest = 20,
                WidthRequest = 20,
                BackgroundColor = Color.White,
                VerticalOptions = LayoutOptions.CenterAndExpand,
            };
            HeightRequest = 30;

            GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(TappedCommand) });
            Content = _image;
        }

        private void TappedCommand()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                IsChecked = !IsChecked;
                Update_iOS();
            });
        }

        private void Update_iOS()
        {
            _image.Source = IsChecked ? "checked_checkbox.png" : "checkbox.png";
        }
    }
}
