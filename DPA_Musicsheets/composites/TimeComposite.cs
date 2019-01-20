using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.domain;

namespace DPA_Musicsheets.composites
{
    class TimeComposite : AbstractComposite
    {
        private Time time;

        public TimeComposite(Time time)
        {
            this.time = time;
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
