using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.domain;
using DPA_Musicsheets.Models;
using PSAMControlLibrary;

namespace DPA_Musicsheets.composites
{
    class SectionEndComposite : AbstractComposite
    {
        private MusicPart _part;
        private bool _backWard;
        private int _alterNativeRepeatNumber;
        public SectionEndComposite(MusicPart part, bool backward, ref int alternativeRepeatNumber)
        {
            _part = part;
            _backWard = backward;
            _alterNativeRepeatNumber = alternativeRepeatNumber;
        }

        public List<MusicalSymbol> visit(List<MusicalSymbol> symbols)
        {
            //this could be more complex, check prev switch if it goes wrong
            if (_alterNativeRepeatNumber == 1 && _part.GetType().DeclaringType == typeof(MusicPartWrapper))
            {
                _alterNativeRepeatNumber++;
            }
            
            if (_backWard)
            {
                symbols.Add(new Barline() { RepeatSign = RepeatSignType.Backward, AlternateRepeatGroup = _alterNativeRepeatNumber });
            }
            else
            {
                symbols.Add(new Barline() { RepeatSign = RepeatSignType.Forward, AlternateRepeatGroup = _alterNativeRepeatNumber });
            }

            return next(symbols);
        }

        public List<MusicalSymbol> next(List<MusicalSymbol> symbols)
        {
            //not possible to go any deeper
            return symbols;
        }
    }
}
