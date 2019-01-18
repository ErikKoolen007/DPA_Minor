using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.domain
{
    public enum WrapperType
    {
        Relative = 0,
        Repeat = 1,
        Alternative = 2
    }
    class MusicPartWrapper : MusicPart
    {
        private WrapperType type;
        public LinkedList<MusicPart> symbols { get; set; }
        public MusicPartWrapper(LinkedList<MusicPart> symbols, WrapperType type)
        {
            this.type = type;
            this.symbols = symbols;
        }

        public override string ToString()
        {
            StringBuilder listContent = new StringBuilder();

            foreach (MusicPart part in symbols)
            {
                listContent.Append(part.ToString());
            }

            switch (type)
            {
                case WrapperType.Alternative:
                    return "\\alternative { \n" + listContent.ToString() + "\n }";
                case WrapperType.Repeat:
                    return "\\repeat volta 2 { \n" + listContent.ToString() + "\n }";
                default:
                    return "\\relative c' { \n" + listContent.ToString() + "\n | }";
            }
        }

        public void WrapAlternative(BaseNote start, BaseNote end)
        {
            if (type != WrapperType.Alternative)
                return;

            start.letter = "{ " + start.letter;
            end.duration = end.duration + " }";
        }
    }
}
