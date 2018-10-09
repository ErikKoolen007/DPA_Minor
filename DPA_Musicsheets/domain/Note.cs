using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DPA_Musicsheets.domain.Lilypond
{
    public abstract class Note
    {
        public string Letter { get; set; }
        public string Number { get; set; }
        public abstract void Decorate();

        public abstract string toString();
    }
}
