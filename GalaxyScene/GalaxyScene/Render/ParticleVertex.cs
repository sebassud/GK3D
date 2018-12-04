using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyScene.Render
{
    struct ParticleVertex : IVertexType
    {
        public Vector3 Position;

        public Vector2 TexCoord;

        public Vector2 TexCoordInter;

        public float Scale;

        public float Ratio;

        public ParticleVertex(Vector3 position, Vector2 texCoord, Vector2 texCoordInter, float scale, float ratio)
        {
            Position = position;
            TexCoord = texCoord;
            TexCoordInter = texCoordInter;
            Scale = scale;
            Ratio = ratio;
        }

        public static readonly VertexDeclaration VertexDeclaration = new VertexDeclaration
        (
          new VertexElement(0, VertexElementFormat.Vector3,
                                 VertexElementUsage.Position, 0),
          new VertexElement(12, VertexElementFormat.Vector2,
                                 VertexElementUsage.TextureCoordinate, 0),
          new VertexElement(20, VertexElementFormat.Vector2,
                                 VertexElementUsage.TextureCoordinate, 0),
          new VertexElement(28, VertexElementFormat.Single,
                                 VertexElementUsage.PointSize, 0),
          new VertexElement(32, VertexElementFormat.Single,
                                 VertexElementUsage.PointSize, 0)

        );

        public const int SizeInBytes = 36;

        VertexDeclaration IVertexType.VertexDeclaration => VertexDeclaration;
    }
}
