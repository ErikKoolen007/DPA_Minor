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
                if (key.Value == false && IsOneCharLetter(key.Key.ToString()))
                {
                    hotkey += key.Key.ToString();
                    keysDown[key.Key] = true;
                }
            }

            return hotkey;
        }

        private bool IsOneCharLetter(string letters)
        {
            return letters.Length == 1 && char.IsLetter(letters[0]);
        }
    }
}
