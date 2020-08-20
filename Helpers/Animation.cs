using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.TextureAtlases;
using System;
using System.Collections.Generic;

namespace mizjam1.Helpers
{

    internal class Animation
    {
        protected List<Sprite> Frames;
        protected float FrameDuration;
        private float TotalDuration;
        private bool Loop = false;

        internal Animation(List<Sprite> frames, float frameDuration, bool loop = true)
        {
            Frames = frames;
            FrameDuration = frameDuration;
            TotalDuration = frames.Count * FrameDuration;
            Loop = loop;
        }

        internal Sprite GetFrame(float elapsed)
        {
            if (Loop)
            {
                return Frames[(int)(elapsed / TotalDuration) % Frames.Count];
            }
            else
            {
                var f = MathHelper.Clamp((int)(elapsed / TotalDuration), 0, Frames.Count - 1);
                return Frames[f];
            }
        }

        internal Animation Copy()
        {
            return new Animation(Frames, FrameDuration);
        }
        internal static Dictionary<string, Animation> GetCropAnimation(Texture2D texture)
        {
            var origin = new Vector2(0, 0);
            var depth = 0.5f;
            return new Dictionary<string, Animation>()
            {
                {
                    "UNWATERED", new Animation(new List<Sprite>() {
                        new Sprite(new TextureRegion2D(texture, 16 * 0, 16 * 2, 16, 16)) { OriginNormalized = origin, Depth = depth },
                    },
                    1 / 60f,
                    false
                    )
                },
                {
                    "WATERED", new Animation(new List<Sprite>()
                    {
                        new Sprite(new TextureRegion2D(texture, 16 * 1, 16 * 2, 16, 16)) { OriginNormalized = origin, Depth = depth },
                    },
                    1f / 60f,
                    false
                    )
                },
                {
                    "GROW_0", new Animation(new List<Sprite>()
                    {
                        new Sprite(new TextureRegion2D(texture, 16 * 2, 16 * 2, 16, 16)) { OriginNormalized = origin, Depth = depth },
                    },
                    1f / 60f,
                    false
                    )
                },
                {
                    "GROW_1", new Animation(new List<Sprite>()
                    {
                        new Sprite(new TextureRegion2D(texture, 16 * 3, 16 * 2, 16, 16)) { OriginNormalized = origin, Depth = depth },
                    },
                    1f / 60f,
                    false
                    )
                },
                {
                    "GROW_2", new Animation(new List<Sprite>()
                    {
                        new Sprite(new TextureRegion2D(texture, 16 * 4, 16 * 2, 16, 16)) { OriginNormalized = origin, Depth = depth },
                    },
                    1f / 60f,
                    false
                    )
                }
            };
        }
        internal static Dictionary<string, Animation> GetPigAnimation(Texture2D texture)
        {
            var origin = new Vector2(0, 0);
            var depth = 0.8f;
            return new Dictionary<string, Animation>()
            {
                {
                    "WALK", new Animation(new List<Sprite>() {
                        new Sprite(new TextureRegion2D(texture, 16 * 0, 16 * 0, 16, 16)) { OriginNormalized = origin, Depth = depth },
                        new Sprite(new TextureRegion2D(texture, 16 * 1, 16 * 0, 16, 16)) { OriginNormalized = origin, Depth = depth },
                    },
                    4 / 60f)
                },
                {
                    "IDLE", new Animation(new List<Sprite>()
                    {
                        new Sprite(new TextureRegion2D(texture, 16 * 0, 16 * 0, 16, 16)) { OriginNormalized = origin, Depth = depth },
                    },
                    1f / 60f,
                    false
                    )
                }
            };
        }
        internal static Dictionary<string, Animation> GetFarmerAnimation(Texture2D texture)
        {
            var origin = new Vector2(0, 0);
            var depth = 0.8f;
            return new Dictionary<string, Animation>()
            {
                {
                    "FARMER_WALK", new Animation(new List<Sprite>() {
                        new Sprite(new TextureRegion2D(texture, 16 * 0, 16 * 1, 16, 16)) { OriginNormalized = origin, Depth = depth },
                        new Sprite(new TextureRegion2D(texture, 16 * 1, 16 * 1, 16, 16)) { OriginNormalized = origin, Depth = depth },
                    },
                    4 / 60f)
                },
                {
                    "FARMER_IDLE", new Animation(new List<Sprite>()
                    {
                        new Sprite(new TextureRegion2D(texture, 16 * 0, 16 * 1, 16, 16)) { OriginNormalized = origin, Depth = depth },
                    },
                    1f / 60f,
                    false
                    )
                },

                {
                    "CHICKEN_WALK", new Animation(new List<Sprite>() {
                        new Sprite(new TextureRegion2D(texture, 16 * 8, 16 * 0, 16, 16)) { OriginNormalized = origin, Depth = depth },
                        new Sprite(new TextureRegion2D(texture, 16 * 9, 16 * 0, 16, 16)) { OriginNormalized = origin, Depth = depth },
                    },
                    4 / 60f)
                },
                {
                    "CHICKEN_IDLE", new Animation(new List<Sprite>()
                    {
                        new Sprite(new TextureRegion2D(texture, 16 * 8, 16 * 0, 16, 16)) { OriginNormalized = origin, Depth = depth },
                    },
                    1f / 60f,
                    false
                    )
                },
            };
        }
        internal static Dictionary<string, Animation> GetSplashAnimation(Texture2D texture)
        {
            var origin = new Vector2(0, 0);
            var depth = 0.99f;
            return new Dictionary<string, Animation>()
            {
                {
                    "IDLE", new Animation(new List<Sprite>() {
                        new Sprite(new TextureRegion2D(texture, 16 * 1, 16 * 5, 16, 16)) { OriginNormalized = origin, Depth = depth },
                        new Sprite(new TextureRegion2D(texture, 16 * 2, 16 * 5, 16, 16)) { OriginNormalized = origin, Depth = depth },
                        new Sprite(new TextureRegion2D(texture, 16 * 3, 16 * 5, 16, 16)) { OriginNormalized = origin, Depth = depth },
                        new Sprite(new TextureRegion2D(texture, 16 * 4, 16 * 5, 16, 16)) { OriginNormalized = origin, Depth = depth },
                        new Sprite(new TextureRegion2D(texture, 16 * 5, 16 * 5, 16, 16)) { OriginNormalized = origin, Depth = depth },
                        new Sprite(new TextureRegion2D(texture, 16 * 6, 16 * 5, 16, 16)) { OriginNormalized = origin, Depth = depth },
                    },
                    0.5f / 60f,
                    false)
                },
            };
        }
        internal static Dictionary<string, Animation> GetSeedAnimation(Texture2D texture)
        {
            var origin = new Vector2(0, 0);
            var depth = 0.99f;
            return new Dictionary<string, Animation>()
            {
                {
                    "IDLE", new Animation(new List<Sprite>() {
                        new Sprite(new TextureRegion2D(texture, 16 * 1, 16 * 6, 16, 16)) { OriginNormalized = origin, Depth = depth },
                        new Sprite(new TextureRegion2D(texture, 16 * 2, 16 * 6, 16, 16)) { OriginNormalized = origin, Depth = depth },
                        new Sprite(new TextureRegion2D(texture, 16 * 3, 16 * 6, 16, 16)) { OriginNormalized = origin, Depth = depth },
                        new Sprite(new TextureRegion2D(texture, 16 * 4, 16 * 6, 16, 16)) { OriginNormalized = origin, Depth = depth },
                        new Sprite(new TextureRegion2D(texture, 16 * 5, 16 * 6, 16, 16)) { OriginNormalized = origin, Depth = depth },
                        new Sprite(new TextureRegion2D(texture, 16 * 6, 16 * 6, 16, 16)) { OriginNormalized = origin, Depth = depth },
                    },
                    0.5f / 60f,
                    false)
                },
            };
        }
        internal static Dictionary<string, Animation> GetPickUpAnimation(Texture2D texture)
        {
            var origin = new Vector2(0, 0);
            var depth = 0.99f;
            return new Dictionary<string, Animation>()
            {
                {
                    "IDLE", new Animation(new List<Sprite>() {
                        new Sprite(new TextureRegion2D(texture, 16 * 1, 16 * 7, 16, 16)) { OriginNormalized = origin, Depth = depth },
                        new Sprite(new TextureRegion2D(texture, 16 * 2, 16 * 7, 16, 16)) { OriginNormalized = origin, Depth = depth },
                        new Sprite(new TextureRegion2D(texture, 16 * 3, 16 * 7, 16, 16)) { OriginNormalized = origin, Depth = depth },
                        new Sprite(new TextureRegion2D(texture, 16 * 4, 16 * 7, 16, 16)) { OriginNormalized = origin, Depth = depth },
                        new Sprite(new TextureRegion2D(texture, 16 * 5, 16 * 7, 16, 16)) { OriginNormalized = origin, Depth = depth },
                        new Sprite(new TextureRegion2D(texture, 16 * 6, 16 * 7, 16, 16)) { OriginNormalized = origin, Depth = depth },
                    },
                    0.5f / 60f,
                    false)
                },
            };
        }
    }
}
