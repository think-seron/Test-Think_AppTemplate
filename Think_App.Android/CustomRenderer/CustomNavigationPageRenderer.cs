using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Reflection;

using Android.Widget;
using Android.Graphics;
using Support = Android.Support.V7.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform.Android.AppCompat;
using Think_App;
using Think_App.Droid;

using AProgressBar = Android.Widget.ProgressBar;
using View = Android.Views.View;
using Android.Views;
using Android.Content;

[assembly: ExportRenderer(typeof(CustomNavigationPage), typeof(CustomNavigationPageRenderer))]
namespace Think_App.Droid
{
	public class CustomNavigationPageRenderer : NavigationPageRenderer
	{
		CustomNavigationPage _CustomNavigationPage;
		private Support.Toolbar _toolbar;
		private AProgressBar _progress;
		TextView titleView;
		ImageView titleImageView;
		ImageView _imageView;
		Android.Views.ViewGroup ViewGroup;

		View _NavigationBarMaskView;
		int _contentInsetLeft, _contentInsetRight;
		bool _hasBackButton;
		List<ToolbarItem> _toolbarItems = new List<ToolbarItem>();


		//protected override void OnLayout(bool changed, int l, int t, int r, int b)
		//{
		//	base.OnLayout(changed, l, t, r, b);
		//	for (var i = 0; i < ChildCount; ++i)
		//	{
		//		var view = GetChildAt(i);
		//		if (view is Support.Toolbar)
		//		{
		//			try
		//			{
		//				_progress?.SetZ(_toolbar.GetZ() + 1);
		//				_progress?.Layout(l, _toolbar.Bottom - 10, r, view.Bottom + 10);
		//			}
		//			catch (Exception ex){			
		//			}
		//		}
		//	}
		//}
		//public override void OnViewAdded(Android.Views.View child)
		//{
		//	base.OnViewAdded(child);
		//	if (child.GetType() == typeof(Support.Toolbar))
		//		_toolbar = (Support.Toolbar)child;
		//}

		protected override void OnElementChanged(ElementChangedEventArgs<NavigationPage> e)
		{
			base.OnElementChanged(e);

			if (e.NewElement != null) {
				_CustomNavigationPage = e.NewElement as CustomNavigationPage;
				SetProgress();
				System.Diagnostics.Debug.WriteLine("OnElementChanged  SetTitleImage :");
				//SetTitleImage();
				_CustomNavigationPage.Popped += (sender, eN) => {
					UpdateTitleImage();
				};

				_CustomNavigationPage.Pushed += (sender, eN) =>
				{
					UpdateTitleImage();
				};

				_CustomNavigationPage.UpdateShadowRequested += OnUpdateShadowRequested;
				_NavigationBarMaskView = new PaintedView(Forms.Context, Xamarin.Forms.Color.FromRgba(0, 0, 0, 0.8).ToAndroid());
			}
			//if (_progress == null)
			//{
			//	_progress = new AProgressBar(Context, null, Android.Resource.Attribute.ProgressBarStyleHorizontal)
			//	{
			//		Indeterminate = true
			//	};

			//	AddView(_progress);
			//	_progress.Visibility = ViewStates.Invisible;
			//}

		}


		void UpdateTitleImage()
		{

			if (_CustomNavigationPage.CurrentPage is CustomHomePage)
			{
				System.Diagnostics.Debug.WriteLine("CustomContentPage is true ");

				var page = _CustomNavigationPage.CurrentPage;
				if (((CustomHomePage)page).TitleImage == null)
				{
					System.Diagnostics.Debug.WriteLine("TitleImage == null  ");
					SetTitle(false);
					titleView.Visibility = ViewStates.Visible;
				}
				else
				{
					
					System.Diagnostics.Debug.WriteLine("TitleImage != null  ");
					SetTitle(true);
					titleView.Visibility = ViewStates.Invisible;
				}
			}
			else {
				System.Diagnostics.Debug.WriteLine("CustomContentPage is false ");

				SetTitle(false);
				titleView.Visibility = ViewStates.Visible;
			}


			//if ((_CustomNavigationPage.CurrentPage is CustomContentPage) && _imageView != null)
			//{
			//	var page = _CustomNavigationPage.CurrentPage;
			//	Device.BeginInvokeOnMainThread(() =>
			//	{
			//		if (((CustomContentPage)page).TitleImage != null)
			//		{
			//			System.Diagnostics.Debug.WriteLine("OnElementChanged  update visible change to visible :");
			//SetTitle(true);
			//			//titleView.RemoveFromParent();
			//			//ViewGroup.AddView(_imageView);
			//			//_imageView.Visibility = ViewStates.Visible;
			//		}
			//		else
			//		{
			//			System.Diagnostics.Debug.WriteLine("OnElementChanged  update visible but TitleImage is null");
			//			//_imageView.Visibility = ViewStates.Invisible;
			//			//titleView.Visibility = ViewStates.Visible;
			//			//_imageView.RemoveFromParent();
			//			//ViewGroup.AddView(titleView);
			//			if (_imageView.Parent != null)
			//			{
			//				_imageView.RemoveFromParent();
			//				_imageView = null;
			//			}
			//			if (titleView.Parent == null)
			//			{
			//				ViewGroup.AddView(titleView);
			//			}
			//		}
			//	});
			//}
			//else 
			//	//if (!(_CustomNavigationPage.CurrentPage is CustomContentPage) && _imageView != null && _imageView.Visibility == ViewStates.Visible)
			//{
			//	System.Diagnostics.Debug.WriteLine("OnElementChanged update visible change to invisible :");
			//	Device.BeginInvokeOnMainThread(() =>
			//	{
			//		//_imageView.Visibility = ViewStates.Invisible;
			///titleView.Visibility = ViewStates.Visible;
			//	});
			//}
		}

