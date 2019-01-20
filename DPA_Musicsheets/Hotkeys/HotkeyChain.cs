using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DPA_Musicsheets.Commands;
using DPA_Musicsheets.Managers;
using DPA_Musicsheets.ViewModels;
using Console = System.Console;

namespace DPA_Musicsheets.Hotkeys
{
    public class HotkeyChain
    {
        private IHotkeyHandlerChain chain;
        private string _hotkeyString = "";
        private Dictionary<string, IKeyCommand> _hotkeyList;
        private ViewModelLocator _viewModelLocator;

        public HotkeyChain()
        {
            _viewModelLocator = new ViewModelLocator();

            _hotkeyList = new Dictionary<string, IKeyCommand>();
            FillHotkeyList();

            chain = new CtrlHandler();
            IHotkeyHandlerChain altHandler = new AltHandler();
            IHotkeyHandlerChain charHandler = new CharHandler();
            IHotkeyHandlerChain numericHandler = new NumericHandler();
            chain.Next = altHandler;
            altHandler.Next = charHandler;
            charHandler.Next = numericHandler;
        }

        public bool Handle(Dictionary<Key, bool> keysDown)
        {
            _hotkeyString = chain.Handle(_hotkeyString, keysDown);
            if (_hotkeyList.ContainsKey(_hotkeyString))
            {
                _hotkeyList[_hotkeyString].Execute(_viewModelLocator);
                _hotkeyString = "";
                return true;
            }
            return false;
        }

        private void FillHotkeyList()
        {
            _hotkeyList.Add("CtrlS", new SaveCommand());
            _hotkeyList.Add("CtrlO", new OpenFileCommand());
            _hotkeyList.Add("CtrlP", new SavePdfCommand());
            _hotkeyList.Add("AltC", new InsertClefCommand());
            _hotkeyList.Add("AltS", new InsertTempoCommand());
            IKeyCommand time44Command = new InsertTime44Command();
            _hotkeyList.Add("AltT", time44Command);
            _hotkeyList.Add("AltT4", time44Command);
            _hotkeyList.Add("AltT3", new InsertTime34Command());
            _hotkeyList.Add("AltT6", new InsertTime68Command());
        }
    }
}
