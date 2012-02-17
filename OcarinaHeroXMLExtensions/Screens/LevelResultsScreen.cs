using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OcarinaHeroLibrary.Controller;
using OcarinaHeroLibrary.Songs;
using OcarinaHeroLibrary.Game;
using OcarinaHeroLibrary.Graphics;

namespace OcarinaHeroLibrary.Screens
{
    public class LevelResultsScreen : IScreen
    {
        private bool fadingIn = true;
        private static TimeSpan fadeDurationLength = TimeSpan.FromMilliseconds(750);
        private TimeSpan fadeDuration = TimeSpan.Zero;

        protected ApplicationSkin skin;
        protected GameLevel level;
        protected bool passed;
        public LevelResultsScreen(ApplicationSkin applicationSkin, GameLevel gameLevel, bool passedLevel)
        {
            skin = applicationSkin;
            level = gameLevel;
            passed = passedLevel;
        }
        public void Update(GameTime gameTime, IController controller, ref Gamer gamer)
        {
            if (fadingIn)
            {
                fadeDuration += gameTime.ElapsedGameTime;
                if (fadeDuration >= fadeDurationLength)
                {
                    fadingIn = false;
                    FadeInFinished.Invoke();
                }
                else
                    return;
            }
            ControllerInput input = controller.GetInput(gameTime);
            if (input.Enter && ReviewFinished != null)
                ReviewFinished.Invoke();
        }
        public void Draw(GameTime gameTime, Gamer gamer, GraphicsDeviceManager graphics, SpriteBatch spriteBatch)
        {
            if (fadingIn)
            {
                spriteBatch.Begin();
                spriteBatch.FillRectangle(0, 0, GlobalConstants.ScreenWidth,
                    GlobalConstants.ScreenHeight * (float)(fadeDuration.TotalMilliseconds /
                    fadeDurationLength.TotalMilliseconds), Color.Black);
                spriteBatch.End();
                return;
            }
            graphics.GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            spriteBatch.DrawString(skin.Font, (passed ? "Passed" : "Failed"), new Vector2(10, 10), Color.White);
            spriteBatch.End();
        }
        public delegate void ReviewFinishedHandler();
        public event ReviewFinishedHandler ReviewFinished;
        public event EmptyArgsHandler FadeInFinished;
    }
}
