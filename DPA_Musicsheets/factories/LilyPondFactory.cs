﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.domain;

namespace DPA_Musicsheets.factories
{
    class LilyPondFactory : FileFactory
    {
        private string file_name;
        private LinkedList<MusicPart> musicParts = new LinkedList<MusicPart>();

        public LilyPondFactory(string file_name)
        {
            this.file_name = file_name;
        }
        public override LinkedList<MusicPart> LoadIntoDomain()
        {


            return null;
        }
    }
}
