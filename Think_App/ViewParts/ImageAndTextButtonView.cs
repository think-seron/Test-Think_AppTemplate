using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Think_App
{
    public class ImageAndTextButtonView : ContentView
    {
        private ImageAndTextButtonViewDroid _droidView;
        private ImageAndTextButtonViewIOS _iOSView;
        public ImageAndTextButtonView()
        {
            Initialize();

            PropertyChanged += ImageAndTextButtonView_PropertyChanged;
        }

        private void ImageAndTextButtonView_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == BtnSizeProperty.PropertyName)
                UpdateBtnSize();
            if (e.PropertyName == BtnBgColorProperty.PropertyName)
                UpdateBtnBgColor();
            if (e.PropertyName == BtnImageProperty.PropertyName)
                UpdateBtnImage();
            if (e.PropertyName == BtnImageHeightProperty.PropertyName)
                UpdateBtnImageHeight();
            if (e.PropertyName == BtnImageWidthProperty.PropertyName)
                UpdateBtnImageWidth();
            if (e.PropertyName == BtnTextProperty.PropertyName)
                UpdateBtnText();
            if (e.PropertyName == BtnFontSizeProperty.PropertyName)
                UpdateBtnFontSize();
            if (e.PropertyName == BtnTextColorProperty.PropertyName)
                UpdateBtnTextColor();
            if (e.PropertyName == BtnCommandProperty.PropertyName)
                UpdateBtnCommand();
        }

        public static readonly BindableProperty BtnSizeProperty = BindableProperty.Create(
            propertyName: nameof(BtnSize),
            returnType: typeof(double),
            declaringType: typeof(ImageAndTextButtonView),
            defaultValue: 0d,
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: (bindable, oldValue, newValue) =>
            ((ImageAndTextButtonView)bindable).BtnSize = (double)newValue);

        public double BtnSize
        {
            get { return (double)GetValue(BtnSizeProperty); }
            set { SetValue(BtnSizeProperty, value); }
        }

        public static readonly BindableProperty BtnBgColorProperty = BindableProperty.Create(
            propertyName: nameof(BtnBgColor),
            returnType: typeof(Color),
            declaringType: typeof(ImageAndTextButtonView),
            defaultValue: Color.White,
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: (bindable, oldValue, newValue) =>
            ((ImageAndTextButtonView)bindable).BtnBgColor = (Color)newValue);

        public Color BtnBgColor
        {
            get { return (Color)GetValue(BtnBgColorProperty); }
            set { SetValue(BtnBgColorProperty, value); }
        }

        public static readonly BindableProperty BtnImageProperty = BindableProperty.Create(
            propertyName: nameof(BtnImage),
            returnType: typeof(ImageSource),
            declaringType: typeof(ImageAndTextButtonView),
            defaultValue: null,
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: (bindable, oldValue, newValue) =>
            ((ImageAndTextButtonView)bindable).BtnImage = (ImageSource)newValue);

        public ImageSource BtnImage
        {
            get { return (ImageSource)GetValue(BtnImageProperty); }
            set { SetValue(BtnImageProperty, value); }
        }

        public static readonly BindableProperty BtnImageHeightProperty = BindableProperty.Create(
            propertyName: nameof(BtnImageHeight),
            returnType: typeof(double),
            declaringType: typeof(ImageAndTextButtonView),
            defaultValue: 0d,
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: (bindable, oldValue, newValue) =>
            ((ImageAndTextButtonView)bindable).BtnImageHeight = (double)newValue);

        public double BtnImageHeight
        {
            get { return (double)GetValue(BtnImageHeightProperty); }
            set { SetValue(BtnImageHeightProperty, value); }
        }

        public static readonly BindableProperty BtnImageWidthProperty = BindableProperty.Create(
            propertyName: nameof(BtnImageWidth),
            returnType: typeof(double),
            declaringType: typeof(ImageAndTextButtonView),
            defaultValue: 0d,
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: (bindable, oldValue, newValue) =>
            ((ImageAndTextButtonView)bindable).BtnImageWidth = (double)newValue);

        public double BtnImageWidth
        {
            get { return (double)GetValue(BtnImageWidthProperty); }
            set { SetValue(BtnImageWidthProperty, value); }
        }

        public static readonly BindableProperty BtnTextProperty = BindableProperty.Create(
            propertyName: nameof(BtnText),
            returnType: typeof(string),
            declaringType: typeof(ImageAndTextButtonView),
            defaultValue: null,
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: (bindable, oldValue, newValue) =>
            ((ImageAndTextButtonView)bindable).BtnText = (string)newValue);

        public string BtnText
        {
            get { return (string)GetValue(BtnTextProperty); }
            set { SetValue(BtnTextProperty, value); }
        }

        public static readonly BindableProperty BtnFontSizeProperty = BindableProperty.Create(
            propertyName: nameof(BtnFontSize),
            returnType: typeof(double),
            declaringType: typeof(ImageAndTextButtonView),
            defaultValue: 0d,
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: (bindable, oldValue, newValue) =>
            ((ImageAndTextButtonView)bindable).BtnFontSize = (double)newValue);

        public double BtnFontSize
        {
            get { return (double)GetValue(BtnFontSizeProperty); }
            set { SetValue(BtnFontSizeProperty, value); }
        }

        public static readonly BindableProperty BtnTextColorProperty = BindableProperty.Create(
            propertyName: nameof(BtnTextColor),
            returnType: typeof(Color),
            declaringType: typeof(ImageAndTextButtonView),
            defaultValue: Color.White,
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: (bindable, oldValue, newValue) =>
            ((ImageAndTextButtonView)bindable).BtnTextColor = (Color)newValue);

        public Color BtnTextColor
        {
            get { return (Color)GetValue(BtnTextColorProperty); }
            set { SetValue(BtnTextColorProperty, value); }
        }

        public static readonly BindableProperty BtnCommandProperty = BindableProperty.Create(
            propertyName: nameof(BtnCommand),
            returnType: typeof(ICommand),
            declaringType: typeof(ImageAndTextButtonView),
            defaultValue: null,
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: (bindable, oldValue, newValue) =>
            ((ImageAndTextButtonView)bindable).BtnCommand = (ICommand)newValue);

        public ICommand BtnCommand
        {
            get { return (ICommand)GetValue(BtnCommandProperty); }
            set { SetValue(BtnCommandProperty, value); }
        }


        private void Initialize()
        {
            if (Device.RuntimePlatform == Device.Android)
                InitializeDroid();
            else
                InitializeIOS();
        }

        private void InitializeDroid()
        {
            _droidView = new ImageAndTextButtonViewDroid
            {
                BtnSize = BtnSize,
                BtnBgColor = BtnBgColor,
                BtnImage = BtnImage,
                BtnImageHeight = BtnImageHeight,
                BtnImageWidth = BtnImageWidth,
                BtnText = BtnText,
                BtnFontSize = BtnFontSize,
                BtnTextColor = BtnTextColor,
                BtnCommand = BtnCommand,
            };
            Content = _droidView;
        }

        private void InitializeIOS()
        {
            _iOSView = new ImageAndTextButtonViewIOS
            {
                BtnSize = BtnSize,
                BtnBgColor = BtnBgColor,
                BtnImage = BtnImage,
                BtnImageHeight = BtnImageHeight,
                BtnImageWidth = BtnImageWidth,
                BtnText = BtnText,
                BtnFontSize = BtnFontSize,
                BtnTextColor = BtnTextColor,
                BtnCommand = BtnCommand,
            };
            Content = _iOSView;
        }

        private void UpdateBtnSize()
        {
            if (Device.RuntimePlatform == Device.Android)
                _droidView.BtnSize = BtnSize;
            else
                _iOSView.BtnSize = BtnSize;
        }

        private void UpdateBtnBgColor()
        {
            if (Device.RuntimePlatform == Device.Android)
                _droidView.BtnBgColor = BtnBgColor;
            else
                _iOSView.BtnBgColor = BtnBgColor;
        }

        private void UpdateBtnImage()
        {
            if (Device.RuntimePlatform == Device.Android)
                _droidView.BtnImage = BtnImage;
            else
                _iOSView.BtnImage = BtnImage;
        }

        private void UpdateBtnImageHeight()
        {
            if (Device.RuntimePlatform == Device.Android)
                _droidView.BtnImageHeight = BtnImageHeight;
            else
                _iOSView.BtnImageHeight = BtnImageHeight;
        }

        private void UpdateBtnImageWidth()
        {
            if (Device.RuntimePlatform == Device.Android)
                _droidView.BtnImageWidth = BtnImageWidth;
            else
                _iOSView.BtnImageWidth = BtnImageWidth;
        }

        private void UpdateBtnText()
        {
            if (Device.RuntimePlatform == Device.Android)
                _droidView.BtnText = BtnText;
            else
                _iOSView.BtnText = BtnText;
        }

        private void UpdateBtnFontSize()
        {
            if (Device.RuntimePlatform == Device.Android)
                _droidView.BtnFontSize = BtnFontSize;
            else
                _iOSView.BtnFontSize = BtnFontSize;
        }

        private void UpdateBtnTextColor()
        {
            if (Device.RuntimePlatform == Device.Android)
                _droidView.BtnTextColor = BtnTextColor;
            else
                _iOSView.BtnTextColor = BtnTextColor;
        }

        private void UpdateBtnCommand()
        {
            if (Device.RuntimePlatform == Device.Android)
                _droidView.BtnCommand = BtnCommand;
            else
                _iOSView.BtnCommand = BtnCommand;
        }
    }
}

