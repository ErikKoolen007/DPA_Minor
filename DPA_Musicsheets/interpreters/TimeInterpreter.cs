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
        public TimeInterpreter(string musicStr, Queue<MusicPart> domain) : base(musicStr, domain)
        {
        }

        protected override Queue<MusicPart> Delegate()
        {
            return _domain;
        }

        public override Queue<MusicPart> Interpret()
        {
            if (_musicPartStr.Contains("\\time "))
            {
                int index = _musicPartStr.IndexOf("\\time ");
                int beatNote = _musicPartStr[index + 6];
                int beatsPerbar = _musicPartStr[index + 8];
                _musicPartStr.Remove(index, 9);
                Time time = new Time(beatNote, beatsPerbar);
                _domain.Enqueue(time);
            }
            return Delegate();
        }
    }
}
