using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using mizjam1.Actors;
using MonoGame.Extended;
using MonoGame.Extended.TextureAtlases;
using System;
using System.Collections.Generic;
using System.Text;

namespace mizjam1.UI
{
    internal class HUD
    {
        internal List<TextureRegion2D> Numbers;
        internal TextureRegion2D Carrot;
        internal TextureRegion2D WaterEmpty;
        internal TextureRegion2D WaterFull;
        internal Player Player;
        internal OrthographicCamera Camera;
        internal TextureRegion2D[,] Borders;
        internal HUD(Player player, OrthographicCamera camera)
        {
            Player = player;
            Camera = camera;
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            var size = 24;
            var scale = Vector2.One * size / 16;
            var p = new Vector2(size, size);

            var hundreds = Player.Seeds / 100;
            spriteBatch.Draw(Numbers[hundreds], p, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 1);
            p.X += size;

            var tens = (Player.Seeds % 100) / 10;
            spriteBatch.Draw(Numbers[tens], p, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 1);
            p.X += size;

            var units = Player.Seeds % 10;
            spriteBatch.Draw(Numbers[units], p, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 1);
            p.X += size;

            spriteBatch.Draw(Carrot, p, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 1);
            p.X += size * 2;
            if (Player.HasWater)
            {
                spriteBatch.Draw(WaterFull, p, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 1);
            }
            else
            {
                spriteBatch.Draw(WaterEmpty, p, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 1);
            }
            p.X += 2 * size;
            p.Y += 2 * size;
            for (int i = 0; i < 8; i++)
            {
                var x = i == 0 ? 0 : i < 7 ? 1 : 2;
                spriteBatch.Draw(Borders[x, 0], new Vector2(size * i, 0), Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0.99f);
                spriteBatch.Draw(Borders[x, 1], new Vector2(size * i, size * 1), Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0.99f);
                spriteBatch.Draw(Borders[x, 2], new Vector2(size * i, size * 2), Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0.99f);
            }
        }
    }
}
