using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.domain.Lilypond
{
    public class CrossDecorator : NoteDecorator
    {
        private string cross = "is";
        public CrossDecorator(Note note) : base(note)
        {
            _note = note;
        }
        public override void Decorate()
        {
        }

        public override string toString()
        {
            return _note.Letter + cross + _note.Number;
        }
    }
}
