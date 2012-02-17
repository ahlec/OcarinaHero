using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OcarinaHeroLibrary.Controller;

namespace OcarinaHeroLibrary.Screens
{
    public interface IScreen
    {
        void Update(GameTime gameTime, IController controller, ref Gamer gamer);
        void Draw(GameTime gameTime, Gamer gamer, GraphicsDeviceManager graphics, SpriteBatch spriteBatch);
    }
    public delegate void OpenScreenHandler(IScreen screen);
}
