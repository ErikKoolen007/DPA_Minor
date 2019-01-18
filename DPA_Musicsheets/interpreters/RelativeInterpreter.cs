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
        public RelativeInterpreter(string musicStr, Queue<MusicPart> domain) : base(musicStr, domain)
        {
        }

        protected override Queue<MusicPart> Delegate()
        {
            MusicPartInterpreter clefIntP = new ClefInterpreter(_musicPartStr, _domain);
            _domain = clefIntP.Interpret();
            _musicPartStr = clefIntP._musicPartStr;

            MusicPartInterpreter timeIntP = new TimeInterpreter(_musicPartStr, _domain);
            _domain = timeIntP.Interpret();
            _musicPartStr = clefIntP._musicPartStr;

            MusicPartInterpreter tempoIntP = new TempoInterpreter(_musicPartStr, _domain);
            _domain = tempoIntP.Interpret();
            _musicPartStr = clefIntP._musicPartStr;

            MusicPartInterpreter repeatIntP = new RepeatInterpreter(_musicPartStr, _domain);
            _domain = repeatIntP.Interpret();
            _musicPartStr = repeatIntP._musicPartStr;
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
