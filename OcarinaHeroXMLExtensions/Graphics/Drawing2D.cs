using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OcarinaHeroLibrary.Graphics
{
    public static class Drawing2D
    {
        private static Texture2D pixel = null;
        public static void DrawLine(this SpriteBatch spriteBatch, Vector2 pointA, Vector2 pointB, Color color, int thickness)
        {
            if (pixel == null)
            {
                pixel = new Texture2D(spriteBatch.GraphicsDevice, 1, 1, 1, TextureUsage.None, SurfaceFormat.Color);
                pixel.SetData(new[] { Color.White });
            }
            spriteBatch.Draw(pixel, pointA, null, color, (float)Math.Atan2((double)(pointB.Y - pointA.Y),
                (double)(pointB.X - pointA.X)), Vector2.Zero, new Vector2(Vector2.Distance(pointA, pointB), thickness),
                SpriteEffects.None,
                0);
        }
        public static void DrawLine(this SpriteBatch spriteBatch, Vector2 pointA, Vector2 pointB, Color color)
        {
            spriteBatch.DrawLine(pointA, pointB, color, 1);
        }
        public static void DrawLine(this SpriteBatch spriteBatch, float x1, float y1, float x2, float y2, Color color)
        {
            spriteBatch.DrawLine(new Vector2(x1, y1), new Vector2(x2, y2), color, 1);
        }
        public static void DrawLine(this SpriteBatch spriteBatch, float x1, float y1, float x2, float y2, Color color, int thickness)
        {
            spriteBatch.DrawLine(new Vector2(x1, y1), new Vector2(x2, y2), color, thickness);
        }
        public static void DrawRectangle(this SpriteBatch spriteBatch, Vector2 position, Vector2 size, Color color, int thickness)
        {
            spriteBatch.DrawLine(position, position + size * Vector2.UnitX, color, thickness);
            spriteBatch.DrawLine(position + size * Vector2.UnitX, position + size, color, thickness);
            spriteBatch.DrawLine(position + size, position + size * Vector2.UnitY, color, thickness);
            spriteBatch.DrawLine(position + size * Vector2.UnitY, position, color, thickness);
        }
        public static void DrawRectangle(this SpriteBatch spriteBatch, Vector2 position, Vector2 size, Color color)
        {
            spriteBatch.DrawRectangle(position, size, color, 1);
        }
        public static void DrawRectangle(this SpriteBatch spriteBatch, float x, float y, float width, float height, Color color)
        {
            spriteBatch.DrawRectangle(new Vector2(x, y), new Vector2(width, height), color, 1);
        }
        public static void DrawRectangle(this SpriteBatch spriteBatch, float x, float y, float width, float height, Color color, int thickness)
        {
            spriteBatch.DrawRectangle(new Vector2(x, y), new Vector2(width, height), color, thickness);
        }
        public static void FillRectangle(this SpriteBatch spriteBatch, Vector2 position, Vector2 size, Color color)
        {
            if (pixel == null)
            {
                pixel = new Texture2D(spriteBatch.GraphicsDevice, 1, 1, 1, TextureUsage.None, SurfaceFormat.Color);
                pixel.SetData(new[] { Color.White });
            }
            spriteBatch.Draw(pixel, position, null, color, 0f, Vector2.Zero, size, SpriteEffects.None, 0);
        }
        public static void FillRectangle(this SpriteBatch spriteBatch, float x, float y, float width, float height, Color color)
        {
            spriteBatch.FillRectangle(new Vector2(x, y), new Vector2(width, height), color);
        }
        public static void FillRectangle(this SpriteBatch spriteBatch, Rectangle rectangle, Color color)
        {
            spriteBatch.FillRectangle(new Vector2(rectangle.X, rectangle.Y), new Vector2(rectangle.Width,
                rectangle.Height), color);
        }
        public static void DrawRectangle(this SpriteBatch spriteBatch, Rectangle rectangle, Color color)
        {
            spriteBatch.DrawRectangle(new Vector2(rectangle.X, rectangle.Y), new Vector2(rectangle.Width,
                rectangle.Height), color);
        }
    }
}
