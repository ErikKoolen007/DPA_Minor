using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DPA_Musicsheets.Hotkeys
{
    public interface IHotkeyHandlerChain
    {
        IHotkeyHandlerChain Next { get; set; }

        string Handle(string hotkey, Dictionary<Key, bool> keysDown);
    }
}
