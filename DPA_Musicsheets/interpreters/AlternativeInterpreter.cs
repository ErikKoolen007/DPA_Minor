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
        private MusicPartWrapper alternative = new MusicPartWrapper(null, WrapperType.Alternative);

        private LinkedList<MusicPart> content = new LinkedList<MusicPart>();

        private bool _wrapFirst;
        private bool _wrapLast;
        public AlternativeInterpreter(string musicStr, LinkedList<MusicPart> domain, string name = "AlternativeInterpreter") : base(musicStr, domain, name)
        {
            noteInterpreter = new NoteInterpreter("", domain);
        }

        protected override LinkedList<MusicPart> Delegate()
        {
            LinkedList<MusicPart> tmpList = noteInterpreter.Interpret();
            MusicPart p = tmpList.First();

            if(_wrapFirst)
                p = alternative.WrapCurlyBraces(p, true);
                _wrapFirst = false;
            if(_wrapLast)
                content.Last.Value = alternative.WrapCurlyBraces(content.Last.Value, false);
                _wrapLast = false;

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
                int startIndex = _musicPartStr.IndexOf("\\alternative");
                int endIndex = _musicPartStr.IndexOf("}  }");
                notesString = _musicPartStr.Substring(startIndex+14, endIndex-12 - startIndex);

                notesArr = notesString.Split(null);
                foreach (var n in notesArr)
                { 

                    if (n.Contains("{"))
                        _wrapFirst = true;

                    if (n.Contains("}"))
                        _wrapLast = true;

                    if (n != "{" && n != "}" && n != "" && !n.Contains("alternative"))
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
                //append the } for the last element
                content.Last.Value = alternative.WrapCurlyBraces(content.Last.Value, false);

                _musicPartStr = _musicPartStr.Remove(startIndex, endIndex - startIndex);
                alternative._symbols = content;
                _domain.AddLast(alternative);
            }
            return _domain;
        }
    }
}
