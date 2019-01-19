using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.domain;

namespace DPA_Musicsheets.interpreters
{
    class RepeatInterpreter : MusicPartInterpreter
    {
        private NoteInterpreter noteInterpreter;
        private LinkedList<MusicPart> content = new LinkedList<MusicPart>();
        private string _currentNote = "";
        public RepeatInterpreter(string musicStr, LinkedList<MusicPart> domain, string name = "RepeatInterpreter") : base(musicStr, domain, name)
        {
            noteInterpreter = new NoteInterpreter(_currentNote, domain);
        }

        protected override LinkedList<MusicPart> Delegate()
        {
            noteInterpreter._musicPartStr = _currentNote;
            content.AddLast(noteInterpreter.Interpret().First);

            return _domain;
        }

        public override LinkedList<MusicPart> Interpret()
        {
            if (_musicPartStr.Contains("\\repeat "))
            {
                int index = _musicPartStr.IndexOf("\\repeat ");
                int rOpen = index + 16;
                int rClosed = _musicPartStr.IndexOf("}");
                string notesString = _musicPartStr.Substring(rOpen + 1, rClosed - 1);
                _musicPartStr = _musicPartStr.Remove(index, rClosed+1);

                while (notesString != "")
                {
                    int mark = notesString.IndexOf("|");

                    if (mark < 2 && mark != -1)
                    {
                        _currentNote = notesString.Split(notesString[mark]).ToString();
                    }
                    else
                    {
                        _currentNote = notesString.Split(null).ToString();       
                    }
                    int noteStrLength = _currentNote.Length;

                    Delegate();
                    notesString = notesString.Remove(0, noteStrLength);
                }
                MusicPartWrapper repeat = new MusicPartWrapper(content, WrapperType.Repeat);
                _domain.AddLast(repeat);
            }
            return _domain;
        }
    }
}
