using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.domain
{
    class Tempo : MusicPart
    {
        private int bpm { get; set; }

        public Tempo(int bpm)
        {
            this.bpm = bpm;
        }

        public override string ToString()
        {
            return "\\tempo 4 = " + bpm + " ";
        }
    }
}
