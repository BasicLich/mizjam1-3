using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Text;

namespace mizjam1.Actors
{
    internal class Crop : Actor
    {
        internal bool Watered;
        internal bool Seeded;
        internal int Growth;
        internal int MaxGrowth = 2;
        internal float GrowthTimer;
        internal float GrowthTime = 2;

        internal override void Update(GameTime gameTime)
        {
            if (Seeded && Growth < MaxGrowth)
            {
                GrowthTimer += gameTime.GetElapsedSeconds();
                if (GrowthTimer > GrowthTime)
                {
                    GrowthTimer = 0;
                    Growth++;
                }
            }
            base.Update(gameTime);

        }
        internal override void Animate(float delta)
        {
            if (Seeded)
            {
                AnimationState = "GROW_" + Growth;
            }
            else if (Watered)
            {
                AnimationState = "WATERED";
            } else
            {
                AnimationState = "UNWATERED";
            }

            Sprite = GetNextFrame(delta);
        }

        internal bool CanBeHarvested()
        {
            return Growth == MaxGrowth;
        }
    }
}