		//imageをセットする場合はtrue,textはfalse
		void SetTitle(bool IsImg)
		{
			if (IsImg)
			{
				if (_imageView == null)
				{
					System.Diagnostics.Debug.WriteLine(" _imageview null  ");
					SetTitleImage();
				}
				if (titleView == null)
				{
					System.Diagnostics.Debug.WriteLine(" titleView == null  ");
					return;
				}
				if (titleView.Visibility != ViewStates.Invisible)
				{
					titleView.Visibility = ViewStates.Invisible;
					//Device.BeginInvokeOnMainThread(() =>
					//{
					//	titleView.RemoveFromParent();
					//});
					//if (_imageView == null)
						
				}
                SetTitleImage();
			}
			else
			{
				if (titleView == null)
				{
					ExtendedActionBar(_CustomNavigationPage);
				}
				else if (titleView.Parent == null)
				{
					Device.BeginInvokeOnMainThread(() =>
					{
						_toolbar.AddView(titleView);
					});
				}
				if (_imageView == null)
					return;
				
				if (_imageView.Parent != null)
				{
					Device.BeginInvokeOnMainThread(() =>
					{
						_imageView.RemoveFromParent();
					});
				}

			}
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
			var page = this.Element as CustomNavigationPage;
			//SetTitleImage();
			SetProgress();
			ExtendedActionBar(page);

			System.Diagnostics.Debug.WriteLine("OnElementPropertyChanged :" + e.PropertyName);

			if (e.PropertyName == CustomNavigationPage.IsRunningPoperty.PropertyName||e.PropertyName==CustomNavigationPage.CurrentPageProperty.PropertyName)
			{
				if (_progress == null||App.customNavigationPage==null)
					return;
				_progress.Visibility = App.customNavigationPage.IsRunning ? ViewStates.Visible : ViewStates.Invisible;
				System.Diagnostics.Debug.WriteLine("_progress.Visibility :"+_progress.Visibility);
			}
				

			//if (_imageView != null)
			//	_imageView.Visibility = ViewStates.Invisible;
			//if(titleView!=null)
			//	titleView.Visibility = ViewStates.Visible;
			//if (e.PropertyName == CustomNavigationPage.TitleImageProperty.PropertyName)
			//{
			//	System.Diagnostics.Debug.WriteLine("OnElementPropertyChanged  SetTitleImage :");
			//	if (_CustomNavigationPage.TitleImage != null)
			//	{
			//		//titleView.Visibility = ViewStates.Invisible;
			//		//titleImageView = (ImageView)_toolbar.FindViewById(Resource.Id.titleImageView);
			//		//titleImageView.Visibility = ViewStates.Visible;
			//		//titleImageView.SetImageResource(Resource.Drawable.googleBtn);
			//		//titleImageView.SetMinimumWidth(100);
			//		//titleImageView.SetMinimumHeight(40);
			//		_imageView.Visibility = ViewStates.Visible;
			//		titleView.Visibility = ViewStates.Invisible;
			//	}
			//}
			//if(_imageView !=null&&titleView !=null)
			//System.Diagnostics.Debug.WriteLine("    _image view visible  :" + _imageView.Visibility + "  _title view visiblity :" + titleView.Visibility);

		}
		void SetTitleImage() {
			System.Diagnostics.Debug.WriteLine("Set Title Image");
			if (_CustomNavigationPage == null)
			{
				
				return;
			}
			ViewGroup = this.Parent as ViewGroup;
			if (ViewGroup == null)
				return;
			if (_CustomNavigationPage.Width <= 0 || _CustomNavigationPage.Height <= 0)
				return;
			if (_toolbar == null)
				return;

			_imageView = new ImageView(Forms.Context)
			{
				Visibility = ViewStates.Visible,
			};

			System.Diagnostics.Debug.WriteLine(" _toolbar.Top :" + (_toolbar.Top +5));
			System.Diagnostics.Debug.WriteLine(" _toolbar.Bottom :" + (_toolbar.Bottom - 5));
			System.Diagnostics.Debug.WriteLine(" _toolbar.Height :" + (_toolbar.Height));

			var bottom = (int)Forms.Context.ToPixels(_CustomNavigationPage.Y);
			System.Diagnostics.Debug.WriteLine("bottom :" + bottom);

			var center =( bottom + _toolbar.Top)/2;
			var result =( center + _imageView.Height) / 2;

			_imageView.SetImageResource(Resource.Drawable.AppHeaderLogo);


_imageView?.Layout
			(0,
				(int)Forms.Context.ToPixels(_CustomNavigationPage.Y),
			     (int)Forms.Context.ToPixels(_CustomNavigationPage.Width),
				 _toolbar.Height + (int)Forms.Context.ToPixels(_CustomNavigationPage.Y));

			_imageView.SetScaleType(ImageView.ScaleType.FitCenter);

			//_imageView.Layout(65, _toolbar.Top, 250, result);

			//_imageView.SetForegroundGravity(GravityFlags.CenterVertical);

			Device.BeginInvokeOnMainThread(() =>
			{
				try
				{
					ViewGroup.AddView(_imageView);
				}
				catch
				{
				}
			});

		}

