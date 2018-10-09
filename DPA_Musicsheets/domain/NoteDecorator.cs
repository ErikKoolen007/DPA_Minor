using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.domain.Lilypond
{
    public abstract class NoteDecorator : Note
    {
        protected Note _note;

        public NoteDecorator(Note note)
        {
            _note = note;
        }

        public override void Decorate()
        {
            _note.Decorate();
        }
    }
}