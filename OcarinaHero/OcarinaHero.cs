using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using System.Xml.Linq;
using OcarinaHeroLibrary;
using OcarinaHeroLibrary.Game;
using OcarinaHeroLibrary.Instruments;
using OcarinaHeroLibrary.Songs;
using OcarinaHeroLibrary.Controller;
using OcarinaHeroLibrary.Graphics;
using OcarinaHeroLibrary.Screens;

namespace OcarinaHero
{
    public class OcarinaHero : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;
        GameLevel level = null;
        Gamer[] gamers;
        int currentGamer;

        public OcarinaHero()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 700;
            graphics.PreferredBackBufferWidth = 1000;
//            graphics.ToggleFullScreen();
        }
        List<BaseSong> songs;

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            skin = ApplicationSkin.FromXML(Content.RootDirectory + "\\DefaultSkin.xml", Content);
            skin.GameTrack.RotationX -= MathHelper.Pi * 0.55f;
            skin.GameTrack.ScaleX = 2;

            songs = new List<BaseSong>();
            DirectoryInfo songbase = new DirectoryInfo(Content.RootDirectory + @"\songs\");
            foreach (FileInfo songfile in songbase.GetFiles())
                songs.Add(BaseSong.FromXML(songfile.FullName, Content, skin));
            FileInfo[] gamerProfileFiles = new DirectoryInfo(Content.RootDirectory + @"\gamers\").GetFiles("*.xml");
            gamers = new Gamer[gamerProfileFiles.Length];
            for (int index = 0; index < gamerProfileFiles.Length; index++)
                gamers[index] = Gamer.Load(gamerProfileFiles[index].FullName);

            //controller = OcarinaController.FromXNAKeyboard(PlayerIndex.One);
            controller = KeyboardController.FromXNAKeyboard(PlayerIndex.One);
            ReturnToSongSelectionScreen();
        }

        public void ReturnToSongSelectionScreen()
        {
            SongSelectionScreen selectionScreen = new SongSelectionScreen(songs.ToArray(), skin, graphics.GraphicsDevice);
            selectionScreen.SongSelected += new SongSelectionScreen.SongSelectedEventHandler(SongSelected);
            selectionScreen.ExitGame += delegate() { this.Exit(); };
            level = null;
            currentScreen = selectionScreen;
        }

        void SongSelected(SongSelectedArgs args)
        {
            level = new GameLevel(args.Song, controller, skin, Content, graphics);
            level.LevelEnded += new GameLevel.OnLevelEndedHandler(LevelEnded);
            currentScreen = null;
            level.Begin();
        }
        protected virtual void LevelEnded(bool passed)
        {
            level.End();
            LevelResultsScreen resultsScreen = new LevelResultsScreen(skin, level, passed);
            resultsScreen.ReviewFinished += new LevelResultsScreen.ReviewFinishedHandler(ReturnToSongSelectionScreen);
            resultsScreen.FadeInFinished += delegate() { level = null; };
            currentScreen = resultsScreen;
        }

        private ApplicationSkin skin;
        private IController controller;
        private IScreen currentScreen;

        public ApplicationSkin ApplicationSkin { get { return skin; } }

        protected override void Update(GameTime gameTime)
        {
            if (level != null)
                level.Update(gameTime);
            if (currentScreen != null)
                    currentScreen.Update(gameTime, controller, ref gamers[currentGamer]);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            if (level != null)
                level.Draw(gameTime, graphics, spriteBatch);
            if (currentScreen != null)
                currentScreen.Draw(gameTime, gamers[currentGamer], graphics, spriteBatch);
            base.Draw(gameTime);
        }
    }
}
