﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Shapes;
using DPA_Musicsheets.domain;
using DPA_Musicsheets.Models;
using DPA_Musicsheets.ViewModels;
using PSAMControlLibrary;
using Sanford.Multimedia.Midi;
using Clef = PSAMControlLibrary.Clef;
using ClefType = PSAMControlLibrary.ClefType;
using Note = PSAMControlLibrary.Note;
using Rest = PSAMControlLibrary.Rest;

namespace DPA_Musicsheets.factories
{
    class WPFFactory : FileFactory
    {
        public Sequence MidiSequence { get; set; }
        public List<MusicalSymbol> WPFStaffs { get; set; } = new List<MusicalSymbol>();
        private LinkedList<MusicPart> _tokens;
        private StaffsViewModel _staffsViewModel;
        private MidiPlayerViewModel _midiViewModel;

        private static List<Char> _notesorder = new List<Char> { 'c', 'd', 'e', 'f', 'g', 'a', 'b' };
        private static List<string> _notesOrderWithCrosses = new List<string>() { "c", "cis", "d", "dis", "e", "f", "fis", "g", "gis", "a", "ais", "b" };

        private int _beatNote = 4;    
        private int _bpm = 120;       
        private int _beatsPerBar;

        public WPFFactory(LinkedList<MusicPart> tokens, StaffsViewModel staffsViewModel, MidiPlayerViewModel midiViewModel)
        {
            _tokens = tokens;
            _staffsViewModel = staffsViewModel;
            _midiViewModel = midiViewModel;
        }
        public override LinkedList<MusicPart> Load()
        {
            WPFStaffs.Clear();

            WPFStaffs.AddRange(GetStaffsFromTokens());
            _staffsViewModel.SetStaffs(this.WPFStaffs);

            MidiSequence = GetSequenceFromWPFStaffs();
            _midiViewModel.MidiSequence = MidiSequence;

            //actually not needed
            return _tokens;
        }

        private IEnumerable<MusicalSymbol> GetStaffsFromTokens()
        {
            List<MusicalSymbol> symbols = new List<MusicalSymbol>();

            try
            {
                MusicPartWrapper relative = (MusicPartWrapper) _tokens.First();
                AbstractComposite composite = new RelativeComposite(relative);
                symbols = composite.visit(new List<MusicalSymbol>());
            }
            catch (InvalidCastException ex)
            {
                Console.WriteLine(ex.StackTrace);
            }

            return symbols;

        }
        public Sequence GetSequenceFromWPFStaffs()
        {
            
            int absoluteTicks = 0;

            Sequence sequence = new Sequence();

            Track metaTrack = new Track();
            sequence.Add(metaTrack);

            // Calculate tempo
            int speed = (60000000 / _bpm);
            byte[] tempo = new byte[3];
            tempo[0] = (byte)((speed >> 16) & 0xff);
            tempo[1] = (byte)((speed >> 8) & 0xff);
            tempo[2] = (byte)(speed & 0xff);
            metaTrack.Insert(0 /* Insert at 0 ticks*/, new MetaMessage(MetaType.Tempo, tempo));

            Track notesTrack = new Track();
            sequence.Add(notesTrack);

            for (int i = 0; i < WPFStaffs.Count; i++)
            {
                var musicalSymbol = WPFStaffs[i];
                switch (musicalSymbol.Type)
                {
                    case MusicalSymbolType.Note:
                        Note note = musicalSymbol as Note;

                        // Calculate duration
                        double absoluteLength = 1.0 / (double)note.Duration;
                        absoluteLength += (absoluteLength / 2.0) * note.NumberOfDots;

                        double relationToQuartNote = _beatNote / 4.0;
                        double percentageOfBeatNote = (1.0 / _beatNote) / absoluteLength;
                        double deltaTicks = (sequence.Division / relationToQuartNote) / percentageOfBeatNote;

                        // Calculate height
                        int noteHeight = _notesOrderWithCrosses.IndexOf(note.Step.ToLower()) + ((note.Octave + 1) * 12);
                        noteHeight += note.Alter;
                        notesTrack.Insert(absoluteTicks, new ChannelMessage(ChannelCommand.NoteOn, 1, noteHeight, 90)); // Data2 = volume

                        absoluteTicks += (int)deltaTicks;
                        notesTrack.Insert(absoluteTicks, new ChannelMessage(ChannelCommand.NoteOn, 1, noteHeight, 0)); // Data2 = volume

                        break;
                    case MusicalSymbolType.TimeSignature:
                        byte[] timeSignature = new byte[4];
                        timeSignature[0] = (byte)_beatsPerBar;
                        timeSignature[1] = (byte)(Math.Log(_beatNote) / Math.Log(2));
                        metaTrack.Insert(absoluteTicks, new MetaMessage(MetaType.TimeSignature, timeSignature));
                        break;
                    default:
                        break;
                }
            }

            notesTrack.Insert(absoluteTicks, MetaMessage.EndOfTrackMessage);
            metaTrack.Insert(absoluteTicks, MetaMessage.EndOfTrackMessage);
            return sequence;
        }
    }
}
