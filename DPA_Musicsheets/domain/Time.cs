using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.domain
{
    class Time : MusicPart
    {
        private int beatNote { get; set; }
        private int beatsPerBar { get; set; }

        public Time(int beatNote, int beatsPerBar)
        {
            this.beatNote = beatNote;
            this.beatsPerBar = beatsPerBar;
        }

        public override string ToString()
        {
            return "\\" + beatNote + "/" + beatsPerBar + " ";
        }
    }
}
