using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.domain
{
    class BaseNoteMole : BaseNote
    {
        public BaseNoteMole(BaseNote note) : base(note.letter, note.duration)
        {
            Decorate();
        }

        public sealed override void Decorate()
        {
            letter = letter + "es";
        }
    }
}
