using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using mizjam1.Actors;
using mizjam1.ContentLoaders;
using mizjam1.Helpers;
using MonoGame.Extended;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.TextureAtlases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mizjam1.Scenes
{
    internal class Chunk
    {
        internal int GridSize = 32;
        internal int TileSize = 16;
        internal TextureRegion2D[,] Floor;

        internal List<Actor> Actors;
        internal List<Actor> AddedActors;
        internal List<Actor> RemovedActors;

        internal Player Player;
        internal Dungeon Dungeon;

        internal MapScene Scene;
        internal Point Position;
        internal bool Up, Down, Left, Right;
        internal int NBushes = 10;

        internal Chunk(MapScene scene, Point position, bool createPlayer, bool hasShipPart, int shipPart, bool up, bool left, bool right, bool down, int gridSize, int tileSize, int farmers)
        {
            Scene = scene;
            Position = position;
            Up = up;
            Left = left;
            Right = right;
            Down = down;
            GridSize = gridSize;
            TileSize = tileSize;

            Dungeon = new Dungeon(GridSize, up, left, right, down);

            Actors = new List<Actor>();
            AddedActors = new List<Actor>();
            RemovedActors = new List<Actor>();
            if (createPlayer)
            {
                CreatePlayer(true);
            }
            NBushes += (int)(RandomHelper.NextFloat() * 5);
            NBushes -= farmers;
            NBushes = Math.Max(1, NBushes);
            CreateBushes(true);
            if (createPlayer)
            {
                CreateWell(true);
            }
            CreateFloor(true);
            CreateTrees(true);
            if (hasShipPart)
            {
                CreateShipPart(FindEmptyPosition(), shipPart, true);
            }
            if (!createPlayer)
            {
                for (int i = 0; i < farmers + (int) (RandomHelper.NextFloat() * (farmers + 1)); i++)
                {
                    CreateFarmer(farmers == 1, true);
                }
            }
        }


        #region CREATORS
        internal void AddActor(Actor actor, bool instantly = false)
        {
            actor.Scene = Scene;
            actor.Chunk = this;
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

        internal bool HasPlayer()
        {
            return Actors.Any(a => a.GetType() == typeof(Player));
        }
        internal void CreateFloor(bool instantly = false)
        {
            Floor = new TextureRegion2D[GridSize, GridSize];

            for (int i = 0; i < GridSize; i++)
            {
                for (int j = 0; j < GridSize; j++)
                {
                    Floor[i, j] = new TextureRegion2D(ContentLoader.Instance.Tileset, TileSize * 0, TileSize * 4, TileSize, TileSize);
                }
            }
            foreach (var p in Dungeon.SafePoints)
            {
                var x = Math.Max(0, (int)(RandomHelper.NextFloat() * 20) - 15);
                Floor[p.X, p.Y] = new TextureRegion2D(ContentLoader.Instance.Tileset, TileSize * x, TileSize * 4, TileSize, TileSize);
            }
        }
        internal void CreateTrees(bool instantly = false)
        {
            for (int i = 0; i < GridSize; i++)
            {
                for (int j = 0; j < GridSize; j++)
                {
                    if (Dungeon.SafePoints.Contains(new Point(i, j)))
                    {
                        continue;
                    }
                    var x = (int)(RandomHelper.NextFloat() * 4) + 3;
                    var Tree = new Actor
                    {
                        Position = new Vector2(TileSize * i, TileSize * j),
                        Sprite = new Sprite(new TextureRegion2D(ContentLoader.Instance.Tileset, TileSize * x, TileSize * 3, TileSize, TileSize)) { Depth = 0.7f, OriginNormalized = Vector2.Zero },
                        Collidable = true,
                        Size = TileSize - 2,
                        CollisionGroup = 0b11,
                        CollidesWith = 0b00,
                    };
                    AddActor(Tree, instantly);
                }
            }
        }
        internal void CreateWell(bool instantly = false)
        {
            var well = new Well()
            {
                Position = FindEmptyPosition(true),
                Sprite = new Sprite(new TextureRegion2D(ContentLoader.Instance.Tileset, TileSize * 0, TileSize * 5, TileSize, TileSize))
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
            for (int _ = 0; _ < NBushes; _++)
            {
                Vector2 pos = FindEmptyPosition();
                var bush = new Bush
                {
                    Position = pos,
                    Sprite = new Sprite(new TextureRegion2D(ContentLoader.Instance.Tileset, TileSize * (int)(RandomHelper.NextFloat() * 3), TileSize * 3, TileSize, TileSize))
                    {
                        OriginNormalized = Vector2.Zero,
                        Depth = 0.6f,
                    },
                    Interactive = true
                };
                AddActor(bush, instantly);
            }
        }
        internal void CreateShipPart(Vector2 position, int shipPart, bool instantly = false)
        {
            var sprite = new Sprite(new TextureRegion2D(ContentLoader.Instance.Tileset, TileSize * 7, TileSize * (7 - shipPart), TileSize, TileSize))
            {
                OriginNormalized = Vector2.Zero,
                Depth = 0.6f,
            };
            var part = new ShipPart(position, sprite, shipPart);
            AddActor(part, instantly);
        }
        internal void CreateShip(Vector2 pos, bool instantly = false)
        {
            var parts = new Sprite[4];
            for (int i = 0; i < 4; i++)
            {
                parts[i] = new Sprite(new TextureRegion2D(ContentLoader.Instance.Tileset, TileSize * 7, TileSize * (7 - i), TileSize, TileSize))
                {
                    OriginNormalized = Vector2.Zero,
                };
            }
            var bush = new Ship
            {
                Position = pos,
                Parts = parts,
                Collidable = true,
                PartsMounted = 0,
                Fire = new Sprite(new TextureRegion2D(ContentLoader.Instance.Tileset, TileSize * 5, TileSize * 2, TileSize, TileSize))
                {
                    OriginNormalized = Vector2.Zero,
                }
            };
            AddActor(bush, instantly);
        }
        internal void CreateBullet(bool instantly = false)
        {
            var sprite = new Sprite(new TextureRegion2D(ContentLoader.Instance.Tileset, TileSize * 5, TileSize * 4, TileSize, TileSize))
            {
                OriginNormalized = new Vector2(0, 0),
                Depth = 0.7f,
            };
            var part = new Bullet(Player.Position + new Vector2(TileSize / 2, TileSize / 2), Player.Speed)
            {
                Sprite = sprite
            };
            AddActor(part, instantly);

        }
        private Vector2 FindEmptyPosition(bool centered = false)
        {
            Vector2 pos;
            if (centered && Dungeon.SafePoints.Any(p => !Actors.Any(a => a.Position.X == TileSize * p.X && a.Position.Y == TileSize * p.Y) && p.X > GridSize / 3 && p.X < GridSize * 2 / 3 && p.Y > GridSize / 3 && p.Y < GridSize * 2 / 3))
            {
                var p = Dungeon.SafePoints.Where(p => !Actors.Any(a => a.Position.X == TileSize * p.X && a.Position.Y == TileSize * p.Y) && p.X > GridSize / 3 && p.X < GridSize * 2 / 3 && p.Y > GridSize / 3 && p.Y < GridSize * 2 / 3).GetRandom();
                pos = new Vector2(TileSize * p.X, TileSize * p.Y);
            }
            else if (Dungeon.SafePoints.Any(p => !Actors.Any(a => a.Position.X == TileSize * p.X && a.Position.Y == TileSize * p.Y) && p.X > 0 && p.X < GridSize - 1 && p.Y > 0 && p.Y < GridSize - 1))
            {
                var p = Dungeon.SafePoints.Where(p => !Actors.Any(a => a.Position.X == TileSize * p.X && a.Position.Y == TileSize * p.Y) && p.X > 0 && p.X < GridSize - 1 && p.Y > 0 && p.Y < GridSize - 1).GetRandom();
                pos = new Vector2(TileSize * p.X, TileSize * p.Y);
                //Dungeon.SafePoints.Remove(p);
            }
            else
            {

                int y;
                int x;
                do
                {
                    x = (int)(GridSize * RandomHelper.NextFloat());
                    y = (int)(GridSize * RandomHelper.NextFloat());
                } while (Dungeon.Grid[x, y] || Actors.Any(a => a.Position.X == TileSize * x && a.Position.Y == TileSize * y));

                pos = new Vector2(TileSize * x, TileSize * y);
            }

            return pos;
        }

        internal void CreatePlayer(bool instantly = false)
        {
            Player = new Player(FindEmptyPosition(true));
            AddActor(Player, instantly);
            Scene.LastCameraPos = GetCameraPosition();
        }
        internal void CreateFarmer(bool easy, bool instantly = false)
        {
            var farmer = new Farmer(Player)
            {
                Position = FindEmptyPosition(true),
                Animations = Animation.GetFarmerAnimation(ContentLoader.Instance.Tileset),
                Moveable = true,
                Controllable = true,
                Animated = true,
                Collidable = true,
                CollisionGroup = 0b11,
                CollidesWith = 0b01,
            };
            if (easy)
            {
                farmer.MaxSpeed *= 0.5f;
            } else
            {
                farmer.MaxSpeed *= 0.75f;
            }
            AddActor(farmer, instantly);
        }

        internal void CreateCrop(Vector2 position, bool instantly = false)
        {
            var crop = new Crop
            {
                Position = position,
                AnimationState = "UNWATERED",
                Animations = Animation.GetCropAnimation(ContentLoader.Instance.Tileset),
                Animated = true,
                Interactive = true
            };
            AddActor(crop, instantly);
        }

        internal void CreateCarrot(Vector2 position, bool instantly = false)
        {
            var sprite = new Sprite(new TextureRegion2D(ContentLoader.Instance.Tileset, TileSize * 7, TileSize * 0, TileSize, TileSize))
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
                Animations = Animation.GetSplashAnimation(ContentLoader.Instance.Tileset),
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
                Animations = Animation.GetSeedAnimation(ContentLoader.Instance.Tileset),
                Animated = true
            };
            AddActor(particle, instantly);
        }

        internal void CreatePickUpParticle(Vector2 position, bool instantly = false)
        {
            var particle = new Particle
            {
                Position = position,
                Animations = Animation.GetPickUpAnimation(ContentLoader.Instance.Tileset),
                Animated = true
            };
            AddActor(particle, instantly);
        }
        #endregion

        internal Vector2 GetCameraPosition()
        {
            var center = Scene.ViewportAdapter.BoundingRectangle.Center;
            var localPos = Player.Position;
            var camPos = -localPos * Scene.Camera.Zoom + new Vector2(center.X, center.Y);

            float rightSide = -GridSize * TileSize * Scene.Camera.Zoom + center.X * 2;
            if (Scene.ViewportAdapter.BoundingRectangle.Width > GridSize * TileSize * Scene.Camera.Zoom)
            {
                camPos.X = rightSide / 2;
            }
            else
            {
                if (camPos.X > 0)
                {
                    camPos.X = 0;
                }

                if (camPos.X < rightSide)
                {
                    camPos.X = rightSide;
                }
            }

            float bottomSide = -GridSize * TileSize * Scene.Camera.Zoom + center.Y * 2;
            if (GridSize * TileSize * Scene.Camera.Zoom < Scene.ViewportAdapter.BoundingRectangle.Height)
            {
                bottomSide += 64;
            }
            if (Scene.ViewportAdapter.BoundingRectangle.Height > GridSize * TileSize * Scene.Camera.Zoom)
            {
                camPos.Y = bottomSide / 2;
            }
            else
            {
                if (camPos.Y > TileSize * 2 * Scene.Camera.Zoom)
                {
                    camPos.Y = TileSize * 2 * Scene.Camera.Zoom;
                }

                if (camPos.Y < bottomSide)
                {
                    camPos.Y = bottomSide;
                }
            }
            return camPos;
        }

        internal void Update(GameTime gameTime)
        {
            Actors.AddRange(AddedActors);
            AddedActors.Clear();

            Actors.ForEach(a => a.Update(gameTime));
            Actors.RemoveAll(a => RemovedActors.Contains(a));

            RemovedActors.Clear();
        }
        internal void Draw(SpriteBatch SpriteBatch)
        {
            var transformMatrix = Scene.Camera.GetViewMatrix();
            var center = Scene.ViewportAdapter.BoundingRectangle.Center;
            var pos = GetCameraPosition();
            pos = pos * 0.05f + Scene.LastCameraPos * 0.95f;
            transformMatrix.Translation = new Vector3(pos, 0);
            Scene.LastCameraPos = pos;
            SpriteBatch.Begin(transformMatrix: transformMatrix, samplerState: SamplerState.PointClamp, sortMode: SpriteSortMode.FrontToBack);
            var screenZero = Scene.Camera.ScreenToWorld(Vector2.Zero);
            for (int i = 0; i < GridSize; i++)
            {
                for (int j = 0; j < GridSize; j++)
                {
                    SpriteBatch.Draw(Floor[i, j], new Vector2(i * TileSize, j * TileSize), Color.White, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, 0);
                }
            }
            //FIXME: LoadBeforehand
            var arrow = new TextureRegion2D(ContentLoader.Instance.Tileset, 7 * TileSize, 2 * TileSize, TileSize, TileSize);
            for (int i = 0; i < 4; i++)
            {
                if (Dungeon.Openings[i])
                {
                    SpriteBatch.Draw(arrow,
                        new Vector2(
                            Dungeon.OpeningLocations[i].X * TileSize + TileSize / 2,
                            Dungeon.OpeningLocations[i].Y * TileSize + TileSize / 2),
                        Color.White,
                        MathF.PI / 2 * i,
                        Vector2.One * TileSize / 2,
                        Vector2.One,
                        SpriteEffects.None,
                        1);
                }
            }
            Actors.ForEach(a => a.Draw(SpriteBatch));

            SpriteBatch.End();
        }
    }
}
