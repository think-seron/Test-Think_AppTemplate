using System.Reflection;
using Java.Lang;
using System.Timers;
using Android.Widget;
using Android.Views;
using System.ComponentModel;
using Android.Graphics;
using Android.Content;
using Think_App;
using Think_App.Droid;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

// TODO Xamarin.Forms.ExportRendererAttribute is not longer supported. For more details see https://github.com/dotnet/maui/wiki/Using-Custom-Renderers-in-.NET-MAUI
[assembly: ExportRenderer(typeof(CarouselLayout), typeof(CarouselLayoutRenderer))]

namespace Think_App.Droid
{
    public class CarouselLayoutRenderer : ScrollViewRenderer
    {
        int _prevScrollX;
        int _deltaX;
        bool _motionDown;
        Timer _deltaXResetTimer;
        Timer _scrollStopTimer;
        HorizontalScrollView _scrollView;
        CarouselLayout _carouselLayout;
        public CarouselLayoutRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            _carouselLayout = e.NewElement as CarouselLayout;
            if (_carouselLayout == null) return;
            _deltaXResetTimer = new Timer(100) { AutoReset = false };
            _deltaXResetTimer.Elapsed += (object sender, ElapsedEventArgs args) => _deltaX = 0;

            _scrollStopTimer = new Timer(200) { AutoReset = false };
            _scrollStopTimer.Elapsed += (object sender, ElapsedEventArgs args2) => UpdateSelectedIndex();

            e.NewElement.PropertyChanged += ElementPropertyChanged;
        }

        void ElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Renderer")
            {
                _scrollView = (HorizontalScrollView)typeof(ScrollViewRenderer)
                    .GetField("_hScrollView", BindingFlags.NonPublic | BindingFlags.Instance)
                    .GetValue(this);

                _scrollView.HorizontalScrollBarEnabled = false;
                _scrollView.Touch += HScrollViewTouch;
            }
            if (e.PropertyName == CarouselLayout.SelectedIndexProperty.PropertyName && !_motionDown)
            {
                ScrollToIndex(((CarouselLayout)this.Element).SelectedIndex);
            }
        }

        void HScrollViewTouch(object sender, TouchEventArgs e)
        {
            if (_carouselLayout == null || !_carouselLayout.IsEnabled) return;
            e.Handled = false;

            switch (e.Event.Action)
            {
                case MotionEventActions.Move:
                    _deltaXResetTimer.Stop();
                    _deltaX = _scrollView.ScrollX - _prevScrollX;
                    _prevScrollX = _scrollView.ScrollX;

                    UpdateSelectedIndex();

                    _deltaXResetTimer.Start();
                    break;
                case MotionEventActions.Down:
                    _motionDown = true;
                    _scrollStopTimer.Stop();
                    break;
                case MotionEventActions.Up:
                    _motionDown = false;
                    SnapScroll();
                    _scrollStopTimer.Start();
                    break;
            }
        }

        void UpdateSelectedIndex()
        {
            var center = _scrollView.ScrollX + (_scrollView.Width / 2);
            var carouselLayout = (CarouselLayout)this.Element;
            Device.BeginInvokeOnMainThread(() =>
            {
                carouselLayout.SelectedIndex = center / _scrollView.Width;
            });
        }

        void SnapScroll()
        {
            var roughIndex = (float)_scrollView.ScrollX / _scrollView.Width;

            var targetIndex =
                _deltaX < 0 ? Math.Floor(roughIndex)
                : _deltaX > 0 ? Math.Ceil(roughIndex)
                : Math.Round(roughIndex);

            ScrollToIndex((int)targetIndex);
        }

        void ScrollToIndex(int targetIndex)
        {
            var targetX = targetIndex * _scrollView.Width;
            _scrollView.Post(new Runnable(() =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    _scrollView.SmoothScrollTo(targetX, 0);
                });
            }));
        }

        bool _initialized = false;
        public override void Draw(Canvas canvas)
        {
            base.Draw(canvas);
            if (_initialized) return;
            _initialized = true;
            var carouselLayout = (CarouselLayout)this.Element;
            Device.BeginInvokeOnMainThread(() =>
            {
                _scrollView.ScrollTo(carouselLayout.SelectedIndex * Width, 0);
            });
        }

        protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
        {
            if (_initialized && (w != oldw))
            {
                _initialized = false;
            }
            base.OnSizeChanged(w, h, oldw, oldh);
        }
    }
}