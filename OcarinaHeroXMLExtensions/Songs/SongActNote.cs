using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OcarinaHeroLibrary.Graphics;
using OcarinaHeroLibrary.Instruments;

namespace OcarinaHeroLibrary.Songs
{
    public class SongActNote : SongAct
    {
        protected bool missed, hit;
        public SongActNote(Time time, InstrumentNote note)
            : base(time)
        {
            _note = note;
            AwardPoints = 2;
        }
        public SongActNote(Time time, string note) : base(time)
        {
            _note = (InstrumentNote)Enum.Parse(typeof(InstrumentNote), note);
            AwardPoints = 2;
        }
        protected InstrumentNote _note;
        public InstrumentNote Note { get { return _note; } }
        public EngineModel Model { get; set; }
        public bool Missed
        {
            get { return missed; }
            set
            {
                missed = value;
                if (value && NoteMissed != null)
                    NoteMissed.Invoke(this);
            }
        }
        public bool Hit
        {
            get { return hit; }
            set
            {
                hit = value;
                if (value && NoteHit != null)
                    NoteHit.Invoke(this);
            }
        }
        public delegate void OnNoteMissedHandler(SongActNote noteAct);
        public event OnNoteMissedHandler NoteMissed;
        public delegate void OnNoteHitHandler(SongActNote noteAct);
        public event OnNoteHitHandler NoteHit;
    }
}
