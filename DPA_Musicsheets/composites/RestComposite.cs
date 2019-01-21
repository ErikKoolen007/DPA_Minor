using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.domain;
using PSAMControlLibrary;

namespace DPA_Musicsheets.composites
{
    class RestComposite : AbstractComposite
    {
        private domain.Rest rest;
        private int _alternativeRepeatNumber;

        public RestComposite(domain.Rest rest, ref int alternativeRepeatNumber)
        {
            this.rest = rest;
            _alternativeRepeatNumber = alternativeRepeatNumber;
        }

        public List<MusicalSymbol> visit(List<MusicalSymbol> symbols)
        {
            string dur = rest.duration;
            int duration;
            foreach (var c in dur)
            {
                if (char.IsDigit(c))
                {
                    duration = Int32.Parse(c.ToString());
                    symbols.Add(new PSAMControlLibrary.Rest((MusicalSymbolDuration)duration));
                    break;
                }
            }
            
            return next(symbols);
        }

        public List<MusicalSymbol> next(List<MusicalSymbol> symbols)
        {
            if (rest.duration.Contains("}"))
            {
                SectionEndComposite sectionEnd = new SectionEndComposite(rest, true, ref _alternativeRepeatNumber);
                symbols = sectionEnd.visit(symbols);
            }

            if (rest.letter.Contains("{"))
            {
                SectionStartComposite sectionStart = new SectionStartComposite(rest, ref _alternativeRepeatNumber);
                symbols = sectionStart.visit(symbols);
            }

            if (rest.duration.Contains("|"))
            {
                BarLineComposite barLineComposite = new BarLineComposite(_alternativeRepeatNumber);
                symbols = barLineComposite.visit(symbols);
            }

            return symbols;
        }
    }
}
