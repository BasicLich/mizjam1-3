using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Text;

namespace mizjam1.Actors
{
    internal class Bullet : Actor
    {
        internal Bullet(Vector2 position, Vector2 speed)
        {
            Position = position;
            
            Speed = speed.NormalizedCopy();
            Position += Speed * Size / 2;
            Speed *= 100;
            Acceleration = Speed;
            MaxSpeed *= 25;
            Moveable = true;
            DestroyOnCollision = true;
            Collidable = true;
            CollisionGroup = 0b1000;
            CollidesWith = 0b0010;
            Rotation = Speed.ToAngle();
        }

        internal override void OnCollision(bool bounds = false)
        {
            base.OnCollision(bounds);
            if (CollidedWith is Farmer farmer)
            {
                farmer.ConvertToChicken();
                Scene.GetCurrentChunk().CreateSeedParticle(Position);
                Sound.SoundPlayer.Instance.Play("CHICKEN");
            }
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            //spriteBatch.DrawCircle(Position, Size / 2, 16, Color.Red, 1, 1);
        }
    }
}
