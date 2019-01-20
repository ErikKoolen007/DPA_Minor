using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.domain;

namespace DPA_Musicsheets.composites
{
    class TempoComposite : AbstractComposite
    {
        private Tempo tempo;

        public TempoComposite(Tempo tempo)
        {
            this.tempo = tempo;
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
