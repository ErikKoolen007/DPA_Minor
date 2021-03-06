﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.domain;

namespace DPA_Musicsheets.interpreters
{
    class TempoInterpreter : MusicPartInterpreter
    {
        public TempoInterpreter(string musicStr, LinkedList<MusicPart> domain, string name = "TempoInterpreter") : base(musicStr, domain, name)
        {
        }

        protected override LinkedList<MusicPart> Delegate()
        {
            return _domain;
        }

        public override LinkedList<MusicPart> Interpret()
        {
            if (_musicPartStr.Contains("\\tempo 4="))
            {
                int index = _musicPartStr.IndexOf("\\tempo 4=");
                string[] split = _musicPartStr.Split(null, 1);
                split = split[0].Split("=".ToCharArray(), 2);
                int bpm;
                if (split[1].Length >= 3)
                {
                    split = split[1].Split(null, 2);
                    bpm = Int32.Parse(split[0]);
                }
                else
                {
                    bpm = Int32.Parse(split[1]);
                }
                Tempo tempo  = new Tempo(bpm);
                _musicPartStr = _musicPartStr.Remove(index, 12);
                _domain.AddLast(tempo);
            }
            return Delegate();
        }
    }
}
