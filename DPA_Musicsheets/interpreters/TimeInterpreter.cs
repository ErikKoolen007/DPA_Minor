using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.domain;

namespace DPA_Musicsheets.interpreters
{
    class TimeInterpreter : MusicPartInterpreter
    {
        public TimeInterpreter(string musicStr, LinkedList<MusicPart> domain, string name = "TimeInterpreter") : base(musicStr, domain, name)
        {
        }

        protected override LinkedList<MusicPart> Delegate()
        {
            return _domain;
        }

        public override LinkedList<MusicPart> Interpret()
        {
            if (_musicPartStr.Contains("\\time "))
            {
                int index = _musicPartStr.IndexOf("\\time ");

                string bNote = _musicPartStr.Substring(index + 6, 1);
                string bPerBar = _musicPartStr.Substring(index + 8, 1);

                int beatNote = Int32.Parse(bNote);
                int beatsPerbar = Int32.Parse(bPerBar);

                _musicPartStr = _musicPartStr.Remove(index, 9);
                Time time = new Time(beatNote, beatsPerbar);
                _domain.AddLast(time);
            }
            return Delegate();
        }
    }
}
