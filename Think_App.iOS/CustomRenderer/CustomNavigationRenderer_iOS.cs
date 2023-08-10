using System;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;
using System.Linq;
using Foundation;
using Xamarin.Forms;
using Xamarin.Forms.Platform;
using Xamarin.Forms.Platform.iOS;
using Think_App;
using Think_App.iOS;
using UIKit;
using CoreAnimation;
using CoreGraphics;
[assembly: ExportRenderer(typeof(CustomNavigationPage), typeof(CustomNavigationRenderer_iOS))]
namespace Think_App.iOS
{
    public class CustomNavigationRenderer_iOS : NavigationRenderer
    {
        UIView _uiView;
        UIProgressView _uiprogressView;
        CABasicAnimation animation;
        //CALayer layer;
        CAGradientLayer layer;
        CustomNavigationPage customNavigationPage;
        CGPoint pt;
        UIView _NavigationBarMaskView;

        int repeat_ct = 10000000;
        public CustomNavigationRenderer_iOS()
        {
        }
        double interval = 5.0;

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {

                customNavigationPage = e.NewElement as CustomNavigationPage;
                UpdateBarTitleColor();
                customNavigationPage.Popped += (sender, eA) =>
                {
                    UpdateTitleImage();
                };
                customNavigationPage.Pushed += (sender, eB) =>
                {
                    UpdateTitleImage();
                };

                customNavigationPage.UpdateShadowRequested += OnUpdateShadowRequested;
            }

            if (customNavigationPage != null)
            {
                customNavigationPage.PropertyChanged += (sender, eN) =>
                {
                    ////toolbar itemの情報を取得し itemがtextだったら文字を戻るボタンと同じ色に、iconならば白に。
                    try
                    {
                        var item = customNavigationPage.CurrentPage.ToolbarItems.FirstOrDefault((arg) =>
                                                                                    arg.Icon != null || arg.Text != null);
                        if (item == null)
                        {


                        }
                        else if (item.Icon != null && item.Text == null)
                        {
                            this.NavigationBar.TintColor = UIColor.FromRGB((nfloat)ColorList.colorWhite.R, (nfloat)ColorList.colorWhite.G, (nfloat)ColorList.colorWhite.B);

                        }
                        else if (item.Text != null && item.Icon == null)
                        {
                            //this.NavigationBar.TintColor = UIColor.FromRGB((nfloat)ColorList.colorFont.R, (nfloat)ColorList.colorFont.G, (nfloat)ColorList.colorFont.B);
                            this.NavigationBar.TintColor = UIColor.FromRGB((nfloat)ColorList.colorWhite.R, (nfloat)ColorList.colorWhite.G, (nfloat)ColorList.colorWhite.B);
                        }
                        else
                        {

                            if (this.NavigationItem.RightBarButtonItems != null)
                            {
                            }
                        }

                        if (eN.PropertyName == CustomNavigationPage.IsRunningPoperty.PropertyName)
                        {
                            layer.Hidden = !customNavigationPage.IsRunning;
                        }
                        else if (eN.PropertyName == CustomNavigationPage.IsBadgeVisbleProperty.PropertyName)
                        {
                            badge.Hidden = !customNavigationPage.IsBadgeVisble;
                        }
                        else if (eN.PropertyName == CustomNavigationPage.BarTextColorProperty.PropertyName)
                        {
                            UpdateBarTitleColor();
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("excepetion  :" + ex);
                    }
                };
            }
        }


