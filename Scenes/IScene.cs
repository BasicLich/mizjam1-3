using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using mizjam1.Actors;
using System;
using System.Collections.Generic;

namespace mizjam1.Scenes
{
    internal interface IScene
    {
        public void Initialize(GameWindow window, GraphicsDevice graphicsDevice, ContentManager content);
        public void Update(GameTime gameTime);
        public void Draw(GameTime gameTime);

        void SizeChanged(int width, int height);
    }
}
