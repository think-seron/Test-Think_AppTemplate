using System;
using System.ComponentModel;
using System.Threading.Tasks;
using UIKit;
using Think_App;
using Think_App.iOS;
using Foundation;
using CoreGraphics;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

// TODO Xamarin.Forms.ExportRendererAttribute is not longer supported. For more details see https://github.com/dotnet/maui/wiki/Using-Custom-Renderers-in-.NET-MAUI
[assembly: ExportRenderer(typeof(CustomButton), typeof(CustomButtonRenderer))]
namespace Think_App.iOS
{
	public class CustomButtonRenderer : ButtonRenderer
	{
		CustomButton _CustomButton;
		nfloat _initImageX, _initImageY;
		bool _imageXInitted, _imageYInitted;
		CGSize _imageSize;

		protected override async void OnElementChanged(ElementChangedEventArgs<Button> e)
		{
			base.OnElementChanged(e);

			if (Control != null && e.NewElement != null)
			{
				_CustomButton = e.NewElement as CustomButton;

				await Update();

				Control.TouchDown += UpdateBackgroundColor;
				Control.TouchCancel += UpdateBackgroundColor;
				Control.TouchUpInside += ChangeDefaultColor;
				Control.TouchDragOutside += UpdateBackgroundColor;
				Control.TouchDragInside += UpdateBackgroundColor;
				Control.TouchDragExit += UpdateBackgroundColor;
				Control.TouchDragEnter += UpdateBackgroundColor;
				Control.TouchUpOutside += ChangeDefaultColor;
			}
			else if (Control != null && e.NewElement == null)
			{
				Control.TouchDown -= UpdateBackgroundColor;
				Control.TouchCancel -= UpdateBackgroundColor;
				Control.TouchUpInside -= ChangeDefaultColor;
				Control.TouchDragOutside -= UpdateBackgroundColor;
				Control.TouchDragInside -= UpdateBackgroundColor;
				Control.TouchDragExit -= UpdateBackgroundColor;
				Control.TouchDragEnter -= UpdateBackgroundColor;
				Control.TouchUpOutside -= ChangeDefaultColor;
			}
		}

		void UpdateBackgroundColor(object sender, EventArgs e)
		{
			if (_CustomButton.UseCustomColor && Control != null && Control.Enabled)
			{
				Control.BackgroundColor = (Control.Highlighted) ? _CustomButton.HighlightColor.ToUIColor() : _CustomButton.BackgroundColor.ToUIColor();
			}
		}

		void ChangeDefaultColor(object sender, EventArgs e)
		{
			if (Control != null && Control.Enabled)
			{
				Control.BackgroundColor = _CustomButton.BackgroundColor.ToUIColor();
			}
		}

		protected override async void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (Control == null)
			{
				return;
			}

			if (e.PropertyName == CustomButton.DisableColorProperty.PropertyName ||
				e.PropertyName == CustomButton.UseCustomColorProperty.PropertyName)
			{
				UpdateDisableColor();
			}
			else if (e.PropertyName == CustomButton.TextColorProperty.PropertyName)
			{
				UpdateTitleColor();
			}
			else if (e.PropertyName == CustomButton.IsEnabledProperty.PropertyName)
			{
				UpdateDisableColor();
			}
			else if (e.PropertyName == CustomButton.SourceProperty.PropertyName ||
					 e.PropertyName == CustomButton.ImageWidthProperty.PropertyName ||
					 e.PropertyName == CustomButton.ImageHeightProperty.PropertyName)

			{
				await UpdateImage();
			}
			else if (e.PropertyName == CustomButton.ImageOffsetProperty.PropertyName ||
					 e.PropertyName == CustomButton.ImagePaddingProperty.PropertyName)
			{
				SetNeedsDisplay();
			}
			else if (e.PropertyName == CustomButton.TextProperty.PropertyName)
			{
				_imageXInitted = false;
				SetNeedsDisplay();
			}
			else if (e.PropertyName == CustomButton.ImageLayoutPositionProperty.PropertyName)
			{
				UpdateTextAlignment();
				SetNeedsDisplay();
			}
			else if (e.PropertyName == CustomButton.WidthProperty.PropertyName ||
			         e.PropertyName == CustomButton.HeightProperty.PropertyName)
			{
				if (_CustomButton.UpdateInChangingSize)
				{
					_imageXInitted = _imageYInitted = false;
					SetNeedsDisplay();
				}
			}
		}

		public override void Draw(CGRect rect)
		{
			// 描画前にInset更新。
			UpdateInset();
			base.Draw(rect);
		}

		async Task Update()
		{
			// ContentEdgeInsetsは初回に完全にクリアして見通しをよくする。
			Control.ContentEdgeInsets = new UIEdgeInsets(0, 0, 0, 0);
			UpdateDisableColor();
			UpdateTitleColor();
			UpdateTextAlignment();
			await UpdateImage();
		}

		void UpdateDisableColor()
		{
			var disableColor = (_CustomButton.UseCustomColor) ? _CustomButton.DisableColor.ToUIColor() : _CustomButton.BackgroundColor.ToUIColor();
			Control.BackgroundColor = (Control.Enabled) ?
				_CustomButton.BackgroundColor.ToUIColor() : disableColor;
		}

		void UpdateTitleColor()
		{
			Control.SetTitleColor(_CustomButton.TextColor.ToUIColor(), UIControlState.Disabled);
		}

