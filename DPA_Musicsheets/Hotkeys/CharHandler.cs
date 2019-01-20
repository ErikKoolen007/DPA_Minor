using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DPA_Musicsheets.Hotkeys
{
    public class CharHandler : BaseHotkeyHandler
    {
        protected override string TryHandle(string hotkey, Dictionary<Key, bool> keysDown)
        {
            for (int i = 0; i < keysDown.Count; i++)
            {
                var key = keysDown.ElementAt(i);
                if (key.Value == false && StringAllLetters(key.Key.ToString()))
                {
                    hotkey += key.Key.ToString();
                    keysDown[key.Key] = true;
                }
            }

            return hotkey;
        }

        private bool StringAllLetters(string letters)
        {
            foreach (char c in letters)
            {
                if (!char.IsLetter(c))
                    return false;
            }

            return true;
        }
    }
}
