using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.domain
{
    abstract class Note : MusicPart
    {
        protected string letter;
        protected int duration;

        public Note(string letter, int duration)
        {
            this.letter = letter;
            this.duration = duration;
        }

        public abstract void decorate();

        public override string ToString()
        {
            return letter + duration;
        }
    }
}
