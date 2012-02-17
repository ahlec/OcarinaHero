using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using OcarinaHeroLibrary.Instruments;

namespace OcarinaHeroLibrary.Songs
{
    public class OcarinaSong : BaseSong
    {
        public OcarinaSong(BaseSong song) : base()
        {
            _instrumentType = InstrumentTypes.Ocarina;
        }
        /*
        public OcarinaSong(Song songFile, string title, SongDifficulty difficulty, float speed, SongAct[] acts) :
            base(songFile, title, difficulty, speed, acts)
        {
            this._instrumentType = InstrumentTypes.Ocarina;
        }*/
    }
}
