using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using mizjam1.Actors;
using MonoGame.Extended.ViewportAdapters;
using System;
using System.Collections.Generic;

namespace mizjam1.Scenes
{
    internal abstract class Scene
    {
        internal ViewportAdapter ViewportAdapter;
        public abstract void Initialize(GameWindow window, GraphicsDevice graphicsDevice, ContentManager content);
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime);
    }
}
