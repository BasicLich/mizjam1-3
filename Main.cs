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
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
            _graphics.PreferredBackBufferWidth = 960;
            _graphics.PreferredBackBufferHeight = 960 * 9 / 10;        
        }

        protected override void Initialize()
        {
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
