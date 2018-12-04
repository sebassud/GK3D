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
        public float Rotation { get; set; }
        public float Scale { get; set; }

        public Particle(Vector3 position, Vector3 velocity, int tTL, float rotation, float scale)
        {
            Position = position;
            Velocity = velocity;
            TTL = tTL;
            Rotation = rotation;
            Scale = scale;
        }

        public bool Update()
        {
            Velocity = new Vector3(Velocity.X, Velocity.Y, Velocity.Z - 0.001f * Scale);
            Position += Velocity * Scale;
            TTL--;
            return TTL > 0;
        }
    }
}
