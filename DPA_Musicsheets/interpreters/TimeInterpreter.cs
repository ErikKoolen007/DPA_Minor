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
        public TimeInterpreter(string musicStr) : base(musicStr)
        {
        }

        protected override Queue<MusicPart> Delegate()
        {
            MusicPartInterpreter next = new TempoInterpreter(_musicPartStr);
            return next.Interpret();
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
                domain.Enqueue(time);
            }
            return Delegate();
        }
    }
}
