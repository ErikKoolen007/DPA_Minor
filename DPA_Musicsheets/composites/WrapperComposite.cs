using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.domain;
using PSAMControlLibrary;

namespace DPA_Musicsheets.composites
{
    class WrapperComposite : AbstractComposite
    {
        private MusicPartWrapper wrapper;
        private LinkedList<MusicPart> children;
        public WrapperComposite(MusicPartWrapper wrapper)
        {
            this.wrapper = wrapper;
            children = wrapper._symbols;
        }

        public List<MusicalSymbol> visit(List<MusicalSymbol> symbols)
        {
            if (wrapper._type == WrapperType.Repeat)
            {
                symbols.Add(new Barline() { RepeatSign = RepeatSignType.Forward });
            }
            next(symbols);
            return symbols;
        }

        public void next(List<MusicalSymbol> symbols)
        {
            foreach (var part in children)
            {

            }
        }
    }
}
