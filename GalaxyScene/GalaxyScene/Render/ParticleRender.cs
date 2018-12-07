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

        private readonly int numberTextures = 16;
        private readonly int numberColumn = 4;
        private readonly int numberRow = 4;

        public ParticleRender()
        {
            list_vertex = new List<ParticleVertex>();           
        }

        public void AddParticle(Particle particle)
        {
            ParticleVertex[] billboardVertices = new ParticleVertex[6];

            var scale = particle.Scale;
            var time = particle.Time;
            int k = numberTextures - (int)Math.Round(time * numberTextures, MidpointRounding.AwayFromZero);
            int y = k / numberColumn;
            int x = k % numberColumn;
            var coordinates = GetCoordinatesTexture(x, y);
            var ratio = 1 - Math.Abs(k - numberTextures * (1 - time));
            if (k < numberTextures * (1 - time))
                k++;
            else
                k--;
            y = k / numberColumn;
            x = k % numberColumn;
            var coordinates2 = GetCoordinatesTexture(x, y);

            billboardVertices[0] = new ParticleVertex(particle.Position, new Vector2(0, 0), new Vector2(coordinates.Item1.X, coordinates.Item1.Y),
                new Vector2(coordinates2.Item1.X, coordinates2.Item1.Y), scale, ratio);
            billboardVertices[1] = new ParticleVertex(particle.Position, new Vector2(1, 0), new Vector2(coordinates.Item2.X, coordinates.Item1.Y),
                new Vector2(coordinates2.Item2.X, coordinates2.Item1.Y), scale, ratio);
            billboardVertices[2] = new ParticleVertex(particle.Position, new Vector2(1, 1), new Vector2(coordinates.Item2.X, coordinates.Item2.Y),
                new Vector2(coordinates2.Item2.X, coordinates2.Item2.Y), scale, ratio);

            billboardVertices[3] = new ParticleVertex(particle.Position, new Vector2(0, 0), new Vector2(coordinates.Item1.X, coordinates.Item1.Y),
                new Vector2(coordinates2.Item1.X, coordinates2.Item1.Y), scale, ratio);
            billboardVertices[4] = new ParticleVertex(particle.Position, new Vector2(1, 1), new Vector2(coordinates.Item2.X, coordinates.Item2.Y),
                new Vector2(coordinates2.Item2.X, coordinates2.Item2.Y), scale, ratio);
            billboardVertices[5] = new ParticleVertex(particle.Position, new Vector2(0, 1), new Vector2(coordinates.Item1.X, coordinates.Item2.Y),
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
            return new Tuple<Vector2, Vector2>(new Vector2(x * ((float)1 / numberRow), y * ((float)1 / numberColumn)), new Vector2((x + 1) * ((float)1 / numberRow), (y + 1) * ((float)1 / numberColumn)));
        }
    }
}
