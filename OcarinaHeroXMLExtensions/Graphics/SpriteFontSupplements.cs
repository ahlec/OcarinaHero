using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OcarinaHeroLibrary.Graphics
{
    public static class SpriteFontSupplements
    {
        public static string[] WordWrap(this SpriteFont font, string str, int widthOfLine)
        {
            List<string> lines = new List<string>();
            List<Int32> lineWidths = new List<Int32>();
            string[] individualWords = str.Split(new char[] { ' ' });
            foreach (string word in individualWords)
            {
                bool preceededWithSpace = (lines.Count > 0 && lines[lines.Count - 1].Length > 0);
                float lengthOfWord = font.MeasureString((preceededWithSpace ? " " : "") + word).X;
                if (lineWidths.Count > 0 && lineWidths[lineWidths.Count - 1] +
                    (int)Math.Ceiling(lengthOfWord) <= widthOfLine)
                {
                    lines[lines.Count - 1] += (preceededWithSpace ? " " : "") + word;
                    lineWidths[lineWidths.Count - 1] += (int)Math.Ceiling(lengthOfWord);
                }
                else
                {
                    lines.Add(word); // Will never start a line with a space
                    lineWidths.Add((int)Math.Ceiling(font.MeasureString(word).X)); // Same reason as above line
                }
            }
            return lines.ToArray();
        }
        public static Vector2 MeasureStringMultiline(this SpriteFont font, string[] strings)
        {
            Vector2 size = Vector2.Zero;
            foreach (string str in strings)
            {
                Vector2 strSize = font.MeasureString(str);
                size.Y += strSize.Y;
                if (strSize.X > size.X)
                    size.X = strSize.X;
            }
            return size;
        } 
    }
}
