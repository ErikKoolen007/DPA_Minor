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
        private string currentNote = "";
        public RepeatInterpreter(string musicStr, Queue<MusicPart> domain) : base(musicStr, domain)
        {
            noteInterpreter = new NoteInterpreter(currentNote, domain);
        }

        protected override Queue<MusicPart> Delegate()
        {
            noteInterpreter._musicPartStr = currentNote;
            content.AddLast(noteInterpreter.Interpret().Dequeue());
            //return not needed in this situation
            return _domain;
        }

        public override Queue<MusicPart> Interpret()
        {
            if (_musicPartStr.Contains("\\repeat volta "))
            {
                int index = _musicPartStr.IndexOf("\\repeat volta ");
                int rOpen = index + 16;
                int rClosed = _musicPartStr.IndexOf("}");
                string notesString = _musicPartStr.Substring(rOpen + 1, rClosed - 1);

                while (notesString != "")
                {
                    currentNote = _musicPartStr.Split(null).ToString();
                    int noteStrLength = currentNote.Length;
                    Delegate();
                    notesString.Remove(0, noteStrLength);
                }

                MusicPartWrapper repeat = new MusicPartWrapper(content, WrapperType.Repeat);
                _domain.Enqueue(repeat);
            }
            return _domain;
        }
    }
}
