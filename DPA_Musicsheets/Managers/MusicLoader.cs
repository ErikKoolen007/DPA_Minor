
using DPA_Musicsheets.Models;
using DPA_Musicsheets.ViewModels;
using PSAMControlLibrary;
using Sanford.Multimedia.Midi;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DPA_Musicsheets.domain;
using DPA_Musicsheets.factories;
using Clef = PSAMControlLibrary.Clef;
using ClefType = PSAMControlLibrary.ClefType;
using Note = PSAMControlLibrary.Note;

namespace DPA_Musicsheets.Managers
{
    public class MusicLoader
    {
        #region Properties
        public string DomainText { get; set; }
        

        public Sequence MidiSequence { get; set; }
        #endregion Properties

        private int _beatNote = 4;    // De waarde van een beatnote.
        private int _bpm = 120;       // Aantal beatnotes per minute.
        private int _beatsPerBar;     // Aantal beatnotes per maat.

        public MainViewModel MainViewModel { get; set; }
        public LilypondViewModel LilypondViewModel { get; set; }
        public MidiPlayerViewModel MidiPlayerViewModel { get; set; }
        public StaffsViewModel StaffsViewModel { get; set; }

        public FileFactory factory { get; set; }

        public void OpenFile(string fileName)
        {
            LinkedList<MusicPart> domain;
            //just moved
            if (Path.GetExtension(fileName).EndsWith(".mid"))
            {

                MidiSequence = new Sequence();
                MidiSequence.Load(fileName);

                MidiPlayerViewModel.MidiSequence = MidiSequence;
                factory = new MidiFactory(fileName);
                domain = factory.Load();

                StringBuilder sb = new StringBuilder();
                foreach (var e in domain)
                {
                    sb.Append(e);
                    DomainText = sb.ToString();
                }
            }
            else if (Path.GetExtension(fileName).EndsWith(".ly"))
            {
                factory = new LilyPondFactory(fileName);
                domain = factory.Load();
                StringBuilder sb = new StringBuilder();
                foreach (var e in domain)
                {
                    sb.Append(e.ToString());
                }
                DomainText= sb.ToString();
            }
            else
            {
                throw new NotSupportedException($"File extension {Path.GetExtension(fileName)} is not supported.");
            }
            this.LilypondViewModel.LilypondTextLoaded(this.DomainText);

            factory = new WPFFactory(domain, StaffsViewModel, MidiPlayerViewModel);
            factory.Load();

            //LoadLilypondIntoWpfStaffsAndMidi(DomainText);
        }

