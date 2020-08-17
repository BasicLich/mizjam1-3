using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using mizjam1.Scenes;

namespace mizjam1
{
    public class Main : Game
    {
        private GraphicsDeviceManager _graphics;
        internal IScene Scene;
        internal int Width, Height;
        internal bool Resizing;

        public Main()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = true;

            _graphics.PreferredBackBufferWidth = 960;
            _graphics.PreferredBackBufferHeight = 768;

            Window.ClientSizeChanged += Window_ClientSizeChanged;
        
        }

        private void Window_ClientSizeChanged(object sender, System.EventArgs e)
        {
            _graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
            _graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
            _graphics.ApplyChanges();
            
            Scene?.SizeChanged(Window.ClientBounds.Width, Window.ClientBounds.Height);
        }

        protected override void Initialize()
        {
            Scene = new MapScene();

            Scene.Initialize(Window, GraphicsDevice, Content);
            base.Initialize();
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

            Scene.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Scene.Draw(gameTime);
        }
    }
}
