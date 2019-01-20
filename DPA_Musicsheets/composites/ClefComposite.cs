using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PSAMControlLibrary;
using Clef = DPA_Musicsheets.domain.Clef;

namespace DPA_Musicsheets.composites
{
    class ClefComposite : AbstractComposite
    {
        private Clef clef;

        public ClefComposite(Clef clef)
        {
            this.clef = clef;
        }
        public List<MusicalSymbol> visit(List<MusicalSymbol> symbols)
        {
            PSAMControlLibrary.Clef currentClef;
            if (clef.ClefType == domain.ClefType.Gclef)
                currentClef = new PSAMControlLibrary.Clef(ClefType.GClef, 2);
            else if (clef.ClefType == domain.ClefType.Fclef)
                currentClef = new PSAMControlLibrary.Clef(ClefType.FClef, 4);
            else if (clef.ClefType == domain.ClefType.Cclef)
                currentClef = new PSAMControlLibrary.Clef(ClefType.CClef, 3);
            else
                throw new NotSupportedException($"Clef is not supported.");
            symbols.Add(currentClef);
            next(symbols);
            return symbols;
        }

        public void next(List<MusicalSymbol> symbols)
        {
            //tree doesn't go any deeper
            return;
        }
    }
}
