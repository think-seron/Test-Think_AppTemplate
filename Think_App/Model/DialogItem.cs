using System;
namespace Think_App
{
    /// <summary>
    /// IDialogServiceのダイアログで使用する項目
    /// </summary>
    public class DialogItem
    {
        public string Text { get; set; }
        public ItemStyle Style { get; set; }
        public DialogItem()
        {
        }

        public DialogItem(string text, ItemStyle itemStyle = ItemStyle.Default)
        {
            Text = text;
            Style = itemStyle;
        }

        public enum ItemStyle
        {
            Default,
            Cancel,
            Destructive
        }
    }
}
