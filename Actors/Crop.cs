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
        internal float GrowthTime = 15;
        internal bool IsFirst = true;
        internal override void Update(GameTime gameTime)
        {
            if (Seeded && Growth < MaxGrowth)
            {
                GrowthTimer += gameTime.GetElapsedSeconds();
                if ((IsFirst && GrowthTimer > 1) || GrowthTimer > GrowthTime)
                {
                    GrowthTimer = 0;
                    Growth++;
                    if (Growth == MaxGrowth)
                    {
                        IsFirst = false;
                    }
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
