﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace GalaxyScene.Render
{
    public class ParticleSystem
    {
        public Vector3 EmitterCenter { get; set; }
        public Vector3 EmitterDirection { get; set; }
        public int ParticlesLimit { get; set; }
        public int MaxTTL { get; set; }
        public List<Particle> Particles { get; set; }
        private Random _rand;

        public ParticleSystem(Vector3 emitterCenter, Vector3 emitterDirection, int particlesLimit, int maxTTL)
        {
            _rand = new Random();
            EmitterCenter = emitterCenter;
            EmitterDirection = emitterDirection;
            ParticlesLimit = particlesLimit;
            MaxTTL = maxTTL;
            Particles = new List<Particle>();
        }

        private Particle CreateParticle()
        {
            var scale = 0.1f;
            var x = EmitterDirection.X + ((float) _rand.NextDouble()- 0.5f)*scale/10;
            var y = EmitterDirection.Y + ((float)_rand.NextDouble() - 0.5f) *scale/10;
            var z = EmitterDirection.Z + (float) _rand.NextDouble() *scale/5;
            var ttl = _rand.Next(10, MaxTTL);
            return new Particle(
                EmitterCenter,
                new Vector3(x, y, z),
                ttl,
                0f,
                scale
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
