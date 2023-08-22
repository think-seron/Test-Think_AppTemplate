using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
namespace Think_App
{
	public class SelectImageSourceViewModel : ViewModelBase
	{
		Command _PhotoButtonClickedCommand;
		public Command PhotoButtonClickedCommand
		{
			get
			{
				return _PhotoButtonClickedCommand;
			}
			set
			{
				SetProperty(ref _PhotoButtonClickedCommand, value);
			}
		}

		Command _CameraButtonClickedCommand;
		public Command CameraButtonClickedCommand
		{
			get
			{
				return _CameraButtonClickedCommand;
			}
			set
			{
				SetProperty(ref _CameraButtonClickedCommand, value);
			}
		}

		Command _CatalogButtonClickedCommand;
		public Command CatalogButtonClickedCommand
		{
			get
			{
				return _CatalogButtonClickedCommand;
			}
			set
			{
				SetProperty(ref _CatalogButtonClickedCommand, value);
			}
		}

		Command _SimulationButtonClickedCommand;
		public Command SimulationButtonClickedCommand
		{
			get
			{
				return _SimulationButtonClickedCommand;
			}
			set
			{
				SetProperty(ref _SimulationButtonClickedCommand, value);
			}
		}

        private bool _SimulationBtnVisible;
        public bool SimulationBtnVisible
        {
            get => _SimulationBtnVisible;
            set
            {
                if (_SimulationBtnVisible != value)
                {
                    _SimulationBtnVisible = value;
                    OnPropertyChanged(nameof(SimulationBtnVisible));
                }
            }
        }
    }
}
