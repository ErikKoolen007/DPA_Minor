﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sanford.Multimedia.Midi;

namespace DPA_Musicsheets.factories
{
    class MidiFactory : FileFactory
    {
        private string file_name;
        private Sequence seq;
        public MidiFactory(string file_name)
        {
            this.file_name = file_name;
        }
        public void LoadIntoDomain()
        {
            open_file();

        }

        private void open_file()
        {
            seq = new Sequence();
            seq.Load(file_name);
        }
    }
}
