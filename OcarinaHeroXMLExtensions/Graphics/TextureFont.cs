using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml.Linq;
using Microsoft.Xna.Framework.Content;

namespace OcarinaHeroLibrary.Graphics
{
    public class TextureFont
    {
        private Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();
        public static TextureFont Load(XElement fontDefinition, ContentManager contentManager)
        {
            TextureFont font = new TextureFont();
            foreach (XElement entry in fontDefinition.Elements("Entry"))
                font.textures.Add(entry.Attribute("Character").Value, contentManager.Load<Texture2D>(entry.Value));
            return font;
        }
        public Texture2D GetTexture(string character)
        {
            return textures[character];
        }
        public Vector2 MeasureString(string text)
        {
            Vector2 size = new Vector2();
            for (int character = 0; character < text.Length; character++)
            {
                if (textures[text[character].ToString()].Width > size.X)
                    size.X = textures[text[character].ToString()].Width;
                if (textures[text[character].ToString()].Height > size.Y)
                    size.Y = textures[text[character].ToString()].Height;
            }
            return size;
        }
    }
    public static class TextureFontExtensions
    {
        public static void DrawString(this SpriteBatch spriteBatch, TextureFont font, string str,
            Vector2 location, Color tinting)
        {
            Vector2 currentLocation = location;
            for (int character = 0; character < str.Length; character++)
            {
                string piece = str.Substring(character, 1);
                spriteBatch.Draw(font.GetTexture(piece), currentLocation, tinting);
                currentLocation.X += font.GetTexture(piece).Width;
            }
        }
        public static void DrawString(this SpriteBatch spriteBatch, TextureFont font, string str,
            Vector2 location, Color tinting, float opacity)
        {
            spriteBatch.DrawString(font, str, location, new Color(tinting, opacity));
        }
    }
}