		async Task UpdateImage()
		{
			if (_CustomButton.Source != null)
			{
				using (var uiImage = await _CustomButton.Source.ToUIImageAsync())
				{
					if (uiImage != null)
					{
						var resizedImage = uiImage.Resize((nfloat)_CustomButton.ImageWidth, (nfloat)_CustomButton.ImageHeight);
						if (resizedImage != null)
						{
							Control.SetImage(resizedImage.ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal), UIControlState.Normal);
							_imageXInitted = _imageYInitted = false;

							// Inset更新が必要
							UpdateInset();
						}
					}
				}
			}
		}

		void UpdateTextAlignment()
		{
			if (_CustomButton.ImageLayoutPosition == CustomButton.LayoutPosition.Top ||
				_CustomButton.ImageLayoutPosition == CustomButton.LayoutPosition.Bottom)
			{
				// テキストを中央寄せ。
				Control.TitleLabel.TextAlignment = UITextAlignment.Center;
			}
			else
			{
				// テキストを左寄せ。
				Control.TitleLabel.TextAlignment = UITextAlignment.Left;
			}
		}

		void UpdateInset()
		{
			if (_CustomButton.Source != null)
			{
				// イメージサイズが前回取れなかった場合、イメージのX,Y位置は再初期化する必要がある。
				if (!(_imageSize.Width > 0 && _imageSize.Height > 0))
				{
					_imageXInitted = false;
					_imageYInitted = false;
				}

				var buttonWidth = Control.Bounds.Width;
				var buttonHeight = Control.Bounds.Height;
				_imageSize = Control.ImageView.Bounds.Size;
				var titleSize = Control.TitleLabel.IntrinsicContentSize;
				var offsetX = (nfloat)_CustomButton.ImageOffset.X;
				var offsetY = (nfloat)_CustomButton.ImageOffset.Y;
				var padding = (nfloat)_CustomButton.ImagePadding;

				if (!_imageXInitted)
				{
					ResetInsets(0, 0, 0);
					_initImageX = Control.ImageView.Frame.X;
					_imageXInitted = true;
				}
				if (!_imageYInitted)
				{
					ResetInsets(0, 0, 0);
					_initImageY = Control.ImageView.Frame.Y;
					_imageYInitted = true;
				}

				if (string.IsNullOrEmpty(_CustomButton.Text))
				{
					Control.ImageEdgeInsets = new UIEdgeInsets(offsetY, offsetX, -offsetY, -offsetX);
					Control.TitleEdgeInsets = new UIEdgeInsets(0, 0, 0, 0);

					// テキストがないので以降の処理は意味がない。
					return;
				}

				if (_CustomButton.ImageLayoutPosition == CustomButton.LayoutPosition.Left)
				{
					ResetInsets(offsetX, offsetY, padding);
				}
				else if (_CustomButton.ImageLayoutPosition == CustomButton.LayoutPosition.Right)
				{
					Control.ImageEdgeInsets = new UIEdgeInsets(offsetY, offsetX + padding + titleSize.Width, -offsetY, -(offsetX + padding + titleSize.Width));
					Control.TitleEdgeInsets = new UIEdgeInsets(0, -(padding + _imageSize.Width), 0, padding + _imageSize.Width);
				}
				else if (_CustomButton.ImageLayoutPosition == CustomButton.LayoutPosition.Top)
				{
					var imageX = (buttonWidth - _imageSize.Width) / 2.0f;
					var imageY = (buttonHeight - (_imageSize.Height + padding + titleSize.Height)) / 2.0f;
					var titleY = buttonHeight - imageY - titleSize.Height;
					var currentTitleY = (buttonHeight - titleSize.Height) / 2.0f;
					Control.ImageEdgeInsets = new UIEdgeInsets(-(_initImageY - imageY - offsetY), imageX - _initImageX + offsetX, _initImageY - imageY - offsetY, -(imageX - _initImageX + offsetX));
					Control.TitleEdgeInsets = new UIEdgeInsets(titleY - currentTitleY, -_imageSize.Width, -(titleY - currentTitleY), 0);
				}
				else if (_CustomButton.ImageLayoutPosition == CustomButton.LayoutPosition.Bottom)
				{
					var imageX = (buttonWidth - _imageSize.Width) / 2.0f;
					var imageY = buttonHeight - (buttonHeight - (titleSize.Height + padding + _imageSize.Height)) / 2.0f - _imageSize.Height;
					var titleY = (buttonHeight - (titleSize.Height + padding + _imageSize.Height)) / 2.0f;
					var currentTitleY = (buttonHeight - titleSize.Height) / 2.0f;
					Control.ImageEdgeInsets = new UIEdgeInsets(_initImageY - imageY + offsetY + titleSize.Height + padding, imageX - _initImageX + offsetX, -(_initImageY - imageY + offsetY + titleSize.Height + padding), -(imageX - _initImageX + offsetX));
					Control.TitleEdgeInsets = new UIEdgeInsets(titleY - currentTitleY, -_imageSize.Width, -(titleY - currentTitleY), 0);
				}
			}
		}

		void ResetInsets(nfloat offsetX, nfloat offsetY, nfloat padding)
		{
			Control.ImageEdgeInsets = new UIEdgeInsets(offsetY, offsetX - padding / 2.0f, -offsetY, -offsetX + padding / 2.0f);
			Control.TitleEdgeInsets = new UIEdgeInsets(0, padding / 2.0f, 0, -padding / 2.0f);
		}
	}
}
