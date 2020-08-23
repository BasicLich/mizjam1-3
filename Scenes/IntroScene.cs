using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using mizjam1.Actors;
using mizjam1.ContentLoaders;
using mizjam1.Helpers;
using mizjam1.Sound;
using mizjam1.UI;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.TextureAtlases;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace mizjam1.Scenes
{
    internal class IntroScene : Scene
    {
        internal int Size = 32;
        internal List<Actor> Actors;
        internal float SoundTimer = 0;
        internal float SoundTime = 0.5f;
        internal bool CanPlay = true;
        internal Color BgColor = new Color(14 / 255f, 7 / 255f, 27 / 255f);
        internal float Timer = 0;
        internal float Time = 10;
        internal List<Point> Stars;
        public override void Initialize(GameWindow window, GraphicsDevice graphicsDevice, ContentManager content, Main main)
        {
            base.Initialize(window, graphicsDevice, content, main);

            Stars = new List<Point>();
            for (int i = 0; i < Window.ClientBounds.Width / 10; i++)
            {
                int x = (int)(Window.ClientBounds.Width * RandomHelper.NextFloat());
                int y = (int)(Window.ClientBounds.Height * RandomHelper.NextFloat());
                Stars.Add(new Point(x, y));
            }
            Window.ClientSizeChanged += Window_ClientSizeChanged;

            Actors = new List<Actor>
            {
                new Button(window, "star pig", Size * 1, Size * 1.5f, PlayPig, null, false) { PlaySound = false },
                new Button(window, "your rocket crashed in a forest", Size * 4, Size / 2, PlayPig, null, false) { PlaySound = false },
                new Button(window, "you have to assemble it back", Size * 5, Size / 2, PlayPig, null, false) { PlaySound = false },
                new Button(window, "farmers have mistaken you for a", Size * 7, Size / 2, PlayPig, null, false) { PlaySound = false },
                new Button(window, "pig and will try to capture you", Size * 8, Size / 2, PlayPig, null, false) { PlaySound = false },
                new Button(window, "luckily you found an edible", Size * 10, Size / 2, PlayPig, null, false) { PlaySound = false },
                new Button(window, "plant that can boost your power", Size * 11, Size / 2, PlayPig, null, false) { PlaySound = false },
                new Button(window, "click", Size * 13, Size, PlayPig, null, false) { PlaySound = false },
            };

        }

        private void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            Stars.Clear();
            for (int i = 0; i < Window.ClientBounds.Width / 10; i++)
            {
                int x = (int)(Window.ClientBounds.Width * RandomHelper.NextFloat());
                int y = (int)(Window.ClientBounds.Height * RandomHelper.NextFloat());
                Stars.Add(new Point(x, y));
            }
        }

        internal void PlayPig()
        {
            if (CanPlay)
            {
                SoundPlayer.Instance.Play("PIG");
                CanPlay = false;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(BgColor);

            SpriteBatch.Begin(samplerState: SamplerState.PointClamp, sortMode: SpriteSortMode.FrontToBack);

            
            foreach (var p in Stars)
            {
                SpriteBatch.DrawPoint(p.X, p.Y, Color.White, 2);
            }

            Actors.ForEach(b => b.Draw(SpriteBatch));

            SpriteBatch.End();
        }


        public override void Update(GameTime gameTime)
        {
            var delta = gameTime.GetElapsedSeconds();
            if (!CanPlay)
            {
                SoundTimer += delta;
                if (SoundTimer > SoundTime)
                {
                    CanPlay = true;
                    SoundTimer = 0;
                }
            }

            Actors.ForEach(b => b.Update(gameTime));

            Time += delta;
            if (Timer > Time || Keyboard.GetState().GetPressedKeyCount() > 0 || Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                Game.NewMenu();
            }

            for (int i = 0; i < Stars.Count; i++)
            {
                Point p = Stars[i];
                int x = (int)(p.X + Math.Max(1, i / (Window.ClientBounds.Width / 50f)));
                x %= Window.ClientBounds.Width;
                Stars[i] = new Point(x, p.Y);
            }
        }
    }
}
