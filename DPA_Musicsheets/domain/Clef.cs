using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.domain
{
    public enum ClefType
    {
        Gclef = 0,
        Fclef = 1,
        Cclef = 2
    }
    class Clef : MusicPart
    {
        private int lineNumber { get; set; }
        private ClefType ClefType { get; set; }


        public Clef(int lineNumber, ClefType clefType)
        {
            this.lineNumber = lineNumber;
            this.ClefType = clefType;
        }

        public override string ToString()
        {
            switch (ClefType)
            {
                case ClefType.Cclef:
                    return "\\clef soprano ";
                case ClefType.Fclef:
                    return "\\clef bass ";
                default:
                    return "\\clef treble ";
            }
        }
    }
}
