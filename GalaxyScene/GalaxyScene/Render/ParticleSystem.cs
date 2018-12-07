using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace GalaxyScene.Render
{
    public class ParticleSystem
    {
        private Random _rand;
        private float speed = 0.02f;
        public Vector3 EmitterCenter { get; set; }
        public Vector3 EmitterDirection { get; set; }
        public int ParticlesLimit { get; set; }
        public int MaxTTL { get; set; }
        public float Scale { get; set; }
        public List<Particle> Particles { get; set; }

        public ParticleSystem(Vector3 emitterCenter, Vector3 emitterDirection, int particlesLimit, int maxTTL, float scale)
        {
            _rand = new Random();
            EmitterCenter = emitterCenter;
            EmitterDirection = emitterDirection;
            ParticlesLimit = particlesLimit;
            MaxTTL = maxTTL;
            Scale = scale;
            Particles = new List<Particle>();
        }

        private Particle CreateParticle()
        {
            var x = EmitterDirection.X + ((float)_rand.NextDouble() - 0.5f);
            var y = EmitterDirection.Y + ((float)_rand.NextDouble() - 0.5f);
            var z = EmitterDirection.Z + ((float)_rand.NextDouble() - 0.5f);
            var ttl = _rand.Next(50, MaxTTL);
            return new Particle(
                EmitterCenter,
                new Vector3(x * speed,  y * speed, z * speed),
                ttl,
                MaxTTL,
                Scale
            );
        }

        public void Update()
        {
            for (int i = 0; i < 10; i++)
            {
                if (Particles.Count < ParticlesLimit)
                    Particles.Add(CreateParticle());
            }
            for (int i = 0; i < Particles.Count; i++)
            {
                if (!Particles[i].Update())
                {
                    Particles.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}