		void SetProgress() {
			System.Diagnostics.Debug.WriteLine("SetProgress ");
			if (_CustomNavigationPage == null)
				return;

			ViewGroup = this.Parent as ViewGroup;
			if (ViewGroup == null)
				return;
			if (_CustomNavigationPage.Width <= 0 || _CustomNavigationPage.Height <= 0)
				return;
			var fieldInfo = typeof(NavigationPageRenderer).GetField("_toolbar", BindingFlags.Instance | BindingFlags.NonPublic);
			_toolbar = (Android.Support.V7.Widget.Toolbar)fieldInfo.GetValue(this);
			if (_toolbar == null)
				return;
			if (_progress == null)
			{
				_progress = new AProgressBar(Context, null, Android.Resource.Attribute.ProgressBarStyleHorizontal)
				{
					Indeterminate = true,
					Visibility= ViewStates.Invisible,

				};
				_progress.IndeterminateDrawable.SetColorFilter(ColorList.colorbatch.ToAndroid(), PorterDuff.Mode.SrcIn);
            //_progress.ProgressDrawable.SetColorFilter(Android.Graphics.Color.Red, PorterDuff.Mode.SrcIn);
				//_progress.ProgressDrawable.SetColorFilter(Android.Graphics.Color.Red,PorterDuff.Mode.SrcIn);
				//_progress.ProgressTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Red);
//					.SetColorFilter(Android.Graphics.Color.Red,PorterDuff.Mode.SrcIn);
				//AddView(_progress);
				//_progress.Visibility = ViewStates.Invisible;
			}

			System.Diagnostics.Debug.WriteLine("ScaleManager.ScreenWidth :" + ScaleManager.ScreenWidth);
			System.Diagnostics.Debug.WriteLine("_toolbar.Bottom :" + _toolbar.Bottom);
			System.Diagnostics.Debug.WriteLine("_toolbar.Height :" + _toolbar.Height);
			System.Diagnostics.Debug.WriteLine("(int)Forms.Context.ToPixels(_CustomNavigationPage.Width):"+(int)Forms.Context.ToPixels(_CustomNavigationPage.Width));


			_progress?.Layout
				//          (
				//	0,
				//	_toolbar.Height + (int)Forms.Context.ToPixels(_CustomNavigationPage.Y),
				//	(int)Forms.Context.ToPixels(_CustomNavigationPage.Width),
				//	_toolbar.Height + (int)Forms.Context.ToPixels(_CustomNavigationPage.Y + 10)
				//);
				//_progress?.SetZ(_toolbar.GetZ() + 1);
				//_progress?.Layout(l, _toolbar.Bottom - 10, r, view.Bottom + 10);
				//(0, _toolbar.Bottom - 10, (int)ScaleManager.ScreenWidth, this.Bottom + 10);
				//(0, _toolbar.Bottom,(int)Forms.Context.ToPixels(_CustomNavigationPage.Width),_toolbar.Bottom + 10);
				(0,
				 _toolbar.Height + (int)Forms.Context.ToPixels(_CustomNavigationPage.Y - 5),
				(int)Forms.Context.ToPixels(_CustomNavigationPage.Width),
				 _toolbar.Height + (int)Forms.Context.ToPixels(_CustomNavigationPage.Y + 5));
			Device.BeginInvokeOnMainThread(() =>
			{
				try
				{
					ViewGroup.AddView(_progress);
				}
				catch
				{
				}
			});

		}



