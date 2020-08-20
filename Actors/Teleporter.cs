using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace mizjam1.Actors
{
    internal class Teleporter : Actor
    {
        internal Point Chunk;
        internal Vector2 ChunkPosition;
        internal int rotation;

        internal void Teleport()
        {
            Scene.TeleportTo(Chunk, ChunkPosition);
        }
    }
}
