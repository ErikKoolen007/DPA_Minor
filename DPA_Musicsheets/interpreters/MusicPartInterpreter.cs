using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.domain;

namespace DPA_Musicsheets.interpreters
{
    interface MusicPartInterpreter
    {
        MusicPart Interpret(string musicPartStr);
        MusicPart Delegate(string musicPartStr);
        string Inverseinterpret(MusicPart part);
        string InverseDelegate(MusicPart part);
    }
}
