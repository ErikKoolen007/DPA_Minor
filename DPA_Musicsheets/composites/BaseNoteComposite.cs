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
            //does this work or extra parameter needed?
            string previousNote = symbols[0].MusicalCharacter.ToLower();
            NoteTieType tie = NoteTieType.None;
            if (this.note.letter.Contains("~"))
            {
                tie = NoteTieType.Stop;
                var lastNote = symbols.Last(s => s is Note) as Note;

                if (lastNote != null) lastNote.TieType = NoteTieType.Start;
                string temp = this.note.duration;
                if (temp.Contains("|") || temp.Contains("}"))
                {

                }
            }

            // Length
            int noteLength = Int32.Parse(Regex.Match(currentToken.Value, @"\d+").Value);
            // Crosses and Moles
            int alter = 0;
            alter += Regex.Matches(currentToken.Value, "is").Count;
            alter -= Regex.Matches(currentToken.Value, "es|as").Count;
            // Octaves
            int distanceWithPreviousNote =
                _notesOrder.IndexOf(currentToken.Value[0]) - _notesOrder.IndexOf(previousNote);
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
                _previousOctave++;
            }
            else if (distanceWithPreviousNote + _notesOrder.IndexOf(previousNote) < 0)
            {
                _previousOctave--;
            }

            // Force up or down.
            _previousOctave += this.note.letter.Count(c => c == '\'');
            _previousOctave -= this.note.letter.Count(c => c == ',');

            previousNote = this.note.letter;

            var note = new Note(currentToken.Value[0].ToString().ToUpper(), alter, _previousOctave,
                (MusicalSymbolDuration) noteLength, NoteStemDirection.Up, tie,
                new List<NoteBeamType>() {NoteBeamType.Single});
            note.NumberOfDots += currentToken.Value.Count(c => c.Equals('.'));

            symbols.Add(note);
            return symbols;
        }

        public void next(List<MusicalSymbol> symbols)
        {
            if (note.duration.Contains("}"))
            {
                SectionEndComposite sectionEnd = new SectionEndComposite(note, true, ref _alternativeRepeatNumber);
                symbols = sectionEnd.visit(symbols);
            }

            if (note.letter.Contains("{"))
            {
                SectionStartComposite sectionStart = new SectionStartComposite(note, ref _alternativeRepeatNumber);
                symbols = sectionStart.visit(symbols);
            }

            if (note.duration.Contains("|"))
            {
                BarLineComposite barLineComposite = new BarLineComposite(_alternativeRepeatNumber);
                symbols = barLineComposite.visit(symbols);
            }
        }
    }
}