        async void UpdateTitleImage()
        {


            if (!(customNavigationPage.CurrentPage is CustomHomePage))
            {
                this.NavigationBar.TopItem.TitleView = null;
                return;
            }
            // ステータスバーの高さ
            var statusBarHeight = UIApplication.SharedApplication.StatusBarFrame.Size.Height;

            var page = customNavigationPage.CurrentPage;
            if (((CustomHomePage)page).TitleImage != null)
            {
                var image = await ((CustomHomePage)page).TitleImage.ToUIImageAsync();

                var uiview = new UIImageView(image);

                this.NavigationBar.TopItem.Title = null;
                this.NavigationBar.TopItem.TitleView = uiview;


            }
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            if (ScaleManager.NavigationHeight <= 0)
            {
                try
                {
                    ScaleManager.NavigationHeight = this.NavigationController.NavigationBar.Frame.Size.Height;

                }
                catch (Exception ex)
                {
                    ScaleManager.NavigationHeight = 0;
                }
                System.Diagnostics.Debug.WriteLine("navigation height :" + ScaleManager.NavigationHeight);
            }


            if (layer == null)
            {
                var top = UIApplication.SharedApplication.KeyWindow.SafeAreaInsets.Top + NavigationBar.Bounds.Height;
                var rect = new CGRect((nfloat)0.0, top, UIScreen.MainScreen.Bounds.Width / 3, (nfloat)2.0);
                layer = new CAGradientLayer();
                layer.StartPoint = new CGPoint((nfloat)0.0, top);
                layer.EndPoint = new CGPoint(UIScreen.MainScreen.Bounds.Width, top);
                layer.Frame = rect;
                layer.Colors = new CGColor[] {

					//UIColor.FromRGB(87,68,27).CGColor,
					//UIColor.FromRGB(89,54,72).CGColor
					ColorList.colorbatch.ToCGColor(),
					//UIColor.FromRGB(171,218,209).CGColor,
					UIColor.White.CGColor
                };


                //layer = new CALayer();	
                pt = layer.Position;
                ////layer.Position = new CGPoint(150, 350);
                //layer.Bounds = rect;
                //var basicAnimation = CABasicAnimation.FromKeyPath("colors");
                //basicAnimation.TimingFunction = CAMediaTimingFunction.FromName(CAMediaTimingFunction.EaseInEaseOut);
                //basicAnimation.RepeatCount = 1000;
                //basicAnimation.Duration = 1.0;
                //animation = CABasicAnimation.FromKeyPath("colors");
                animation = CABasicAnimation.FromKeyPath("position");
                animation.TimingFunction = CAMediaTimingFunction.FromName(CAMediaTimingFunction.EaseInEaseOut);
                animation.RepeatCount = repeat_ct;
                animation.Duration = 0.5;
                //animation.SetFrom(UIColor.Blue.CGColor);
                //animation.SetTo(UIColor.White.CGColor);

                animation.From = NSValue.FromCGPoint(pt);
                animation.To = NSValue.FromCGPoint(new CGPoint(UIScreen.MainScreen.Bounds.Width, top));

                layer.AddAnimation(animation, "position");
                this.View.Layer.AddSublayer(layer);
                layer.Hidden = true;
            }


            //
            if (badge == null)
            {
                var rect = new CGRect(((UIScreen.MainScreen.Bounds.Width) - (nfloat)20.0), (nfloat)25.0, (nfloat)10.0, (nfloat)10.0);

                badge = new CAShapeLayer();
                badge.CornerRadius = (nfloat)5.0;
                badge.BackgroundColor = new CGColor((nfloat)0.0, (nfloat)1.0, (nfloat)0.0);
                badge.FillColor = new CGColor((nfloat)0.0, (nfloat)0.0, (nfloat)1.0);
                badge.Frame = rect;
                //shape.FillColor
                this.View.Layer.AddSublayer(badge);
                badge.Hidden = true;
                badge.ZPosition = (nfloat)1.0;

            }




        }
        CAShapeLayer badge;
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            UpdateBarTitleColor();

            // 影マスクの色。
            var shadowColor = Color.FromRgba(0, 0, 0, 0.8).ToUIColor();

            // ステータスバーの高さ
            var statusBarHeight = UIApplication.SharedApplication.StatusBarFrame.Size.Height;

            // 影マスク作成。
            _NavigationBarMaskView = new UIView();
            _NavigationBarMaskView.Frame = new CGRect(NavigationBar.Bounds.X,
                                                      NavigationBar.Bounds.Y - statusBarHeight,
                                                      NavigationBar.Bounds.Width,
                                                      NavigationBar.Bounds.Height + statusBarHeight);
            _NavigationBarMaskView.Opaque = false;
            _NavigationBarMaskView.BackgroundColor = shadowColor;

            // zを最大にしておくことで、すべてのコントロールの一番上に置かれる。
            _NavigationBarMaskView.Layer.ZPosition = nfloat.MaxValue;
        }

        const string homeTitle = "ホーム";

        //public override void ViewDidLoad()
        //{
        //	base.ViewDidLoad();
        //}

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
        }

        void OnUpdateShadowRequested(object sender, bool shadow)
        {
            if (shadow)
            {
                // ステータスバーとナビゲーションバーに影をつける。
                NavigationBar.InsertSubview(_NavigationBarMaskView, 0);
            }
            else
            {
                // ステータスバーとナビゲーションバーの影を削除する。
                _NavigationBarMaskView.RemoveFromSuperview();
            }
        }

        void UpdateBarTitleColor()
        {
            if (customNavigationPage == null) return;
            if (NavigationBar == null) return;

            var barTextColor = customNavigationPage.BarTextColor;

            // Determine new title text attributes via global static data
            var globalTitleTextAttributes = UINavigationBar.Appearance.TitleTextAttributes;
            var titleTextAttributes = new UIStringAttributes
            {
                ForegroundColor = ColorList.colorNavibarTextColor.ToUIColor(),
                Font = globalTitleTextAttributes?.Font
            };
            if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
            {
                if (NavigationBar.CompactAppearance != null)
                    NavigationBar.CompactAppearance.TitleTextAttributes = titleTextAttributes;

                if (NavigationBar.StandardAppearance != null)
                    NavigationBar.StandardAppearance.TitleTextAttributes = titleTextAttributes;

                if (NavigationBar.ScrollEdgeAppearance != null)
                    NavigationBar.ScrollEdgeAppearance.TitleTextAttributes = titleTextAttributes;
            }
            else
            {
                NavigationBar.TitleTextAttributes = titleTextAttributes;
            }
        }
    }
}
