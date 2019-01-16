using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
        private int _lowestKey;
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
            for (int i = 0; i < seq.Count(); i++)
            {
                Track track = seq[i];

                foreach (var midiEvent in track.Iterator())
                {
                    IMidiMessage midiMessage = midiEvent.MidiMessage;

                    if (midiMessage.MessageType == MessageType.Meta)
                    {
                        MetaTypeAdapter metaTypeAdapter = null;
                        var metaMessage = midiMessage as MetaMessage;
                        MusicPart part = LoadMetaMsg(metaMessage);
                    }

                    if (midiMessage.MessageType == MessageType.Channel)
                    {
                        var channelMessage = midiEvent.MidiMessage as ChannelMessage;
                        checkLowestNote(channelMessage.Data1);
                        LoadChannelMsg(channelMessage, midiEvent);
                    }
                }
            }

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
                    note = TranslateMidiName(msg.Data1 % 12, note, _previousMidiKey);

                    _previousMidiKey = msg.Data1;
                    _startedNoteIsClosed = false;
                    return note;
                }

                if (!_startedNoteIsClosed)
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
                    return note;
                }

                int restDuration = TranslateMidiDuration(e.AbsoluteTicks, division, null);
                Rest r = new Rest(restDuration);
                return r;
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

        private BaseNote TranslateMidiName(int midiKey, BaseNote note, int previousMidiKey)
        {
            BaseNoteCross crossNote;
            BaseNoteMole moleNote;

            int KeyNumber = midiKey % 12;
            int prevMidiKeyNumber = previousMidiKey % 12;

            if (KeyNumber == 0)
            {
                note.letter = "c";
            }

            if (KeyNumber == 1 && prevMidiKeyNumber == 0)
            {
                note.letter = "c";
                crossNote = new BaseNoteCross(note);
                note = crossNote;
            }

            if (KeyNumber == 1 && prevMidiKeyNumber == 2)
            {
                note.letter = "d";
                moleNote = new BaseNoteMole(note);
                note = moleNote;
            }

            if (KeyNumber == 2)
            {
                note.letter = "d";
            }

            if (KeyNumber == 3 && prevMidiKeyNumber == 2)
            {
                note.letter = "d";
                crossNote = new BaseNoteCross(note);
                note = crossNote;
            }

            if (KeyNumber == 3 && prevMidiKeyNumber == 4)
            {
                note.letter = "e";
                moleNote = new BaseNoteMole(note);
                note = moleNote;
            }

            if (KeyNumber == 4)
            {
                note.letter = "e";
            }

            if (KeyNumber == 5)
            {
                note.letter = "f";
            }

            if (KeyNumber == 6 && prevMidiKeyNumber == 5)
            {
                note.letter = "f";
                crossNote = new BaseNoteCross(note);
                note = crossNote;
            }

            if (KeyNumber == 7)
            {
                note.letter = "g";
            }

            if (KeyNumber == 8 && prevMidiKeyNumber == 7)
            {
                note.letter = "g";
                crossNote = new BaseNoteCross(note);
                note = crossNote;
            }

            if (KeyNumber == 8 && prevMidiKeyNumber == 9)
            {
                note.letter = "g";
                moleNote = new BaseNoteMole(note);
                note = moleNote;
            }

            if (KeyNumber == 9)
            {
                note.letter = "a";
            }

            if (KeyNumber == 10 && prevMidiKeyNumber == 9)
            {
                note.letter = "a";
                crossNote = new BaseNoteCross(note);
                note = crossNote;
            }

            if (KeyNumber == 10 && prevMidiKeyNumber == 11)
            {
                note.letter = "a";
                moleNote = new BaseNoteMole(note);
                note = moleNote;
            }

            if (KeyNumber == 11)
            {
                note.letter = "b";
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

        private void checkLowestNote(int midiKey)
        {
            if (midiKey < _lowestKey)
            {
                _lowestKey = midiKey;
            }
        }
    }
}
