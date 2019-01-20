using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PSAMControlLibrary;

namespace DPA_Musicsheets.composites
{
    class BarLineComposite : AbstractComposite
    {
        private int _alternativeRepeatNumber;

        public BarLineComposite(int alternativeRepeatNumber)
        {
            _alternativeRepeatNumber = alternativeRepeatNumber;
        }

        public List<MusicalSymbol> visit(List<MusicalSymbol> symbols)
        {
            symbols.Add(new Barline() { AlternateRepeatGroup = _alternativeRepeatNumber });
            return symbols;
        }

        public void next(List<MusicalSymbol> symbols)
        {
            //not possible to go any deeper
            return;
        }
    }
}
