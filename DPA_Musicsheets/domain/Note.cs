using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.domain;

namespace DPA_Musicsheets.domain
{
    public class Note : BaseNote
    {
        public Note(string letter, string duration) : base(letter, duration)
        {
            this.letter = letter;
            this.duration = duration;
        }

        public override void Decorate()
        {
            throw new NotImplementedException();
        }
    }
}
