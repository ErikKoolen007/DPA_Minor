using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.adapters;
using DPA_Musicsheets.domain;
using DPA_Musicsheets.Managers;
using Sanford.Multimedia.Midi;

namespace DPA_Musicsheets.factories
{
    class MidiFactory : FileFactory
    {
        private string file_name;
        private Sequence seq;

        double _percentageOfBar;
        private bool _startedNoteIsClosed = true;
        private int _previousMidiKey = 60; // Central C;
        private int _previousNoteAbsoluteTicks = 0;
        private double _percentageOfBarReached = 0;
        public int _beatNote { get; set; }
        public int _beatsPerBar { get; set; }
        public MidiFactory(string file_name)
        {
            this.file_name = file_name;
        }
        public override void LoadIntoDomain()
        {
            open_file();
            
        }

        private void open_file()
        {
            seq = new Sequence();
            seq.Load(file_name);
        }

        private MusicPart LoadChannelMsg(ChannelMessage msg, MidiEvent e)
        {
            BaseNote note = new Note("", "");
            int division = seq.Division;

            if (msg.Command == ChannelCommand.NoteOn)
            {
                if (msg.Data2 > 0) // Data2 = loudness
                {
                    // Append the new baseNote.
                    //note.letter = MidiToLilyHelper.GetLilyNoteName(_previousMidiKey, msg.Data1);
                    note = TranslateMidiName(msg.Data1 % 12, note);

                    _previousMidiKey = msg.Data1;
                    _startedNoteIsClosed = false;
                }
                else if (!_startedNoteIsClosed)
                {
                    // Finish the previous baseNote with the length.
                    
                    //note.duration = MidiToLilyHelper.GetLilypondNoteLength(_previousNoteAbsoluteTicks, e.AbsoluteTicks, division, _beatNote, _beatsPerBar, file_namepercentageOfBar);
                    note.duration = "" + TranslateMidiDuration(e.AbsoluteTicks, division, note);
                    _previousNoteAbsoluteTicks = e.AbsoluteTicks;
                    
                    BaseNoteSpace noteSpace = new BaseNoteSpace(note);
                    note = noteSpace;
                   // lilypondContent.Append(" ");

                    _percentageOfBarReached += _percentageOfBar;
                    if (_percentageOfBarReached >= 1)
                    {
                        BaseNoteMark noteMark = new BaseNoteMark(note);
                        note = noteMark;
                        //lilypondContent.AppendLine("|");
                        _percentageOfBarReached -= 1;
                    }
                    _startedNoteIsClosed = true;
                }
                else
                {
                    //lilypondContent.Append("r");
                    Rest r = new Rest();
                }
            }

            return null;
        }

        private MusicPart LoadMetaMsg(MetaMessage msg)
        {
            MetaTypeAdapter adapter;
            switch (msg.MetaType)
            {
                case MetaType.TimeSignature:
                    adapter = new TimeSignatureAdaptee(msg, this);
                    return adapter.LoadIntoDomain();
                case MetaType.Tempo:
                    adapter = new TempoAdaptee(msg);
                    return adapter.LoadIntoDomain();
                //perhaps add end of track?
            }

            return null;
        }

        private BaseNote TranslateMidiName(int midiKey, BaseNote note)
        {
            BaseNoteCross crossNote = null;

            switch (midiKey % 12)
            {
                case 0:
                    note.letter = "c";
                    break;
                case 1:
                    note.letter = "c";
                    crossNote = new BaseNoteCross(note);
                    note = crossNote;
                    break;
                case 2:
                    note.letter = "d";
                    break;
                case 3:
                    note.letter = "d";
                    crossNote = new BaseNoteCross(note);
                    note = crossNote;
                    break;
                case 4:
                    note.letter = "e";
                    break;
                case 5:
                    note.letter = "f";
                    break;
                case 6:
                    note.letter = "f";
                    crossNote = new BaseNoteCross(note);
                    note = crossNote;
                    break;
                case 7:
                    note.letter = "g";
                    break;
                case 8:
                    note.letter = "g";
                    crossNote = new BaseNoteCross(note);
                    note = crossNote;
                    break;
                case 9:
                    note.letter = "a";
                    break;
                case 10:
                    note.letter = "a";
                    crossNote = new BaseNoteCross(note);
                    note = crossNote;
                    break;
                case 11:
                    note.letter = "b";
                    break;
            }

            return note;
        }

        private int TranslateMidiDuration(int absoluteTicks, int division, BaseNote note)
        {
            int duration = 0;
            int dots = 0;

            //is this correct?
            double deltaTicks = _previousNoteAbsoluteTicks - absoluteTicks;

            if (deltaTicks <= 0)
            {
                _percentageOfBar = 0;
                return 0;
            }

            double percentageOfBeatNote = deltaTicks / division;
            _percentageOfBar = (1.0 / _beatsPerBar) * percentageOfBeatNote;

            for (int noteLength = 32; noteLength >= 1; noteLength -= 1)
            {
                double absoluteNoteLength = (1.0 / noteLength);

                if (_percentageOfBar <= absoluteNoteLength)
                {
                    if (noteLength < 2)
                        noteLength = 2;

                    int subtractDuration;

                    if (noteLength == 32)
                        subtractDuration = 32;
                    else if (noteLength >= 16)
                        subtractDuration = 16;
                    else if (noteLength >= 8)
                        subtractDuration = 8;
                    else if (noteLength >= 4)
                        subtractDuration = 4;
                    else
                        subtractDuration = 2;

                    if (noteLength >= 17)
                        duration = 32;
                    else if (noteLength >= 9)
                        duration = 16;
                    else if (noteLength >= 5)
                        duration = 8;
                    else if (noteLength >= 3)
                        duration = 4;
                    else
                        duration = 2;

                    double currentTime = 0;

                    while (currentTime < (noteLength - subtractDuration))
                    {
                        var addtime = 1 / ((subtractDuration / _beatNote) * Math.Pow(2, dots));
                        if (addtime <= 0) break;
                        currentTime += addtime;
                        if (currentTime <= (noteLength - subtractDuration))
                        {
                            dots++;
                        }
                        if (dots >= 4) break;
                        note = WrapDots(dots, note);
                    }
                    
                    break;
                }
            }

            return duration;
        }

        private BaseNote WrapDots(int numberDots, BaseNote note)
        {
            BaseNoteDot dotNote = null;
            for (int i = 0; i < numberDots; i++)
            {
                dotNote = new BaseNoteDot(note);
                note = dotNote;
            }

            return note;
        }
    }
}
