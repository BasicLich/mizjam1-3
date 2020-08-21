

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
        internal float CollectingTime = 0.5f;
        internal Collectable(Vector2 position, Sprite sprite, bool initialSpeed = true)
        {
            Position = position;
            TruePosition = position;
            if (initialSpeed)
            {
                Speed = new Vector2(RandomHelper.NextFloat() - 0.5f, RandomHelper.NextFloat() - 0.5f) * 100;
            }

            Moveable = true;
            Interactive = true;
            Sprite = sprite;
            SpeedZ = 10;
            CollisionGroup = 0b100;
            CollidesWith = 0b010;
            Collidable = true;
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
            if (Collidable && SpeedZ > 0)
            {
                if (!Collide(Position + Speed * delta))
                {
                    Position += Speed * delta;
                }
            }
            CheckBounds();
        }
    }
}
