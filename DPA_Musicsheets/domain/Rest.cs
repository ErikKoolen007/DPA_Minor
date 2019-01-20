using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.domain
{
    class Rest : MusicPart
    {
        public string letter { get; set; }= "r";
        public string duration { get; set; }
        public Rest(int duration)
        {
            this.duration = duration.ToString();
        }
        public string ToString()
        {
            return letter + duration + " ";
        }
    }
}
