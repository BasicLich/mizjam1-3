using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using mizjam1.Actors;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using System;
using System.Collections.Generic;

namespace mizjam1.Scenes
{
    internal abstract class Scene
    {
        internal GameWindow Window;
        internal GraphicsDevice GraphicsDevice;
        internal SpriteBatch SpriteBatch;
        internal OrthographicCamera Camera;
        internal ViewportAdapter ViewportAdapter;
        internal Main Game;
        public virtual void Initialize(GameWindow window, GraphicsDevice graphicsDevice, ContentManager content, Main main)
        {
            Game = main;
            Window = window;
            GraphicsDevice = graphicsDevice;
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            ViewportAdapter = new DefaultViewportAdapter(GraphicsDevice);
            Camera = new OrthographicCamera(ViewportAdapter)
            {
                Zoom = 2
            };
            Window.ClientSizeChanged += (s, e) => ViewportAdapter.Reset();
        }
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime);
    }
}