        #region Midi loading (loads midi to lilypond)

//        public string LoadMidiIntoLilypond(Sequence sequence)
//        {
//            StringBuilder lilypondContent = new StringBuilder();
//            lilypondContent.AppendLine("\\relative c' {");
//            lilypondContent.AppendLine("\\clef treble");
//
//            int division = sequence.Division;
//            int previousMidiKey = 60; // Central C;
//            int previousNoteAbsoluteTicks = 0;
//            double percentageOfBarReached = 0;
//            bool startedNoteIsClosed = true;
//
//            for (int i = 0; i < sequence.Count(); i++)
//            {
//                Track track = sequence[i];
//
//                foreach (var midiEvent in track.Iterator())
//                {
//                    IMidiMessage midiMessage = midiEvent.MidiMessage;
//                    switch (midiMessage.MessageType)
//                    {
//                        case MessageType.Meta:
//                            var metaMessage = midiMessage as MetaMessage;
//                            switch (metaMessage.MetaType)
//                            {
//                                case MetaType.TimeSignature:
//                                    byte[] timeSignatureBytes = metaMessage.GetBytes();
//                                    _beatNote = timeSignatureBytes[0];
//                                    _beatsPerBar = (int)(1 / Math.Pow(timeSignatureBytes[1], -2));
//                                    lilypondContent.AppendLine($"\\time {_beatNote}/{_beatsPerBar}");
//                                    break;
//                                case MetaType.Tempo:
//                                    byte[] tempoBytes = metaMessage.GetBytes();
//                                    int tempo = (tempoBytes[0] & 0xff) << 16 | (tempoBytes[1] & 0xff) << 8 | (tempoBytes[2] & 0xff);
//                                    _bpm = 60000000 / tempo;
//                                    lilypondContent.AppendLine($"\\tempo 4={_bpm}");
//                                    break;
//                                //Wordt het onderstaande überhaupt ergens voor gebruikt?
//                                case MetaType.EndOfTrack:
//                                    if (previousNoteAbsoluteTicks > 0)
//                                    {
//                                        // Finish the last notelength.
//                                        double percentageOfBar;
//                                        lilypondContent.Append(MidiToLilyHelper.GetLilypondNoteLength(previousNoteAbsoluteTicks, midiEvent.AbsoluteTicks, division, _beatNote, _beatsPerBar, out percentageOfBar));
//                                        lilypondContent.Append(" ");
//
//                                        percentageOfBarReached += percentageOfBar;
//                                        if (percentageOfBarReached >= 1)
//                                        {
//                                            lilypondContent.AppendLine("|");
//                                            percentageOfBar = percentageOfBar - 1;
//                                        }
//                                    }
//                                    break;
//                                default: break;
//                            }
//                            break;
//                        case MessageType.Channel:
//                            var channelMessage = midiEvent.MidiMessage as ChannelMessage;
//                            if (channelMessage.Command == ChannelCommand.NoteOn)
//                            {
//                                if(channelMessage.Data2 > 0) // Data2 = loudness
//                                {
//                                    // Append the new baseNote.
//                                    lilypondContent.Append(MidiToLilyHelper.GetLilyNoteName(previousMidiKey, channelMessage.Data1));
//                                    
//                                    previousMidiKey = channelMessage.Data1;
//                                    startedNoteIsClosed = false;
//                                }
//                                else if (!startedNoteIsClosed)
//                                {
//                                    // Finish the previous baseNote with the length.
//                                    double percentageOfBar;
//                                    lilypondContent.Append(MidiToLilyHelper.GetLilypondNoteLength(previousNoteAbsoluteTicks, midiEvent.AbsoluteTicks, division, _beatNote, _beatsPerBar, out percentageOfBar));
//                                    previousNoteAbsoluteTicks = midiEvent.AbsoluteTicks;
//                                    lilypondContent.Append(" ");
//
//                                    percentageOfBarReached += percentageOfBar;
//                                    if (percentageOfBarReached >= 1)
//                                    {
//                                        lilypondContent.AppendLine("|");
//                                        percentageOfBarReached -= 1;
//                                    }
//                                    startedNoteIsClosed = true;
//                                }
//                                else
//                                {
//                                    lilypondContent.Append("r");
//                                }
//                            }
//                            break;
//                    }
//                }
//            }
//
//            lilypondContent.Append("}");
//
//            return lilypondContent.ToString();
//        }

        #endregion Midiloading (loads midi to lilypond)

