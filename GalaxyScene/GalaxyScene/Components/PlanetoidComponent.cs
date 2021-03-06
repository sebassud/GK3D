﻿using GalaxyScene.Render;
using GalaxyScene.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyScene.Components
{
    public class PlanetoidComponent : BaseGameComponent
    {
        private VertexBuffer _vertexBuf;
        private IndexBuffer _indexBuf;
        private int _nVerticies;
        private int _nFaces;
        private Texture2D _texture;
        private Matrix _world;
        private Effect _effect;
        private Effect _effect2;

        public PlanetoidComponent(Game game) : base(game)
        {
            
        }

        public override void Initialize()
        {
            Sphere(5, 128);
            _world = Matrix.CreateScale(gameService.Scale);
            base.Initialize();
        }

        public override void LoadContent()
        {
            base.LoadContent();
            _texture = Game.Content.Load<Texture2D>("Planetoida/white-sand2");
            _effect = GetEffect();
            _effect2 = Game.Content.Load<Effect>("Shader/Shader_ShadowMap");
        }

        private void Sphere(float radius, int slices)
        {
            _nVerticies = (slices + 1) * (slices + 1);
            int nIndicies = 6 * slices * (slices + 1);

            var indices = new int[nIndicies];
            var vertices = new VertexPositionNormalTexture[_nVerticies];
            float thetaStep = MathHelper.Pi / slices;
            float phiStep = MathHelper.TwoPi / slices;

            int iIndex = 0;
            int iVertex = 0;
            int iVertex2 = 0;

            for (int sliceTheta = 0; sliceTheta < slices + 1; sliceTheta++)
            {
                float r = (float)Math.Sin(sliceTheta * thetaStep);
                float z = (float)Math.Cos(sliceTheta * thetaStep);

                for (int slicePhi = 0; slicePhi < (slices + 1); slicePhi++)
                {
                    float x = r * (float)Math.Sin(slicePhi * phiStep);
                    float y = r * (float)Math.Cos(slicePhi * phiStep);

                    vertices[iVertex].Position = new Vector3(x, y, z) * radius;
                    vertices[iVertex].Normal = Vector3.Normalize(new Vector3(x, y, z));
                    vertices[iVertex].TextureCoordinate = new Vector2((float)slicePhi / slices,
                        (float)sliceTheta / slices);
                    iVertex++;

                    if (sliceTheta != (slices - 1))
                    {
                        indices[iIndex++] = iVertex2 + (slices + 1);
                        indices[iIndex++] = iVertex2 + 1;
                        indices[iIndex++] = iVertex2;
                        indices[iIndex++] = iVertex2 + (slices);
                        indices[iIndex++] = iVertex2 + (slices + 1);
                        indices[iIndex++] = iVertex2;
                        iVertex2++;
                    }
                }
            }

            GraphicsDevice device = Game.GraphicsDevice;
            _vertexBuf = new VertexBuffer(device, typeof(VertexPositionNormalTexture), _nVerticies, BufferUsage.None);
            _indexBuf = new IndexBuffer(device, typeof(int), nIndicies, BufferUsage.None);
            _vertexBuf.SetData(vertices, 0, vertices.Length);
            _indexBuf.SetData(indices, 0, indices.Length);
            _nFaces = nIndicies / 3;
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice device = Game.GraphicsDevice;
            device.Indices = _indexBuf;
            device.SetVertexBuffer(_vertexBuf);


            var effect = GetEffect(_effect);
            effect.Parameters["ModelTexture2"].SetValue(_texture);
            effect.Parameters["World"].SetValue(_world);
            effect.CurrentTechnique = effect.Techniques["Textured"];

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, _nFaces);
            }
        }

        public override void DrawShadowMap()
        {
            base.DrawShadowMap();

            GraphicsDevice device = Game.GraphicsDevice;
            device.Indices = _indexBuf;
            device.SetVertexBuffer(_vertexBuf);


            var effect = _effect2;
            effect.Parameters["WorldViewProj"].SetValue(_world * lightViewProjection);

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, _nFaces);
            }
        }

    }
}
