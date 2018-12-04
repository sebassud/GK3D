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
        public Vector3 Position { get; set; }
        public Vector3 Velocity { get; set; }
        public int TTL { get; set; }
        public int MaxTTL { get; set; }
        public float Rotation { get; set; }
        public float Scale { get; set; }

        public Particle(Vector3 position, Vector3 velocity, int tTL, float rotation, float scale)
        {
            Position = position;
            Velocity = velocity;
            TTL = tTL;
            MaxTTL = tTL;
            Rotation = rotation;
            Scale = scale;
        }

        public bool Update()
        {
            Velocity = Velocity - Vector3.Normalize(Position) * Scale * 0.001f;
            Position += Velocity * Scale;
            TTL--;
            return TTL > 0;
        }
    }
}
