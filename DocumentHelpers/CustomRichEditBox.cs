using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Input;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.System;
using Windows.UI.Xaml.Input;

namespace mIDE.DocumentHelpers
{
    class CustomRichEditBox : RichEditBox
    {
        public List<VirtualKey> keysToRaiseEvent = new List<VirtualKey>() { VirtualKey.Down, VirtualKey.Up };

        protected override void OnKeyDown(KeyRoutedEventArgs e)
        {
            if (keysToRaiseEvent.Contains(e.Key))
            {
                //Intellisense error : the even UIElement.KeyDown can only on the left side of += or -= ...
                //KeyDown(this, e);
                CustomKeyDown(this, e);
            }
            if (!e.Handled)
            {
                base.OnKeyDown(e);
            }
        }

        public delegate void CustomKeyDownHandler(object raised, KeyRoutedEventArgs e);
        public event CustomKeyDownHandler CustomKeyDown;
    }
}
