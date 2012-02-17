using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OcarinaHeroLibrary.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline;
using System.Xml.Linq;
using OcarinaHeroLibrary.Instruments;

namespace OcarinaHeroLibrary
{
    public class ApplicationSkin
    {
        protected ContentManager content;
        private EngineModel _track, trackA, trackB, trackC, trackD, trackE, collisionArea, ocarina;
        private PlanePrimitive cA;
        private SpriteFont _font, messageBoxFont;
        private float trackEndZ, trackStartZ;
        private TextureFont scoreFont, greenFont;
        private Texture2D sky;
        private ProgressBar progressBar;

        public Texture2D SkyTexture { get { return sky; } }
        public EngineModel GameTrack { get { return _track; } }
        public EngineModel TrackA { get { return trackA; } }
        public EngineModel TrackB { get { return trackB; } }
        public EngineModel TrackC { get { return trackC; } }
        public EngineModel TrackD { get { return trackD; } }
        public EngineModel TrackE { get { return trackE; } }
//        public PlanePrimitive CollisionArea { get { return cA; } }
        public EngineModel CollisionArea { get { return collisionArea; } }
        public EngineModel Ocarina { get { return ocarina; } }
        public TextureFont ScoreFont { get { return scoreFont; } }
        public TextureFont GreenFont { get { return greenFont; } }
        public ProgressBar ProgressBar { get { return progressBar; } }

        public SpriteFont Font { get { return _font; } }
        public SpriteFont MessageBoxFont { get { return messageBoxFont; } }

        public float TrackStartZ { get { return trackStartZ; } }
        public float TrackEndZ { get { return trackEndZ; } }

