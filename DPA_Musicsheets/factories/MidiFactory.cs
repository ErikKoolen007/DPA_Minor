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

        private bool startedNoteIsClosed = true;
        private int previousMidiKey = 60; // Central C;
        private int previousNoteAbsoluteTicks = 0;
        private double percentageOfBarReached = 0;
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

        private MusicPart loadChannelMsg(ChannelMessage msg, MidiEvent e)
        {
            BaseNote note = new Note("", "");
            int division = seq.Division;

            if (msg.Command == ChannelCommand.NoteOn)
            {
                if (msg.Data2 > 0) // Data2 = loudness
                {
                    // Append the new baseNote.
                    note.letter = MidiToLilyHelper.GetLilyNoteName(previousMidiKey, msg.Data1);
                    previousMidiKey = msg.Data1;
                    startedNoteIsClosed = false;
                }
                else if (!startedNoteIsClosed)
                {
                    // Finish the previous baseNote with the length.
                    double percentageOfBar;
                    note.duration = MidiToLilyHelper.GetLilypondNoteLength(previousNoteAbsoluteTicks, e.AbsoluteTicks, division, _beatNote, _beatsPerBar, out percentageOfBar);
                    previousNoteAbsoluteTicks = e.AbsoluteTicks;
                    
                    BaseNoteSpace noteSpace = new BaseNoteSpace(note);
                    note = noteSpace;
                   // lilypondContent.Append(" ");

                    percentageOfBarReached += percentageOfBar;
                    if (percentageOfBarReached >= 1)
                    {
                        BaseNoteMark noteMark = new BaseNoteMark(note);
                        note = noteMark;
                        //lilypondContent.AppendLine("|");
                        percentageOfBarReached -= 1;
                    }
                    startedNoteIsClosed = true;
                }
                else
                {
                    //lilypondContent.Append("r");
                }
            }

            return null;
        }

        private MusicPart loadMetaMsg(MetaMessage msg)
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
    }
}
