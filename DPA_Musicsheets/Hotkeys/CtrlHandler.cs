using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DPA_Musicsheets.Hotkeys
{
    public class CtrlHandler : BaseHotkeyHandler
    {
        protected override string TryHandle(string hotkey, Dictionary<Key, bool> keysDown)
        {
            for (int i = 0; i < keysDown.Count; i++)
            {
                var key = keysDown.ElementAt(i);

                if (key.Key.ToString() == "LeftCtrl")
                {
                    if (!string.IsNullOrEmpty(hotkey) && i == 0)
                        hotkey = "";
                       
                    hotkey += "Ctrl";
                    keysDown[key.Key] = true;
                    break;
                }
            }

            return hotkey;
        }
    }
}
