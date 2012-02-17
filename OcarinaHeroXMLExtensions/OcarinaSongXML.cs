using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using Microsoft.Xna.Framework.Media;
using OcarinaHeroLibrary.Songs;

namespace OcarinaHeroLibrary.Songs
{
    public class OcarinaSongXMLReader : ContentTypeReader<OcarinaSong>
    {
        protected override OcarinaSong Read(ContentReader input, OcarinaSong existingInstance)
        {
            //OcarinaSong song = new OcarinaSong();
//            song.Initialize(
            Song songFile = input.ContentManager.Load<Song>(input.ReadString());
            string title = input.ReadString();
            SongDifficulty difficulty = (SongDifficulty)Enum.Parse(typeof(SongDifficulty), input.ReadString());
            List<SongAct> acts = new List<SongAct>();
            OcarinaSong song = new OcarinaSong(songFile, title, difficulty, acts.ToArray());
//            SongAct _act = input.ReadObject<SongAct>();
//            while (_act
            return song;
        }
    }
    [ContentTypeWriter]
    public class OcarinaSongXMLWriter : ContentTypeWriter<OcarinaSong>
    {
        protected override void Write(ContentWriter output, OcarinaSong value)
        {
            output.Write(value.SongFileAssetName);
            output.Write(value.Title);
            output.Write(Enum.GetName(typeof(SongDifficulty), value.Difficulty));
        }
        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(OcarinaSongXMLReader).AssemblyQualifiedName;
        }
    }
}