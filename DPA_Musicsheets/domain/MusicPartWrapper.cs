using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PSAMControlLibrary;

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
        public WrapperType _type { get; }
        public LinkedList<MusicPart> _symbols { get; set; }

        public MusicPartWrapper(LinkedList<MusicPart> symbols, WrapperType type)
        {
            _type = type;
            _symbols = symbols;
        }

        public override string ToString()
        {
            StringBuilder listContent = new StringBuilder();

            foreach (MusicPart part in _symbols)
            {
                listContent.Append(part.ToString());
            }

            switch (_type)
            {
                case WrapperType.Alternative:
                    return "\\alternative { \r\n" + listContent.ToString() + "\r\n}";
                case WrapperType.Repeat:
                    return "\\repeat volta 2 { \r\n" + listContent.ToString() + "\r\n }";
                default:
                    return "\\relative c' {  \r\n" + listContent.ToString() + "\r\n | }";
            }
        }

        public MusicPart WrapCurlyBraces(MusicPart part, bool open)
        {
            if (part.GetType() == typeof(Rest))
            {
                Rest r;
                r = (Rest)part;

                if (open)
                    r.letter = "{ " + r.letter;
                else
                    r.duration = r.duration + " } ";
                return r;
            }

            if (part.GetType().BaseType == typeof(BaseNote))
            {
                BaseNote n;
                n = (BaseNote) part;

                if (open)
                    n.letter = "{ " + n.letter;
                else
                    n.duration = n.duration + " } ";
                return n;
            }

            return part;
        }
    }
}
