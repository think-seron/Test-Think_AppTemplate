using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Think_App
{
    /// <summary>
    /// DependencyServiceで使用されるダイアログ呼び出し
    /// 各プラットフォーム固有の処理を行う
    /// </summary>
    public interface IDialogService
    {
        Task DisplayFormsViewAsync(View formsView);
        Task HideFormsViewAsync();
        Task DisplayAlert(string title, string message, DialogItem cancelItem);
        Task<bool> DisplayAlert(string title, string message, DialogItem acceptItem, DialogItem cancelItem);
        Task<string> DisplayActionSheet(string title, string message, string cancel, string destruction, DialogItem[] items);
    }
}
