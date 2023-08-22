using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
    public class DialogManager
    {
        static readonly Lazy<DialogManager> _instance = new Lazy<DialogManager>(() => new DialogManager());

        public static DialogManager Instance => _instance.Value;

        public bool BackButtonEnable { get; private set; }


        public async Task ShowDialogView(View view)
        {
            await DependencyService.Get<IDialogService>().DisplayFormsViewAsync(view);
            BackButtonEnable = false;
        }

        public async Task HideView()
        {
            BackButtonEnable = true;
            await DependencyService.Get<IDialogService>().HideFormsViewAsync();
        }

    }
}
