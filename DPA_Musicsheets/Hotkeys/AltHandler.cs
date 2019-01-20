using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DPA_Musicsheets.Hotkeys
{
    public class AltHandler : BaseHotkeyHandler
    {
        protected override string TryHandle(string hotkey, Dictionary<Key, bool> keysDown)
        {

            for (int i = 0; i < keysDown.Count; i++)
            {
                var key = keysDown.ElementAt(i);

                //My laptop(Erik) is strange, alt key is seen as system key. Therefore accept alternate modifier key
                if (key.Key.ToString() == "LeftAlt" || key.Key.ToString() == "RightCtrl")
                {
                    if (!string.IsNullOrEmpty(hotkey) && i == 0)
                        hotkey = "";

                    hotkey += "Alt";
                    keysDown[key.Key] = true;
                    break;
                }
            }

            return hotkey;
        }
    }
}
