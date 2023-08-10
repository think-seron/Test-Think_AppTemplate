using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Think_App
{
	public interface IModalPageService
	{
		/// <summary>
		/// Gets the size of the screen.
		/// </summary>
		/// <returns>The screen size.</returns>
		//Size ModalGetScreenSize();

		///// <summary>
		///// Shows the card async.
		///// </summary>
		///// <returns>The async.</returns>
		//Task ModalShowAsync(ModalPage page);

		///// <summary>
		///// Closes the card async
		///// </summary>
		///// <returns>The aync.</returns>
		//Task ModalCloseAsync(ModalPage page);


		bool ModalControlAnimatesItself { get; }
	}
}
