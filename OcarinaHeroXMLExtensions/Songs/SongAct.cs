using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace OcarinaHeroLibrary.Songs
{
    public class SongAct
    {
        protected Time _time, _length;
        public SongAct(Time time)
        {
            _time = time;
            _length = null;
            Phrase = "";
            AwardPoints = 1;
        }
        public string Phrase { get; set; }
        public Time Time { get { return _time; } }
        public Time Length { get { return _length; } }
        public bool RespondedTo { get; set; }
        public float AwardPoints { get; set; }
        public int SongActIndex { get; set; }
        public bool Active { get; set; }
        public bool Passed { get; set; }
        public bool InFeedbackZone { get; set; }
    }
}
