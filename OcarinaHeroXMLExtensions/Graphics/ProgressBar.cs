using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml.Linq;
using Microsoft.Xna.Framework.Content;

namespace OcarinaHeroLibrary.Graphics
{
    public class ProgressBar
    {
        private Texture2D leftEnd, rightEnd, blueValue, orangeValue, transition;
        public void Draw(SpriteBatch spriteBatch, int maxValue, int currentValue, Vector2 position,
            int width, Color tinting)
        {
            if (currentValue > maxValue)
                throw new ArgumentException();
            float percentage = ((float)currentValue / (float)maxValue);
            float widthOfOrange = (width - rightEnd.Width - leftEnd.Width) * percentage;
            spriteBatch.Draw(orangeValue, new Rectangle((int)position.X + leftEnd.Width, (int)position.Y,
                (int)widthOfOrange, orangeValue.Height), tinting);
            float widthOfBlue = width - leftEnd.Width - rightEnd.Width - widthOfOrange;
            spriteBatch.Draw(blueValue, new Rectangle((int)position.X + leftEnd.Width + (int)Math.Ceiling(widthOfOrange),
                (int)position.Y, (int)widthOfBlue, blueValue.Height), tinting);
            spriteBatch.Draw(transition, new Vector2(position.X + leftEnd.Width + widthOfOrange -
                (transition.Width / 2), position.Y), tinting);
            spriteBatch.Draw(leftEnd, position, tinting);
            spriteBatch.Draw(rightEnd, new Vector2(position.X + width - rightEnd.Width, position.Y), tinting);
        }
        public static ProgressBar Load(XElement xElement, ContentManager contentManager)
        {
            ProgressBar progressBar = new ProgressBar();
            progressBar.leftEnd = contentManager.Load<Texture2D>(xElement.Element("LeftEnd").Value);
            progressBar.rightEnd = contentManager.Load<Texture2D>(xElement.Element("RightEnd").Value);
            progressBar.orangeValue = contentManager.Load<Texture2D>(xElement.Element("Orange").Value);
            progressBar.blueValue = contentManager.Load<Texture2D>(xElement.Element("Blue").Value);
            progressBar.transition = contentManager.Load<Texture2D>(xElement.Element("Transition").Value);
            return progressBar;
        }
    }
}
