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

        internal float CollideTimer = 0;
        internal float CollideTime = 0.5f;
        internal bool Colliding = false;

        internal bool DrawBorder;

        internal float WaitTimer = 0;
        internal float WaitTime = 0.5f;

        internal Color Color = Color.White;

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
            if (DrawBorder)
            {
                Bounds = new RectangleF(
                                Position.X + Size * (-Text.Length / 2f - 0.5f),
                                Position.Y - Size * 0.5f,
                                Size * (Text.Length + 1),
                                Size * 2);
            } else
            {
                Bounds = new RectangleF(
                                Position.X + Size * (-Text.Length / 2f - 0.5f),
                                Position.Y,
                                Size * (Text.Length + 1),
                                Size);

            }
            if (Player == null)
            {
                return;
            }
            WaitTimer += gameTime.GetElapsedSeconds();

            var m = Mouse.GetState();

            if (Bounds.Contains(new Point(m.X, m.Y)))
            {
                Color = new Color(255 / 255f,200 / 255f, 37 / 255f);
            }
            else
            {
                Color = Color.White;
            }
            if (!Colliding && WaitTimer > WaitTime)
            {
                if (Bounds.Contains(Player.Position + new Vector2(Size * Scale / 2f, Size * Scale / 2f)))
                {
                    Collide();
                    return;
                }

                if (m.LeftButton == ButtonState.Pressed && Bounds.Contains(new Point(m.X, m.Y)))
                {
                    Collide();
                    return;
                }
            }
            else
            {
                CollideTimer += gameTime.GetElapsedSeconds();
                if (CollideTime < CollideTimer)
                {
                    CollideTimer = 0;
                    Colliding = false;
                }
                else
                {
                    Colliding = true;
                }
            }

        }
        internal void Collide()
        {
            Colliding = true;
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

                    spriteBatch.Draw(ContentLoader.Instance.Border9Path[x, 0], Position + new Vector2(Size * i, Size * -1f), Color, 0, Vector2.Zero, scale, SpriteEffects.None, 0.5f);
                    spriteBatch.Draw(ContentLoader.Instance.Border9Path[x, 1], Position + new Vector2(Size * i, Size * 0f), Color, 0, Vector2.Zero, scale, SpriteEffects.None, 0.5f);
                    spriteBatch.Draw(ContentLoader.Instance.Border9Path[x, 2], Position + new Vector2(Size * i, Size * 1f), Color, 0, Vector2.Zero, scale, SpriteEffects.None, 0.5f);
                }
            }

            spriteBatch.DrawString(ContentLoader.Instance.Font, Text, Position + new Vector2((start + 1) * Size, 0), Color, 0, Vector2.Zero, scale, SpriteEffects.None, 1);

            //var m = Mouse.GetState();
            //spriteBatch.DrawCircle(m.X, m.Y, 16, 16, Color.Red);
            //spriteBatch.DrawRectangle(Bounds, Color.Red);
        }
    }
}
