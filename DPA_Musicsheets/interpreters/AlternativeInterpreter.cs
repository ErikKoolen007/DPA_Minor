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
        private bool first;
        private bool last;
        public AlternativeInterpreter(string musicStr, LinkedList<MusicPart> domain, string name = "AlternativeInterpreter") : base(musicStr, domain, name)
        {
            noteInterpreter = new NoteInterpreter("", domain);
        }

        protected override LinkedList<MusicPart> Delegate()
        {
            LinkedList<MusicPart> tmpList = noteInterpreter.Interpret();
            MusicPart p = tmpList.First();
            tmpList.RemoveFirst();
            content.AddLast(p);

            return _domain;
        }

        public override LinkedList<MusicPart> Interpret()
        {
            if (_musicPartStr.Contains("\\alternative "))
            {
                string notesString;
                string[] notesArr;
                int endIndex = _musicPartStr.IndexOf("}  }");
                notesString = _musicPartStr.Substring(0, endIndex);

                notesArr = notesString.Split(null);
                foreach (var n in notesArr)
                {
                    if (n != "" && n != "{" && n != "}" && !n.Contains("alternative"))
                    {
                        if (n == "|")
                        {
                            try
                            {
                                BaseNote tmpN = (BaseNote)content.Last();
                                BaseNoteMark newN = new BaseNoteMark(tmpN);
                                content.RemoveLast();
                                content.AddLast(newN);
                            }
                            catch (InvalidCastException ex)
                            {
                                Console.WriteLine(ex.StackTrace);
                                break;
                            }
                        }
                        else
                        {
                            noteInterpreter._musicPartStr = n;
                            Delegate();
                        }
                    }
                }

                //                int index = _musicPartStr.IndexOf("\\alternative ");
                //                int aOpen = index + 13;
                //                int aClosed = _musicPartStr.IndexOf("} }") + 2;
                //                string alternativeString = _musicPartStr.Substring(aOpen + 1, aClosed - 1);
                //                _musicPartStr = _musicPartStr.Remove(index, aClosed + 1);
                //
                //                while(alternativeString != "")
                //                {
                //                    int sOpen = alternativeString.IndexOf("{");
                //                    int sClosed = alternativeString.IndexOf("}");
                //                    string notesString = alternativeString.Substring(sOpen + 1, sClosed - 1);
                //
                //                    while (notesString != "")
                //                    {
                //                        int mark = notesString.IndexOf("|");
                //
                //                        if (mark < 2 && mark != -1)
                //                            _currentNote = notesString.Split(notesString[mark]).ToString();
                //                        else
                //                            _currentNote = notesString.Split(null).ToString();
                //
                //                        CheckFirstLast(alternativeString, sOpen, sClosed);
                //
                //                        int noteStrLength = _currentNote.Length;
                //                        Delegate();
                //                        notesString = notesString.Remove(0, noteStrLength);
                //                    }
                //                    alternativeString = alternativeString.Remove(sOpen, sClosed + 1);
                //                }
                _musicPartStr = _musicPartStr.Remove(_musicPartStr.IndexOf("\\alternative"), endIndex);
                MusicPartWrapper alternative = new MusicPartWrapper(content, WrapperType.Alternative);
                _domain.AddLast(alternative);
            }
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
