using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.domain;
using Sanford.Multimedia.Midi;

namespace DPA_Musicsheets.adapters
{
    public abstract class MetaTypeAdapter
    {
        protected MetaMessage msg;
        public abstract MusicPart LoadIntoDomain();
        public abstract void decodeMsg();
    }
}
