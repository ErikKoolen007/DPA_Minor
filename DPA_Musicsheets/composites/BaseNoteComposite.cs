using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DPA_Musicsheets.domain;
using PSAMControlLibrary;
using Note = PSAMControlLibrary.Note;

namespace DPA_Musicsheets.composites
{
    class BaseNoteComposite : AbstractComposite
    {
        private BaseNote note;
        private int _previousOctave;
        private int _alternativeRepeatNumber;
        private List<Char> _notesOrder;

        public BaseNoteComposite(BaseNote note, ref int previousOctave, List<Char> notesOrder, ref int alternativeRepeatNumber)
        {
            _previousOctave = previousOctave;
            _notesOrder = notesOrder;
            this.note = note;
        }

        public List<MusicalSymbol> visit(List<MusicalSymbol> symbols)
        {
            string letter = this.note.letter;
            string duration = this.note.duration;
            int index;

            if (this.note.duration.Contains("}"))
            {
                SectionEndComposite sectionEnd = new SectionEndComposite(this.note, true, ref _alternativeRepeatNumber);

                index = duration.IndexOf("}");
                duration = duration.Remove(index, 1);

                symbols = sectionEnd.visit(symbols);
            }

            if (this.note.letter.Contains("{"))
            {
                SectionStartComposite sectionStart = new SectionStartComposite(this.note, ref _alternativeRepeatNumber);

                index = duration.IndexOf("{");
                duration = duration.Remove(index, 1);

                symbols = sectionStart.visit(symbols);
            }

            if (this.note.duration.Contains("|"))
            {
                BarLineComposite barLineComposite = new BarLineComposite(_alternativeRepeatNumber);

                index = duration.IndexOf("|");
                duration = duration.Remove(index, 1);

                symbols = barLineComposite.visit(symbols);
            }

            if (this.note.duration.Contains("."))
            {
                index = duration.IndexOf(".");
                while (index != -1)
                {
                    duration = duration.Remove(index, 1);
                }
            }

            if (this.note.letter.Contains("~"))
            {
                index = letter.IndexOf("~");
                letter = letter.Remove(index, 1);
            }

            if (this.note.letter.Contains(","))
            {
                index = letter.IndexOf(",");
                letter = letter.Remove(index, 1);
            }

            if (this.note.letter.Contains("'"))
            {
                index = letter.IndexOf("'");
                letter = letter.Remove(index, 1);
            }

            if (this.note.letter.Contains("is"))
            {
                index = letter.IndexOf("is");
                letter = letter.Remove(index, 1);
            }

            if (this.note.letter.Contains("es"))
            {
                index = letter.IndexOf("es");
                letter = letter.Remove(index, 1);
            }
        }

        public void next(List<MusicalSymbol> symbols)
        {
            //structure does not go any deeper
        }
    }
}
