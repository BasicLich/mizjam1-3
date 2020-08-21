using Microsoft.Xna.Framework;
using mizjam1.Helpers;
using mizjam1.Scenes;
using mizjam1.Sound;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mizjam1.Actors
{
    internal class Farmer : Actor
    {
        internal Player Player;
        internal float ChangeMovementTimer = 0;
        internal float ChangeMovementTime = 1;
        internal bool IsChicken;

        internal bool ChickenFlicker;
        internal float ChickenFlickerTimer = 0;
        internal float ChickenFlickerTime = 1;

        internal float ChickenTimer = 0;
        internal float ChickenTime = 10;

        internal float DamageTimer = 0;
        internal float DamageTime = 1;
        internal bool CanDamage = true;

        internal Farmer(Player player)
        {
            Player = player;
            Controllable = true;
            MaxSpeed = MaxSpeed * 0.5f;
            //IsChicken = true;
        }

        internal void ConvertToChicken()
        {
            IsChicken = true;
            ChickenTimer = 0;
            ChickenFlickerTimer = 0;
            ChickenFlicker = false;
        }

        internal override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            var delta = gameTime.GetElapsedSeconds();
            ChickenLogic(delta);
            if (!CanDamage)
            {
                DamageTimer += delta;
                if (DamageTimer > DamageTime)
                {
                    DamageTimer = 0;
                    CanDamage = true;
                }
            }
        }

        internal override void OnCollision(bool bounds = false)
        {
            base.OnCollision(bounds);

            if (IsChicken || !CanDamage || !(CollidedWith is Player))
            {
                return;
            }
            SoundPlayer.Instance.Play("PIG");
            Scene.CreatePickUpParticle(Scene.Player.Position);
            Scene.Player.Health--;
            Scene.Player.TakenDamage = true;
            CanDamage = false;
        }

        internal void ChickenLogic(float delta)
        {
            if (!IsChicken)
            {
                return;
            }

            ChickenTimer += delta;

            if (ChickenTimer > ChickenTime - ChickenFlickerTime)
            {
                ChickenFlickerTimer += delta;
                if (ChickenFlickerTimer > 0.0625f)
                {
                    ChickenFlicker = !ChickenFlicker;
                    ChickenFlickerTimer = 0;
                }
            }

            if (ChickenTimer > ChickenTime)
            {
                ChickenTimer = 0;
                IsChicken = false;
            }

        }

        internal override void Animate(float delta)
        {
            var type = "FARMER_";
            if (IsChicken && !ChickenFlicker)
            {
                type = "CHICKEN_";
            }

            if (Down || Up || Left || Right)
            {
                SetAnimation(type + "WALK");
            }
            else
            {
                SetAnimation(type + "IDLE");
            }


            GetNextSprite(delta);
        }
        internal void RandomControl(float delta)
        {
            ChangeMovementTimer += delta;
            if (ChangeMovementTime > ChangeMovementTimer)
            {
                return;
            }
            ChangeMovementTime = 0.5f + RandomHelper.NextFloat();
            ChangeMovementTimer = 0;
            Up = false;
            Down = false;
            Left = false;
            Right = false;

            var rY = RandomHelper.NextFloat();
            var rX = RandomHelper.NextFloat();
            if (rX < 0.125f)
            {
                Left = true;
            }
            else if (rY < 0.25f)
            {
                Right = true;
            }
            if (rY < 0.125f)
            {
                Up = true;
            }
            else if (rY < 0.25f)
            {
                Down = true;
            }
        }
        internal override void AIControl(float delta)
        {
            if (!Chunk.HasPlayer() || (Scene.Player.NTeleports == 1 && !Scene.Player.Attacked))
            {
                RandomControl(delta);
            }
            else
            {
                var defaultPress = IsChicken;
                GoTowards(Scene.Player.Position, defaultPress);
            }
        }
    }
}
