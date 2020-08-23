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
        internal Chunk Chunk;

        internal Vector2 Position;
        internal Vector2 Speed;
        internal Vector2 Acceleration;
        internal float InverseFriction = 45f;
        internal float MaxSpeed = 50;
        internal float WalkAcceleration = 2000;

        internal Sprite Sprite;
        internal float Rotation = 0;

        internal float Size = 16;

        internal float Scale = 1;

        internal Dictionary<string, Animation> Animations;
        internal float Elapsed;
        internal string AnimationState = "IDLE";
        internal bool FlipX;

        internal bool Moveable = false;
        internal bool Animated = false;
        internal bool PlayerControllable = false;
        internal bool Controllable = false;
        internal bool Interactive = false;
        internal bool Collidable = false;
        internal bool BorderCollide = true;
        internal bool DestroyOnCollision = false;

        internal int CollisionGroup = 1;
        internal int CollidesWith = 1;

        internal bool CanShoot = false;
        internal bool Shooting = false;

        internal bool Healing = false;

        internal Actor CollidedWith;

        internal bool Left, Right, Up, Down;


        internal void Remove()
        {
            if (Scene == null)
            {
                return;
            }
            Scene.RemoveActor(this);
        }

        internal virtual void Update(GameTime gameTime)
        {
            float delta = gameTime.GetElapsedSeconds();
            if (Controllable)
            {

                if (PlayerControllable)
                {
                    Left = Input.IsKeyDown(Keys.A) || Input.IsKeyDown(Keys.Left);
                    Right = Input.IsKeyDown(Keys.D) || Input.IsKeyDown(Keys.Right);
                    Up = Input.IsKeyDown(Keys.W) || Input.IsKeyDown(Keys.Up);
                    Down = Input.IsKeyDown(Keys.S) || Input.IsKeyDown(Keys.Down);
                }
                else
                {
                    AIControl(delta);
                }

                Control();

            }
            if (Moveable)
            {
                Physics(delta);
            }
            if (CanShoot && Shooting)
            {
                Shoot();
            }
            if (Animated)
            {
                Animate(delta);
            }
            if (Healing)
            {
                Heal();
            }
        }
        internal virtual void Heal()
        {

        }
        internal virtual void Shoot()
        {

        }
        internal virtual void AIControl(float delta)
        {

        }
        internal virtual void Control()
        {
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

            GetNextSprite(delta);
        }

        internal void GetNextSprite(float delta)
        {
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
                else
                {
                    OnCollision();
                }
            }
            else
            {
                Position += Speed * delta;
            }
            if (BorderCollide)
            {
                CheckBounds();
            }
        }

        internal virtual void OnCollision(bool bounds = false)
        {
            if (DestroyOnCollision)
            {
                Scene.GetCurrentChunk().RemoveActor(this);
            }

        }
        internal void CheckBounds()
        {
            bool bounds = false;
            if (Position.X < 0)
            {
                Position.X = 0;
                bounds = true;
            }
            if (Position.Y < 0)
            {
                Position.Y = 0;
                bounds = true;
            }
            if (Position.X > Scene.TileSize * (Scene.GridSize - 1))
            {
                Position.X = Scene.TileSize * (Scene.GridSize - 1);
                bounds = true;
            }
            if (Position.Y > Scene.TileSize * (Scene.GridSize - 1))
            {
                Position.Y = Scene.TileSize * (Scene.GridSize - 1);
                bounds = true;
            }
            if (Collidable && bounds)
            {
                OnCollision(true);
            }
        }

        internal bool Collide(Vector2 position)
        {
            var collided = false;
            if (Scene == null)
            {
                return false;
            }

            foreach (var actor in Scene.GetActors().Where(a => a.Collidable && ((a.CollisionGroup & CollidesWith) > 0) && a != this))
            {
                var diff = actor.Position - position;
                var dist2 = diff.LengthSquared();
                var radiiSum = (actor.Size * 0.5f) + (Size * 0.5f);
                if (dist2 > radiiSum * radiiSum)
                {
                    continue;
                }
                var dist = MathF.Sqrt(dist2);
                var distanceToMove = radiiSum - dist;
                diff.Normalize();
                position -= diff * distanceToMove;
                //Speed.X *= -1f;
                //Speed.Y *= -1f;
                CollidedWith = actor;
                collided = true;
            }
            if (collided)
            {
                Position = position;
            }

            return collided;
        }

        internal virtual void Draw(SpriteBatch spriteBatch)
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
                    ),
                    Rotation,
                    Vector2.One * Scale
                 );

            //spriteBatch.DrawCircle(Position + new Vector2(Size * Scale / 2f, Size * Scale / 2f), Size * Scale / 2f, 16, Color.Red, 1, 1);
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

        internal void GoTowards(Vector2 pos, bool defaultPress = false, float? margin = null)
        {
            Up = defaultPress;
            Down = defaultPress;
            Left = defaultPress;
            Right = defaultPress;
            if (!margin.HasValue)
            {
                margin = Size / 2;
            }
            if (pos.X > Position.X + margin)
            {
                Right = !defaultPress;
            }
            else if (pos.X < Position.X - margin)
            {
                Left = !defaultPress;
            }
            if (pos.Y > Position.Y + margin)
            {
                Down = !defaultPress;
            }
            else if (pos.Y < Position.Y - margin)
            {
                Up = !defaultPress;
            }
        }
    }
}
