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
        private int number { get; set; }
        private int tempo { get; set; }

        public Tempo(int bpm, int number)
        {
            this.bpm = bpm;
            this.number = number;
        }

        public override string ToString()
        {
            return "\\tempo 4 = " + bpm;
        }

        private void calculateTempo()
        {
            //not implemented yet
        }
    }
}