        #region Staffs loading (loads lilypond to WPF staffs)
//        private static IEnumerable<MusicalSymbol> GetStaffsFromTokens(LinkedList<LilypondToken> tokens)
//        {
//            List<MusicalSymbol> symbols = new List<MusicalSymbol>();
//
//            Clef currentClef = null;
//            int previousOctave = 4;
//            char previousNote = 'c';
//            bool inRepeat = false;
//            bool inAlternative = false;
//            int alternativeRepeatNumber = 0;
//
//            LilypondToken currentToken = tokens.First();
//            while (currentToken != null)
//            {
//                switch (currentToken.TokenKind)
//                {
//                    case LilypondTokenKind.Unknown:
//                        break;
//                    case LilypondTokenKind.Repeat:
//                        inRepeat = true;
//                        symbols.Add(new Barline() { RepeatSign = RepeatSignType.Forward });
//                        break;
//                    case LilypondTokenKind.SectionEnd:
//                        if (inRepeat && currentToken.NextToken?.TokenKind != LilypondTokenKind.Alternative)
//                        {
//                            inRepeat = false;
//                            symbols.Add(new Barline() { RepeatSign = RepeatSignType.Backward, AlternateRepeatGroup = alternativeRepeatNumber });
//                        }
//                        else if (inAlternative && alternativeRepeatNumber == 1)
//                        {
//                            alternativeRepeatNumber++;
//                            symbols.Add(new Barline() { RepeatSign = RepeatSignType.Backward, AlternateRepeatGroup = alternativeRepeatNumber });
//                        }
//                        else if (inAlternative && currentToken.NextToken.TokenKind == LilypondTokenKind.SectionEnd)
//                        {
//                            inAlternative = false;
//                            alternativeRepeatNumber = 0;
//                        }
//                        break;
//                    case LilypondTokenKind.SectionStart:
//                        if (inAlternative && currentToken.PreviousToken.TokenKind != LilypondTokenKind.SectionEnd)
//                        {
//                            alternativeRepeatNumber++;
//                            symbols.Add(new Barline() { AlternateRepeatGroup = alternativeRepeatNumber });
//                        }
//                        break;
//                    case LilypondTokenKind.Alternative:
//                        inAlternative = true;
//                        inRepeat = false;
//                        currentToken = currentToken.NextToken; // Skip the first bracket open.
//                        break;
//                    case LilypondTokenKind.Note:
//                        NoteTieType tie = NoteTieType.None;
//                        if (currentToken.Value.StartsWith("~"))
//                        {
//                            tie = NoteTieType.Stop;
//                            var lastNote = symbols.Last(s => s is Note) as Note;
//                            if (lastNote != null) lastNote.TieType = NoteTieType.Start;
//                            currentToken.Value = currentToken.Value.Substring(1);
//                        }
//                        // Length
//                        int noteLength = Int32.Parse(Regex.Match(currentToken.Value, @"\d+").Value);
//                        // Crosses and Moles
//                        int alter = 0;
//                        alter += Regex.Matches(currentToken.Value, "is").Count;
//                        alter -= Regex.Matches(currentToken.Value, "es|as").Count;
//                        // Octaves
//                        int distanceWithPreviousNote = notesorder.IndexOf(currentToken.Value[0]) - notesorder.IndexOf(previousNote);
//                        if (distanceWithPreviousNote > 3) // Shorter path possible the other way around
//                        {
//                            distanceWithPreviousNote -= 7; // The number of notes in an octave
//                        }
//                        else if (distanceWithPreviousNote < -3)
//                        {
//                            distanceWithPreviousNote += 7; // The number of notes in an octave
//                        }
//
//                        if (distanceWithPreviousNote + notesorder.IndexOf(previousNote) >= 7)
//                        {
//                            previousOctave++;
//                        }
//                        else if (distanceWithPreviousNote + notesorder.IndexOf(previousNote) < 0)
//                        {
//                            previousOctave--;
//                        }
//
//                        // Force up or down.
//                        previousOctave += currentToken.Value.Count(c => c == '\'');
//                        previousOctave -= currentToken.Value.Count(c => c == ',');
//
//                        previousNote = currentToken.Value[0];
//
//                        var note = new Note(currentToken.Value[0].ToString().ToUpper(), alter, previousOctave, (MusicalSymbolDuration)noteLength, NoteStemDirection.Up, tie, new List<NoteBeamType>() { NoteBeamType.Single });
//                        note.NumberOfDots += currentToken.Value.Count(c => c.Equals('.'));
//                        
//                        symbols.Add(note);
//                        break;
//                    case LilypondTokenKind.Rest:
//                        var restLength = Int32.Parse(currentToken.Value[1].ToString());
//                        //_symbols.Add(new Rest((MusicalSymbolDuration)restLength));
//                        break;
//                    case LilypondTokenKind.Bar:
//                        symbols.Add(new Barline() { AlternateRepeatGroup = alternativeRepeatNumber });
//                        break;
//                    case LilypondTokenKind.Clef:
//                        currentToken = currentToken.NextToken;
//                        if (currentToken.Value == "treble")
//                            currentClef = new Clef(ClefType.GClef, 2);
//                        else if (currentToken.Value == "bass")
//                            currentClef = new Clef(ClefType.FClef, 4);
//                        else if (currentToken.Value == "soprano")
//                            currentClef = new Clef(ClefType.CClef, 3);
//                        else
//                            throw new NotSupportedException($"Clef {currentToken.Value} is not supported.");
//
//                        symbols.Add(currentClef);
//                        break;
//                    case LilypondTokenKind.Time:
//                        currentToken = currentToken.NextToken;
//                        var times = currentToken.Value.Split('/');
//                        symbols.Add(new TimeSignature(TimeSignatureType.Numbers, UInt32.Parse(times[0]), UInt32.Parse(times[1])));
//                        break;
//                    case LilypondTokenKind.Tempo:
//                        // Tempo not supported
//                        break;
//                    default:
//                        break;
//                }
//                currentToken = currentToken.NextToken;
//            }
//
//            return symbols;
//        }
        
//        private static LinkedList<LilypondToken> GetTokensFromLilypond(string content)
//        {
//            var tokens = new LinkedList<LilypondToken>();
//
//            foreach (string s in content.Split(' ').Where(item => item.Length > 0))
//            {
//                LilypondToken token = new LilypondToken()
//                {
//                    Value = s
//                };
//
//                switch (s)
//                {
//                    case "\\relative": token.TokenKind = LilypondTokenKind.Staff; break;
//                    case "\\clef": token.TokenKind = LilypondTokenKind.Clef; break;
//                    case "\\time": token.TokenKind = LilypondTokenKind.Time; break;
//                    case "\\tempo": token.TokenKind = LilypondTokenKind.Tempo; break;
//                    case "\\repeat": token.TokenKind = LilypondTokenKind.Repeat; break;
//                    case "\\alternative": token.TokenKind = LilypondTokenKind.Alternative; break;
//                    case "{": token.TokenKind = LilypondTokenKind.SectionStart; break;
//                    case "}": token.TokenKind = LilypondTokenKind.SectionEnd; break;
//                    case "|": token.TokenKind = LilypondTokenKind.Bar; break;
//                    default: token.TokenKind = LilypondTokenKind.Unknown; break;
//                }
//
//                if (token.TokenKind == LilypondTokenKind.Unknown && new Regex(@"[~]?[a-g][,'eis]*[0-9]+[.]*").IsMatch(s))
//                {
//                    token.TokenKind = LilypondTokenKind.Note;
//                }
//                else if (token.TokenKind == LilypondTokenKind.Unknown && new Regex(@"r.*?[0-9][.]*").IsMatch(s))
//                {
//                    token.TokenKind = LilypondTokenKind.Rest;
//                }
//
//                if (tokens.Last != null)
//                {
//                    tokens.Last.Value.NextToken = token;
//                    token.PreviousToken = tokens.Last.Value;
//                }
//
//                tokens.AddLast(token);
//            }
//
//            return tokens;
//        }
        #endregion Staffs loading (loads lilypond to WPF staffs)

