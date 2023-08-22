using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Input;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
	public class GalleryView : ScrollView
	{
		#region CommandParameter BindableProperty
		public static readonly BindableProperty CommandParameterProperty =
			BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(GalleryView), null,
				propertyChanged: (bindable, oldValue, newValue) =>
					((GalleryView)bindable).CommandParameter = (object)newValue);

		public object CommandParameter
		{
			get { return GetValue(CommandParameterProperty); }
			set { SetValue(CommandParameterProperty, value); }
		}
		#endregion

		#region Command BindableProperty
		public static readonly BindableProperty CommandProperty =
			BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(GalleryView), null,
				propertyChanged: (bindable, oldValue, newValue) =>
					((GalleryView)bindable).Command = (ICommand)newValue);

		public ICommand Command
		{
			get { return (ICommand)GetValue(CommandProperty); }
			set { SetValue(CommandProperty, value); }
		}
		#endregion

		#region ItemTemplate BindableProperty
		public static readonly BindableProperty ItemTemplateProperty =
			BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(GalleryView), default(DataTemplate),
				propertyChanged: (bindable, oldValue, newValue) =>
					((GalleryView)bindable).ItemTemplate = (DataTemplate)newValue);

		public DataTemplate ItemTemplate
		{
			get { return (DataTemplate)GetValue(ItemTemplateProperty); }
			set { SetValue(ItemTemplateProperty, value); }
		}
		#endregion

		#region ItemsSource BindableProperty
		public static readonly BindableProperty ItemsSourceProperty =
			BindableProperty.Create(nameof(ItemsSource), typeof(IList), typeof(GalleryView), null,
				propertyChanged: (bindable, oldValue, newValue) =>
					((GalleryView)bindable).ItemsSource = (IList)newValue);

		public IList ItemsSource
		{
			get { return (IList)GetValue(ItemsSourceProperty); }
			set { SetValue(ItemsSourceProperty, value); }
		}
		#endregion

		#region TileHeight BindableProperty
		public static readonly BindableProperty TileHeightProperty =
			BindableProperty.Create(nameof(TileHeight), typeof(double), typeof(GalleryView), (double)100.0,
				propertyChanged: (bindable, oldValue, newValue) =>
					((GalleryView)bindable).TileHeight = (double)newValue);

		public double TileHeight
		{
			get { return (double)GetValue(TileHeightProperty); }
			set { SetValue(TileHeightProperty, value); }
		}
		#endregion

		#region MaxColumns BindableProperty
		public static readonly BindableProperty MaxColumnsProperty =
			BindableProperty.Create(nameof(MaxColumns), typeof(int), typeof(GalleryView), 2,
				propertyChanged: (bindable, oldValue, newValue) =>
					((GalleryView)bindable).MaxColumns = (int)newValue);

		public int MaxColumns
		{
			get { return (int)GetValue(MaxColumnsProperty); }
			set { SetValue(MaxColumnsProperty, value); }
		}
		#endregion

		#region ColumnSpacing BindableProperty
		public static readonly BindableProperty ColumnSpacingProperty =
			BindableProperty.Create(nameof(ColumnSpacing), typeof(double), typeof(GalleryView), (double)10.0,
				propertyChanged: (bindable, oldValue, newValue) =>
					((GalleryView)bindable).ColumnSpacing = (double)newValue);

		public double ColumnSpacing
		{
			get { return (double)GetValue(ColumnSpacingProperty); }
			set { SetValue(ColumnSpacingProperty, value); }
		}
		#endregion

		#region RowSpacing BindableProperty
		public static readonly BindableProperty RowSpacingProperty =
			BindableProperty.Create(nameof(RowSpacing), typeof(double), typeof(GalleryView), (double)10.0,
				propertyChanged: (bindable, oldValue, newValue) =>
					((GalleryView)bindable).RowSpacing = (double)newValue);

		public double RowSpacing
		{
			get { return (double)GetValue(RowSpacingProperty); }
			set { SetValue(RowSpacingProperty, value); }
		}
		#endregion

		#region InnerPadding BindableProperty
		public static readonly BindableProperty InnerPaddingProperty =
			BindableProperty.Create(nameof(InnerPadding), typeof(Thickness), typeof(GalleryView), default(Thickness),
				propertyChanged: (bindable, oldValue, newValue) =>
					((GalleryView)bindable).InnerPadding = (Thickness)newValue);

		public Thickness InnerPadding
		{
			get { return (Thickness)GetValue(InnerPaddingProperty); }
			set { SetValue(InnerPaddingProperty, value); }
		}
		#endregion


		readonly Grid _Grid;
		private IList _Tiles;

		public GalleryView()
		{
			// 内部メインコンテンツとしてGridを用意する。
			_Grid = new Grid();
			this.Content = _Grid;
		}

		protected override void OnPropertyChanged(string propertyName = null)
		{
			base.OnPropertyChanged(propertyName);

			if (propertyName == ColumnSpacingProperty.PropertyName)
			{
				_Grid.ColumnSpacing = ColumnSpacing;
			}
			else if (propertyName == RowSpacingProperty.PropertyName)
			{
				_Grid.RowSpacing = RowSpacing;
			}
			else if (propertyName == InnerPaddingProperty.PropertyName)
			{
				_Grid.Padding = InnerPadding;
			}
		}

		public void Setup()
		{
			Setup(ItemsSource);
		}

		public void Setup(IList tiles)
		{
			if (_Grid == null)
			{
				return;
			}

			if (_Grid.ColumnDefinitions.Any())
			{
				_Grid.ColumnDefinitions.Clear();
			}
			if (_Grid.RowDefinitions.Any())
			{
				_Grid.RowDefinitions.Clear();
			}

			// Gridの高さを決定する。
			_Tiles = tiles;
			var numberOfRows = (int)Math.Ceiling(_Tiles.Count / (float)MaxColumns);
			var gridHeight = TileHeight * numberOfRows + RowSpacing * (numberOfRows - 1);
			_Grid.HeightRequest = gridHeight;

			for (var i = 0; i < MaxColumns; i++)
			{
				// カラム幅はすべて同じサイズとします。
				_Grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1.0, GridUnitType.Star) });
			}

			for (var i = 0; i < numberOfRows; i++)
			{
				_Grid.RowDefinitions.Add(new RowDefinition { Height = TileHeight });
			}
		}

		public async Task LoadTiles(int index, int lendth)
		{
			for (int i = index; i < index + lendth; ++i)
			{
				if (i < _Tiles.Count)
				{
					var tile = await BuildTile(_Tiles[i]);
					var column = i % MaxColumns;
					var row = (int)Math.Floor(i / (float)MaxColumns);

					if (tile != null)
					{
						_Grid.Children.Add(tile, column, row);
					}
				}
				else
				{
					break;
				}
			}
		}

		public void RemoveTiles(int index, int length)
		{
			for (int i = 0; i < length; ++i)
			{
				try
				{
					// Removeすると、要素が詰められるので、常に同じindexを指定する。
					_Grid.Children.RemoveAt(index);
				}
				catch (Exception ex)
				{
					System.Diagnostics.Debug.WriteLine(ex);
				}
			}
		}

		public void RemoveAllTiles()
		{
			_Grid.Children.Clear();
		}

		private async Task<View> BuildTile(object item)
		{
			return await Task.Run(() =>
			{
				var content = ItemTemplate?.CreateContent();

				if (!(content is View) && !(content is ViewCell)) return null;
				var buildTile = (content is View) ? content as View : ((ViewCell)content).View;
				buildTile.BindingContext = item;
				var tapGestureRecognizer = new TapGestureRecognizer
				{
					Command = Command,
					CommandParameter = item
				};

				buildTile.GestureRecognizers.Add(tapGestureRecognizer);
				return buildTile;
			});
		}
	}
}

