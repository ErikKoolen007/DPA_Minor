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
        public RepeatInterpreter(string musicStr, LinkedList<MusicPart> domain, string name = "RepeatInterpreter") : base(musicStr, domain, name)
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
            if (_musicPartStr.Contains("\\repeat "))
            {
                int endIndex = _musicPartStr.IndexOf("}");
                string notesString = _musicPartStr.Substring(0, endIndex); ;
                string[] notesArr;
                
                notesArr = notesString.Split(null);
                foreach (var n in notesArr)
                {
                    if (n != "" && !n.Contains("repeat") && !n.Contains("volta") && n !="2" && n != "{")
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
                _musicPartStr = _musicPartStr.Remove(_musicPartStr.IndexOf("\\repeat"), endIndex);
                MusicPartWrapper repeat = new MusicPartWrapper(content, WrapperType.Repeat);
                _domain.AddLast(repeat);
            }
            return _domain;
        }
    }
}
