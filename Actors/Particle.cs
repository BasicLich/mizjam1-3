using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Text;

namespace mizjam1.Actors
{
    internal class Particle : Actor
    {
        internal float Timer;
        internal float Time = 0.6f;

        internal override void Update(GameTime gameTime)
        {
            float delta = gameTime.GetElapsedSeconds();
            Timer += delta;
            if (Timer > Time)
            {
                Scene.RemoveActor(this);
            }

            Animate(delta);
        }
    }
}
