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
        private BaseNote _note = new Note("L", "D");

        public NoteInterpreter(string musicPartStr, MusicPartInterpreter innerInterpreter) : base(musicPartStr)
        {
        }

        protected override Queue<MusicPart> Delegate()
        {
            throw new NotImplementedException();
        }

        public override Queue<MusicPart> Interpret()
        {
            int index;
            if (_musicPartStr.Contains("is"))
            {
                index = _musicPartStr.IndexOf("is");
                _musicPartStr.Remove(index, 2);
                _note = new BaseNoteCross(_note);
            }

            if (_musicPartStr.Contains("es"))
            {
                index = _musicPartStr.IndexOf("es");
                _musicPartStr.Remove(index, 2);
                _note = new BaseNoteMole(_note);
            }

            if (_musicPartStr.Contains("'"))
            {
                index = _musicPartStr.IndexOf("'");
                _musicPartStr.Remove(index, 1);
                _note = new BaseNoteApostrophe(_note);
            }

            if (_musicPartStr.Contains(","))
            {
                index = _musicPartStr.IndexOf(",");
                _musicPartStr.Remove(index, 1);
                _note = new BaseNoteComma(_note);
            }

            if (_musicPartStr.Contains("~"))
            {
                index = _musicPartStr.IndexOf("~");
                _musicPartStr.Remove(index, 1);
                _note = new BaseNoteResound(_note);
            }

            if (_musicPartStr.Contains("."))
            {
                foreach (char c in _musicPartStr)
                {
                    index = _musicPartStr.IndexOf(".");
                    _musicPartStr.Remove(index, 1);
                    _note = new BaseNoteDot(_note);
                }
            }

            if (_musicPartStr.Contains("|"))
            {
                index = _musicPartStr.IndexOf("|");
                _musicPartStr.Remove(index, 1);
                _note = new BaseNoteMark(_note);
            }
            //only letter and duration of the note are left
            _note.letter = _note.letter.Replace("L", _musicPartStr[0].ToString());
            _note.duration = _note.duration.Replace("D", _musicPartStr[1].ToString());

            return Delegate();
        }
    }
}
