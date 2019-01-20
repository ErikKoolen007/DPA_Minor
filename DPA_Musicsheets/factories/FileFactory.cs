using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.domain;

namespace DPA_Musicsheets.factories
{
    public abstract class FileFactory
    {
        protected LinkedList<MusicPart> content { get; set; } = new LinkedList<MusicPart>();
        public abstract LinkedList<MusicPart> Load();
    }
}
