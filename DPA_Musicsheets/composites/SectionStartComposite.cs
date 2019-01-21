using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.domain;
using PSAMControlLibrary;

namespace DPA_Musicsheets.composites
{
    class SectionStartComposite : AbstractComposite
    {
        private MusicPart _part;
        private int _alternativeRepeatNumber;
        public SectionStartComposite(MusicPart part, ref int alternativeRepeatNumber)
        {
            _part = part;
            _alternativeRepeatNumber = alternativeRepeatNumber;
        }
        public List<MusicalSymbol> visit(List<MusicalSymbol> symbols)
        {
            if (_part.GetType().DeclaringType == typeof(MusicPartWrapper))
            {
                _alternativeRepeatNumber++;
                symbols.Add(new Barline() { AlternateRepeatGroup = _alternativeRepeatNumber });
            }

            return next(symbols);
        }

        public List<MusicalSymbol> next(List<MusicalSymbol> symbols)
        {
            return symbols;
        }
    }
}
