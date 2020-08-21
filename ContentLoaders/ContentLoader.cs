using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using mizjam1.Sound;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.TextureAtlases;
using System;
using System.Collections.Generic;
using System.Text;

namespace mizjam1.ContentLoaders
{
    internal class ContentLoader
    {
        internal ContentManager Content;
        internal Texture2D Tileset, Numbers, Borders;
        internal BitmapFont Font;
        internal SoundEffect Dirt;
        internal TextureRegion2D[,] Border9Path;

        internal static ContentLoader Instance { get; set; } = new ContentLoader();
        internal void Init(ContentManager content)
        {
            Content = content;
            Tileset = Content.Load<Texture2D>("tileset");

            Font = Content.Load<BitmapFont>("font");

            Numbers = Content.Load<Texture2D>("numbers");
            Dirt = Content.Load<SoundEffect>("dirt");

            Borders = Content.Load<Texture2D>("borders");


            SoundPlayer.Instance.Init(
                   Content.Load<SoundEffect>("theme"),
                   Content.Load<SoundEffect>("gameover"),
                   Content.Load<SoundEffect>("win"),
                   Content.Load<SoundEffect>("dirt"),
                   Content.Load<SoundEffect>("water_pick"),
                   Content.Load<SoundEffect>("water_drop"),
                   Content.Load<SoundEffect>("pick"),
                   Content.Load<SoundEffect>("cut"),
                   Content.Load<SoundEffect>("chicken"),
                   Content.Load<SoundEffect>("pig"),
                   Content.Load<SoundEffect>("laser")
            );
            Border9Path = new TextureRegion2D[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Border9Path[i, j] = new TextureRegion2D(Instance.Borders, 16 * i, 16 * j, 16, 16);
                }
            }
        }
    }
}
