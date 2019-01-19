using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.domain;

namespace DPA_Musicsheets.interpreters
{
    class RelativeInterpreter : MusicPartInterpreter
    {
        private LinkedList<MusicPart> content = new LinkedList<MusicPart>();

        private Dictionary<int, string> interpreterOrder = new Dictionary<int, string>();
        private NoteInterpreter noteIntP;
        private MusicPartInterpreter clefIntP;
        private MusicPartInterpreter timeIntP;
        private MusicPartInterpreter tempoIntP;
        private MusicPartInterpreter repeatIntP;
        private MusicPartInterpreter alternativeIntP;
        public RelativeInterpreter(string musicStr, Queue<MusicPart> domain, string name = "RelativeInterpreter") : base(musicStr, domain, name)
        {
            initInterpreters();
        }

        protected override Queue<MusicPart> Delegate()
        {

            MusicPartWrapper relative = new MusicPartWrapper(content, WrapperType.Relative);
            _domain.Enqueue(relative);
            return _domain;
        }

        public override Queue<MusicPart> Interpret()
        {
            int index;

            if (_musicPartStr.Contains("\\relative c'"))
            {
                index = _musicPartStr.IndexOf("\\relative");
                _musicPartStr = _musicPartStr.Remove(0, index+11);
                index = _musicPartStr.LastIndexOf("}");
                _musicPartStr = _musicPartStr.Remove(index);
            }
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
            while (index != -1)
            {
                index = copyNoteStr.IndexOf("\\");
                string sub = copyNoteStr.Substring(index+2 + 13);
                if (!sub.Contains("{") && !sub.Contains("\\"))
                {
                    interpreterOrder.Add(index + 15, noteIntP._name);
                }

                if (index != -1)
                {
                    copyNoteStr.Insert(index + 1, "xxx");
                }
            }

            index = 0;
            while (index != -1)
            {
                index = copyStr.IndexOf("\\clef treble");
                interpreterOrder.Add(index, clefIntP._name);
                if (index != -1)
                {
                    copyStr = copyStr.Insert(index + 1, "xxx");
                }
            }

            index = 0;
            while (index != -1)
            {
                index = copyStr.IndexOf("\\clef bass");
                interpreterOrder.Add(index, clefIntP._name);
                if (index != -1)
                {
                    copyStr = copyStr.Insert(index + 1, "xxx");
                }
            }

            index = 0;
            while (index != -1)
            {
                index = copyStr.IndexOf("\\clef soprano");
                interpreterOrder.Add(index, clefIntP._name);
                if (index != -1)
                {
                    copyStr = copyStr.Insert(index + 1, "xxx");
                }
            }

            index = 0;
            while (index != -1)
            {
                index = copyStr.IndexOf("\\time");
                interpreterOrder.Add(index, timeIntP._name);
                if (index != -1)
                {
                    copyStr = copyStr.Insert(index + 1, "xxx");
                }
            }

            index = 0;
            while (index != -1)
            {
                index = copyStr.IndexOf("\\tempo");
                interpreterOrder.Add(index, tempoIntP._name);
                if (index != -1)
                {
                    copyStr = copyStr.Insert(index + 1, "xxx");
                }
            }

            index = 0;
            while (index != -1)
            {
                index = copyStr.IndexOf("\\repeat");
                interpreterOrder.Add(index, repeatIntP._name);
                if (index != -1)
                {
                    copyStr = copyStr.Insert(index + 1, "xxx");
                }
            }

            index = 0;
            while (index != -1)
            {
                index = copyStr.IndexOf("\\alternative");
                interpreterOrder.Add(index, alternativeIntP._name);
                if (index != -1)
                {
                    copyStr = copyStr.Insert(index + 1, "xxx");
                }
            }
        }
    }
}
