using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using mizjam1.Actors;
using mizjam1.ContentLoaders;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
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
        internal TextureRegion2D HeartFull;
        internal TextureRegion2D HeartHalf;
        internal TextureRegion2D HeartEmpty;

        internal Player Player;
        internal GameWindow Window;
        internal OrthographicCamera Camera;
        internal BitmapFont Font;

        internal HUD(Player player, OrthographicCamera camera, GameWindow window)
        {
            Player = player;
            Camera = camera;
            Window = window;
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            var size = 32;
            float xOffset = Window.ClientBounds.Width / 2f;
            xOffset -= size * 7f;
            var scale = Vector2.One * size / 16;
            var p = new Vector2(xOffset, size / 2);

            var carrots = Math.Min(Player.Carrots, 999);
            var hundreds = carrots / 100;
            spriteBatch.Draw(Numbers[hundreds], p, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 1);
            p.X += size;

            var tens = (carrots % 100) / 10;
            spriteBatch.Draw(Numbers[tens], p, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 1);
            p.X += size;

            var units = carrots % 10;
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

            var gray = 61 / 255f;
            var grayColor = new Color(gray, gray, gray);
            var doneColor = new Color(90 / 255f, 197 / 255f, 79 / 255f);
            var ship = "ship";
            for (int i = 0; i < 4; i++)
            {
                var str = ship;
                var col = grayColor;
                if (Player.ShipParts[i])
                {
                    str = str.ToUpper();
                    col = Color.White;
                }
                if (Player.ShipPartsAdded[i])
                {
                    str = str.ToUpper();
                    col = doneColor;
                }
                spriteBatch.DrawString(Font, str.Substring(i, 1), p, col, 0, Vector2.Zero, scale, SpriteEffects.None, 1);
                p.X += size;
            }

            p.X += size;
            for (float i = 0; i < Player.MaxHealth / 2f; i++)
            {
                if ((Player.Health - 1) / 2f + 0.5f <= i)
                {
                    spriteBatch.Draw(HeartEmpty, p, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 1);
                }
                else if ((Player.Health - 1) / 2f <= i)
                {
                    spriteBatch.Draw(HeartHalf, p, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 1);
                }
                else
                {
                    spriteBatch.Draw(HeartFull, p, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 1);
                }
                p.X += size;
            }


            p.X += size;
            p.Y += 2 * size;


            int start = (int)(xOffset / size) - 1;
            int end = (int)(p.X / size);
            for (int i = start; i < end; i++)
            {
                var x = i == start ? 0 : i < (end - 1) ? 1 : 2;
                spriteBatch.Draw(ContentLoader.Instance.Border9Path[x, 0], new Vector2(size * i, -size / 2), Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0.99f);
                spriteBatch.Draw(ContentLoader.Instance.Border9Path[x, 1], new Vector2(size * i, size * 0.5f), Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0.99f);
                spriteBatch.Draw(ContentLoader.Instance.Border9Path[x, 2], new Vector2(size * i, size * 1.5f), Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0.99f);
            }

            //DrawText(spriteBatch, "press K to interact with crops", size, 3 * size);
            //DrawText(spriteBatch, "bushes and the well", size, 4 * size);


            if (Player.Health <= 0)
            {
                DrawText(spriteBatch, "game over", size, 3 * size);
                return;
            }
            if (!Player.CropDigged)
            {
                DrawText(spriteBatch, "press K to dig a crop", size, 3 * size);
            }
            else if (!Player.WaterPicked)
            {
                DrawText(spriteBatch, "press K pick up water", size, 3 * size);
            }
            else if (!Player.CropWatered)
            {
                DrawText(spriteBatch, "press K to water a crop", size, 3 * size);
            }
            else if (!Player.BushCollected)
            {
                DrawText(spriteBatch, "press K to shake the bushes", size, 3 * size);
            }
            else if (!Player.CarrotCollected)
            {
                DrawText(spriteBatch, "pick up the carrots", size, 3 * size);
            }
            else if (!Player.CropPlanted)
            {
                DrawText(spriteBatch, "press K to plant the carrot", size, 3 * size);
            }
            else if (!Player.CropHarvested)
            {
                DrawText(spriteBatch, "press K to harvest", size, 3 * size);
            }
            else if (!Player.PartCollected)
            {
                DrawText(spriteBatch, "find all the ship parts", size, 3 * size);
            }
            else if (!Player.PartPlanted)
            {
                DrawText(spriteBatch, "press L to place the part", size, 3 * size);
            }
            else if (!Player.Teleported)
            {
                DrawText(spriteBatch, "move towards the arrows", size, 3 * size);
                DrawText(spriteBatch, "explore the world", size, 4 * size, true);
                DrawText(spriteBatch, "remember where you started", size, 5 * size, true);
            }
            else if (!Player.Attacked)
            {
                DrawText(spriteBatch, "press J to attack while moving", size, 3 * size);
                DrawText(spriteBatch, "it costs a carrot", size, 4 * size, true);
            }
            else if (Player.TakenDamage && !Player.Healed)
            {
                DrawText(spriteBatch, "press H to heal", size, 3 * size);
                DrawText(spriteBatch, "it costs four carrots", size, 4 * size, true);
            }

        }

        private void DrawText(SpriteBatch spriteBatch, string text, int size, float height, bool shortTop = false)
        {
            if (Player.Position.Y > size)
            {
                float xOffset = Window.ClientBounds.Width / 2f;
                var dark = 16 / 255f;
                var str = text;
                var strWidth = Font.MeasureString(text).Width;
                var p = new Vector2(xOffset - strWidth / 2, height);
                spriteBatch.DrawString(Font, str, p, Color.White, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, 1);
                var bgColor = new Color(dark, dark, dark, 0.75f);
                var start = xOffset - strWidth / 2 - size * 0.5f;
                var end = strWidth + size;

                float startY = height;
                float endY = size;
                if (!shortTop)
                {
                    startY -= 0.5f * size;
                    endY *= 1.5f;
                }
                spriteBatch.FillRectangle(start, startY, end, endY, bgColor, 0);
            }
        }
    }
}
