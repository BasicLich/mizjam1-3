using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;
using System.Text;

namespace mizjam1.Actors
{
    internal class Ship : Actor
    {
        internal int PartsMounted = -1;
        internal Sprite[] Parts = new Sprite[4];
        internal Sprite Fire;
        internal bool Fired;
        internal bool Flip;
        internal float FiredTimer = 0;
        internal float FiredTime = 0.25f;

        internal int AddPart(bool[] shipParts)
        {
            if (PartsMounted < 3 && shipParts[PartsMounted + 1])
            {
                PartsMounted++;
            }
            return PartsMounted;
        }
        internal override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (Fired)
            {
                FiredTimer += gameTime.GetElapsedSeconds();
                if (FiredTimer > FiredTime)
                {
                    FiredTimer = 0;
                    Flip = !Flip;
                }
            }
        }
        internal override void Draw(SpriteBatch spriteBatch)
        {
            if (Fired)
            {
                Fire.Depth = 0.9f;
                if (Flip)
                {
                    Fire.Effect = SpriteEffects.None;
                } else
                {
                    Fire.Effect = SpriteEffects.FlipHorizontally;
                }
                SpriteExtensions.Draw(
                        spriteBatch,
                        Fire,
                        new Vector2(
                            (int)Position.X - Fire.Origin.X,
                            (int)Position.Y - Fire.Origin.Y + Size / 2
                        )
                     );
            }

            for (int i = 0; i <= PartsMounted; i++)
            {
                Parts[i].Depth = 1f;

                SpriteExtensions.Draw(
                        spriteBatch,
                        Parts[i],
                        new Vector2(
                            (int)Position.X - Parts[i].Origin.X,
                            (int)Position.Y - Parts[i].Origin.Y - (Size * i)
                        )
                     );
            }

        }


    }
}
