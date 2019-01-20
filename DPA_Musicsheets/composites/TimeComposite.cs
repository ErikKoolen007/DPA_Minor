using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.domain;
using PSAMControlLibrary;

namespace DPA_Musicsheets.composites
{
    class TimeComposite : AbstractComposite
    {
        private Time time;

        public TimeComposite(Time time)
        {
            this.time = time;
        }
        public List<MusicalSymbol> visit(List<MusicalSymbol> symbols)
        {
            var times = time.ToString().Split('/');
            symbols.Add(new TimeSignature(TimeSignatureType.Numbers, UInt32.Parse(times[0].Substring(times[0].Length-1)), UInt32.Parse(times[1])));
            visit(symbols);
            return symbols;
        }

        public void next(List<MusicalSymbol> symbols)
        {
            //tree doesn't go any deeper
            return;
        }
    }
}
