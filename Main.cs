using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using mizjam1.ContentLoaders;
using mizjam1.Helpers;
using mizjam1.Scenes;
using mizjam1.Sound;

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
            ContentLoader.Instance.Init(Content);

            _graphics.PreferredBackBufferWidth = 16 * 16 * 2;
            _graphics.PreferredBackBufferHeight = 16 * 16 * 2 + 64;
            _graphics.ApplyChanges();
            NewMenu();
            base.Initialize();

            Scene.ViewportAdapter.Reset();
        }

        protected override void LoadContent()
        {
        }

        internal void NewEasyGame()
        {
            Scene = new MapScene(16, 3, 1);
            Scene.Initialize(Window, GraphicsDevice, Content, this);
        }
        internal void NewNormalGame()
        {
            Scene = new MapScene(24, 4, 3);
            Scene.Initialize(Window, GraphicsDevice, Content, this);
        }

        internal void NewHardGame()
        {
            Scene = new MapScene(32, 5, 5);
            Scene.Initialize(Window, GraphicsDevice, Content, this);
        }
        internal void NewMenu()
        {
            Scene = new MenuScene();
            Scene.Initialize(Window, GraphicsDevice, Content, this);
        }
        protected override void Update(GameTime gameTime)
        {
            if (Input.IsKeyJustPressed(Keys.M))
            {
                SoundPlayer.Instance.Toggle();
            }
            Input.Update(Keyboard.GetState());

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }
            if (Input.IsKeyJustPressed(Keys.R))
            {
                NewMenu();
            }
            if (Input.IsKeyJustPressed(Keys.F12))
            {
                _graphics.ToggleFullScreen();
            }
            Scene.Update(gameTime);
            base.Update(gameTime);

            SoundPlayer.Instance.Play();
        }

        protected override void Draw(GameTime gameTime)
        {
            Scene.Draw(gameTime);
        }
    }
}
