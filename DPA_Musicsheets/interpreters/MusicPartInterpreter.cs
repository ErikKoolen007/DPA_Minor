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
        public string _name { get; set; }
        public string _musicPartStr { get; set; }
        protected Queue<MusicPart> _domain = new Queue<MusicPart>();

        public MusicPartInterpreter(string musicStr, Queue<MusicPart> domain, string name)
        {
            _musicPartStr = musicStr;
            _domain = domain;
            _name = name;
        }
        protected abstract Queue<MusicPart> Delegate();
        public abstract Queue<MusicPart> Interpret();
    }
}
