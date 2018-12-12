using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace GalaxyScene.Render
{
    public class Particle
    {
        private int startTTL { get; set; }
        private int maxTTL { get; set; }
        private float scaleGlobal { get; set; }
        private int tTL { get; set; }
        private Vector3 velocity { get; set; }
        public Vector3 Position { get; set; }
        public float Scale
        {
            get
            {
                return (float)tTL / startTTL;
            }
        }
        public float Time => (tTL + (maxTTL - startTTL)) / (float)maxTTL;

        public Particle(Vector3 position, Vector3 velocity, int tTL, int maxTTL, float scale)
        {
            Position = position;
            this.velocity = velocity;
            this.tTL = tTL;
            startTTL = tTL;
            this.maxTTL = maxTTL;
            scaleGlobal = scale;
        }

        public bool Update()
        {
            velocity = velocity - Vector3.Normalize(Position) * scaleGlobal * 0.002f;
            Position += velocity * scaleGlobal;
            tTL--;
            return tTL > 0;
        }
    }
}
