using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.domain;

namespace DPA_Musicsheets.interpreters
{
    class TempoInterpreter : MusicPartInterpreter
    {
        public TempoInterpreter(string musicStr, Queue<MusicPart> domain, string name = "TempoInterpreter") : base(musicStr, domain, name)
        {
        }

        protected override Queue<MusicPart> Delegate()
        {
            return _domain;
        }

        public override Queue<MusicPart> Interpret()
        {
            if (_musicPartStr.Contains("\\tempo 4="))
            {
                int index = _musicPartStr.IndexOf("\\tempo 4=");
                string substr = _musicPartStr.Substring(index+8, 12);

                try
                {
                    //maybe " " a problem?
                    int bpm = Int32.Parse(substr);
                    Tempo tempo  = new Tempo(bpm);
                    _musicPartStr = _musicPartStr.Remove(index, 12);
                    _domain.Enqueue(tempo);
                }
                catch (InvalidCastException)
                {
                    Console.WriteLine("Could not parse bpm string to integers at TempoInterpreter");
                }
            }
            return Delegate();
        }
    }
}
