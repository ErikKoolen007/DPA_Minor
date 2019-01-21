using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.domain;

namespace DPA_Musicsheets.interpreters
{
    class RelativeInterpreter : MusicPartInterpreter
    {
        private SortedDictionary<int, MusicPartInterpreter> interpreterOrder = new SortedDictionary<int, MusicPartInterpreter>();
        private NoteInterpreter noteIntP;
        private MusicPartInterpreter clefIntP;
        private MusicPartInterpreter timeIntP;
        private MusicPartInterpreter tempoIntP;
        private MusicPartInterpreter repeatIntP;
        private MusicPartInterpreter alternativeIntP;
        public RelativeInterpreter(string musicStr, LinkedList<MusicPart> domain, string name = "RelativeInterpreter") : base(musicStr, domain, name)
        {

            int index = _musicPartStr.IndexOf("relative");
            _musicPartStr = _musicPartStr.Remove(0, index -1);
            initInterpreters();
        }

        protected override LinkedList<MusicPart> Delegate()
        {
            fillHashSet();

            foreach (KeyValuePair<int, MusicPartInterpreter> entry in interpreterOrder)
            {
                if(entry.Value.GetType() == typeof(NoteInterpreter))
                {
                    string notesString;
                    string[] notesArr;
                    if (_musicPartStr.IndexOf("\\") != -1)
                    {
                        int endIndex = _musicPartStr.IndexOf("\\");
                        notesString = _musicPartStr.Substring(0, endIndex);
                    }
                    else
                    {
                        notesString = _musicPartStr.Substring(0);
                    }
                    notesArr = notesString.Split(null);
                    foreach (var n in notesArr)
                    {
                        if (n != "" && n != "}")
                        {
                            if (n == "|")
                            {
                                try
                                {
                                    BaseNote tmpN = (BaseNote)_domain.Last();
                                    BaseNoteMark newN = new BaseNoteMark(tmpN);
                                    _domain.RemoveLast();
                                    _domain.AddLast(newN);
                                }
                                catch (InvalidCastException ex)
                                {
                                    Console.WriteLine(ex.StackTrace);
                                    break;
                                }
                            }
                            else
                            {
                                entry.Value._musicPartStr = n;
                                LinkedList<MusicPart> tmpList = entry.Value.Interpret();
                                MusicPart p = tmpList.First();
                                tmpList.RemoveFirst();
                                _domain.AddLast(p);
                            }
                        }
                    }
                }
                else
                {
                    entry.Value._musicPartStr = _musicPartStr;
                    _domain = entry.Value.Interpret();
                    _musicPartStr = entry.Value._musicPartStr;
                }
            }

            MusicPartWrapper relative = new MusicPartWrapper(_domain, WrapperType.Relative);
            LinkedList<MusicPart> relativeList = new LinkedList<MusicPart>();
            relativeList.AddLast(relative);
            return relativeList;
        }

        public override LinkedList<MusicPart> Interpret()
        {
            return Delegate();
        }

        private void initInterpreters()
        {
            noteIntP = new NoteInterpreter(_musicPartStr, _domain);
            clefIntP = new ClefInterpreter(_musicPartStr, _domain);
            timeIntP = new TimeInterpreter(_musicPartStr, _domain);
            tempoIntP = new TempoInterpreter(_musicPartStr, _domain);
            repeatIntP = new RepeatInterpreter(_musicPartStr, _domain);
            alternativeIntP = new AlternativeInterpreter(_musicPartStr, _domain);
        }

        private void fillHashSet()
        {
            string copyStr = _musicPartStr;
            string copyNoteStr = copyStr;

            int index = 0;
            int i = copyStr.IndexOf("\\relative");
            copyStr = copyStr.Remove(i, 14);
            while (index != -1)
            {
                try
                {   
                    if (i != -1 && !copyNoteStr.Substring(i, 12).Contains("\\") && 
                        !copyNoteStr.Substring(i, 12).Contains("{") &&
                        !copyNoteStr.Substring(i, 12).Contains("repeat") &&
                        !copyNoteStr.Substring(i, 12).Contains("clef"))
                    {
                        interpreterOrder.Add(i+10, noteIntP);
                    }
                    else if (!copyNoteStr.Contains("\\") && !copyNoteStr.Contains("{") && copyNoteStr != "")
                    {
                        interpreterOrder.Add(i+10, noteIntP);
                    }
                    i = copyNoteStr.IndexOf("\\");
                    copyNoteStr = copyNoteStr.Remove(i, 1);

                    if (copyNoteStr == "" || i + 10 > copyNoteStr.Length)
                    {
                        index = -1;
                    }
                }
                catch (Exception ex)
                {
                    break;
                }
            }

            index = 0;
            while (index != -1)
            {
                index = copyStr.IndexOf("clef treble");
                if (index != -1)
                {
                    interpreterOrder.Add(index, clefIntP);
                    copyStr = copyStr.Insert(index + 1, "xxx");
                }
            }

            index = 0;
            while (index != -1)
            {
                index = copyStr.IndexOf("clef bass");
                if (index != -1)
                {
                    interpreterOrder.Add(index, clefIntP);
                    copyStr = copyStr.Insert(index + 1, "xxx");
                }
            }

            index = 0;
            while (index != -1)
            {
                index = copyStr.IndexOf("clef soprano");
                if (index != -1)
                {
                    interpreterOrder.Add(index, clefIntP);
                    copyStr = copyStr.Insert(index + 1, "xxx");
                }
            }

            index = 0;
            while (index != -1)
            {
                index = copyStr.IndexOf("time");
                if (index != -1)
                {
                    interpreterOrder.Add(index, timeIntP);
                    copyStr = copyStr.Insert(index + 1, "xxx");
                }
            }

            index = 0;
            while (index != -1)
            {
                index = copyStr.IndexOf("tempo");
                if (index != -1)
                {
                    interpreterOrder.Add(index, tempoIntP);
                    copyStr = copyStr.Insert(index + 1, "xxx");
                }
            }

            index = 0;
            while (index != -1)
            {
                index = copyStr.IndexOf("repeat");
                if (index != -1)
                {
                    interpreterOrder.Add(index, repeatIntP);
                    copyStr = copyStr.Insert(index + 1, "xxx");
                }
            }

            index = 0;
            while (index != -1)
            {
                index = copyStr.IndexOf("alternative");
                if (index != -1)
                {
                    interpreterOrder.Add(index, alternativeIntP);
                    copyStr = copyStr.Insert(index + 1, "xxx");
                }
            }

            index = _musicPartStr.IndexOf("relative");
            _musicPartStr = _musicPartStr.Remove(0, index + 15);
            index = _musicPartStr.LastIndexOf("}");
            _musicPartStr = _musicPartStr.Remove(index);
        }
    }
}
