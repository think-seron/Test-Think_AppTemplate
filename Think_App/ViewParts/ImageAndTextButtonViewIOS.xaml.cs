﻿using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace Think_App
{
    public partial class ImageAndTextButtonViewIOS : Frame
    {
        public ImageAndTextButtonViewIOS()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty BtnSizeProperty = BindableProperty.Create(
            propertyName: nameof(BtnSize),
            returnType: typeof(double),
            declaringType: typeof(ImageAndTextButtonViewIOS),
            defaultValue: 0d,
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: (bindable, oldValue, newValue) =>
            ((ImageAndTextButtonViewIOS)bindable).BtnSize = (double)newValue);

        public double BtnSize
        {
            get { return (double)GetValue(BtnSizeProperty); }
            set { SetValue(BtnSizeProperty, value); }
        }

        public static readonly BindableProperty BtnBgColorProperty = BindableProperty.Create(
            propertyName: nameof(BtnBgColor),
            returnType: typeof(Color),
            declaringType: typeof(ImageAndTextButtonViewIOS),
            defaultValue: Color.White,
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: (bindable, oldValue, newValue) =>
            ((ImageAndTextButtonViewIOS)bindable).BtnBgColor = (Color)newValue);

        public Color BtnBgColor
        {
            get { return (Color)GetValue(BtnBgColorProperty); }
            set { SetValue(BtnBgColorProperty, value); }
        }

        public static readonly BindableProperty BtnImageProperty = BindableProperty.Create(
            propertyName: nameof(BtnImage),
            returnType: typeof(ImageSource),
            declaringType: typeof(ImageAndTextButtonViewIOS),
            defaultValue: null,
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: (bindable, oldValue, newValue) =>
            ((ImageAndTextButtonViewIOS)bindable).BtnImage = (ImageSource)newValue);

        public ImageSource BtnImage
        {
            get { return (ImageSource)GetValue(BtnImageProperty); }
            set { SetValue(BtnImageProperty, value); }
        }

        public static readonly BindableProperty BtnImageHeightProperty = BindableProperty.Create(
            propertyName: nameof(BtnImageHeight),
            returnType: typeof(double),
            declaringType: typeof(ImageAndTextButtonViewIOS),
            defaultValue: 0d,
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: (bindable, oldValue, newValue) =>
            ((ImageAndTextButtonViewIOS)bindable).BtnImageHeight = (double)newValue);

        public double BtnImageHeight
        {
            get { return (double)GetValue(BtnImageHeightProperty); }
            set { SetValue(BtnImageHeightProperty, value); }
        }

        public static readonly BindableProperty BtnImageWidthProperty = BindableProperty.Create(
            propertyName: nameof(BtnImageWidth),
            returnType: typeof(double),
            declaringType: typeof(ImageAndTextButtonViewIOS),
            defaultValue: 0d,
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: (bindable, oldValue, newValue) =>
            ((ImageAndTextButtonViewIOS)bindable).BtnImageWidth = (double)newValue);

        public double BtnImageWidth
        {
            get { return (double)GetValue(BtnImageWidthProperty); }
            set { SetValue(BtnImageWidthProperty, value); }
        }

        public static readonly BindableProperty BtnTextProperty = BindableProperty.Create(
            propertyName: nameof(BtnText),
            returnType: typeof(string),
            declaringType: typeof(ImageAndTextButtonViewIOS),
            defaultValue: null,
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: (bindable, oldValue, newValue) =>
            ((ImageAndTextButtonViewIOS)bindable).BtnText = (string)newValue);

        public string BtnText
        {
            get { return (string)GetValue(BtnTextProperty); }
            set { SetValue(BtnTextProperty, value); }
        }

        public static readonly BindableProperty BtnFontSizeProperty = BindableProperty.Create(
            propertyName: nameof(BtnFontSize),
            returnType: typeof(double),
            declaringType: typeof(ImageAndTextButtonViewIOS),
            defaultValue: 0d,
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: (bindable, oldValue, newValue) =>
            ((ImageAndTextButtonViewIOS)bindable).BtnFontSize = (double)newValue);

        public double BtnFontSize
        {
            get { return (double)GetValue(BtnFontSizeProperty); }
            set { SetValue(BtnFontSizeProperty, value); }
        }

        public static readonly BindableProperty BtnTextColorProperty = BindableProperty.Create(
            propertyName: nameof(BtnTextColor),
            returnType: typeof(Color),
            declaringType: typeof(ImageAndTextButtonViewIOS),
            defaultValue: Color.White,
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: (bindable, oldValue, newValue) =>
            ((ImageAndTextButtonViewIOS)bindable).BtnTextColor = (Color)newValue);

        public Color BtnTextColor
        {
            get { return (Color)GetValue(BtnTextColorProperty); }
            set { SetValue(BtnTextColorProperty, value); }
        }

        public static readonly BindableProperty BtnCommandProperty = BindableProperty.Create(
            propertyName: nameof(BtnCommand),
            returnType: typeof(ICommand),
            declaringType: typeof(ImageAndTextButtonViewIOS),
            defaultValue: null,
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: (bindable, oldValue, newValue) =>
            ((ImageAndTextButtonViewIOS)bindable).BtnCommand = (ICommand)newValue);

        public ICommand BtnCommand
        {
            get { return (ICommand)GetValue(BtnCommandProperty); }
            set { SetValue(BtnCommandProperty, value); }
        }
    }
}