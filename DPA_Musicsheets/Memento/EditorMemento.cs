using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Memento
{
    class EditorMemento
    {
        public string EditorText { get; set; }

        public EditorMemento(string text)
        {
            EditorText = text;
        }
    }
}
