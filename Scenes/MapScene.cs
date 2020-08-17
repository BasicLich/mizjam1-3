using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using mizjam1.Actors;
using mizjam1.Helpers;
using mizjam1.UI;
using MonoGame.Extended;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.TextureAtlases;
using MonoGame.Extended.ViewportAdapters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace mizjam1.Scenes
{
    internal class MapScene : IScene
    {
        internal GameWindow Window;
        internal GraphicsDevice GraphicsDevice;
        internal ContentManager Content;
        internal SpriteBatch SpriteBatch;
        internal OrthographicCamera Camera;
        internal ViewportAdapter ViewportAdapter;
        internal int GridSize = 20;
        internal TextureRegion2D[,] Floor;
        internal Texture2D Tileset;

        internal List<Actor> Actors;
        internal List<Actor> AddedActors;
        internal List<Actor> RemovedActors;
        internal Player Player;

        internal HUD HUD;

        public void Initialize(GameWindow window, GraphicsDevice graphicsDevice, ContentManager content)
        {
            Window = window;
            GraphicsDevice = graphicsDevice;
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            ViewportAdapter = new WindowViewportAdapter(Window, GraphicsDevice);
            Camera = new OrthographicCamera(ViewportAdapter)
            {
                Zoom = 4
            };
            Content = content;
            Tileset = Content.Load<Texture2D>("tileset");

            Actors = new List<Actor>();
            AddedActors = new List<Actor>();
            RemovedActors = new List<Actor>();
            CreatePlayer(true);
            CreateBushes(true);
            CreateWell(true);
            CreateFloor(true);
            var numbersTexture = Content.Load<Texture2D>("numbers");

            var numbers = new List<TextureRegion2D>();
            for (int i = 0; i < 10; i++)
            {
                numbers.Add(new TextureRegion2D(numbersTexture, 16 * i, 0, 16, 16));
            }
            HUD = new HUD(Player, Camera)
            {
                Numbers = numbers,
                Carrot = new TextureRegion2D(Tileset, 16 * 7, 0, 16, 16),
                WaterEmpty = new TextureRegion2D(Tileset, 16 * 5, 0, 16, 16),
                WaterFull = new TextureRegion2D(Tileset, 16 * 6, 0, 16, 16),
            };
        }
        internal void AddActor(Actor actor, bool instantly = false)
        {
            actor.Scene = this;
            if (instantly)
            {
                Actors.Add(actor);
            }
            else
            {
                AddedActors.Add(actor);
            }
        }
        internal void RemoveActor(Actor actor)
        {
            RemovedActors.Add(actor);
        }
        internal List<Actor> GetActors(Type type)
        {
            return Actors.Where(a => a.GetType() == type).ToList();
        }
        internal void CreateFloor(bool instantly = false)
        {
            Floor = new TextureRegion2D[GridSize, GridSize];

            for (int i = 0; i < GridSize; i++)
            {
                for (int j = 0; j < GridSize; j++)
                {
                    var x = (int)(RandomHelper.NextFloat() * 5);
                    Floor[i, j] = new TextureRegion2D(Tileset, 16 * x, 16 * 4, 16, 16);
                }
            }
        }

        internal void CreateWell(bool instantly = false)
        {
            var well = new Well()
            {
                Position = new Vector2(16, 16),
                Sprite = new Sprite(new TextureRegion2D(Tileset, 16 * 0, 16 * 5, 16, 16))
                {
                    OriginNormalized = Vector2.Zero,
                    Depth = 0.6f,
                },
                Collidable = true,
                Interactive = true
            };
            AddActor(well, instantly);
        }

        internal void CreateBushes(bool instantly = false)
        {
            for (int _ = 0; _ < 20; _++)
            {
                var bush = new Bush
                {
                    Position = new Vector2(16 * (int)(GridSize * RandomHelper.NextFloat()), 16 * (int)(GridSize * RandomHelper.NextFloat())),
                    Sprite = new Sprite(new TextureRegion2D(Tileset, 16 * (int)(RandomHelper.NextFloat() * 3), 16 * 3, 16, 16))
                    {
                        OriginNormalized = Vector2.Zero,
                        Depth = 0.6f,
                    },
                    Interactive = true
                };
                AddActor(bush, instantly);
            }
        }

        internal void CreatePlayer(bool instantly = false)
        {
            Player = new Player
            {
                Position = new Vector2(0, 0),
                Animations = Animation.GetPigAnimation(Tileset),
                Moveable = true,
                Controllable = true,
                Animated = true,
                Collidable = true
            };
            AddActor(Player, instantly);
        }

        internal void CreateCrop(Vector2 position, bool instantly = false)
        {
            var crop = new Crop
            {
                Position = position,
                AnimationState = "UNWATERED",
                Animations = Animation.GetCropAnimation(Tileset),
                Animated = true,
                Interactive = true
            };
            AddActor(crop, instantly);
        }

        internal void CreateCarrot(Vector2 position, bool instantly = false)
        {
            var sprite = new Sprite(new TextureRegion2D(Tileset, 16 * 7, 16 * 0, 16, 16))
            {
                OriginNormalized = Vector2.Zero,
                Depth = 0.6f,
            };
            var carrot = new Carrot(position, sprite);
            AddActor(carrot, instantly);
        }

        internal void CreateSplashParticle(Vector2 position, bool instantly = false)
        {
            position.Y -= 8;
            var particle = new Particle
            {
                Position = position,
                Animations = Animation.GetSplashAnimation(Tileset),
                Animated = true
            };
            AddActor(particle, instantly);
        }
        internal void CreateSeedParticle(Vector2 position, bool instantly = false)
        {
            position.Y -= 8;
            var particle = new Particle
            {
                Position = position,
                Animations = Animation.GetSeedAnimation(Tileset),
                Animated = true
            };
            AddActor(particle, instantly);
        }

        internal void CreatePickUpParticle(Vector2 position, bool instantly = false)
        {
            var particle = new Particle
            {
                Position = position,
                Animations = Animation.GetPickUpAnimation(Tileset),
                Animated = true
            };
            AddActor(particle, instantly);
        }

        public void Update(GameTime gameTime)
        {
            Input.Update(Keyboard.GetState());
            Actors.AddRange(AddedActors);
            AddedActors.Clear();

            Actors.ForEach(a => a.Update(gameTime));
            Actors.RemoveAll(a => RemovedActors.Contains(a));

            RemovedActors.Clear();
            var camPos = Player.Position;

            Camera.LookAt(camPos);
        }
        public void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            var transformMatrix = Camera.GetViewMatrix();
            SpriteBatch.Begin(transformMatrix: transformMatrix, samplerState: SamplerState.PointClamp, sortMode: SpriteSortMode.FrontToBack);
            var screenZero = Camera.ScreenToWorld(Vector2.Zero);
            HUD.Draw(SpriteBatch);
            for (int i = 0; i < GridSize; i++)
            {
                for (int j = 0; j < GridSize; j++)
                {
                    SpriteBatch.Draw(Floor[i, j], new Vector2(i * 16, j * 16), Color.White, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, 0);
                }
            }

            Actors.ForEach(a => a.Draw(SpriteBatch));

            SpriteBatch.End();
        }

        public void SizeChanged(int width, int height)
        {
            ViewportAdapter.Reset();
        }
    }
}
