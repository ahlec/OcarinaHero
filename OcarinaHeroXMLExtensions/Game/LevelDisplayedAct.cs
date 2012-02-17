using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OcarinaHeroLibrary.Graphics;
using OcarinaHeroLibrary.Songs;

namespace OcarinaHeroLibrary.Game
{
    public class LevelDisplayedAct
    {
        public LevelDisplayedAct(SongAct act, EngineModel model)
        {
            Act = act;
            Model = model;
        }
        public SongAct Act { get; set; }
        public EngineModel Model { get; set; }
    }
}
