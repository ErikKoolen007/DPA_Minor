using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.domain;

namespace DPA_Musicsheets.composites
{
    class WrapperComposite : AbstractComposite
    {
        private MusicPartWrapper wrapper;

        public WrapperComposite(MusicPartWrapper wrapper)
        {
            this.wrapper = wrapper;
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
