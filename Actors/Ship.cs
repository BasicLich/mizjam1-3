using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        internal int AddPart(bool[] shipParts)
        {
            if (PartsMounted < 3 && shipParts[PartsMounted + 1])
            {
                PartsMounted++;
            }
            return PartsMounted;
        }
        internal override void Draw(SpriteBatch spriteBatch)
        {
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
