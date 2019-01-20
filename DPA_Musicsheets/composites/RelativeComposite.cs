using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.composites;
using DPA_Musicsheets.domain;

namespace DPA_Musicsheets
{
    class RelativeComposite : AbstractComposite
    {
        MusicPartWrapper _relative;
        public RelativeComposite(MusicPartWrapper relative)
        {
            _relative = relative;
        }
        public void visit()
        {
            next();
        }

        public void next()
        {
            foreach (var part in _relative._symbols)
            {
                if (part.GetType() == typeof(Clef))
                {
                    Clef clef = (Clef) part;
                    ClefComposite clefComposite = new ClefComposite(clef);
                    clefComposite.visit();
                } else if (part.GetType() == typeof(Time))
                {
                    Time time = (Time) part;
                    TimeComposite timeComposite = new TimeComposite(time);
                    timeComposite.visit();
                } else if (part.GetType() == typeof(Tempo))
                {
                    Tempo tempo = (Tempo) part;
                    TempoComposite tempoComposite = new TempoComposite(tempo);
                    tempoComposite.visit();
                } else if (part.GetType().BaseType == typeof(BaseNote))
                {
                    BaseNote note = (BaseNote) part;
                    BaseNoteComposite noteComposite = new BaseNoteComposite(note);
                    noteComposite.visit();
                } else if (part.GetType() == typeof(MusicPartWrapper))
                {
                    MusicPartWrapper wrapper = (MusicPartWrapper) part;
                    WrapperComposite wrapperComposite = new WrapperComposite(wrapper);
                    wrapperComposite.visit();
                }
            }
        }
    }
}
