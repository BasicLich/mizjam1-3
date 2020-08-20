using Microsoft.Xna.Framework;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;
using System.Text;

namespace mizjam1.Actors
{
    internal class ShipPart : Collectable
    {
        internal int Part;
        internal ShipPart(Vector2 position, Sprite sprite, int part) : base(position, sprite, false)
        {
            Part = part;
        }
    }
}
