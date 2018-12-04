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

        private List<VertexPositionTexture> list_vertex;

        public ParticleRender()
        {
            list_vertex = new List<VertexPositionTexture>();           
        }

        public void AddParticle(Particle particle)
        {
            VertexPositionTexture[] billboardVertices = new VertexPositionTexture[6];

            var coordinates = GetCoordinatesTexture((float)particle.TTL / particle.MaxTTL);
            billboardVertices[0] = new VertexPositionTexture(particle.Position, new Vector2(coordinates.Item1.X, coordinates.Item1.Y));
            billboardVertices[1] = new VertexPositionTexture(particle.Position, new Vector2(coordinates.Item2.X, coordinates.Item1.Y));
            billboardVertices[2] = new VertexPositionTexture(particle.Position, new Vector2(coordinates.Item2.X, coordinates.Item2.Y));

            billboardVertices[3] = new VertexPositionTexture(particle.Position, new Vector2(coordinates.Item1.X, coordinates.Item1.Y));
            billboardVertices[4] = new VertexPositionTexture(particle.Position, new Vector2(coordinates.Item2.X, coordinates.Item2.Y));
            billboardVertices[5] = new VertexPositionTexture(particle.Position, new Vector2(coordinates.Item1.X, coordinates.Item2.Y));

            list_vertex.AddRange(billboardVertices);
        }

        internal void Clear()
        {
            list_vertex.Clear();
        }

        public VertexBuffer GetBuffer(GraphicsDevice graphicsDevice)
        {
            buffor = new VertexBuffer(graphicsDevice, typeof(VertexPositionTexture), list_vertex.Count, BufferUsage.WriteOnly);
            buffor.SetData(list_vertex.ToArray());
            return buffor;
        }

        private Tuple<Vector2, Vector2> GetCoordinatesTexture(float ratio)
        {
            int k = 16 - (int)Math.Round(ratio * 16, MidpointRounding.AwayFromZero);
            int y = k / 4;
            int x = k % 4;

            return new Tuple<Vector2, Vector2>(new Vector2(x * 0.25f, y * 0.25f), new Vector2((x + 1) * 0.25f, (y + 1) * 0.25f));
        }
    }
}
