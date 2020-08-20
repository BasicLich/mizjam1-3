using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using mizjam1.Actors;
using mizjam1.Helpers;
using mizjam1.Sound;
using mizjam1.UI;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.TextureAtlases;
using MonoGame.Extended.ViewportAdapters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace mizjam1.Scenes
{
    internal class MapScene : Scene
    {
        internal GameWindow Window;
        internal GraphicsDevice GraphicsDevice;
        internal ContentManager Content;
        internal SpriteBatch SpriteBatch;
        internal OrthographicCamera Camera;
        internal int ChunkSize = 5;
        internal int GridSize = 32;
        internal int TileSize = 16;
        internal TextureRegion2D[,] Floor;
        internal Texture2D Tileset;


        internal Chunk[,] Chunks;
        internal Point CurrentChunk;
        internal Vector2 LastCameraPos;

        internal Player Player;
        internal BitmapFont Font;
        internal HUD HUD;
        internal bool IsGameOver;
        public override void Initialize(GameWindow window, GraphicsDevice graphicsDevice, ContentManager content)
        {
            Window = window;
            GraphicsDevice = graphicsDevice;
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            ViewportAdapter = new DefaultViewportAdapter(GraphicsDevice);
            Camera = new OrthographicCamera(ViewportAdapter)
            {
                Zoom = 2
            };
            Window.ClientSizeChanged += (s, e) => ViewportAdapter.Reset();

            Content = content;
            Tileset = Content.Load<Texture2D>("tileset");

            var dirt = Content.Load<SoundEffect>("dirt");
            SoundPlayer.Instance.Init(
                Content.Load<SoundEffect>("dirt"),
                Content.Load<SoundEffect>("water_pick"),
                Content.Load<SoundEffect>("water_drop"),
                Content.Load<SoundEffect>("pick"),
                Content.Load<SoundEffect>("cut"),
                Content.Load<SoundEffect>("chicken"),
                Content.Load<SoundEffect>("pig"),
                Content.Load<SoundEffect>("laser")

                );



            var numbersTexture = Content.Load<Texture2D>("numbers");

            var numbers = new List<TextureRegion2D>();
            for (int i = 0; i < 10; i++)
            {
                numbers.Add(new TextureRegion2D(numbersTexture, TileSize * i, 0, TileSize, TileSize));
            }
            var bordersTexture = Content.Load<Texture2D>("borders");
            var borders = new TextureRegion2D[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    borders[i, j] = new TextureRegion2D(bordersTexture, TileSize * i, TileSize * j, TileSize, TileSize);
                }
            }
            Chunks = new Chunk[ChunkSize, ChunkSize];
            var chunkList = new List<Chunk>();

            CurrentChunk = new Point((int) (RandomHelper.NextFloat() * ChunkSize), (int)(RandomHelper.NextFloat() * ChunkSize));
            var wellChunk = new Point((int)(RandomHelper.NextFloat() * ChunkSize), (int)(RandomHelper.NextFloat() * ChunkSize));
            var shipParts = new List<Point>
            {
                CurrentChunk,
                new Point((int)(RandomHelper.NextFloat() * ChunkSize), (int)(RandomHelper.NextFloat() * ChunkSize)),
                new Point((int)(RandomHelper.NextFloat() * ChunkSize), (int)(RandomHelper.NextFloat() * ChunkSize)),
                new Point((int)(RandomHelper.NextFloat() * ChunkSize), (int)(RandomHelper.NextFloat() * ChunkSize)),
            };

            for (int i = 0; i < ChunkSize; i++)
            {
                for (int j = 0; j < ChunkSize; j++)
                {
                    var p = new Point(i, j);
                    bool spawnPlayer = p == CurrentChunk;
                    bool up = j > 0;
                    bool left = i > 0;
                    bool right = i < ChunkSize - 1;
                    bool down = j < ChunkSize - 1;

                    bool hasShipPart = shipParts.Contains(p);
                    int shipPart = -1;
                    if (hasShipPart)
                    {
                        shipPart = shipParts.IndexOf(p);
                    }
                    Chunks[i, j] = new Chunk(this, new Point(i, j), spawnPlayer, hasShipPart, shipPart, up, left, right, down, GridSize, TileSize);
                    chunkList.Add(Chunks[i, j]);
                }
            }
            foreach (var chunk in chunkList)
            {
                if (chunk.Up)
                {
                    var next = chunk.Position + new Point(0, -1);
                    var nextChunk = Chunks[next.X, next.Y];
                    var teleporter = new Teleporter
                    {
                        Position = new Vector2(chunk.Dungeon.OpeningLocations[0].X * TileSize, chunk.Dungeon.OpeningLocations[0].Y * TileSize),
                        Chunk = next,
                        ChunkPosition = new Vector2(nextChunk.Dungeon.OpeningLocations[2].X * TileSize, (nextChunk.Dungeon.OpeningLocations[2].Y - 1) * TileSize)
                    };
                    chunk.AddActor(teleporter, true);
                }
                if (chunk.Down)
                {
                    var next = chunk.Position + new Point(0, 1);
                    var nextChunk = Chunks[next.X, next.Y];
                    var teleporter = new Teleporter
                    {
                        Position = new Vector2(chunk.Dungeon.OpeningLocations[2].X * TileSize, chunk.Dungeon.OpeningLocations[2].Y * TileSize),
                        Chunk = next,
                        ChunkPosition = new Vector2(nextChunk.Dungeon.OpeningLocations[0].X * TileSize, (nextChunk.Dungeon.OpeningLocations[0].Y + 1) * TileSize)
                    };
                    chunk.AddActor(teleporter, true);
                }
                if (chunk.Left)
                {
                    var next = chunk.Position + new Point(-1, 0);
                    var nextChunk = Chunks[next.X, next.Y];
                    var teleporter = new Teleporter
                    {
                        Position = new Vector2(chunk.Dungeon.OpeningLocations[3].X * TileSize, chunk.Dungeon.OpeningLocations[3].Y * TileSize),
                        Chunk = next,
                        ChunkPosition = new Vector2((nextChunk.Dungeon.OpeningLocations[1].X - 1) * TileSize, nextChunk.Dungeon.OpeningLocations[1].Y * TileSize)
                    };
                    chunk.AddActor(teleporter, true);
                }
                if (chunk.Right)
                {
                    var next = chunk.Position + new Point(1, 0);
                    var nextChunk = Chunks[next.X, next.Y];
                    var teleporter = new Teleporter
                    {
                        Position = new Vector2(chunk.Dungeon.OpeningLocations[1].X * TileSize, chunk.Dungeon.OpeningLocations[1].Y * TileSize),
                        Chunk = next,
                        ChunkPosition = new Vector2((nextChunk.Dungeon.OpeningLocations[3].X + 1) * TileSize, nextChunk.Dungeon.OpeningLocations[3].Y * TileSize)
                    };
                    chunk.AddActor(teleporter, true);
                }
            }

            Player = GetCurrentChunk().Player;

            Font = Content.Load<BitmapFont>("font");

            HUD = new HUD(Player, Camera, window)
            {
                Numbers = numbers,
                Carrot = new TextureRegion2D(Tileset, TileSize * 7, 0, TileSize, TileSize),
                WaterEmpty = new TextureRegion2D(Tileset, TileSize * 5, 0, TileSize, TileSize),
                WaterFull = new TextureRegion2D(Tileset, TileSize * 6, 0, TileSize, TileSize),
                HeartFull = new TextureRegion2D(Tileset, TileSize * 4, 0, TileSize, TileSize),
                HeartHalf = new TextureRegion2D(Tileset, TileSize * 3, 0, TileSize, TileSize),
                HeartEmpty = new TextureRegion2D(Tileset, TileSize * 2, 0, TileSize, TileSize),
                Borders = borders,
                Font = Font
            };

        }

        internal void GameOver()    
        {
            IsGameOver = true;
        }

        #region CREATORS
        internal void AddActor(Actor actor, bool instantly = false)
        {

            GetCurrentChunk().AddActor(actor, instantly);
        }
        internal void RemoveActor(Actor actor)
        {
            GetCurrentChunk().RemovedActors.Add(actor);
        }

        internal void CreateCrop(Vector2 position, bool instantly = false)
        {
            GetCurrentChunk().CreateCrop(position, instantly);
        }

        internal void CreateCarrot(Vector2 position, bool instantly = false)
        {
            GetCurrentChunk().CreateCarrot(position, instantly);
        }

        internal void CreateSplashParticle(Vector2 position, bool instantly = false)
        {
            GetCurrentChunk().CreateSplashParticle(position, instantly);
        }
        internal void CreateSeedParticle(Vector2 position, bool instantly = false)
        {
            GetCurrentChunk().CreateSeedParticle(position, instantly);
        }

        internal void CreatePickUpParticle(Vector2 position, bool instantly = false)
        {
            GetCurrentChunk().CreatePickUpParticle(position, instantly);
        }
        #endregion

        internal List<Actor> GetActors()
        {
            return GetCurrentChunk().Actors;
        }
        internal Chunk GetCurrentChunk()
        {
            return Chunks[CurrentChunk.X, CurrentChunk.Y];
        }

        public override void Update(GameTime gameTime)
        {
            Input.Update(Keyboard.GetState());
            if (!IsGameOver)
            {
                GetCurrentChunk().Update(gameTime);
                //TODO Update Crops and Farmers in all chunks
            }

            SoundPlayer.Instance.Play();
        }
        public override void Draw(GameTime gameTime)
        {
            

            GraphicsDevice.Clear(Color.Black);

            GetCurrentChunk().Draw(SpriteBatch);

            SpriteBatch.Begin(samplerState: SamplerState.PointClamp, sortMode: SpriteSortMode.FrontToBack);
                HUD.Draw(SpriteBatch);
                if (IsGameOver)
                {
                    //TODO Add game over screen
                }
            SpriteBatch.End();
        }

        internal void TeleportTo(Point chunk, Vector2 position)
        {
            GetCurrentChunk().Player = null;
            Player.Position = position;
            Player.Teleporting = true;
            GetCurrentChunk().RemoveActor(Player);
            CurrentChunk = chunk;
            GetCurrentChunk().Player = Player;
            GetCurrentChunk().AddActor(Player);
            LastCameraPos = GetCurrentChunk().GetCameraPosition();
        }
    }
}
