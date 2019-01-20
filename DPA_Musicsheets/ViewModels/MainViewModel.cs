using DPA_Musicsheets.Managers;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using PSAMWPFControlLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using DPA_Musicsheets.Hotkeys;

namespace DPA_Musicsheets.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private string _fileName;
        public string FileName
        {
            get
            {
                return _fileName;
            }
            set
            {
                _fileName = value;
                RaisePropertyChanged(() => FileName);
            }
        }

        public bool FileSaved { get; set; } = true;

        /// <summary>
        /// The current state can be used to display some text.
        /// "Rendering..." is a text that will be displayed for example.
        /// </summary>
        private string _currentState;
        public string CurrentState
        {
            get { return _currentState; }
            set { _currentState = value; RaisePropertyChanged(() => CurrentState); }
        }

        public MusicLoader MusicLoader;
        private HotkeyChain _hotkeyChain;
        private Dictionary<Key, bool> _keysDown;

        public MainViewModel(MusicLoader musicLoader)
        {
            // TODO: Can we use some sort of eventing system so the managers layer doesn't have to know the viewmodel layer?
            MusicLoader = musicLoader;
            _hotkeyChain = new HotkeyChain();
            _keysDown = new Dictionary<Key, bool>();
            FileName = @"Files/Alle-eendjes-zwemmen-in-het-water.mid";
        }

        public ICommand OpenFileCommand => new RelayCommand(() =>
        {
            OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = "Midi or LilyPond files (*.mid *.ly)|*.mid;*.ly" };
            if (openFileDialog.ShowDialog() == true)
            {
                FileName = openFileDialog.FileName;
            }
        });

        public ICommand LoadCommand => new RelayCommand(() =>
        {
            MusicLoader.OpenFile(FileName);
        });

        #region Focus and key commands, these can be used for implementing hotkeys
        public ICommand OnLostFocusCommand => new RelayCommand(() =>
        {
            Console.WriteLine("Maingrid Lost focus");
        });

        public ICommand OnKeyDownCommand => new RelayCommand<KeyEventArgs>((e) =>
        {
            if(!_keysDown.ContainsKey(e.Key) && !e.IsRepeat)
                _keysDown.Add(e.Key, false);

            if (_hotkeyChain.Handle(_keysDown))
            {
                e.Handled = true;
                _keysDown.Clear();
            }

            Console.WriteLine($"Key down: {e.Key}");
        });

        public ICommand OnKeyUpCommand => new RelayCommand(() =>
        {
            Console.WriteLine("Key Up");
        });

        public ICommand OnWindowClosingCommand => new RelayCommand<CancelEventArgs>(args =>
        {
            if (!FileSaved)
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show(
                    "The data has changed. Exit without saving?", "Confirm", System.Windows.MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    ViewModelLocator.Cleanup();
                }
                else
                {
                    args.Cancel = true;
                }
            }
            else
            {
                ViewModelLocator.Cleanup();
            }
        });
        #endregion Focus and key commands, these can be used for implementing hotkeys
    }
}
