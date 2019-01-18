using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.domain;

namespace DPA_Musicsheets.interpreters
{
    public abstract class MusicPartInterpreter
    {
        protected string _musicPartStr;
        protected Queue<MusicPart> _domain = new Queue<MusicPart>();

        public MusicPartInterpreter(string musicStr, Queue<MusicPart> domain)
        {
            _musicPartStr = musicStr;
        }
        protected abstract Queue<MusicPart> Delegate();
        public abstract Queue<MusicPart> Interpret();
    }
}
