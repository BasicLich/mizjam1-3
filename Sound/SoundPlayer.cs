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
        internal Dictionary<string, SoundEffectInstance> Current;
        internal Dictionary<string, int> CurrentNote;
        internal List<float> Scale;
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
            Current = new Dictionary<string, SoundEffectInstance>
            {
                ["DIRT"] = null,
                ["WATERPICK"] = null,
                ["WATERDROP"] = null,
                ["PICK"] = null,
                ["CUT"] = null,
            };
            CurrentNote = new Dictionary<string, int>
            {
                ["DIRT"] = 0,
                ["WATERPICK"] = 0,
                ["WATERDROP"] = 0,
                ["PICK"] = 0,
                ["CUT"] = 0,
            };
            Scale = new List<float>
            {
                1,
                1.12246f,
                1.25992f,
                1.33483f,
                1.49831f,
                1.68179f,
                1.88775f,
                2
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

                if (Current[k] != null && Current[k].State == SoundState.Stopped)
                {
                    Current[k] = null;
                }
                if (Current[k] != null && Current[k].State == SoundState.Playing)
                {
                    continue;
                }
                var q = Queues[k];
                if (!q.Any())
                {
                    continue;
                }
                Current[k] = q.Dequeue();
                var note = CurrentNote[k];
                Current[k].Pitch = Scale[note] - 1;
                Current[k].Play();
                CurrentNote[k] = (note + 1) % 8;
            }
        }
    }
}
