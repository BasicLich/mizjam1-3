using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using mizjam1.Scenes;

namespace mizjam1
{
    public class Main : Game
    {
        private GraphicsDeviceManager _graphics;
        internal Scene Scene;
        internal int Width, Height;
        internal bool Resizing;

        public Main()
        {
            Window.AllowUserResizing = true;

            _graphics = new GraphicsDeviceManager(this);


            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 544;
            _graphics.PreferredBackBufferHeight = 544 + 64;
            _graphics.ApplyChanges();
            Scene = new MapScene();

            Scene.Initialize(Window, GraphicsDevice, Content);
            base.Initialize();

            Scene.ViewportAdapter.Reset();
        }

        protected override void LoadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }
            if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                Scene = new MapScene();
                Scene.Initialize(Window, GraphicsDevice, Content);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.F12))
            {
                _graphics.ToggleFullScreen();
            }
            Scene.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Scene.Draw(gameTime);
        }
    }
}
