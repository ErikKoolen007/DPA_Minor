using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DPA_Musicsheets.Hotkeys
{
    public abstract class BaseHotkeyHandler : IHotkeyHandlerChain
    {
        public IHotkeyHandlerChain Next { get; set; }

        public string Handle(string hotkey, Dictionary<Key, bool> keysDown)
        {
            string hotkeyString = TryHandle(hotkey, keysDown);

            return Next != null ? Next.Handle(hotkeyString, keysDown) : hotkeyString;
        }

        protected abstract string TryHandle(string hotkey, Dictionary<Key, bool> keysDown);

        public void SetKeysDownFalse(Dictionary<Key, bool> keysDown)
        {
            for (int i = 0; i < keysDown.Count; i++)
            {
                var key = keysDown.ElementAt(i);
                keysDown[key.Key] = false;
            }
        }
    }
}
