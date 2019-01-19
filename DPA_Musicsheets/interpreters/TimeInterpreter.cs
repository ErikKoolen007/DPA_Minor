using System;
using System.Collections.Generic;
using System.Linq;
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
                int beatNote = _musicPartStr[index + 6];
                int beatsPerbar = _musicPartStr[index + 8];
                _musicPartStr = _musicPartStr.Remove(index, 9);
                Time time = new Time(beatNote, beatsPerbar);
                _domain.AddLast(time);
            }
            return Delegate();
        }
    }
}
