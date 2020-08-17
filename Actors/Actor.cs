using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using mizjam1.Helpers;
using mizjam1.Scenes;
using MonoGame.Extended;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mizjam1.Actors
{
    internal class Actor
    {
        internal MapScene Scene;
        internal Vector2 Position;
        internal Vector2 Speed;
        internal Vector2 Acceleration;
        internal float InverseFriction = 45f;
        internal float MaxSpeed = 50;
        internal float WalkAcceleration = 100;

        internal Sprite Sprite;

        internal float Size = 16;

        internal Dictionary<string, Animation> Animations;
        internal float Elapsed;
        internal string AnimationState = "IDLE";
        internal bool FlipX;

        internal bool Moveable = false;
        internal bool Animated = false;
        internal bool Controllable = false;
        internal bool Interactive = false;
        internal bool Collidable = false;

        internal bool Left, Right, Up, Down;

        internal void Remove()
        {
            Scene.RemoveActor(this);
        }

        internal virtual void Update(GameTime gameTime)
        {
            float delta = gameTime.GetElapsedSeconds();
            if (Controllable)
            {
                Control();
            }
            if (Moveable)
            {
                Physics(delta);
            }
            if (Animated)
            {
                Animate(delta);
            }
        }

        internal virtual void Control()
        {
            Left = Input.IsKeyDown(Keys.A);
            Right = Input.IsKeyDown(Keys.D);
            Up = Input.IsKeyDown(Keys.W);
            Down = Input.IsKeyDown(Keys.S);

            if (Left)
            {
                FlipX = true;
            }
            if (Right)
            {
                FlipX = false;
            }
            if (Down ^ Up)
            {
                if (Down)
                {
                    Acceleration.Y = WalkAcceleration;
                }
                if (Up)
                {
                    Acceleration.Y = -WalkAcceleration;
                }
            }
            else
            {
                Acceleration.Y = 0;
            }

            if (Left ^ Right)
            {
                if (Left)
                {
                    Acceleration.X = -WalkAcceleration;
                }
                if (Right)
                {
                    Acceleration.X = WalkAcceleration;
                }
            }
            else
            {
                Acceleration.X = 0;
            }
        }

        internal virtual void Animate(float delta)
        {
            if (Down || Up || Left || Right)
            {
                SetAnimation("WALK");
            }
            else
            {
                SetAnimation("IDLE");
            }

            Sprite = GetNextFrame(delta);
            if (FlipX)
            {
                Sprite.Effect = SpriteEffects.FlipHorizontally;
            }
            else
            {
                Sprite.Effect = SpriteEffects.None;
            }
        }

        internal virtual void Physics(float delta)
        {
            Speed += Acceleration * delta;
            if (Acceleration.X == 0 && Speed.X != 0)
            {
                var inverseFrictionAmmount = Math.Min(1, delta * InverseFriction);
                Speed.X *= inverseFrictionAmmount;
            }
            if (Acceleration.Y == 0 && Speed.Y != 0)
            {
                var inverseFrictionAmmount = Math.Min(1, delta * InverseFriction);
                Speed.Y *= inverseFrictionAmmount;
            }
            if (Speed.EqualsWithTolerence(Vector2.Zero))
            {
                Speed.X = 0;
                Speed.Y = 0;
            }
            if (Speed.LengthSquared() > MaxSpeed * MaxSpeed)
            {
                Speed.Normalize();
                Speed *= MaxSpeed;
            }
            if (Collidable && Speed.LengthSquared() > 0)
            {
                if (!Collide(Position + Speed * delta))
                {
                    Position += Speed * delta;
                }
            }
            else
            {
                Position += Speed * delta;
            }
        }
        internal bool Collide(Vector2 position)
        {
            var collided = false;
            foreach (var actor in Scene.Actors.Where(a => a.Collidable && a != this))
            {
                var diff = actor.Position - position;
                var dist2 = diff.LengthSquared();
                var radiiSum = (actor.Size / 2) + (Size / 2);
                if (dist2 > radiiSum * radiiSum)
                {
                    continue;
                }
                var dist = MathF.Sqrt(dist2);
                var distanceToMove = radiiSum - dist;
                diff.Normalize();
                Position = position - diff * distanceToMove;
                Speed.X = 0;
                Speed.Y = 0;
                collided = true;
            }
            return collided;
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            if (Sprite == null)
            {
                return;
            }
            SpriteExtensions.Draw(
                    spriteBatch,
                    Sprite,
                    new Vector2(
                        (int)Position.X - Sprite.Origin.X,
                        (int)Position.Y - Sprite.Origin.Y
                    )
                 );
        }

        internal void SetAnimation(string animation)
        {
            if (animation.Equals(AnimationState))
            {
                return;
            }
            Elapsed = 0;
            AnimationState = animation;
        }

        internal Sprite GetNextFrame(float elapsed)
        {
            Elapsed += elapsed;
            return Animations[AnimationState].GetFrame(Elapsed);
        }
    }
}
