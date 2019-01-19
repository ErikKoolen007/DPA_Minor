﻿using System;
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
        private Rest _r = null;
        private LinkedList<MusicPart> tmpQueue = new LinkedList<MusicPart>();

        public NoteInterpreter(string musicPartStr, LinkedList<MusicPart> domain, string name = "NoteInterpreter") : base(musicPartStr, domain, name)
        {
        }

        protected override LinkedList<MusicPart> Delegate()
        {
            if (_r == null)
            {
                tmpQueue.AddLast(_note);
            }
            else
            {
                tmpQueue.AddLast(_r);
            }
            
            return tmpQueue;
        }

        public override LinkedList<MusicPart> Interpret()
        {
            int index;
            if (_musicPartStr.Contains("r"))
            {
                index = _musicPartStr.IndexOf("r");
                int duration = Int32.Parse(_musicPartStr.Substring(index + 1));
                _musicPartStr = _musicPartStr.Remove(index);
                _r = new Rest(duration);
            }

            if (_musicPartStr.Contains("is"))
            {
                index = _musicPartStr.IndexOf("is");
                _musicPartStr = _musicPartStr.Remove(index, 2);
                _note = new BaseNoteCross(_note);
            }

            if (_musicPartStr.Contains("es"))
            {
                index = _musicPartStr.IndexOf("es");
                _musicPartStr = _musicPartStr.Remove(index, 2);
                _note = new BaseNoteMole(_note);
            }

            if (_musicPartStr.Contains("'"))
            {
                index = _musicPartStr.IndexOf("'");
                _musicPartStr = _musicPartStr.Remove(index, 1);
                _note = new BaseNoteApostrophe(_note);
            }

            if (_musicPartStr.Contains(","))
            {
                index = _musicPartStr.IndexOf(",");
                _musicPartStr = _musicPartStr.Remove(index, 1);
                _note = new BaseNoteComma(_note);
            }

            if (_musicPartStr.Contains("~"))
            {
                index = _musicPartStr.IndexOf("~");
                _musicPartStr = _musicPartStr.Remove(index, 1);
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
                _musicPartStr = _musicPartStr.Remove(index, 1);
                _note = new BaseNoteMark(_note);
            }
            //only letter and duration of the note are left
            _note.letter = _note.letter.Replace("L", _musicPartStr[0].ToString());
            _note.duration = _note.duration.Replace("D", _musicPartStr[1].ToString());

            return Delegate();
        }
    }
}
