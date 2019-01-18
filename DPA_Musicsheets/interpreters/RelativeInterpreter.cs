using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.domain;

namespace DPA_Musicsheets.interpreters
{
    class RelativeInterpreter : MusicPartInterpreter
    {
        public RelativeInterpreter(string musicStr) : base(musicStr)
        {
        }

        protected override Queue<MusicPart> Delegate()
        {
            MusicPartInterpreter next = new ClefInterpreter(_musicPartStr);
            return next.Interpret();
        }

        public override Queue<MusicPart> Interpret()
        {
            int index;
            if (_musicPartStr.Contains("\\relative c'"))
            {
                index = _musicPartStr.IndexOf("\\relative");
                _musicPartStr.Remove(0, index);
                _musicPartStr.Remove(index, 14);
                _musicPartStr.Remove(_musicPartStr.Length - 1);
                MusicPartWrapper wrapper = new MusicPartWrapper(null, WrapperType.Relative);
            }
            return Delegate();
        }
    }
}
