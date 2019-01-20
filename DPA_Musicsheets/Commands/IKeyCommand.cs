using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.ViewModels;

namespace DPA_Musicsheets.Commands
{
    public interface IKeyCommand
    {
        void Execute(ViewModelLocator viewModelLocator);
    }
}
