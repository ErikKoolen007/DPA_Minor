﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.domain
{
    class BaseNoteCross : BaseNote
    {
        public BaseNoteCross(BaseNote note) : base(note.letter, note.duration)
        {
            Decorate();
        }

        public sealed override void Decorate()
        {
            letter = letter + "is";
        }
    }
}
