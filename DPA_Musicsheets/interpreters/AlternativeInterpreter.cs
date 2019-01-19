using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using DPA_Musicsheets.domain;

namespace DPA_Musicsheets.interpreters
{
    class AlternativeInterpreter : MusicPartInterpreter
    {
        private NoteInterpreter noteInterpreter;
        private LinkedList<MusicPart> content = new LinkedList<MusicPart>();
        private MusicPartWrapper alternative = null;
        private string _currentNote = "";
        private bool first;
        private bool last;
        public AlternativeInterpreter(string musicStr, LinkedList<MusicPart> domain, string name = "AlternativeInterpreter") : base(musicStr, domain, name)
        {
        }

        protected override LinkedList<MusicPart> Delegate()
        {
            alternative = new MusicPartWrapper(content, WrapperType.Alternative);
            noteInterpreter._musicPartStr = _currentNote;

            //either a rest or a note
            MusicPart part = noteInterpreter.Interpret().First();
            if (first)
                alternative.WrapCurlyBraces(part, true);

            if(last)
                alternative.WrapCurlyBraces(part, false);

            content.AddLast(part);

            return _domain;
        }

        public override LinkedList<MusicPart> Interpret()
        {
            if (_musicPartStr.Contains("\\alternative "))
            {
                int index = _musicPartStr.IndexOf("\\alternative ");
                int aOpen = index + 13;
                int aClosed = _musicPartStr.IndexOf("} }") + 2;
                string alternativeString = _musicPartStr.Substring(aOpen + 1, aClosed - 1);
                _musicPartStr = _musicPartStr.Remove(index, aClosed + 1);

                while(alternativeString != "")
                {
                    int sOpen = alternativeString.IndexOf("{");
                    int sClosed = alternativeString.IndexOf("}");
                    string notesString = alternativeString.Substring(sOpen + 1, sClosed - 1);

                    while (notesString != "")
                    {
                        int mark = notesString.IndexOf("|");

                        if (mark < 2 && mark != -1)
                            _currentNote = notesString.Split(notesString[mark]).ToString();
                        else
                            _currentNote = notesString.Split(null).ToString();

                        CheckFirstLast(alternativeString, sOpen, sClosed);

                        int noteStrLength = _currentNote.Length;
                        Delegate();
                        notesString = notesString.Remove(0, noteStrLength);
                    }
                    alternativeString = alternativeString.Remove(sOpen, sClosed + 1);
                }
            }
            alternative._symbols = content;
            _domain.AddLast(alternative);
            return _domain;
        }

        private void CheckFirstLast(string alternativeString, int sOpen, int sClosed)
        {
            if (alternativeString.IndexOf(_currentNote) == sOpen + 1)
                first = true;
            else
                first = false;

            if (alternativeString.IndexOf(_currentNote) == sClosed - 1)
                last = true;
            else
                last = false;
        }
    }
}
