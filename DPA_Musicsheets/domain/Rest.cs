using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.domain
{
    class Rest : MusicPart
    {
        private int duration;
        public Rest(int duration)
        {
            this.duration = duration;
        }
        public string ToString()
        {
            return "r" + duration;
        }
    }
}
