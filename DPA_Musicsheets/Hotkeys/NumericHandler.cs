using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DPA_Musicsheets.Hotkeys
{
    public class NumericHandler : BaseHotkeyHandler
    {
        protected override string TryHandle(string hotkey, Dictionary<Key, bool> keysDown)
        {
            for (int i = 0; i < keysDown.Count; i++)
            {
                var key = keysDown.ElementAt(i);
                if (key.Value == false && key.Key.ToString()[0] == 'D' && char.IsDigit(key.Key.ToString().ElementAtOrDefault(1)))
                {
                    hotkey += key.Key.ToString()[1];
                    keysDown[key.Key] = true;
                }
            }

            return hotkey;
        }
    }
}
