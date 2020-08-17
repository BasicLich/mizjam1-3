using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using mizjam1.Helpers;
using mizjam1.Sound;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mizjam1.Actors
{
    internal class Player : Actor
    {
        internal bool DoAction;
        internal bool HasWater = true;
        internal int Seeds = 1;

        internal override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            foreach(Carrot c in Scene.Actors.Where(c => c is Carrot carrot && carrot.CanBeCollected() && (c.Position - Position).LengthSquared() < Size * Size))
            {
                Scene.CreatePickUpParticle(c.Position);
                SoundPlayer.Instance.Play("PICK");
                Seeds++;
                Scene.RemoveActor(c);
            }
            if (DoAction)
            {
                Well well = (Well)Scene.Actors.Where(c => c is Well).OrderBy(c => (c.Position - Position).LengthSquared()).FirstOrDefault();
                Actor actor;

                var dist2 = (well.Position - Position).LengthSquared();
                if (dist2 >= (2 * Size) * (2 * Size) || !DoWellActions(well))
                {
                    actor = Scene.Actors.Where(c => c.Interactive).OrderBy(c => (c.Position - Position).LengthSquared()).FirstOrDefault();
                    dist2 = (actor.Position - Position).LengthSquared();
                } else
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
        }

        private bool DoWellActions(Well well)
        {
            if (HasWater)
            {
                return false;
            }
            HasWater = true;
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
                return true;
            }
            if (Seeds > 0 && crop.Watered && !crop.Seeded)
            {
                crop.Seeded = true;

                Scene.CreateSeedParticle(crop.Position);
                SoundPlayer.Instance.Play("DIRT");
                Seeds--;
                return true;
            }
            if (HasWater && !crop.Watered && !crop.Seeded)
            {
                crop.Watered = true;

                Scene.CreateSplashParticle(crop.Position);

                SoundPlayer.Instance.Play("WATERDROP");
                HasWater = false;
                return true;
            }
            if (crop.CanBeHarvested())
            {
                Scene.CreateSeedParticle(crop.Position);
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
            Scene.RemoveActor(bush);
            Scene.CreateCarrot(bush.Position);

            SoundPlayer.Instance.Play("CUT");
            return true;
        }

        internal override void Control()
        {
            base.Control();
            DoAction = Input.IsKeyJustPressed(Keys.J);
        }
    }
}
