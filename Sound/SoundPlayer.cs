using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mizjam1.Sound
{
    internal class SoundPlayer
    {
        public static SoundPlayer Instance = new SoundPlayer();

        internal Dictionary<string, SoundEffect> Sounds;

        internal Dictionary<string, Queue<SoundEffectInstance>> Queues;

        public void Init(SoundEffect dirt, SoundEffect waterPick, SoundEffect waterDrop, SoundEffect pick, SoundEffect cut)
        {
            Sounds = new Dictionary<string, SoundEffect>
            {
                ["DIRT"] = dirt,
                ["WATERPICK"] = waterPick,
                ["WATERDROP"] = waterDrop,
                ["PICK"] = pick,
                ["CUT"] = cut
            };
            Queues = new Dictionary<string, Queue<SoundEffectInstance>>
            {

                ["DIRT"] = new Queue<SoundEffectInstance>(),
                ["WATERPICK"] = new Queue<SoundEffectInstance>(),
                ["WATERDROP"] = new Queue<SoundEffectInstance>(),
                ["PICK"] = new Queue<SoundEffectInstance>(),
                ["CUT"] = new Queue<SoundEffectInstance>()
            };
        }
        public void Play(string name)
        {
            Queues[name].Enqueue(Sounds[name].CreateInstance());
        }

        public void Play()
        {
            foreach (var k in Queues.Keys)
            {
                var q = Queues[k];
                if (!q.Any())
                {
                    continue;
                }
                var state = q.Peek().State;
                if (state == SoundState.Playing)
                {
                    q.Peek().Play();
                }
                if (state == SoundState.Stopped)
                {
                    q.Dequeue();
                    if (!q.Any())
                    {
                        continue;
                    }
                    q.Peek().Play();
                }
            }
        }
    }
}
