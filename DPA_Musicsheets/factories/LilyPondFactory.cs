using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.factories
{
    class LilyPondFactory : FileFactory
    {
        private string file_name;

        public LilyPondFactory(string file_name)
        {
            this.file_name = file_name;
        }
        public override void LoadIntoDomain()
        {

        }
    }
}
