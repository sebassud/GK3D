﻿using Microsoft.Xna.Framework;
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

        public Vector2 TexCoord1;

        public Vector2 TexCoord2;

        public float Scale;

        public float Ratio;

        public ParticleVertex(Vector3 position, Vector2 texCoord, Vector2 texCoordCorner1, Vector2 texCoordCorner2, float scale, float ratio)
        {
            Position = position;
            TexCoord = texCoord;
            TexCoord1 = texCoordCorner1;
            TexCoord2 = texCoordCorner2;
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
          new VertexElement(28, VertexElementFormat.Vector2,
                                 VertexElementUsage.TextureCoordinate, 0),
          new VertexElement(36, VertexElementFormat.Single,
                                 VertexElementUsage.PointSize, 0),
          new VertexElement(40, VertexElementFormat.Single,
                                 VertexElementUsage.PointSize, 0)

        );

        public const int SizeInBytes = 44;

        VertexDeclaration IVertexType.VertexDeclaration => VertexDeclaration;
    }
}
