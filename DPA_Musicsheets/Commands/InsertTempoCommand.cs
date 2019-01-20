using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DPA_Musicsheets.ViewModels;

namespace DPA_Musicsheets.Commands
{
    public class InsertTempoCommand : IKeyCommand
    {
        public void Execute(ViewModelLocator viewModelLocator)
        {
            int caretIndex = viewModelLocator.LilypondViewModel.CaretIndex;
            viewModelLocator.LilypondViewModel.LilypondText =
                viewModelLocator.LilypondViewModel.LilypondText.Insert(caretIndex, "\\tempo 4=120");
        }
    }
}
