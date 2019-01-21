using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.domain;
using PSAMControlLibrary;
using Rest = DPA_Musicsheets.domain.Rest;

namespace DPA_Musicsheets.composites
{
    class WrapperComposite : AbstractComposite
    {
        private MusicPartWrapper wrapper;
        private LinkedList<MusicPart> children;
        public WrapperComposite(MusicPartWrapper wrapper)
        {
            this.wrapper = wrapper;
            children = wrapper._symbols;
        }

        public List<MusicalSymbol> visit(List<MusicalSymbol> symbols, ref int previousOctave, ref int alternativeRepeatNumber, ref char previousNote)
        {
            if (wrapper._type == WrapperType.Repeat)
            {
                symbols.Add(new Barline() { RepeatSign = RepeatSignType.Forward });
            }
            return next(symbols, ref previousOctave, ref alternativeRepeatNumber, ref previousNote);
        }

        public List<MusicalSymbol> next(List<MusicalSymbol> symbols, ref int previousOctave, ref int alternativeRepeatNumber, ref char previousNote)
        {
            foreach (var part in children)
            {
                if (part.GetType().BaseType == typeof(BaseNote))
                {
                    BaseNote note = (BaseNote)part;
                    BaseNoteComposite noteComposite = new BaseNoteComposite(note);
                    symbols = noteComposite.visit(symbols, ref previousOctave, ref alternativeRepeatNumber, ref previousNote);
                }
                else if (part.GetType() == typeof(Rest))
                {
                    Rest rest = (Rest)part;
                    RestComposite restComposite = new RestComposite(rest, ref alternativeRepeatNumber);
                    symbols = restComposite.visit(symbols);
                }
                else if (part.GetType() == typeof(MusicPartWrapper))
                {
                    MusicPartWrapper wrapper = (MusicPartWrapper)part;
                    WrapperComposite wrapperComposite = new WrapperComposite(wrapper);
                    symbols = wrapperComposite.visit(symbols, ref previousOctave, ref alternativeRepeatNumber, ref previousNote);
                }
            }

            return symbols;
        }

        public List<MusicalSymbol> visit(List<MusicalSymbol> symbols)
        {
            throw new NotImplementedException();
        }

        public List<MusicalSymbol> next(List<MusicalSymbol> symbols)
        {
            throw new NotImplementedException();
        }
    }
}
