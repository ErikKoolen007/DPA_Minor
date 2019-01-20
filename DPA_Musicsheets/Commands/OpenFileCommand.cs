using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DPA_Musicsheets.ViewModels;

namespace DPA_Musicsheets.Commands
{
    public class OpenFileCommand : IKeyCommand
    {
        public void Execute(ViewModelLocator viewModelLocator)
        {
            viewModelLocator.MainViewModel.OpenFileCommand.Execute(null);
        }
    }
}
