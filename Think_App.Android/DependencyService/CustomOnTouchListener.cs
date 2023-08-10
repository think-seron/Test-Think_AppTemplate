using System;
using Android.Views;
using static Android.Views.View;

namespace Think_App.Droid
{
    /// <summary>
    /// タッチ操作を検出し、引数に応じて有効/無効を制御する。
    /// </summary>
    public class CustomOnTouchListener : Java.Lang.Object, IOnTouchListener
    {
        bool _ignoreTouch;
        public CustomOnTouchListener(bool ignoreTouch)
        {
            _ignoreTouch = ignoreTouch;
        }

        public bool OnTouch(View v, MotionEvent e)
        {
            return _ignoreTouch;
        }
    }
}
