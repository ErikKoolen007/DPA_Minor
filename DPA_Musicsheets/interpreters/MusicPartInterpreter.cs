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
        protected LinkedList<MusicPart> _domain = new LinkedList<MusicPart>();

        public MusicPartInterpreter(string musicStr, LinkedList<MusicPart> domain, string name)
        {
            _musicPartStr = musicStr;
            _domain = domain;
            _name = name;
        }
        protected abstract LinkedList<MusicPart> Delegate();
        public abstract LinkedList<MusicPart> Interpret();
    }
}
