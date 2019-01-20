using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DPA_Musicsheets.ViewModels;
using Microsoft.Win32;

namespace DPA_Musicsheets.Commands
{
    public class SavePdfCommand : IKeyCommand
    {
        public void Execute(ViewModelLocator viewModelLocator)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog() { Filter = "PDF|*.pdf" };
            if (saveFileDialog.ShowDialog() == true)
            {
                viewModelLocator.MainViewModel.MusicLoader.SaveToPDF(saveFileDialog.FileName);
            }

            viewModelLocator.MainViewModel.FileSaved = true;
        }
    }
}
