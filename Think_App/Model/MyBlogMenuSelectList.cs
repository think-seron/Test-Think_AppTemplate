using System;
namespace Think_App
{
    public class MyBlogMenuSelectList : ViewModelBase
    {
        public MyBlogMenuSelectList(string label)
        {
            MyBlogMenuSelectListText = label;
        }
        private string _myBlogMenuSelectListText;
        public string MyBlogMenuSelectListText
        {
            get => _myBlogMenuSelectListText;
            set
            {
                if (_myBlogMenuSelectListText == value) return;
                _myBlogMenuSelectListText = value;
                OnPropertyChanged(nameof(MyBlogMenuSelectListText));
            }
        }
    }
}
