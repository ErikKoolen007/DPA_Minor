using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.domain
{
    class BaseNoteMark : BaseNote
    {
        public BaseNoteMark(BaseNote note) : base(note.letter, note.duration)
        {
            Decorate();
        }

        public sealed override void Decorate()
        {
            duration = duration + " | \r\n";
        }
    }
}
