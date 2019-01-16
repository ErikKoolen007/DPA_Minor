using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.domain
{
    public abstract class BaseNote : MusicPart
    {
        public string letter { get; set; }
        public string duration { get; set; }

        public BaseNote(string letter, string duration)
        {
            this.letter = letter;
            this.duration = duration;
        }
        public abstract void Decorate();

        public override string ToString()
        {
            return letter + duration + " ";
        }
    }
}
