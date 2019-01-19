using DPA_Musicsheets.Managers;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DPA_Musicsheets.Memento;

namespace DPA_Musicsheets.ViewModels
{
    public class LilypondViewModel : ViewModelBase
    {
        private MusicLoader _musicLoader;
        private MainViewModel _mainViewModel { get; set; }
        private CareTaker _careTaker;

        private string _text;

        /// <summary>
        /// This text will be in the textbox.
        /// It can be filled either by typing or loading a file so we only want to set previoustext when it's caused by typing.
        /// </summary>
        public string LilypondText
        {
            get => _text;
            set
            {
                _text = value;
                RaisePropertyChanged(() => LilypondText);
            }
        }

        private bool _textChangedByLoad = false;
        private bool _textChangedByMemento = false;
        private DateTime _lastChange;
        private static int MILLISECONDS_BEFORE_CHANGE_HANDLED = 1500;

        public LilypondViewModel(MainViewModel mainViewModel, MusicLoader musicLoader)
        {
            // TODO: Can we use some sort of eventing system so the managers layer doesn't have to know the viewmodel layer and viewmodels don't know each other?
            // And viewmodels don't 
            _mainViewModel = mainViewModel;
            _musicLoader = musicLoader;
            _musicLoader.LilypondViewModel = this;
            _careTaker = new CareTaker();
            
            _text = "Your lilypond text will appear here.";
        }

        public void LilypondTextLoaded(string text)
        {
            _textChangedByLoad = true;

            _careTaker.Clear(); // can't go back to bookmark from a previous file
            _careTaker.Save(new EditorMemento(text));

            LilypondText = text;
            _textChangedByLoad = false;
        }

        /// <summary>
        /// This occurs when the text in the textbox has changed. This can either be by loading or typing.
        /// </summary>
        public ICommand TextChangedCommand => new RelayCommand<TextChangedEventArgs>((args) =>
        {
            // If we were typing, we need to do things.
            if (!_textChangedByLoad)
            {
                _mainViewModel.FileSaved = false;
                _lastChange = DateTime.Now;

                _mainViewModel.CurrentState = "Rendering...";

                Task.Delay(MILLISECONDS_BEFORE_CHANGE_HANDLED).ContinueWith((task) =>
                {
                    if ((DateTime.Now - _lastChange).TotalMilliseconds >= MILLISECONDS_BEFORE_CHANGE_HANDLED)
                    {
                        if(!_textChangedByMemento)
                            _careTaker.Save(new EditorMemento(LilypondText));
                        _textChangedByMemento = false;

                        _musicLoader.LoadLilypondIntoWpfStaffsAndMidi(LilypondText);
                        _mainViewModel.CurrentState = "";
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext()); // Request from main thread.
            }
        });

        #region Commands for buttons like Undo, Redo and SaveAs
        public RelayCommand UndoCommand => new RelayCommand(() =>
        {
            _textChangedByMemento = true;
            LilypondText = _careTaker.Undo().EditorText;
        }, () => _careTaker.CanUndo());

        public RelayCommand RedoCommand => new RelayCommand(() =>
        {
            _textChangedByMemento = true;
            LilypondText = _careTaker.Redo().EditorText;
        }, () => _careTaker.CanRedo());

        public ICommand SaveAsCommand => new RelayCommand(() =>
        {
            // TODO: In the application a lot of classes know which filetypes are supported. Lots and lots of repeated code here...
            // Can this be done better?
            SaveFileDialog saveFileDialog = new SaveFileDialog() { Filter = "Midi|*.mid|Lilypond|*.ly|PDF|*.pdf" };
            if (saveFileDialog.ShowDialog() == true)
            {
                string extension = Path.GetExtension(saveFileDialog.FileName);
                if (extension.EndsWith(".mid"))
                {
                    _musicLoader.SaveToMidi(saveFileDialog.FileName);
                    _mainViewModel.FileSaved = true;
                }
                else if (extension.EndsWith(".ly"))
                {
                    _musicLoader.SaveToLilypond(saveFileDialog.FileName);
                    _mainViewModel.FileSaved = true;
                }
                else if (extension.EndsWith(".pdf"))
                {
                    _musicLoader.SaveToPDF(saveFileDialog.FileName);
                    _mainViewModel.FileSaved = true;
                }
                else
                {
                    MessageBox.Show($"Extension {extension} is not supported.");
                }
            }
        });
        #endregion Commands for buttons like Undo, Redo and SaveAs
    }
}
