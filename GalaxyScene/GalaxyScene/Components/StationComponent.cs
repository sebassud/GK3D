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
    public class StationComponent: BaseGameComponent
    {
        private VertexBuffer _vertexBuf;
        private IndexBuffer _indexBuf;
        private int _nVerticies;
        private int _nFaces;
        private VertexBuffer _vertexBuf2;
        private IndexBuffer _indexBuf2;
        private int _nVerticies2;
        private int _nFaces2;
        private Texture2D _texture;
        private Matrix _world1;
        private Matrix _world2;
        private Matrix _world3;
        private Effect _effect;

        public StationComponent(Game game) : base(game)
        {
        }

        public override void Initialize()
        {
            HalfSphere(0.3f, 64);
            HalfSphere2(0.1f, 64);
            _world1 = Matrix.CreateTranslation(new Vector3(0, -0.4f, 4.9f)) * Matrix.CreateScale(gameService.Scale);
            _world2 = Matrix.CreateTranslation(new Vector3(0, 0.4f, 4.9f)) * Matrix.CreateScale(gameService.Scale);
            _world3 = Matrix.CreateTranslation(new Vector3(0, 0, 4.99f)) * Matrix.CreateScale(gameService.Scale);
            base.Initialize();
        }

        public override void LoadContent()
        {
            base.LoadContent();
            _texture = Game.Content.Load<Texture2D>("Station/texture-crush");
            _effect = GetEffect();
        }

        private void HalfSphere(float radius, int slices)
        {
            _nVerticies = (slices + 1) * (slices + 1);
            int nIndicies = 6 * slices * (slices + 1);

            var indices = new int[nIndicies];
            var vertices = new VertexPositionNormalTexture[_nVerticies];
            float thetaStep = MathHelper.PiOver2 / slices;
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

        private void HalfSphere2(float radius, int slices)
        {
            _nVerticies2 = (slices + 1) * (slices + 1);
            int nIndicies = 6 * slices * (slices + 1);

            var indices = new int[nIndicies];
            var vertices = new VertexPositionNormalTexture[_nVerticies2];
            float thetaStep = MathHelper.PiOver2 / slices;
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
                    float y = 2 * r * (float)Math.Cos(slicePhi * phiStep);

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
            _vertexBuf2 = new VertexBuffer(device, typeof(VertexPositionNormalTexture), _nVerticies2, BufferUsage.None);
            _indexBuf2 = new IndexBuffer(device, typeof(int), nIndicies, BufferUsage.None);
            _vertexBuf2.SetData(vertices, 0, vertices.Length);
            _indexBuf2.SetData(indices, 0, indices.Length);
            _nFaces2 = nIndicies / 3;
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice device = Game.GraphicsDevice;
            device.Indices = _indexBuf;
            device.SetVertexBuffer(_vertexBuf);

            var effect = GetEffect(_effect);
            effect.Parameters["ModelTexture"].SetValue(_texture);
            effect.CurrentTechnique = effect.Techniques["Textured"];

            effect.Parameters["World"].SetValue(_world1);
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, _nFaces);
            }

            effect.Parameters["World"].SetValue(_world2);
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, _nFaces);
            }

            device.Indices = _indexBuf2;
            device.SetVertexBuffer(_vertexBuf2);
            effect.Parameters["World"].SetValue(_world3);
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, _nFaces2);
            }
        }
    }
}
