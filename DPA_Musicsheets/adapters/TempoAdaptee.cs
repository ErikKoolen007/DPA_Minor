using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.domain;
using Sanford.Multimedia.Midi;

namespace DPA_Musicsheets.adapters
{
    class TempoAdaptee : MetaTypeAdapter
    {
        private int _bpm;
        public TempoAdaptee(MetaMessage msg)
        {
            if (msg.MetaType != MetaType.Tempo)
                throw new TypeAccessException();
            this.msg = msg;
            decodeMsg();
        }
        public override MusicPart LoadIntoDomain()
        {
            Tempo tempo = new Tempo(_bpm);
            return tempo;
        }

        public sealed override void decodeMsg()
        {
            byte[] tempoBytes = msg.GetBytes();
            int tempo = (tempoBytes[0] & 0xff) << 16 | (tempoBytes[1] & 0xff) << 8 | (tempoBytes[2] & 0xff);
            _bpm = 60000000 / tempo;
            //lilypondContent.AppendLine($"\\tempo 4={_bpm}");
        }
    }
}
