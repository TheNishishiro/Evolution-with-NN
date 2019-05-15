using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFramework
{
    class NDrawing
    {
        public static Dictionary<string, Texture2D> Textures = new Dictionary<string, Texture2D>();
        private static Dictionary<string, SpriteFont> Fonts = new Dictionary<string, SpriteFont>();
        private static SpriteBatch sb;
        private static ContentManager c;
        private static int frameRate = 0;
        private static int frameCounter = 0;
        private static TimeSpan elapsedTime = TimeSpan.Zero;

        public static void SetFramework(SpriteBatch spriteBatch, ContentManager Content)
        {
            sb = spriteBatch;
            c = Content;
        }

        public static void AddTexture(string Fullpath, string name)
        {
            Textures.Add(name, c.Load<Texture2D>(Fullpath));
        }

        public static void AddTexture(string name, Texture2D texture)
        {
            Textures.Add(name, texture);
        }
        public static void AddTexture(string Fullpath)
        {
            Textures.Add(Fullpath, c.Load<Texture2D>(Fullpath));
        }
        public static void AddPixelTexture(string name, GraphicsDevice gd)
        {
            Textures.Add(name, NGraphics.Texture_CreatePixel(gd, Color.White));
        }
        public static void AddFont(string FullPath, string name)
        {
            Fonts.Add(name, c.Load<SpriteFont>(FullPath));
        }

        public static void Draw(string texture, Vector2 position)
        {
            sb.Draw(Textures[texture], position, Color.White);
        }
        public static void Draw(string texture, Rectangle position)
        {
            sb.Draw(Textures[texture], position, Color.White);
        }
        public static void Draw(string texture, Vector2 position, Color Color)
        {
            sb.Draw(Textures[texture], position, Color);
        }
        public static void Draw(string texture, Rectangle position, Color Color)
        {
            sb.Draw(Textures[texture], position, Color);
        }
        public static void Draw(string texture, Vector2 position, Color Color, float scale = 1, float rotation = 0, bool MiddleOrigin = false)
        {
            if (MiddleOrigin == false)
                sb.Draw(Textures[texture], position, new Rectangle(0, 0, Textures[texture].Width, Textures[texture].Height), Color, rotation, Vector2.Zero, scale, SpriteEffects.None, 0);
            else
                sb.Draw(Textures[texture], position, new Rectangle(0, 0, Textures[texture].Width, Textures[texture].Height), Color, rotation, new Vector2(Textures[texture].Width / 2, Textures[texture].Height / 2), scale, SpriteEffects.None, 0);
        }

        public static void DrawText(string name, string text, Vector2 position)
        {
            sb.DrawString(Fonts[name], text, position, Color.White);
        }
        public static void DrawText(string text, Vector2 position)
        {
            sb.DrawString(Fonts["font"], text, position, Color.White);
        }
        public static void DrawText(string text, Vector2 position, Color color)
        {
            try
            {
                sb.DrawString(Fonts["font"], text, position, color);
            }
            catch (System.ArgumentException)
            {

            }
        }
        public static void DrawText(string name, string text, Vector2 position, Color color)
        {
            sb.DrawString(Fonts[name], text, position, color);
        }

        public static void FPS_Draw(Vector2 Position, Color TextColor, GameTime gameTime)
        {
            frameCounter++;
            elapsedTime += gameTime.ElapsedGameTime;

            if (elapsedTime > TimeSpan.FromSeconds(1))
            {
                elapsedTime -= TimeSpan.FromSeconds(1);
                frameRate = frameCounter;
                frameCounter = 0;
            }
            DrawText("FPS: " + frameRate, Position);
        }

        public static void Draw_Line_Between_Points(Vector2 Position1, Vector2 Position2, string texture)
        {
            Vector2 direction = Position2 - Position1;
            float angle = (float)Math.Atan2(direction.Y, direction.X);
            float distance = Vector2.Distance(Position1, Position2);

            sb.Draw(Textures[texture], Position1, new Rectangle((int)Position1.X, (int)Position1.Y, (int)distance, 1), Color.White, angle, Vector2.Zero, 1, SpriteEffects.None, 0);
        }

        public static void Draw_Line_Between_Points(Vector2 Position1, Vector2 Position2, string texture, Color color)
        {
            Vector2 direction = Position2 - Position1;
            float angle = (float)Math.Atan2(direction.Y, direction.X);
            float distance = Vector2.Distance(Position1, Position2);

            sb.Draw(Textures[texture], Position1, new Rectangle((int)Position1.X, (int)Position1.Y, (int)distance, 1), color, angle, Vector2.Zero, 1, SpriteEffects.None, 0);
        }

        public static void Draw_Line_Between_Points(Vector2 Position1, Vector2 Position2, string texture, double size)
        {
            Vector2 direction = Position2 - Position1;
            float angle = (float)Math.Atan2(direction.Y, direction.X);
            float distance = Vector2.Distance(Position1, Position2);

            sb.Draw(Textures[texture], Position1, new Rectangle((int)Position1.X, (int)Position1.Y, (int)distance, (int)size), Color.White, angle, new Vector2(0, (float)size / 2), 1, SpriteEffects.None, 0);
        }
    }
}
