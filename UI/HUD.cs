using Microsoft.Xna.Framework;
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

        internal HUD(Player player, OrthographicCamera camera)
        {
            Player = player;
            Camera = camera;
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            var p = new Vector2();

            var hundreds = Player.Seeds / 100;
            spriteBatch.Draw(Numbers[hundreds], Camera.ScreenToWorld(p), Color.White, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, 1);
            p.X += 16 * Camera.Zoom;

            var tens = (Player.Seeds % 100) / 10;
            spriteBatch.Draw(Numbers[tens], Camera.ScreenToWorld(p), Color.White, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, 1);           
            p.X += 16 * Camera.Zoom;

            var units = Player.Seeds % 10;
            spriteBatch.Draw(Numbers[units], Camera.ScreenToWorld(p), Color.White, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, 1);
            p.X += 16 * Camera.Zoom;

            spriteBatch.Draw(Carrot, Camera.ScreenToWorld(p), Color.White, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, 1);
            p.X += 16 * Camera.Zoom * 2;
            if (Player.HasWater)
            {
                spriteBatch.Draw(WaterFull, Camera.ScreenToWorld(p), Color.White, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, 1);
            } else
            {
                p.X += 16 * Camera.Zoom;
            }
            p.X += 16 * Camera.Zoom;

        }
    }
}
