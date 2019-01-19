using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.domain;

namespace DPA_Musicsheets.factories
{
    class LilyPondFactory : FileFactory
    {
        private string file_name;
        private LinkedList<MusicPart> musicParts = new LinkedList<MusicPart>();

        public LilyPondFactory(string file_name)
        {
            this.file_name = file_name;
        }
        public override LinkedList<MusicPart> LoadIntoDomain()
        {
            string fileText =  OpenFile();

            while (fileText != "")
            {

            }

            return null;
        }

        private string OpenFile()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var line in File.ReadAllLines(file_name))
            {
                sb.AppendLine(line);
            }
            return sb.ToString();
        }
    }
}
