using System;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
namespace Think_App
{
	public class MessageMainPageModel : ViewModelBase
	{
		public MessageMainPageModel()
		{
			//ToolbarIcon = "Icon_Home.png";
		}
		bool _PageTappedEnable;
		public bool PageTappedEnable
		{
			get
			{
				return _PageTappedEnable;
			}
			set
			{
				SetProperty(ref _PageTappedEnable, value);
			}
		}

		Command _PageTappedCommand;
		public Command PageTappedCommand
		{
			get
			{
				return _PageTappedCommand;
			}
			set
			{
				SetProperty(ref _PageTappedCommand, value);
			}
		}

		ObservableCollection<MessageListCellModel> _MessageListViewItemsSource;
		public ObservableCollection<MessageListCellModel> MessageListViewItemsSource
		{
			get
			{
				return _MessageListViewItemsSource;
			}
			set
			{
				SetProperty(ref _MessageListViewItemsSource, value);
			}
		}

		double _SelectImageSourceViewHeight;
		public double SelectImageSourceViewHeight
		{
			get
			{
				return _SelectImageSourceViewHeight;
			}
			set
			{
				SetProperty(ref _SelectImageSourceViewHeight, value);
			}
		}

		SelectImageSourceViewModel _SelectImageSourceViewModel;
		public SelectImageSourceViewModel SelectImageSourceViewModel
		{
			get
			{
				return _SelectImageSourceViewModel;
			}
			set
			{
				SetProperty(ref _SelectImageSourceViewModel, value);
			}
		}
        private string _ToolberIcon;
        public string ToolberIcon
        {
            get { return _ToolberIcon; }
            set
            {
                if (_ToolberIcon != value)
                {
                    _ToolberIcon = value;
                    OnPropertyChanged("ToolberIcon");
                }
            }
        }
		//private string _ToolbarIcon;
        //public string ToolbarIcon
		//{
		//	get { return _ToolbarIcon; }
		//	set
		//	{
		//		if (_ToolbarIcon != value)
		//		{
		//			_ToolbarIcon = value;
		//			System.Diagnostics.Debug.WriteLine(" toolbar icon : " + value);
		//			OnPropertyChanged("ToolbarIcon");
		//		}
		//	}
		//}
		private Command _ToolbarItemsClick;
		public Command ToolbarItemsClick
		{
			get { return _ToolbarItemsClick; }
			set
			{
				if (_ToolbarItemsClick != value)
				{
					_ToolbarItemsClick = value;
					OnPropertyChanged("ToolbarItemsClick");
				}
			}
		}

	}
}
