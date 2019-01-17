using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.domain;

namespace DPA_Musicsheets.interpreters
{
    class RelativeInterpreter : MusicPartInterpreter
    {
        public MusicPart Interpret(string musicPartStr)
        {
            int index;
            if (musicPartStr.Contains("\\relative c'"))
            {
                index = musicPartStr.IndexOf("\\relative");
                musicPartStr.Remove(0, index);
                musicPartStr.Remove(index, 14);
                musicPartStr.Remove(musicPartStr.Length - 1);
                MusicPartWrapper wrapper = new MusicPartWrapper(null, WrapperType.Relative);
                return wrapper;
            }
            throw new EntryPointNotFoundException();
        }

        public MusicPart Delegate(string musicPartStr)
        {
            throw new NotImplementedException();
        }

        public string Inverseinterpret(MusicPart part)
        {
            throw new NotImplementedException();
        }

        public string InverseDelegate(MusicPart part)
        {
            throw new NotImplementedException();
        }
    }
}
