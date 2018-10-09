using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.domain.Lilypond
{
    public class ConcreteNote : Note
    {

        public ConcreteNote(string letter, string number)
        {
        }
        public override void Decorate()
        {
            throw new NotImplementedException();
        }

        public override string toString()
        {
            return _letter + _number;
        }
    }
}
