using System;
namespace Think_App
{
	public class MessageEditor : CustomEditor
	{
		public event EventHandler<Lines> TextLinesChanged = delegate {};

		public enum Lines
		{
			Single,
			Multi
		}

		public Lines TextLines { get; private set; }

		public MessageEditor()
		{
			// 初期設定
			TextLines = (this.Text != null && this.Text.Contains(Environment.NewLine)) ? Lines.Multi : Lines.Single;
		}

		public void Reset()
		{
			TextLines = (this.Text != null && this.Text.Contains(Environment.NewLine)) ? Lines.Multi : Lines.Single;
		}

		public void UpdateLines(Lines lines)
		{
			if (lines != this.TextLines)
			{
				var str = (lines == Lines.Multi) ? "複数行" : "１行";
				System.Diagnostics.Debug.WriteLine("Editorが {0} に変わりました", str);

				// プロパティ更新
				this.TextLines = lines;
				// イベント発行
				if (TextLinesChanged != null)
				{
                    TextLinesChanged(this, lines);
				}
			}
		}
	}
}
