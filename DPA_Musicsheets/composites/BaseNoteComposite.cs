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
        private static List<Char> _notesOrder = new List<Char> { 'c', 'd', 'e', 'f', 'g', 'a', 'b' };

        public BaseNoteComposite(BaseNote note)
        {
            this.note = note;
        }

        public List<MusicalSymbol> visit(List<MusicalSymbol> symbols, ref int previousOctave, ref int alternativeRepeatNumber, ref char previousNote)
        {
            string letter = this.note.letter;
            string duration = this.note.duration;
            int dotsCount = 0;
            int index;
            int alter = 0;

            NoteTieType tie = NoteTieType.None;

            //process all decorations
            if (this.note.duration.Contains("}"))
            {
                SectionEndComposite sectionEnd = new SectionEndComposite(this.note, true, ref alternativeRepeatNumber);

                index = duration.IndexOf("}");
                duration = duration.Remove(index, 1);

                symbols = sectionEnd.visit(symbols);
            }

            if (this.note.letter.Contains("{"))
            {
                SectionStartComposite sectionStart = new SectionStartComposite(this.note, ref alternativeRepeatNumber);

                index = duration.IndexOf("{");
                duration = duration.Remove(index, 1);

                symbols = sectionStart.visit(symbols);
            }

            if (this.note.duration.Contains("|"))
            {
                BarLineComposite barLineComposite = new BarLineComposite(alternativeRepeatNumber);

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
                    dotsCount++;
                }
            }

            if (this.note.letter.Contains("~"))
            {
                index = letter.IndexOf("~");
                letter = letter.Remove(index, 1);

                tie = NoteTieType.Stop;
                var lastNote = symbols.Last(s => s is Note) as Note;
                if (lastNote != null) lastNote.TieType = NoteTieType.Start;
            }

            if (this.note.letter.Contains("is"))
            {
                index = letter.IndexOf("is");
                letter = letter.Remove(index, 1);

                alter++;
            }

            if (this.note.letter.Contains("es"))
            {
                index = letter.IndexOf("es");
                letter = letter.Remove(index, 1);

                alter--;
            }

            int distanceWithPreviousNote = 0;
            if (isValid(note.letter) != '?')
            {
                distanceWithPreviousNote = _notesOrder.IndexOf(isValid(note.letter)) - _notesOrder.IndexOf(previousNote);
            }
            
            if (distanceWithPreviousNote > 3) // Shorter path possible the other way around
            {
                distanceWithPreviousNote -= 7; // The number of notes in an octave
            }
            else if (distanceWithPreviousNote < -3)
            {
                distanceWithPreviousNote += 7; // The number of notes in an octave
            }

            if (distanceWithPreviousNote + _notesOrder.IndexOf(previousNote) >= 7)
            {
                previousOctave++;
            }
            else if (distanceWithPreviousNote + _notesOrder.IndexOf(previousNote) < 0)
            {
                previousOctave--;
            }

            if (this.note.letter.Contains(","))
            {
                index = letter.IndexOf(",");
                letter = letter.Remove(index, 1);
                previousOctave--;
            }

            if (this.note.letter.Contains("'"))
            {
                index = letter.IndexOf("'");
                letter = letter.Remove(index, 1);
                previousOctave++;
            }

            previousNote = note.letter[0];
            int dur = Int32.Parse(duration);
            var psamNote = new Note(letter.ToUpper(), alter, previousOctave, (MusicalSymbolDuration)dur, NoteStemDirection.Up, tie, new List<NoteBeamType>() { NoteBeamType.Single });
            psamNote.NumberOfDots += dotsCount;

            symbols.Add(psamNote);

            return next(symbols);
        }

        public List<MusicalSymbol> visit(List<MusicalSymbol> symbols)
        {
            throw new NotImplementedException();
        }

        public List<MusicalSymbol> next(List<MusicalSymbol> symbols)
        {
            return symbols;
        }

        private static char isValid(String str)
        {
            foreach(char c in str)
            {
                if (char.IsLetter(c))
                    return c;
            }
            return '?';
        }
    }
}
