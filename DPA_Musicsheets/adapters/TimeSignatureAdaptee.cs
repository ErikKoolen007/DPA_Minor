using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.domain;
using DPA_Musicsheets.factories;
using Sanford.Multimedia.Midi;

namespace DPA_Musicsheets.adapters
{
    class TimeSignatureAdaptee : MetaTypeAdapter
    {
        private int _beatNote { get; set; }
        private int _beatsPerBar { get; set; }
        private MidiFactory factory;
        public TimeSignatureAdaptee(MetaMessage msg, MidiFactory factory)
        {
            if (msg.MetaType != MetaType.TimeSignature)
                throw new TypeAccessException();
            this.msg = msg;
            this.factory = factory;
            decodeMsg();
        }

        public override MusicPart LoadIntoDomain()
        {
            Time time = new Time(_beatNote, _beatsPerBar);
            return time;
        }

        public sealed override void decodeMsg()
        {
            byte[] timeSignatureBytes = msg.GetBytes();
            _beatNote = timeSignatureBytes[0];
            _beatsPerBar = (int)(1 / Math.Pow(timeSignatureBytes[1], -2));
            factory._beatNote = _beatNote;
            factory._beatsPerBar = _beatsPerBar;
        }
    }
}
