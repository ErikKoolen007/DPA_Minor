using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.domain;

namespace DPA_Musicsheets.interpreters
{
    class ClefInterpreter : MusicPartInterpreter
    {
        public ClefInterpreter(string musicStr) : base(musicStr)
        {
        }

        protected override Queue<MusicPart> Delegate()
        {
            MusicPartInterpreter next = new TimeInterpreter(_musicPartStr);
            return next.Interpret();
        }

        public override Queue<MusicPart> Interpret()
        {
            if (_musicPartStr.Contains("\\clef "))
            {
                int index = _musicPartStr.IndexOf("\\clef ");
                Clef clef;
                if (_musicPartStr.Contains("\\clef bass"))
                {
                    clef = new Clef(4, ClefType.Fclef);
                    _musicPartStr.Remove(index, 10);
                } else if (_musicPartStr.Contains("\\clef soprano"))
                {
                    clef = new Clef(1, ClefType.Cclef);
                    _musicPartStr.Remove(index, 13);
                }
                else
                {
                    clef = new Clef(2, ClefType.Gclef);
                    _musicPartStr.Remove(index, 12);
                }
                domain.Enqueue(clef);
            }

            return Delegate();
        }
    }
}