		void updateProgress() {
			
		}
		void ExtendedActionBar(CustomNavigationPage page) {

			try
			{
				var bar = (Android.Support.V7.Widget.Toolbar)typeof(NavigationPageRenderer)
					.GetField("_toolbar", BindingFlags.NonPublic | BindingFlags.Instance)
				.GetValue(this);
				//bar.SetNavigationIcon(Resource.Drawable.Icon_GoLeft);

				bar.SetBackgroundColor(ColorList.colorMain.ToAndroid());

				if (page != null && _toolbar != null)
				{
					titleView = (TextView)_toolbar.FindViewById(Resource.Id.titleView);
					titleView.SetText(page.CurrentPage.Title,TextView.BufferType.Normal);
				}

				//System.Diagnostics.Debug.WriteLine("  page   title image :" + page.TitleImage);
				//if (page.TitleImage != null)
				//{
				//	titleView.Visibility = ViewStates.Invisible;
				//	titleImageView = (ImageView)_toolbar.FindViewById(Resource.Id.titleImageView);
				//	titleImageView.Visibility = ViewStates.Visible;
				//	titleImageView.SetImageResource(Resource.Drawable.googleBtn);
				//}
			}
			catch(Exception ex) {
				System.Diagnostics.Debug.WriteLine(" ex  :" + ex);
			}
		}

		void OnUpdateShadowRequested(object sender, bool shadow)
		{
			if (shadow)
			{
				// ナビゲーションバーに影をつける。
				try
				{
					_hasBackButton = NavigationPage.GetHasBackButton(_CustomNavigationPage.CurrentPage);
					if (_hasBackButton)
					{
						// 戻るボタンを消去する。
						NavigationPage.SetHasBackButton(_CustomNavigationPage.CurrentPage, false);
					}
					// ツールバーアイテムをいったん退避。
					foreach (var item in _CustomNavigationPage.CurrentPage.ToolbarItems)
					{
						_toolbarItems.Add(item);
					}
					// ツールバーアイテムを削除。
					_CustomNavigationPage.CurrentPage.ToolbarItems.Clear();

					var layerParams = new LayoutParams(MainActivity.ToolBar.Width, MainActivity.ToolBar.Height);
					_contentInsetLeft = MainActivity.ToolBar.ContentInsetLeft;
					_contentInsetRight = MainActivity.ToolBar.ContentInsetRight;
					MainActivity.ToolBar.SetContentInsetsAbsolute(0, 0);
					MainActivity.ToolBar.AddView(_NavigationBarMaskView, 0, layerParams);
				}
				catch (Exception ex)
				{
					System.Diagnostics.Debug.WriteLine(ex);
				}
			}
			else
			{
				// ナビゲーションバーから影を削除する。
				try
				{
					if (_hasBackButton)
					{
						// 戻るボタンを復帰する。
						NavigationPage.SetHasBackButton(_CustomNavigationPage.CurrentPage, true);
					}
					if (_toolbarItems.Count > 0)
					{
						// ツールバーアイテムを復帰する。
						foreach (var item in _toolbarItems)
						{
							_CustomNavigationPage.CurrentPage.ToolbarItems.Add(item);
						}
					}
					_toolbarItems.Clear();
                    if (MainActivity.ToolBar == null) return;
					MainActivity.ToolBar.SetContentInsetsAbsolute(_contentInsetLeft, _contentInsetRight);
					MainActivity.ToolBar.RemoveView(_NavigationBarMaskView);
				}
				catch (Exception ex)
				{
					System.Diagnostics.Debug.WriteLine(ex);
				}
			}
		}

		public class PaintedView : View
		{
			private Android.Graphics.Color _color;

			public PaintedView(Context context, Android.Graphics.Color color) : base(context)
			{
				_color = color;
			}

			public override void Draw(Canvas canvas)
			{
				//base.Draw(canvas);
				var width = MainActivity.ToolBar != null ? MainActivity.ToolBar.Width : Width;
				var height = MainActivity.ToolBar != null ? MainActivity.ToolBar.Height : Height;
				using (var paint = new Paint())
				{
					paint.Color = _color;
					canvas.DrawRect(new Android.Graphics.Rect(0, 0, width, height), paint);
				}
			}
		}
	}
}
