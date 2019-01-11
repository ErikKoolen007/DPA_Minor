using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.domain
{
    class NoteComma : Note
    {
        public NoteComma(string letter, int duration) : base(letter, duration)
        {
            decorate();
        }

        public sealed override void decorate()
        {
            letter = "," + letter;
        }
    }
}
