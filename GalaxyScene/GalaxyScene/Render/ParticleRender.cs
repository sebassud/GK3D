using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyScene.Render
{
    public class ParticleRender
    {
        private VertexBuffer buffor;

        private List<ParticleVertex> list_vertex;

        public ParticleRender()
        {
            list_vertex = new List<ParticleVertex>();           
        }

        public void AddParticle(Particle particle)
        {
            ParticleVertex[] billboardVertices = new ParticleVertex[6];

            var scale = (float)particle.TTL / particle.MaxTTL;
            int k = 16 - (int)Math.Round(scale * 16, MidpointRounding.AwayFromZero);
            int y = k / 4;
            int x = k % 4;
            var coordinates = GetCoordinatesTexture(x, y);
            var ratio = 1 - Math.Abs(k - (16 - scale * 16));
            if (k < 16 - scale * 16)
                k++;
            else
                k--;
            y = k / 4;
            x = k % 4;
            var coordinates2 = GetCoordinatesTexture(x, y);

            billboardVertices[0] = new ParticleVertex(particle.Position, new Vector2(coordinates.Item1.X, coordinates.Item1.Y),
                new Vector2(coordinates2.Item1.X, coordinates2.Item1.Y), scale, ratio);
            billboardVertices[1] = new ParticleVertex(particle.Position, new Vector2(coordinates.Item2.X, coordinates.Item1.Y),
                new Vector2(coordinates2.Item2.X, coordinates2.Item1.Y), scale, ratio);
            billboardVertices[2] = new ParticleVertex(particle.Position, new Vector2(coordinates.Item2.X, coordinates.Item2.Y),
                new Vector2(coordinates2.Item2.X, coordinates2.Item2.Y), scale, ratio);

            billboardVertices[3] = new ParticleVertex(particle.Position, new Vector2(coordinates.Item1.X, coordinates.Item1.Y),
                new Vector2(coordinates2.Item1.X, coordinates2.Item1.Y), scale, ratio);
            billboardVertices[4] = new ParticleVertex(particle.Position, new Vector2(coordinates.Item2.X, coordinates.Item2.Y),
                new Vector2(coordinates2.Item2.X, coordinates2.Item2.Y), scale, ratio);
            billboardVertices[5] = new ParticleVertex(particle.Position, new Vector2(coordinates.Item1.X, coordinates.Item2.Y),
                new Vector2(coordinates2.Item1.X, coordinates2.Item2.Y), scale, ratio);

            list_vertex.AddRange(billboardVertices);
        }

        internal void Clear()
        {
            list_vertex.Clear();
        }

        public VertexBuffer GetBuffer(GraphicsDevice graphicsDevice)
        {
            buffor = new VertexBuffer(graphicsDevice, typeof(ParticleVertex), list_vertex.Count, BufferUsage.WriteOnly);
            buffor.SetData(list_vertex.ToArray());
            return buffor;
        }

        private Tuple<Vector2, Vector2> GetCoordinatesTexture(int x, int y)
        {
            return new Tuple<Vector2, Vector2>(new Vector2(x * 0.25f, y * 0.25f), new Vector2((x + 1) * 0.25f, (y + 1) * 0.25f));
        }
    }
}
