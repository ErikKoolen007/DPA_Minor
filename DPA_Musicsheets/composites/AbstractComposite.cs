using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PSAMControlLibrary;

namespace DPA_Musicsheets
{
    interface AbstractComposite
    {
        List<MusicalSymbol> visit(List<MusicalSymbol> symbols);
        void next(List<MusicalSymbol> symbols);
    }
}
