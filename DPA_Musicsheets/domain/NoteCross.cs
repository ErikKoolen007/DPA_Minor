using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.domain
{
    class NoteCross : Note
    {
        public NoteCross(string letter, int duration) : base(letter, duration)
        {
            decorate();
        }

        public sealed override void decorate()
        {
            letter = letter + "is";
        }
    }
}
