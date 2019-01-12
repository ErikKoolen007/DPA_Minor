using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.domain
{
    class BaseNoteApostrophe : BaseNote
    {
        public BaseNoteApostrophe(BaseNote note) : base(note.letter, note.duration)
        {
            Decorate();
        }

        //check this
        public sealed override void Decorate()
        {
            letter = "'" + letter;
        } 
    }
}
