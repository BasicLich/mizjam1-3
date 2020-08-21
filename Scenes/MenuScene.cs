using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using mizjam1.Actors;
using mizjam1.ContentLoaders;
using mizjam1.Sound;
using mizjam1.UI;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.TextureAtlases;
using System;
using System.Collections.Generic;
using System.Text;

namespace mizjam1.Scenes
{
    internal class MenuScene : Scene
    {
        internal int Size = 32;
        internal List<Actor> Actors;
        internal Player Player;
        internal float SoundTimer = 0;
        internal float SoundTime = 0.5f;
        internal bool CanPlay = true;
        internal Color BgColor = new Color(57 / 255f, 31 / 255f, 33 / 255f);

        public override void Initialize(GameWindow window, GraphicsDevice graphicsDevice, ContentManager content, Main main)
        {
            base.Initialize(window, graphicsDevice, content, main);
            Player = new Player(new Vector2(50, 100))
            {
                BorderCollide = false,
                CanTeleport = false,
                Scale = 3,
                MaxSpeed = 100,
            };
            Actors = new List<Actor>
            {
                new Button(window, "Star Pig", Size * 1, Size * 1.5f, PlayPig, Player, false) { PlaySound = false },
                new Button(window, "easy", Size * 6, Size, Game.NewEasyGame, Player),
                new Button(window, "normal", Size * 9, Size, Game.NewNormalGame, Player),
                new Button(window, "hard", Size * 12, Size, Game.NewHardGame, Player),
                new Button(window, "exit", Size * 15, Size, Game.Exit, Player),
                Player,
            };

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

            Actors.ForEach(b => b.Draw(SpriteBatch));

            SpriteBatch.End();
        }


        public override void Update(GameTime gameTime)
        {
            if (!CanPlay)
            {
                SoundTimer += gameTime.GetElapsedSeconds();
                if (SoundTimer > SoundTime)
                {
                    CanPlay = true;
                    SoundTimer = 0;
                }
            }
            Actors.ForEach(b => b.Update(gameTime));
        }
    }
}
