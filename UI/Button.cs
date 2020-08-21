using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using mizjam1.Actors;
using mizjam1.ContentLoaders;
using mizjam1.Sound;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.TextureAtlases;
using System;
using System.Collections.Generic;
using System.Text;

namespace mizjam1.UI
{
    internal class Button : Actor
    {
        internal GameWindow Window;
        internal string Text;
        internal float Height;
        internal float Size = 64;
        internal RectangleF Bounds;
        internal Action OnCollide;
        internal Player Player;
        internal bool PlaySound = true;

        internal bool DrawBorder;

        internal Button(GameWindow window, string text, float height, float size, Action onCollide, Player player, bool drawBorder = true)
        {
            Window = window;
            Text = text;
            Height = height;
            Position = new Vector2(0, height);
            Size = size;
            Bounds = new RectangleF(
                Position.X + Size * (-Text.Length / 2f + 1) + Size * 0.5f,
                Position.Y - Size * 0.5f,
                Size * (Text.Length + 1),
                Size * 2);

            OnCollide = onCollide;
            Player = player;
            DrawBorder = drawBorder;
        }


        internal override void Update(GameTime gameTime)
        {
            float xOffset = Window.ClientBounds.Width / 2f;
            Position = new Vector2(xOffset, Height);
            Bounds = new RectangleF(
                            Position.X + Size * (-Text.Length / 2f - 0.5f),
                            Position.Y - Size * 0.5f,
                            Size * (Text.Length + 1),
                            Size * 2);

            if (Bounds.Contains(Player.Position + new Vector2(Size * Scale / 2f, Size * Scale / 2f)))
            {
                Collide();

            }
            var m = Mouse.GetState();
            
            if (m.LeftButton == ButtonState.Pressed && Bounds.Contains(new Point(m.X, m.Y)))
            {
                Collide();
            }
        }
        internal void Collide()
        {
            if (PlaySound)
            {
                SoundPlayer.Instance.Play("PICK");
                SoundPlayer.Instance.Play();
            }
            OnCollide();
        }
        internal override void Draw(SpriteBatch spriteBatch)
        {

            var scale = Vector2.One * Size / 16f;
            float start = -Text.Length / 2f - 1;
            if (DrawBorder)
            {
                float end = start + Text.Length + 2;
                for (float i = start; i < end; i++)
                {
                    var x = i == start ? 0 : i < (end - 1) ? 1 : 2;

                    spriteBatch.Draw(ContentLoader.Instance.Border9Path[x, 0], Position + new Vector2(Size * i, Size * -1f), Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0.5f);
                    spriteBatch.Draw(ContentLoader.Instance.Border9Path[x, 1], Position + new Vector2(Size * i, Size * 0f), Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0.5f);
                    spriteBatch.Draw(ContentLoader.Instance.Border9Path[x, 2], Position + new Vector2(Size * i, Size * 1f), Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0.5f);
                }
            }

            spriteBatch.DrawString(ContentLoader.Instance.Font, Text, Position + new Vector2((start + 1) * Size, 0), Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 1);

            //var m = Mouse.GetState();
            //spriteBatch.DrawCircle(m.X, m.Y, 16, 16, Color.Red);
            //spriteBatch.DrawRectangle(Bounds, Color.Red);
        }
    }
}
