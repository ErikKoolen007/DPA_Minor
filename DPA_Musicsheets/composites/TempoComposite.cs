using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.domain;
using PSAMControlLibrary;

namespace DPA_Musicsheets.composites
{
    class TempoComposite : AbstractComposite
    {
        private Tempo tempo;

        public TempoComposite(Tempo tempo)
        {
            this.tempo = tempo;
        }
        public List<MusicalSymbol> visit(List<MusicalSymbol> symbols)
        {
            //not supported - as it already was
            return next(symbols);
        }

        public List<MusicalSymbol> next(List<MusicalSymbol> symbols)
        {
            //tree doesn't go any deeper
            return symbols;
        }
    }
}