        #region Saving to files
        internal void SaveToMidi(string fileName)
        {
            if(factory.GetType() == typeof(WPFFactory))
            {
                WPFFactory f = (WPFFactory) factory;
                Sequence sequence = f.GetSequenceFromWPFStaffs();
                sequence.Save(fileName);
            }
        }

        internal void SaveToPDF(string fileName)
        {
            string withoutExtension = Path.GetFileNameWithoutExtension(fileName);
            string tmpFileName = $"{fileName}-tmp.ly";
            SaveToLilypond(tmpFileName);

            string lilypondLocation = @"C:\Program Files (x86)\LilyPond\usr\bin\lilypond.exe";
            string sourceFolder = Path.GetDirectoryName(tmpFileName);
            string sourceFileName = Path.GetFileNameWithoutExtension(tmpFileName);
            string targetFolder = Path.GetDirectoryName(fileName);
            string targetFileName = Path.GetFileNameWithoutExtension(fileName);

            var process = new Process
            {
                StartInfo =
                {
                    WorkingDirectory = sourceFolder,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    Arguments = String.Format("--pdf \"{0}\\{1}.ly\"", sourceFolder, sourceFileName),
                    FileName = lilypondLocation
                }
            };

            process.Start();
            while (!process.HasExited) { /* Wait for exit */
                }
                if (sourceFolder != targetFolder || sourceFileName != targetFileName)
            {
                File.Move(sourceFolder + "\\" + sourceFileName + ".pdf", targetFolder + "\\" + targetFileName + ".pdf");
                File.Delete(tmpFileName);
            }
        }

        internal void SaveToLilypond(string fileName)
        {
            using (StreamWriter outputFile = new StreamWriter(fileName))
            {
                outputFile.Write(DomainText);
                outputFile.Close();
            }
        }
        #endregion Saving to files
    }
}
