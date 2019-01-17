using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using DPA_Musicsheets.domain;

namespace DPA_Musicsheets.interpreters
{
    class NoteInterpreter : MusicPartInterpreter
    {
        protected BaseNote _note = new Note("L", "D");
        public MusicPart Interpret(string musicPartStr)
        {
            int index;
            if (musicPartStr.Contains("is"))
            {
                index = musicPartStr.IndexOf("is");
                musicPartStr.Remove(index, 2);
                _note = new BaseNoteCross(_note);
            }

            if (musicPartStr.Contains("es"))
            {
                index = musicPartStr.IndexOf("es");
                musicPartStr.Remove(index, 2);
                _note = new BaseNoteMole(_note);
            }

            if (musicPartStr.Contains("'"))
            {
                index = musicPartStr.IndexOf("'");
                musicPartStr.Remove(index, 1);
                _note = new BaseNoteApostrophe(_note);
            }

            if (musicPartStr.Contains(","))
            {
                index = musicPartStr.IndexOf(",");
                musicPartStr.Remove(index, 1);
                _note = new BaseNoteComma(_note);
            }

            if (musicPartStr.Contains("~"))
            {
                index = musicPartStr.IndexOf("~");
                musicPartStr.Remove(index, 1);
                _note = new BaseNoteResound(_note);
            }

            if (musicPartStr.Contains("."))
            {
                foreach (char c in musicPartStr)
                {
                    index = musicPartStr.IndexOf(".");
                    musicPartStr.Remove(index, 1);
                    _note = new BaseNoteDot(_note);
                }
            }

            if (musicPartStr.Contains("|"))
            {
                index = musicPartStr.IndexOf("|");
                musicPartStr.Remove(index, 1);
                _note = new BaseNoteMark(_note);
            }
            //only letter and duration of the note are left
            _note.letter = _note.letter.Replace("L", musicPartStr[0].ToString());
            _note.duration = _note.duration.Replace("D", musicPartStr[1].ToString());

            return _note;
        }

        public MusicPart Delegate(string musicPartStr)
        {
            throw new NotImplementedException();
        }

        public string Inverseinterpret(MusicPart part)
        {
            throw new NotImplementedException();
        }

        public string InverseDelegate(MusicPart part)
        {
            throw new NotImplementedException();
        }
    }
}
