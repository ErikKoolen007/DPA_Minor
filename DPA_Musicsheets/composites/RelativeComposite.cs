using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.composites;
using DPA_Musicsheets.domain;
using PSAMControlLibrary;
using Clef = DPA_Musicsheets.domain.Clef;
using Rest = DPA_Musicsheets.domain.Rest;

namespace DPA_Musicsheets
{
    class RelativeComposite : AbstractComposite
    {
        private MusicPartWrapper _relative;
        private LinkedList<MusicPart> _parts;
        public RelativeComposite(MusicPartWrapper relative, LinkedList<MusicPart> parts)
        {
            _relative = relative;
            _parts = parts;
        }

        public List<MusicalSymbol> visit(List<MusicalSymbol> symbols)
        {
            return next(symbols);
        }

        public List<MusicalSymbol> next(List<MusicalSymbol> symbols)
        {
            int previousOctave = 4;
            char previousNote = 'c';
            int alternativeRepeatNumber = 0;

            foreach (var part in _parts)
            {
                if (part.GetType() == typeof(Clef))
                {
                    Clef clef = (Clef) part;
                    ClefComposite clefComposite = new ClefComposite(clef);
                    symbols = clefComposite.visit(symbols);
                } else if (part.GetType() == typeof(Time))
                {
                    Time time = (Time) part;
                    TimeComposite timeComposite = new TimeComposite(time);
                    symbols = timeComposite.visit(symbols);
                } else if (part.GetType() == typeof(Tempo))
                {
                    Tempo tempo = (Tempo) part;
                    TempoComposite tempoComposite = new TempoComposite(tempo);
                    symbols = tempoComposite.visit(symbols);
                } else if (part.GetType().BaseType == typeof(BaseNote))
                {
                    BaseNote note = (BaseNote) part;
                    BaseNoteComposite noteComposite = new BaseNoteComposite(note);
                    symbols = noteComposite.visit(symbols, ref previousOctave, ref alternativeRepeatNumber, ref previousNote);
                } else if (part.GetType() == typeof(Rest))
                {
                    Rest rest = (Rest) part;
                    RestComposite restComposite = new RestComposite(rest, ref alternativeRepeatNumber);
                    symbols = restComposite.visit(symbols);
                } else if (part.GetType() == typeof(MusicPartWrapper))
                {
                    MusicPartWrapper wrapper = (MusicPartWrapper) part;
                    WrapperComposite wrapperComposite = new WrapperComposite(wrapper);
                    symbols = wrapperComposite.visit(symbols, ref previousOctave, ref alternativeRepeatNumber, ref previousNote);
                }
            }
            return symbols;
        }
    }
}
