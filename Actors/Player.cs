using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using mizjam1.ContentLoaders;
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
    internal class Player : Actor
    {
        internal bool DoInteraction;
        internal bool DoShipAction;
        internal bool HasWater;
        internal int Carrots;
        internal bool Teleporting;
        internal bool CanTeleport = true;
        internal float TeleportCooldownTimer = 0;
        internal float TeleportCooldownTime = 1;
        internal bool[] ShipParts = new bool[4];
        internal bool[] ShipPartsAdded = new bool[4];

        internal int MaxHealth = 4;
        internal int Health = 4;

        internal bool CropDigged, CropWatered, CropHarvested, CropPlanted, BushCollected, CarrotCollected, WaterPicked;
        internal bool Teleported;
        internal bool Died, TakenDamage, Attacked, Healed;
        internal bool PartCollected, PartPlanted, AllPartsCollected, AllPartsPlanted;
        internal int NTeleports = 0;

        internal Player(Vector2 position)
        {

            Position = position;
            Animations = Animation.GetPigAnimation(ContentLoader.Instance.Tileset);
            Moveable = true;
            Controllable = true;
            PlayerControllable = true;
            Animated = true;
            Collidable = true;
            CanShoot = true;
            CollisionGroup = 0b01;
            CollidesWith = 0b01;
        }
        internal override void Update(GameTime gameTime)
        {
            if (Health < 0)
            {
                Died = true;
                Scene.GameOver();
                return;
            }
            if (AllPlanted())
            {
                PlayerControllable = false;
                var ship = (Ship)Scene.GetCurrentChunk().Actors.FirstOrDefault(a => a is Ship);
                ship.Collidable = false;
                Collidable = false;
                WalkAcceleration = 10;
                var diff = ship.Position - Position;
                if (diff.LengthSquared() > Size / 2)
                {
                    GoTowards(ship.Position, false, 0);
                    diff.Normalize();
                    Speed = diff * MaxSpeed / 2f;
                    FlipX = Speed.X < 0;
                    SoundPlayer.Instance.SetWin();
                    if (!SoundPlayer.Instance.IsMuted && SoundPlayer.Instance.IsSongEnded())
                    {
                        Scene.Win();
                    }
                    else if (SoundPlayer.Instance.IsMuted && ship.Position.Y < -Size * 2)
                    {
                        Scene.Win();
                    }
                }
                else
                {

                    Moveable = false;
                    Animated = false;
                    Sprite = null;
                    ship.Moveable = true;
                    ship.BorderCollide = false;
                    ship.MaxSpeed = 40;
                    ship.Acceleration.Y = -100;
                    ship.Fired = true;

                }
            }
            base.Update(gameTime);
            if (AllPlanted())
            {
                return;
            }
            if (Teleporting)
            {
                CanTeleport = false;
                TeleportCooldownTimer += gameTime.GetElapsedSeconds();
                if (TeleportCooldownTimer > TeleportCooldownTime)
                {
                    Teleporting = false;
                    CanTeleport = true;
                    NTeleports++;
                    TeleportCooldownTimer = 0;
                }
                return;
            }
            var dist2 = Size * 1.5f;
            dist2 *= dist2;
            if (CanTeleport)
            {
                foreach (Teleporter t in Scene.GetActors().Where(c => c is Teleporter && (c.Position - Position).LengthSquared() < Size * Size))
                {
                    t.Teleport();
                    CanTeleport = false;
                    Teleported = true;
                }
            }
            if (Scene != null)
            {
                foreach (Carrot c in Scene.GetActors().Where(c => c is Carrot carrot && carrot.CanBeCollected() && (c.Position - Position).LengthSquared() < dist2))
                {
                    Scene.CreatePickUpParticle(c.Position);
                    SoundPlayer.Instance.Play("PICK");
                    Carrots++;
                    CarrotCollected = true;
                    Scene.RemoveActor(c);
                }
                foreach (ShipPart c in Scene.GetActors().Where(c => c is ShipPart carrot && (c.Position - Position).LengthSquared() < dist2))
                {
                    PartCollected = true;
                    Scene.CreatePickUpParticle(c.Position);
                    SoundPlayer.Instance.Play("PICK");
                    ShipParts[c.Part] = true;
                    Scene.RemoveActor(c);
                }
            }
            Interaction();
            ShipAction();
        }
        internal override void Heal()
        {
            if (Carrots >= 4 && Health < MaxHealth)
            {
                Carrots -= 4;
                Health++;
                Healing = false;
                Healed = true;

                Scene.CreateSeedParticle(Position);
                //TODO: Add sound
            }
            SoundPlayer.Instance.Play("PIG");

        }
        internal void Interaction()
        {
            if (!DoInteraction || Scene == null)
            {
                return;
            }
            Well well = (Well)Scene.GetActors().Where(c => c is Well).OrderBy(c => (c.Position - Position).LengthSquared()).FirstOrDefault();
            Actor actor = null;
            float dist2 = float.MaxValue;
            if (well != null)
            {
                dist2 = (well.Position - Position).LengthSquared();
            }
            if (dist2 >= (2 * Size) * (2 * Size) || (well != null && !DoWellActions(well)))
            {
                actor = Scene.GetActors().Where(c => c.Interactive).OrderBy(c => (c.Position - Position).LengthSquared()).FirstOrDefault();
                dist2 = (actor.Position - Position).LengthSquared();
            }
            else
            {
                return;
            }
            if (actor == null || dist2 >= Size * Size)
            {
                DoCropActions(null);
                return;
            }
            else
            {
                if (actor is Crop crop)
                {
                    if (DoCropActions(crop))
                    {
                        return;
                    }
                }
                if (actor is Bush bush)
                {
                    if (DoBushActions(bush))
                    {
                        return;
                    }
                }

                //DoCropActions(null);
            }
        }
        private void ShipAction()
        {
            if (!DoShipAction || Scene == null)
            {
                return;
            }
            if (!ShipParts[0])
            {
                return;
            }
            Ship ship = (Ship)Scene.GetActors().FirstOrDefault(c => c is Ship && (c.Position - Position).LengthSquared() < (Size * 1.25f) * (Size * 1.25f));
            if (ShipPartsAdded[0] && ship != null)
            {
                var max = ship.AddPart(ShipParts);
                for (int i = 0; i <= max; i++)
                {
                    ShipPartsAdded[i] = true;
                }
                SoundPlayer.Instance.Play("WATERDROP");
            }
            else if (!ShipPartsAdded[0])
            {
                var pos = new Vector2((int)(Position.X / Size), (int)(Position.Y / Size)) * Size;
                Vector2 closest = Position;
                var dist = float.MaxValue;
                for (int i = -1; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        if (i == 0 && j == 0)
                        {
                            continue;
                        }
                        var v = pos + new Vector2(i * Size, j * Size);
                        if (Scene.GetActors().Any(a => a.Position == v))
                        {
                            continue;
                        }
                        var d = v.Dist(Position);
                        if (d < dist)
                        {
                            dist = d;
                            closest = v;
                        }
                    }
                }
                Scene.GetCurrentChunk().CreateShip(closest);
                ShipPartsAdded[0] = true;
                SoundPlayer.Instance.Play("WATERDROP");
                PartPlanted = true;
            }
        }
        private bool DoWellActions(Well well)
        {
            if (HasWater)
            {
                return false;
            }
            HasWater = true;
            WaterPicked = true;
            Scene.CreateSplashParticle(well.Position);

            SoundPlayer.Instance.Play("WATERPICK");
            return true;
        }
        private bool DoCropActions(Crop crop)
        {
            if (crop == null)
            {
                var intPos = Position / Size;
                var position = new Vector2((float)(Size * Math.Round(intPos.X)), (float)(Size * Math.Round(intPos.Y)));
                Scene.CreateCrop(position);
                SoundPlayer.Instance.Play("DIRT");
                CropDigged = true;
                return true;
            }
            if (Carrots > 0 && crop.Watered && !crop.Seeded)
            {
                crop.Seeded = true;

                Scene.CreateSeedParticle(crop.Position);
                SoundPlayer.Instance.Play("DIRT");
                CropPlanted = true;
                Carrots--;
                return true;
            }
            if (HasWater && !crop.Watered && !crop.Seeded)
            {
                crop.Watered = true;
                CropWatered = true;
                Scene.CreateSplashParticle(crop.Position);

                SoundPlayer.Instance.Play("WATERDROP");
                HasWater = false;
                return true;
            }
            if (crop.CanBeHarvested())
            {
                Scene.CreateSeedParticle(crop.Position);
                CropHarvested = true;
                crop.Watered = false;
                crop.Seeded = false;
                crop.Growth = 0;
                crop.GrowthTimer = 0;

                SoundPlayer.Instance.Play("CUT");
                for (int _ = 0; _ < 4; _++)
                {
                    Scene.CreateCarrot(crop.Position);
                }
                return true;
            }
            return false;
        }

        private bool DoBushActions(Bush bush)
        {
            if (bush == null)
            {
                return false;
            }
            //Seeds++;
            BushCollected = true;
            Scene.RemoveActor(bush);
            Scene.CreateCarrot(bush.Position);

            SoundPlayer.Instance.Play("CUT");
            return true;
        }
        internal override void Shoot()
        {
            if (Speed.LengthSquared() > Size && Carrots > 0)
            {
                Attacked = true;
                Scene.GetCurrentChunk().CreateBullet();
                SoundPlayer.Instance.Play("LASER");
                Carrots--;
            }
        }
        internal bool AllPlanted()
        {
            return ShipPartsAdded.All(p => p);
        }
        internal override void Control()
        {
            base.Control();
            if (AllPlanted())
            {
                return;
            }
            DoInteraction = Input.IsKeyJustPressed(Keys.K);
            DoShipAction = Input.IsKeyJustPressed(Keys.L);
            Shooting = Input.IsKeyJustPressed(Keys.J);
            Healing = Input.IsKeyJustPressed(Keys.H);
        }

    }
}
