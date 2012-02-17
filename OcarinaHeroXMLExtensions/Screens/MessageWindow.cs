using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OcarinaHeroLibrary.Screens
{
    public class MessageWindow : Microsoft.Xna.Framework.Game
    {
        protected GraphicsDeviceManager graphics;
        protected SpriteBatch spriteBatch;
        
        protected bool usingSkin;
        protected ApplicationSkin skin;
        protected string caption;
        protected string title;
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                Window.Title = title;
            }
        }

        protected SpriteFont backupFont;

        public MessageWindow(ApplicationSkin applicationSkin, string dialogCaption)
        {
            graphics = new GraphicsDeviceManager(this);
            skin = applicationSkin;
            usingSkin = (skin != null);
            caption = dialogCaption;
            Content.RootDirectory = "Content";
            Title = "";
        }
        public MessageWindow(ApplicationSkin applicationSkin, string dialogCaption, string title)
        {
            Title = title;
            graphics = new GraphicsDeviceManager(this);
            skin = applicationSkin;
            usingSkin = (skin != null);
            caption = dialogCaption;
            Content.RootDirectory = "Content";
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            if (!usingSkin)
                backupFont = Content.Load<SpriteFont>("SpriteFont1");
            Vector2 captionSize = (usingSkin ? skin.Font : backupFont).MeasureString(caption);
            base.LoadContent();
        }
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.LightGray);
            spriteBatch.Begin();
            if (usingSkin)
            {
                spriteBatch.DrawString(skin.Font, caption, new Vector2(5, 5), Color.Black);
            }
            else
                spriteBatch.DrawString(backupFont, caption, new Vector2(5, 5), Color.Black);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
