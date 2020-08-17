

using Microsoft.Xna.Framework;
using mizjam1.Helpers;
using MonoGame.Extended.Sprites;
using System;

namespace mizjam1.Actors
{
    internal class Collectable : Actor
    {
        internal Vector2 TruePosition;
        internal float Z;
        internal float OriginalY;
        internal float SpeedZ = 5;
        internal float AccelerationZ = -50;
        internal bool Landed;
        internal float Counter = 0;
        internal float CollectingTimer = 0;
        internal float CollectingTime = 1;
        internal Collectable(Vector2 position, Sprite sprite)
        {
            Position = position;
            TruePosition = position;
            Speed = new Vector2(RandomHelper.NextFloat() - 0.5f, RandomHelper.NextFloat() - 0.5f) * 200;
            Moveable = true;
            Interactive = true;
            Sprite = sprite;
            SpeedZ = 10;
        }
        internal bool CanBeCollected()
        {
            return CollectingTimer >= CollectingTime;
        }
        internal override void Physics(float delta)
        {
            if (CollectingTimer < CollectingTime)
            {
                CollectingTimer += delta;
            }

            if (!Landed)
            {
                TruePosition += Speed * delta;
                SpeedZ += AccelerationZ * delta;
                Z += SpeedZ * delta;
                Position = TruePosition;
                Position.Y += Z;
                if (Z < 0)
                {
                    Z = 0;
                    Landed = true;
                }
            } else
            {
                Position.Y = 2 * MathF.Sin(Counter) + TruePosition.Y;
                Counter += delta * 3;
            }
        }
    }
}
