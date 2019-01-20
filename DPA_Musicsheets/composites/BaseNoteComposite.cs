using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.domain;

namespace DPA_Musicsheets.composites
{
    class BaseNoteComposite : AbstractComposite 
    {
        private BaseNote note;

        public BaseNoteComposite(BaseNote note)
        {
            this.note = note;
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
