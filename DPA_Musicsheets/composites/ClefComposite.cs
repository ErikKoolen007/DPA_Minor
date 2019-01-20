using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.domain;

namespace DPA_Musicsheets.composites
{
    class ClefComposite : AbstractComposite
    {
        private Clef clef;

        public ClefComposite(Clef clef)
        {
            this.clef = clef;
        }

        public void visit()
        {
            throw new NotImplementedException();
        }

        public void next()
        {
            throw new NotImplementedException();
        }
    }
}
