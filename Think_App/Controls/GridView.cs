using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
	public class GridView : Grid
	{
		#region CommandParameter BindableProperty
		public static readonly BindableProperty CommandParameterProperty =
			BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(GridView), null,
				propertyChanged: (bindable, oldValue, newValue) =>
					((GridView)bindable).CommandParameter = (object)newValue);

		public object CommandParameter
		{
			get { return GetValue(CommandParameterProperty); }
			set { SetValue(CommandParameterProperty, value); }
		}
		#endregion

		#region Command BindableProperty
		public static readonly BindableProperty CommandProperty =
			BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(GridView), null,
				propertyChanged: (bindable, oldValue, newValue) =>
					((GridView)bindable).Command = (ICommand)newValue);

		public ICommand Command
		{
			get { return (ICommand)GetValue(CommandProperty); }
			set { SetValue(CommandProperty, value); }
		}
		#endregion

		#region ItemTemplate BindableProperty
		public static readonly BindableProperty ItemTemplateProperty =
			BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(GridView), default(DataTemplate),
				propertyChanged: (bindable, oldValue, newValue) =>
					((GridView)bindable).ItemTemplate = (DataTemplate)newValue);

		public DataTemplate ItemTemplate
		{
			get { return (DataTemplate)GetValue(ItemTemplateProperty); }
			set { SetValue(ItemTemplateProperty, value); }
		}
		#endregion

		#region ItemsSource BindableProperty
		public static readonly BindableProperty ItemsSourceProperty =
			BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(GridView), null,
				propertyChanged: (bindable, oldValue, newValue) =>
					((GridView)bindable).ItemsSource = (IEnumerable)newValue);

		public IEnumerable ItemsSource
		{
			get { return (IEnumerable)GetValue(ItemsSourceProperty); }
			set { SetValue(ItemsSourceProperty, value); }
		}
		#endregion

		#region TileHeight BindableProperty
		public static readonly BindableProperty TileHeightProperty =
			BindableProperty.Create(nameof(TileHeight), typeof(double), typeof(GridView), (double)100.0,
				propertyChanged: (bindable, oldValue, newValue) =>
					((GridView)bindable).TileHeight = (double)newValue);

		public double TileHeight
		{
			get { return (double)GetValue(TileHeightProperty); }
			set { SetValue(TileHeightProperty, value); }
		}
		#endregion

		#region MaxColumns BindableProperty
		public static readonly BindableProperty MaxColumnsProperty =
			BindableProperty.Create(nameof(MaxColumns), typeof(int), typeof(GridView), 2,
				propertyChanged: (bindable, oldValue, newValue) =>
					((GridView)bindable).MaxColumns = (int)newValue);

		public int MaxColumns
		{
			get { return (int)GetValue(MaxColumnsProperty); }
			set { SetValue(MaxColumnsProperty, value); }
		}
		#endregion

		public GridView()
		{
			
		}

		public async Task BuildTiles(IEnumerable tiles)
		{
			if (ColumnDefinitions.Any())
			{
				ColumnDefinitions.Clear();
			}

			for (var i = 0; i < MaxColumns; i++)
			{
				// カラム幅はすべて同じサイズとします。
				ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1.0, GridUnitType.Star) });
			}

			// Wipe out the previous row definitions if they're there.
			if (RowDefinitions.Any())
			{
				RowDefinitions.Clear();
			}

			var enumerable = tiles as IList ?? tiles.Cast<object>().ToArray();
			var numberOfRows = Math.Ceiling(enumerable.Count / (float)MaxColumns);
			for (var i = 0; i < numberOfRows; i++)
			{
				RowDefinitions.Add(new RowDefinition { Height = TileHeight });
			}

			for (var index = 0; index < enumerable.Count; index++)
			{
				var column = index % MaxColumns;
				var row = (int)Math.Floor(index / (float)MaxColumns);

				var tile = await BuildTile(enumerable[index]);

				if (tile != null)
				{
					Children.Add(tile, column, row);
				}
			}
		}

		public async Task BuildTiles()
		{
			await BuildTiles(ItemsSource);
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

