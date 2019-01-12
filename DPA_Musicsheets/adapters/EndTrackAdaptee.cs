using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.domain;
using DPA_Musicsheets.Managers;
using Sanford.Multimedia.Midi;

namespace DPA_Musicsheets.adapters
{
    class EndTrackAdaptee : MetaTypeAdapter
    {
        private int _beatNote;
        private int _beatsPerBar;
        private int _division;
        private int _prevNoteAbsoluteTicks;
        private double _prctBarBarReached;
        private MidiEvent _ev;

        public EndTrackAdaptee(MetaMessage msg, int beatNote, int beatsPerBar, int division, int prevNoteAbsoluteTicks, double prctBarReached, MidiEvent ev)
        {
            if (msg.MetaType != MetaType.EndOfTrack)
                throw new TypeAccessException();

            this.msg = msg;
            _beatNote = beatNote;
            _beatsPerBar = beatsPerBar;
            _division = division;
            _prevNoteAbsoluteTicks = prevNoteAbsoluteTicks;
            _prctBarBarReached = prctBarReached;
            _ev = ev;

            decodeMsg();
        }
        public override MusicPart LoadIntoDomain()
        {
            throw new NotImplementedException();
        }

        public sealed override void decodeMsg()
        {
            if (_prevNoteAbsoluteTicks > 0)
            {
                // Finish the last notelength.
                double percentageOfBar;
                MidiToLilyHelper.GetLilypondNoteLength(_prevNoteAbsoluteTicks, _ev.AbsoluteTicks, _division,
                    _beatNote, _beatsPerBar, out percentageOfBar);
                //lilypondContent.Append(MidiToLilyHelper.GetLilypondNoteLength(previousNoteAbsoluteTicks, midiEvent.AbsoluteTicks, _division, _beatNote, _beatsPerBar, out percentageOfBar));
                //lilypondContent.Append(" ");

                _prctBarBarReached += percentageOfBar;
                if (_prctBarBarReached >= 1)
                {

                    //lilypondContent.AppendLine("|");
                    percentageOfBar = percentageOfBar - 1;
                }
            }
        }
    }
}
