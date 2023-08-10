using System;
using Xamarin.Forms;
namespace Think_App
{
	public class SelectSalonMessageListCellModel : ViewModelBase
	{
		Color _SSMLCBackgroundColor;
		public Color SSMLCBackgroundColor
		{
			get
			{
				return _SSMLCBackgroundColor;
			}
			set
			{
				SetProperty(ref _SSMLCBackgroundColor, value);
			}
		}

		string _SSMLCSalonName;
		public string SSMLCSalonName
		{
			get
			{
				return _SSMLCSalonName;
			}
			set
			{
				SetProperty(ref _SSMLCSalonName, value);
			}
		}

		string _SSMLCNewMessage;
		public string SSMLCNewMessage
		{
			get
			{
				return _SSMLCNewMessage;
			}
			set
			{
				SetProperty(ref _SSMLCNewMessage, value);
			}
		}

		bool _SSMLCBatchVisible;
		public bool SSMLCBatchVisible
		{
			get
			{
				return _SSMLCBatchVisible;
			}
			set
			{
				SetProperty(ref _SSMLCBatchVisible, value);
			}
		}

		string _SSMLCMessageDate;
		public string SSMLCMessageDate
		{
			get
			{
				return _SSMLCMessageDate;
			}
			set
			{
				SetProperty(ref _SSMLCMessageDate, value);
			}
		}

		Color _SSMLCSeparatorColor;
		public Color SSMLCSeparatorColor
		{
			get
			{
				return _SSMLCSeparatorColor;
			}
			set
			{
				SetProperty(ref _SSMLCSeparatorColor, value);
			}
		}

		public int SalonId { get; set; }
	}
}