        public static ApplicationSkin FromXML(string filepath, ContentManager content)
        {
            ApplicationSkin skin = new ApplicationSkin();
            System.IO.Stream stream = new System.IO.FileStream(filepath, System.IO.FileMode.Open);
            XDocument xmlFile = XDocument.Load(System.Xml.XmlReader.Create(stream), LoadOptions.None);
            stream.Close();
            skin.content = content;
            XElement xmlSkin = xmlFile.Element("Skin");

            skin.sky = content.Load<Texture2D>("images\\sky04");

            XElement xmlTrack = xmlSkin.Element("Track");
            EngineModel trackModel = content.Load<Model>(xmlTrack.Element("Model").Value);
            if (xmlTrack.Element("X") != null)
                trackModel.X = float.Parse(xmlTrack.Element("X").Value);
            if (xmlTrack.Element("Y") != null)
                trackModel.Y = float.Parse(xmlTrack.Element("Y").Value);
            if (xmlTrack.Element("Z") != null)
                trackModel.Z = float.Parse(xmlTrack.Element("Z").Value);
            if (xmlTrack.Element("Scale") != null)
                trackModel.Scale = float.Parse(xmlTrack.Element("Scale").Value) * Vector3.One;
            //trackModel.Scale = new Vector3(0.005f, 0.33f, 0.05f);
            trackModel.RotationX += MathHelper.PiOver2;
            skin._track = trackModel;

            XElement xmlCollision = xmlSkin.Element("CollisionArea");
            /*PlanePrimitive plane = new PlanePrimitive(new Vector3(0, -10, trackModel.Y + 5),
                new Vector2(100, 10), Color.Purple);
            skin.cA = plane;*/
            EngineModel collisionModel = content.Load<Model>("models\\Individual tracks");
            //collisionModel.RotationY += MathHelper.PiOver2;
            collisionModel.SetTexture(0, content.Load<Texture2D>(@"models\notetex3"));
            collisionModel.Z = float.Parse(xmlCollision.Element("Z").Value);
            collisionModel.Y = trackModel.Y + 5;
            collisionModel.X = 100;
            collisionModel.RotationZ += MathHelper.PiOver2;
             collisionModel.Opacity = 50;
             collisionModel.Scale = new Vector3(0.5f);
            skin.collisionArea = collisionModel;
            /*
            EngineModel collisionModel = content.Load<Model>(xmlCollision.Element("Model").Value);
            collisionModel.Z = float.Parse(xmlCollision.Element("Z").Value);
            collisionModel.Y = trackModel.Y - 5;
            skin.collisionArea = collisionModel;*/

            skin.progressBar = ProgressBar.Load(xmlSkin.Element("ProgressBar"), content);

            foreach (InstrumentNote note in Enum.GetValues(typeof(InstrumentNote)))
            {
                XElement noteXML = xmlSkin.Element(Enum.GetName(typeof(InstrumentNote), note));
                skin.noteInfo.Add(note, new NoteModelInfo(noteXML.Element("Texture").Value,
                    float.Parse(noteXML.Element("X").Value)));
            }

            skin.trackEndZ = -50f;// float.Parse(xmlSkin.Element("CollisionArea").Element("Z").Value);
            skin.trackStartZ = -150f;

            skin.trackA = content.Load<Model>("models\\Individual tracks");
            skin.trackA.Position = new Vector3(-20, trackModel.Y - 30, skin.TrackEndZ - 25);//trackModel.Z - 100);
            skin.trackA.SetTexture(0, content.Load<Texture2D>(@"models\notetex2"));
            skin.trackA.ScaleX = .5f;

            skin.trackB = content.Load<Model>("models\\Individual tracks");
            skin.trackB.Position = new Vector3(-10, trackModel.Y - 30, skin.TrackEndZ - 25);//trackModel.Z - 100);
            skin.trackB.SetTexture(0, content.Load<Texture2D>(@"models\notetex3"));
            skin.trackB.ScaleX = .5f;

            skin.trackC = content.Load<Model>("models\\Individual tracks");
            skin.trackC.Position = new Vector3(0, trackModel.Y - 30, skin.TrackEndZ - 25);//trackModel.Z - 100);
            skin.trackC.SetTexture(0, content.Load<Texture2D>(@"models\notetex4"));
            skin.trackC.ScaleX = .5f;

            skin.trackD = content.Load<Model>("models\\Individual tracks");
            skin.trackD.Position = new Vector3(10, trackModel.Y - 30, skin.TrackEndZ - 25);//trackModel.Z - 100);
            skin.trackD.SetTexture(0, content.Load<Texture2D>(@"models\notetex5"));
            skin.trackD.ScaleX = .5f;

            skin.trackE = content.Load<Model>("models\\Individual tracks");
            skin.trackE.Position = new Vector3(20, trackModel.Y - 30, skin.TrackEndZ - 25);//trackModel.Z - 100);
            skin.trackE.SetTexture(0, content.Load<Texture2D>(@"models\notetex1"));
            skin.trackE.ScaleX = .5f;

            skin.ocarina = content.Load<Model>(xmlSkin.Element("Ocarina").Element("Model").Value);
            if (xmlSkin.Element("Ocarina").Element("Scale") != null)
                skin.ocarina.Scale = Vector3.One * float.Parse(xmlSkin.Element("Ocarina").Element("Scale").Value);
            skin.ocarina.Position = new Vector3(float.Parse(xmlSkin.Element("Ocarina").Element("X").Value),
                float.Parse(xmlSkin.Element("Ocarina").Element("Y").Value),
                float.Parse(xmlSkin.Element("Ocarina").Element("Z").Value));

            skin.noteBaseModel = xmlSkin.Element("NoteBase").Element("Model").Value;
            skin.noteBaseY = (xmlSkin.Element("NoteBase").Element("Y") != null ?
                float.Parse(xmlSkin.Element("NoteBase").Element("Y").Value) : 0);
            skin.noteBaseZ = (xmlSkin.Element("NoteBase").Element("Z") != null ?
                float.Parse(xmlSkin.Element("NoteBase").Element("Z").Value) : 0);
            skin.noteBaseScale = (xmlSkin.Element("NoteBase").Element("Scale") != null ?
                float.Parse(xmlSkin.Element("NoteBase").Element("Scale").Value) * Vector3.One : Vector3.One);

            skin._font = content.Load<SpriteFont>(xmlSkin.Element("Font").Value);
            skin.messageBoxFont = content.Load<SpriteFont>(xmlSkin.Element("Fonts").Element("MessageWindow").Value);

            skin.scoreFont = TextureFont.Load(xmlSkin.Element("ScoreFont"), content);
            skin.greenFont = TextureFont.Load(xmlSkin.Element("GreenFont"), content);


            return skin;
        }
        private string noteBaseModel;
        private float noteBaseY, noteBaseZ;
        private Vector3 noteBaseScale;
        private Dictionary<InstrumentNote, NoteModelInfo> noteInfo = new Dictionary<InstrumentNote, NoteModelInfo>();
        private class NoteModelInfo
        {
            private float xPosition;
            private string texture;
            public NoteModelInfo(string texture, float x)
            {
                xPosition = x;
                this.texture = texture;
            }
            public string Texture { get { return texture; } }
            public float X { get { return xPosition; } }
        }
        public EngineModel GetNoteModel(Instruments.InstrumentNote note)
        {
            EngineModel model = content.Load<Model>(noteBaseModel);
            model.Position = new Vector3(noteInfo[note].X, noteBaseY, noteBaseZ);
            model.Scale = noteBaseScale;
            model.SetTexture(0, content.Load<Texture2D>(noteInfo[note].Texture));
            return model;
        }
    }
}
